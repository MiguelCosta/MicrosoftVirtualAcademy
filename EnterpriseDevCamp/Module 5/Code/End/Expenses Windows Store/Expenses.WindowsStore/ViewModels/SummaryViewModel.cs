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
using System.Windows;
using System.Windows.Input;

namespace Expenses.WindowsStore
{
    public class SummaryViewModel : ViewModelBase
    {
        public SummaryItemsViewModel SummaryItemsViewModel
        {
            get { return this._summaryItemsViewModel; }
            set
            {
                this._summaryItemsViewModel = value;
                this.NotifyOfPropertyChange(() => this.SummaryItemsViewModel);
            }
        }
        private SummaryItemsViewModel _summaryItemsViewModel;

        public ChargesViewModel ChargesViewModel
        {
            get { return this._chargesViewModel; }
            set
            {
                this._chargesViewModel = value;
                this.NotifyOfPropertyChange(() => this.ChargesViewModel);
            }
        }
        private ChargesViewModel _chargesViewModel;
        
        public ApproveExpenseReportsViewModel ApproveExpenseReportsViewModel
        {
            get { return this._approveExpenseReportsViewModel; }
            set
            {
                this._approveExpenseReportsViewModel = value;
                this.NotifyOfPropertyChange(() => this.ApproveExpenseReportsViewModel);
            }
        }
        private ApproveExpenseReportsViewModel _approveExpenseReportsViewModel;

        public EmployeeViewModel EmployeeViewModel
        {
            get { return this._employeeViewModel; }
            set
            {
                this._employeeViewModel = value;
                this.NotifyOfPropertyChange(() => this.EmployeeViewModel);
            }
        }
        private EmployeeViewModel _employeeViewModel;

        private IViewService ViewService;

        private SummaryViewModel() { }

        public SummaryViewModel(EmployeeViewModel employeeViewModel)
        {
            this.EmployeeViewModel = employeeViewModel;
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();
            this.ApproveExpenseReportsViewModel = new ApproveExpenseReportsViewModel();
            this.ChargesViewModel = new ChargesViewModel();
            this.SummaryItemsViewModel = new SummaryItemsViewModel(this.EmployeeViewModel);
        }

        public async Task LoadAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    await this.ApproveExpenseReportsViewModel.LoadReportsForApprovalAsync();
                    await this.ChargesViewModel.LoadChargesAsync();
                    await this.SummaryItemsViewModel.GetSummaryItems();
                });
        }
    }
}
