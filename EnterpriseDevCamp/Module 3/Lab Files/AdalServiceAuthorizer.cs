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
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Expenses.Wpf
{
    public class AdalServiceAuthorizer : IServiceAuthorizer
    {
        private Uri _redirectUri = new Uri("http://expenseswcf.azurewebsites.net");
        private string _clientId = "29c217e1-12e7-45ee-a6d6-1ec615bb93f3";
        private Uri _resourceBaseAddress = new Uri("http://expenseswcf.azurewebsites.net");

        private string _authority = string.Empty;
        private string _resourceAppIdUri = string.Empty;
        private AuthenticationContext _authenticationContext;

        public AdalServiceAuthorizer()
        {
            try
            {
                // Use AuthenticationParameters to send a request to the RP and receive tenant information in the 401 challenge.
                AuthenticationParameters parameters = AuthenticationParameters.CreateFromResourceUrl(new Uri(_resourceBaseAddress + "ExpenseService.svc"));

                _authority = parameters.Authority;

                // validate resourceId that is obtained in a 401 challenge out of band to mitigate attacks from a malicious RP impersonating as a valid RP.
                // here we are doing a sanity check by verifying that the resourceId is bound to the physical address of the resource
                if (parameters.Resource.Contains(this._resourceBaseAddress.Host))
                {
                    _resourceAppIdUri = parameters.Resource;
                }
                else
                {
                    throw new Exception(string.Format("The resource obtained in 401 challenge, {0}, is not bound to the resource's physical address, {1}", parameters.Resource, this._resourceBaseAddress));
                }

                // Initialize the AuthenticationContext by providing the tenant authority endpoint. 
                // By default, address validation of the authority endpoint is on. Always validate the tenant endpoint that's received in 401 challenge.
                // CredManCache is a custom cache that uses windows Credential Manager to manage the token cache.
                // When the authority is ADFS, apps must do authority validation out of band, so pass false as the second parameter in the below constructor.
                _authenticationContext = new AuthenticationContext(_authority, new CredManCache());
            }
            catch (Exception e)
            {
                MessageBox.Show("401 discovery failed ", e.Message);
                return;
            }
        }

        public string GetAuthorizationHeader()
        {
            AuthenticationResult authenticationResult = null;

            try
            {
                authenticationResult = this._authenticationContext.AcquireToken(this._resourceAppIdUri, this._clientId, this._redirectUri);

                return authenticationResult.CreateAuthorizationHeader();
            }
            catch (ActiveDirectoryAuthenticationException ex)
            {
                string message = ex.Message;

                if (ex.InnerException != null)
                {
                    message += "InnerException : " + ex.InnerException.Message;
                }

                MessageBox.Show(message);
            }

            return null;
        }
    }
}
