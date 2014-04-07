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
using Expenses.Model;

namespace Expenses.ViewModel
{
    public class EmployeesViewModel : ViewModelBase
    {
        #region Properties

        private static Uri baseUri = new Uri("ms-appx:///");

        private int id = 0;
        public int Id
        {
            get
            { return id; }

            set
            {
                if (id == value)
                { return; }

                id = value;
                this.NotifyOfPropertyChange(() => this.Id);
            }
        }

        private string name = string.Empty;
        public string Name
        {
            get
            { return name; }

            set
            {
                if (name == value)
                { return; }

                name = value;
                this.NotifyOfPropertyChange(() => this.Name);
            }
        }

        private string alias = string.Empty;
        public string Alias
        {
            get
            { return alias; }

            set
            {
                if (alias == value)
                { return; }

                alias = value;
                this.NotifyOfPropertyChange(() => this.Alias);
            }
        }

        private string imagePath = string.Empty;
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                if (imagePath == value)
                { return; }

                imagePath = value;
                this.NotifyOfPropertyChange(() => this.ImagePath);
            }
        }

        private byte[] image = null;
        public byte[] Image
        {
            get
            {
                return image;
            }

            set
            {
                if (image == value)
                { return; }

                image = value;
                this.NotifyOfPropertyChange(() => this.Image);
            }
        }

        private List<Employee> employees = null;
        public List<Employee> Employees
        {
            get
            {
                return employees;
            }

            set
            {
                if (employees == value)
                {
                    return;
                }

                employees = value;
                this.NotifyOfPropertyChange(() => this.Employees);
            }
        }
        #endregion "Properties"

        public EmployeesViewModel()
        {

        }

    }
}
