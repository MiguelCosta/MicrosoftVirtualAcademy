// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using Expenses.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LobCampPhone
{
    public class Wp8ServiceAuthorizer : IServiceAuthorizer
    {
        public Uri LoginUri
        {
            get
            {
                return new Uri(
                    string.Format(
                        "https://login.windows.net/{0}/oauth2/authorize?response_type=code&resource={1}&client_id={2}&redirect_uri={3}",
                        this.DomainName,
                        this.Resource,
                        this.ClientID,
                        this.RedirectUri));
            }
        }

        public string RedirectUri { get; set; }
        public string DomainName { get; set; }
        public string ClientID { get; set; }
        public string Resource { get; set; }
        public string Code { get; set; }
        public string AccessToken { get; set; }

        public event EventHandler TokenReceived;

        public Wp8ServiceAuthorizer()
        {            
            this.Code = string.Empty;
            this.AccessToken = string.Empty;
        }

        public string GetAuthorizationHeader()
        {
            return string.Format("Bearer {0}", this.AccessToken);
        }

        public void ProcessUri(Uri uri)
        {
            // 1. Remember the dispatcher so we can raise our event on the same thread.
            Dispatcher dispatcher = Application.Current.RootVisual.Dispatcher;

            // 2. Get the URL being navigated to.
            string url = uri.ToString();

            // 3. If it starts with the Redirect URI we've configured, that means we've
            // completed the login successfully.
            if (url.StartsWith(this.RedirectUri))
            {
                // 4. Now we'll extract the code from the query string.
                this.Code = uri.Query.Remove(0, 6);

                // 5. We'll set up a Web request to get an access token. Part of
                // this involves providing a callback for when the request stream is ready.
                HttpWebRequest hwr = WebRequest.Create(string.Format("https://login.windows.net/{0}/oauth2/token", this.DomainName)) as HttpWebRequest;
                hwr.Method = "POST";
                hwr.ContentType = "application/x-www-form-urlencoded";
                hwr.BeginGetRequestStream(
                    (iar) =>
                    {
                        // 6. Now that we have the request stream, we'll send data as the
                        // POST body that includes the details for our request.
                        byte[] bodyBits = Encoding.UTF8.GetBytes(
                            string.Format(
                                "grant_type=authorization_code&code={0}&client_id={1}&redirect_uri={2}",
                                this.Code,
                                this.ClientID,
                                HttpUtility.UrlEncode(this.RedirectUri)));
                        Stream st = hwr.EndGetRequestStream(iar);
                        st.Write(bodyBits, 0, bodyBits.Length);
                        st.Close();

                        // 7. We'll wait for the response by providing a callback.
                        hwr.BeginGetResponse(
                            (iar2) =>
                            {
                                // 8. In this callback, we're going to parse out the access 
                                // token. This is easy with JSON.net.
                                HttpWebResponse resp = hwr.EndGetResponse(iar2) as HttpWebResponse;
                                StreamReader sr = new StreamReader(resp.GetResponseStream());
                                string responseString = sr.ReadToEnd();
                                JObject jo = JsonConvert.DeserializeObject(responseString) as JObject;
                                this.AccessToken = (string)jo["access_token"];

                                // 9. Finally, we'll raise an event on the UI thread so it
                                // can continue with the app as authenticated.
                                dispatcher.BeginInvoke(
                                    () =>
                                    {
                                        this.OnTokenReceived();
                                    });
                            }, null);
                    },
                    hwr);
            }
        }

        protected void OnTokenReceived()
        {
            EventHandler handler = this.TokenReceived;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
