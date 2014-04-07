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
using System.Windows.Threading;

namespace Expenses.WindowsPhone
{
    /// <summary>
    /// Not strictly a ViewModel since we're working with the WebBrowser (which is not designed for MVVM).
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        public Uri LoginUri
        {
            get { return this._wp8ServiceAuthorizer.LoginUri; }
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

        private Wp8ServiceAuthorizer _wp8ServiceAuthorizer;

        private LoginViewModel() { }

        public LoginViewModel(Wp8ServiceAuthorizer wp8ServiceAuthorizer, ICommand loginCommand)
        {
            this.ShowBrowser = true;

            this._wp8ServiceAuthorizer = wp8ServiceAuthorizer;
            this.LoginCommand = loginCommand;
        }

        public void ProcessUri(Uri uri)
        {
            Dispatcher dispatcher = Application.Current.RootVisual.Dispatcher;

            string url = uri.ToString();
            if (url.StartsWith(this._wp8ServiceAuthorizer.RedirectUri))
            {
                this.ShowBrowser = false;

                this._wp8ServiceAuthorizer.Code = uri.Query.Remove(0, 6);

                HttpWebRequest hwr = WebRequest.Create(string.Format("https://login.windows.net/{0}/oauth2/token", this._wp8ServiceAuthorizer.DomainName)) as HttpWebRequest;
                hwr.Method = "POST";
                hwr.ContentType = "application/x-www-form-urlencoded";
                hwr.BeginGetRequestStream(
                    (iar) =>
                    {
                        byte[] bodyBits = Encoding.UTF8.GetBytes(
                            string.Format(
                                "grant_type=authorization_code&code={0}&client_id={1}&redirect_uri={2}",
                                this._wp8ServiceAuthorizer.Code,
                                this._wp8ServiceAuthorizer.ClientID,
                                HttpUtility.UrlEncode(this._wp8ServiceAuthorizer.RedirectUri)));
                        Stream st = hwr.EndGetRequestStream(iar);
                        st.Write(bodyBits, 0, bodyBits.Length);
                        st.Close();
                        hwr.BeginGetResponse(
                            (iar2) =>
                            {
                                HttpWebResponse resp = hwr.EndGetResponse(iar2) as HttpWebResponse;

                                StreamReader sr = new StreamReader(resp.GetResponseStream());
                                string responseString = sr.ReadToEnd();
                                JObject jo = JsonConvert.DeserializeObject(responseString) as JObject;

                                this._wp8ServiceAuthorizer.AccessToken = (string)jo["access_token"];

                                dispatcher.BeginInvoke(
                                    () =>
                                    {
                                        this.LoginCommand.Execute(null);
                                    });
                            }, null);
                    },
                    hwr);
            }
        }
    }
}
