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

namespace Expenses.WindowsStore
{
    public class WinRtServiceAuthorizer : IServiceAuthorizer
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

        public WinRtServiceAuthorizer()
        {            
            this.Code = string.Empty;
            this.AccessToken = string.Empty;
        }

        public string GetAuthorizationHeader()
        {
            return string.Format("Bearer {0}", this.AccessToken);
        }
    }
}
