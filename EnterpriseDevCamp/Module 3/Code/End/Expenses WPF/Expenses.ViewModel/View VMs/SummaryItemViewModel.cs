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
using Expenses;
using Expenses.Model;

namespace Expenses.ViewModel
{
    public class SummaryItemViewModel : ViewModelBase
    {
        #region Properties

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id == value)
                {
                    return;
                }
                id = value;
                
                this.NotifyOfPropertyChange(() => this.Id);
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get
            { 
                return date;
            }
            set
            {
                if (date == value)
                {
                    return;
                }
                date = value;
                this.NotifyOfPropertyChange(() => this.Date);
            }
        }

        private string description = string.Empty;
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                if (description == value)
                {
                    return;
                }

                description = value;
                this.NotifyOfPropertyChange(() => this.Description);
            }
        }

        private decimal? amount = 0.00M;
        public decimal? Amount
        {
            get
            {
                return amount;
            }

            set
            {
                if (amount == value)
                {
                    return;
                }

                amount = value;
                this.NotifyOfPropertyChange(() => this.Amount);
            }
        }

        private string submitter = string.Empty;
        public string Submitter
        {
            get
            {
                return submitter;
            }

            set
            {
                if (submitter == value)
                {
                    return;
                }

                submitter = value;
                this.NotifyOfPropertyChange(() => this.Submitter);
            }
        }

        private ItemType itemType = ItemType.Charge;
        public ItemType ItemType
        {
            get
            {
                return itemType;
            }

            set
            {
                if (itemType == value)
                {
                    return;
                }

                itemType = value;
                this.NotifyOfPropertyChange(() => this.ItemType);
            }
        }

        #endregion
    }
}
