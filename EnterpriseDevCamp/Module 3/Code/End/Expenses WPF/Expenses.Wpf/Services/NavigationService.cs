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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Expenses.Wpf
{
    public class NavigationService : INavigationService
    {
        public event EventHandler<EventArgs<ChargeViewModel>> ShowChargeRequested;
        public event EventHandler ShowChargesRequested;
        public event EventHandler<EventArgs<ExpenseReportViewModel>> ShowExpenseReportRequested;
        public event EventHandler ShowPendingExpenseReportsRequested;
        public event EventHandler ShowSavedExpenseReportsRequested;
        public event EventHandler ShowPastExpenseReportsRequested;

        public void ShowCharge(ChargeViewModel chargeViewModel)
        {
            EventHandler<EventArgs<ChargeViewModel>> handler = this.ShowChargeRequested;
            if (handler != null)
            {
                handler(this, new EventArgs<ChargeViewModel>(chargeViewModel));
            }
        }

        public void ShowCharges()
        {
            EventHandler handler = this.ShowChargesRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        
        public void ShowExpenseReport(ExpenseReportViewModel expenseReportViewModel)
        {
            EventHandler<EventArgs<ExpenseReportViewModel>> handler = this.ShowExpenseReportRequested;
            if (handler != null)
            {
                handler(this, new EventArgs<ExpenseReportViewModel>(expenseReportViewModel));
            }
        }

        public void ShowPendingExpenseReports()
        {
            EventHandler handler = this.ShowPendingExpenseReportsRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void ShowSavedExpenseReports()
        {
            EventHandler handler = this.ShowSavedExpenseReportsRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void ShowPastExpenseReports()
        {
            EventHandler handler = this.ShowPastExpenseReportsRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
