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

using Expenses.ExpenseRepository.WcfClient.WcfExpenseService;
using Expenses.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Expenses.ExpenseRepository.WcfClient
{
    class AuthorizedExpenseServiceClient : ExpenseServiceClient, IDisposable
    {
        private OperationContextScope _scope;
        private IServiceAuthorizer _serviceAuthorizer;
        private bool _gotChannel;
        private IExpenseService _expenseService;

        public AuthorizedExpenseServiceClient(string serviceUrl, IServiceAuthorizer serviceAuthorizer) : base(new EndpointConfiguration(), serviceUrl)
        {
            this._serviceAuthorizer = serviceAuthorizer;
        }

        protected override IExpenseService CreateChannel()
        {
            // This part is a little hacky. In summary, we need to create the OperationContextScope so we can add our authorization
            // header. Unfortunately, since we're in a PCL, our extensibility options are very limited. However, we can sneak in
            // while the channel is being created to add the scope and set the header. The challenge is that the call to InnerChannel
            // re-enters this method, so we need to avoid the stack overflow. At the same time, all calls to this method need to 
            // return the same result for this instance for it all to work properly.
            if (!this._gotChannel)
            {
                this._gotChannel = true;

                this._scope = new OperationContextScope(this.InnerChannel);
                HttpRequestMessageProperty hrmp = new HttpRequestMessageProperty();
                hrmp.Headers[HttpRequestHeader.Authorization] = this._serviceAuthorizer.GetAuthorizationHeader();
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = hrmp;
            }
            else
            {
                this._expenseService = base.CreateChannel();
            }

            return this._expenseService as IExpenseService;
        }
        
        public void Dispose()
        {
            if (this._scope != null)
            {
                this._scope.Dispose();
                this._scope = null;
            }
        }
    }
}
