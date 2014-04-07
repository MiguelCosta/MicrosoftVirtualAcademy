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

//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Expenses.Model;

namespace Expenses.ViewModel
{
    public class EmployeeViewModel : ViewModelBase
    {
        public int EmployeeId
        {
            get { return this._employeeId; }
            private set
            {
                if (this._employeeId == value) { return; }
                this._employeeId = value;
                this.NotifyOfPropertyChange(() => this.EmployeeId);
            }
        }
        private int _employeeId;

        public string Manager
        {
            get { return this._manager; }
            private set
            {
                if (this._manager == value) { return; }
                this._manager = value;
                this.NotifyOfPropertyChange(() => this.Manager);
            }
        }
        private string _manager;

        public string Alias
        {
            get { return this._alias; }
            private set
            {
                if (this._alias == value) { return; }
                this._alias = value;
                this.NotifyOfPropertyChange(() => this.Alias);
            }
        }
        private string _alias;

        public string Name
        {
            get { return this._name; }
            private set
            {
                if (this._name == value) { return; }
                this._name = value;
                this.NotifyOfPropertyChange(() => this.Name);
            }
        }
        private string _name;

        public bool IsManager
        {
            get { return this._isManager; }
            set
            {
                if (this._isManager == value) { return; }
                this._isManager = value;
                this.NotifyOfPropertyChange(() => this.IsManager);
            }
        }
        private bool _isManager;

        public Employee Employee
        {
            set
            {
                if (value == null) { throw new ArgumentNullException("Employee cannot be null"); }

                this.Alias = value.Alias;
                this.EmployeeId = value.EmployeeId;
                this.Manager = value.Manager;
                this.Name = value.Name;

                this.IsManager = true;
            }
        }

        public EmployeeViewModel()
        {
        }

    }
}
