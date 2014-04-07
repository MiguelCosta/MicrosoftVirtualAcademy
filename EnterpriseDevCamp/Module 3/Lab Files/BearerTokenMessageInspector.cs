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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Metadata;
using System.IdentityModel.Selectors;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Expenses.WcfService
{

    public class BearerTokenMessageInspector : IDispatchMessageInspector
    {
        const string audience = "http://expenseswcf.azurewebsites.net";
        const string authority = "https://login.windows.net/expenses.onmicrosoft.com";

        static string _issuer = string.Empty;
        static List<X509SecurityToken> _signingTokens = null;
        static DateTime _stsMetadataRetrievalTime = DateTime.MinValue;
        static string scopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";

        public BearerTokenMessageInspector()
        {
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            object correlationState = null;

            HttpRequestMessageProperty requestMessage = request.Properties["httpRequest"] as HttpRequestMessageProperty;
            if (request == null)
            {
                throw new InvalidOperationException("Invalid request type.");
            }
            string authHeader = requestMessage.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !this.Authenticate(authHeader))
            {
                WcfErrorResponseData error = new WcfErrorResponseData(HttpStatusCode.Unauthorized, string.Empty, new KeyValuePair<string, string>("WWW-Authenticate", "Bearer authorization_uri=\"" + authority + "\"" + "," + "resource_id=" + audience));
                correlationState = error;
            }

            return correlationState;
        }

        private bool Authenticate(string authHeader)
        {
            const string bearer = "Bearer ";
            if (!authHeader.StartsWith(bearer, StringComparison.InvariantCultureIgnoreCase)) { return false; }

            string jwtToken = authHeader.Substring(bearer.Length);
            string issuer;
            string stsMetadataAddress = string.Format("{0}/federationmetadata/2007-06/federationmetadata.xml", authority);
            List<X509SecurityToken> signingTokens;

            // Get tenant information that's used to validate incoming jwt tokens
            GetTenantInformation(stsMetadataAddress, out issuer, out signingTokens);

            JwtSecurityTokenHandler tokenHandler =
                new JwtSecurityTokenHandler()
                {
                    // For demo purposes certificate validation is turned off. Please note that this shouldn't be done in production code.
                    CertificateValidator = X509CertificateValidator.None
                };

            TokenValidationParameters validationParameters =
                new TokenValidationParameters()
                {
                    AllowedAudience = audience,
                    ValidIssuer = issuer,
                    SigningTokens = signingTokens,
                };

            // Validate token
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters);

            // Set the ClaimsPrincipal on the current thread.
            Thread.CurrentPrincipal = claimsPrincipal;

            // Set the ClaimsPrincipal on HttpContext.Current if the app is running in web hosted environment.
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = claimsPrincipal;
            }

            // if the token is scoped, verify that required permission is set in the scope claim
            if ((ClaimsPrincipal.Current.FindFirst(scopeClaimType) != null) && (ClaimsPrincipal.Current.FindFirst(scopeClaimType).Value != "user_impersonation"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Parses the federation metadata document and gets issuer Name and Signing Certificates
        /// </summary>
        /// <param name="metadataAddress">URL of the Federation Metadata document</param>
        /// <param name="issuer">Issuer Name</param>
        /// <param name="signingTokens">Signing Certificates in the form of X509SecurityToken</param>
        static void GetTenantInformation(string metadataAddress, out string issuer, out List<X509SecurityToken> signingTokens)
        {
            signingTokens = new List<X509SecurityToken>();

            // The issuer and signingTokens are cached for 24 hours. They are updated if any of the conditions in the if condition is true.            
            if ((DateTime.UtcNow.Subtract(_stsMetadataRetrievalTime).TotalHours > 24)
                || string.IsNullOrEmpty(_issuer)
                || _signingTokens == null)
            {
                MetadataSerializer serializer = new MetadataSerializer()
                {
                    // turning off certificate validation for demo. Don't use this in production code.
                    CertificateValidationMode = X509CertificateValidationMode.None
                };
                MetadataBase metadata = serializer.ReadMetadata(XmlReader.Create(metadataAddress));

                EntityDescriptor entityDescriptor = (EntityDescriptor)metadata;

                // get the issuer name
                if (!string.IsNullOrWhiteSpace(entityDescriptor.EntityId.Id))
                {
                    _issuer = entityDescriptor.EntityId.Id;
                }

                // get the signing certs
                _signingTokens = ReadSigningCertsFromMetadata(entityDescriptor);

                _stsMetadataRetrievalTime = DateTime.UtcNow;
            }

            issuer = _issuer;
            signingTokens = _signingTokens;
        }

        static List<X509SecurityToken> ReadSigningCertsFromMetadata(EntityDescriptor entityDescriptor)
        {
            List<X509SecurityToken> stsSigningTokens = new List<X509SecurityToken>();

            SecurityTokenServiceDescriptor stsd = entityDescriptor.RoleDescriptors.OfType<SecurityTokenServiceDescriptor>().First();

            if (stsd != null)
            {
                IEnumerable<X509RawDataKeyIdentifierClause> x509DataClauses = stsd.Keys.Where(key => key.KeyInfo != null && (key.Use == KeyType.Signing || key.Use == KeyType.Unspecified)).
                                                             Select(key => key.KeyInfo.OfType<X509RawDataKeyIdentifierClause>().First());

                stsSigningTokens.AddRange(x509DataClauses.Select(token => new X509SecurityToken(new X509Certificate2(token.GetX509RawData()))));
            }
            else
            {
                throw new InvalidOperationException("There is no RoleDescriptor of type SecurityTokenServiceType in the metadata");
            }

            return stsSigningTokens;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            WcfErrorResponseData error = correlationState as WcfErrorResponseData;
            if (error != null)
            {
                HttpResponseMessageProperty responseProperty = new HttpResponseMessageProperty();
                reply.Properties["httpResponse"] = responseProperty;
                responseProperty.StatusCode = error.StatusCode;

                IList<KeyValuePair<string, string>> headers = error.Headers;
                if (headers != null)
                {
                    for (int i = 0; i < headers.Count; i++)
                    {
                        responseProperty.Headers.Add(headers[i].Key, headers[i].Value);
                    }
                }
            }
        }
    }

    public class BearerTokenServiceBehavior : IServiceBehavior
    {
        public BearerTokenServiceBehavior()
        {

        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            // no-op
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher chDisp in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher epDisp in chDisp.Endpoints)
                {
                    epDisp.DispatchRuntime.MessageInspectors.Add(new BearerTokenMessageInspector());
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // no-op
        }
    }

    public class BearerTokenExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(BearerTokenServiceBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new BearerTokenServiceBehavior();
        }
    }

    internal class WcfErrorResponseData
    {
        public WcfErrorResponseData(HttpStatusCode status) :
            this(status, string.Empty, new KeyValuePair<string, string>[0])
        {
        }
        public WcfErrorResponseData(HttpStatusCode status, string body) :
            this(status, body, new KeyValuePair<string, string>[0])
        {
        }
        public WcfErrorResponseData(HttpStatusCode status, string body, params KeyValuePair<string, string>[] headers)
        {
            StatusCode = status;
            Body = body;
            Headers = headers;
        }


        public HttpStatusCode StatusCode
        {
            private set;
            get;
        }

        public string Body
        {
            private set;
            get;
        }

        public IList<KeyValuePair<string, string>> Headers
        {
            private set;
            get;
        }
    }
}