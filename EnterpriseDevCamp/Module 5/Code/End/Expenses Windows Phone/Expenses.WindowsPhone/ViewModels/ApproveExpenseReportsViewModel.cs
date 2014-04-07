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
using System.Windows.Input;

namespace Expenses.WindowsPhone
{
    public class ApproveExpenseReportsViewModel : ExpenseReportsViewModel
    {
        private IViewService ViewService;

        public ICommand ApproveReportCommand { get; private set; }

        public ApproveExpenseReportsViewModel()
        {
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();

            this.ApproveReportCommand = new RelayCommand(async(report) => 
            {
                await this.ApproveExpenseReportAsync(report as ExpenseReportViewModel);
                await this.LoadReportsForApprovalAsync();
            });
        }

        private async Task ApproveExpenseReportAsync(ExpenseReportViewModel reportViewModel)
        {
            await reportViewModel.ApproveAsync();
        }

        public async Task LoadReportsForApprovalAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Load(await this._repository.GetExpenseReportsForApprovalAsync(this.EmployeeViewModel.Alias));
                });
        }

        
    }
}
