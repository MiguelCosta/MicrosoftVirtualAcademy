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

using Expenses.ViewModel;
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
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Expenses.WindowsStore
{
    /// <summary>
    /// Not strictly a ViewModel since we're working with the WebBrowser (which is not designed for MVVM).
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        public Uri LoginUri
        {
            get { return this._winRtServiceAuthorizer.LoginUri; }
        }

        public bool ShowBrowser
        {
            get { return this._showBrowser; }
            set
            {
                if (this._showBrowser == value) { return; }

                this._showBrowser = value;
                this.NotifyOfPropertyChange(() => this.ShowBrowser);
            }
        }
        private bool _showBrowser;

        public readonly ICommand LoginCommand;

        private WinRtServiceAuthorizer _winRtServiceAuthorizer;

        private LoginViewModel() { }

        public LoginViewModel(WinRtServiceAuthorizer wp8ServiceAuthorizer, ICommand loginCommand)
        {
            this.ShowBrowser = true;

            this._winRtServiceAuthorizer = wp8ServiceAuthorizer;
            this.LoginCommand = loginCommand;
        }

        public bool ProcessUri(Uri uri)
        {
            // 2. Get the URL being navigated to.
            string url = uri.ToString();

            // 3. If it starts with the Redirect URI we've configured, that means we've
            // completed the login successfully.
            if (!url.StartsWith(this._winRtServiceAuthorizer.RedirectUri))
            {
                return false;
            }

            // 4. Now we'll extract the code from the query string.
            this._winRtServiceAuthorizer.Code = uri.Query.Remove(0, 6);

            // 5. We'll set up a Web request to get an access token. Part of
            // this involves providing a callback for when the request stream is ready.
            HttpWebRequest hwr = WebRequest.Create(string.Format("https://login.windows.net/{0}/oauth2/token", this._winRtServiceAuthorizer.DomainName)) as HttpWebRequest;
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
                            this._winRtServiceAuthorizer.Code,
                            this._winRtServiceAuthorizer.ClientID,
                            WebUtility.UrlEncode(this._winRtServiceAuthorizer.RedirectUri)));
                    Stream st = hwr.EndGetRequestStream(iar);
                    st.Write(bodyBits, 0, bodyBits.Length);
                    st.Flush();

                    // 7. We'll wait for the response by providing a callback.
                    hwr.BeginGetResponse(
                        async (iar2) =>
                        {
                            // 8. In this callback, we're going to parse out the access 
                            // token. This is easy with JSON.net.
                            HttpWebResponse resp = hwr.EndGetResponse(iar2) as HttpWebResponse;
                            StreamReader sr = new StreamReader(resp.GetResponseStream());
                            string responseString = sr.ReadToEnd();
                            JObject jo = JsonConvert.DeserializeObject(responseString) as JObject;
                            this._winRtServiceAuthorizer.AccessToken = (string)jo["access_token"];

                            // 9. Finally, we'll raise an event on the UI thread so it
                            // can continue with the app as authenticated.
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                () =>
                                {
                                    this.LoginCommand.Execute(null);
                                });
                        }, null);
                },
                hwr);

            return true;
        }
    }
}
