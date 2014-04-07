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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Expenses.ViewModel
{
    public class ServiceLocator
    {
        static public ServiceLocator Current
        {
            get { return ServiceLocator._current; }
        }
        static private ServiceLocator _current = new ServiceLocator();

        private Dictionary<Type, object> _services = new Dictionary<Type, object>();

        private ServiceLocator() { }

        public void SetService<T>(T service)
        {
            this._services.Add(typeof(T), service);
        }

        public T GetService<T>()
        {
            return (T)this._services[typeof(T)];
        }
    }
}