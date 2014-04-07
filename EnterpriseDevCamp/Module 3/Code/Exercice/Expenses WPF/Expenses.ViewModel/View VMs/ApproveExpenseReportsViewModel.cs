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

namespace Expenses.ViewModel
{
    public class ApproveExpenseReportsViewModel : ExpenseReportsViewModel
    {
        public ICommand ApproveReportCommand { get; private set; }

        private IViewService _viewService;

        public ApproveExpenseReportsViewModel()
        {
            this.ApproveReportCommand = 
                new RelayCommand(
                    async(report) => 
                    {
                        await this.ApproveExpenseReportAsync(report as ExpenseReportViewModel);
                        await this.LoadReportsForApprovalAsync();
                    });

            this._viewService = ServiceLocator.Current.GetService<IViewService>();
        }

        private async Task ApproveExpenseReportAsync(ExpenseReportViewModel reportViewModel)
        {
            if (await this._viewService.ConfirmAsync(
                "Confirm expense report",
                "Are you sure you want to approve this expense report?"))
            {
                await reportViewModel.ApproveAsync();
            }
        }

        public async Task LoadReportsForApprovalAsync()
        {
            this.Load(await this._repository.GetExpenseReportsForApprovalAsync(this.EmployeeViewModel.Alias));
        }

        
    }
}
