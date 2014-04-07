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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.ExpenseRepository.WcfClient.WcfExpenseService.Extensions;

namespace Expenses.ExpenseRepository.WcfClient
{
    public class WcfClientExpenseRepository : IExpenseRepository
    {
        private readonly string _serviceUrl;

        private WcfClientExpenseRepository() { }

        public WcfClientExpenseRepository(string serviceUrl)
        {
            this._serviceUrl = serviceUrl;
        }

        private WcfExpenseService.ExpenseServiceClient CreateExpenseServiceClient()
        {
            return new WcfExpenseService.ExpenseServiceClient(
                new Expenses.ExpenseRepository.WcfClient.WcfExpenseService.ExpenseServiceClient.EndpointConfiguration(),
                this._serviceUrl);
        }

        private bool VerifyResult<T>(AsyncCompletedEventArgs args, TaskCompletionSource<T> tcs)
        {
            if (args.Cancelled)
            {
                tcs.TrySetCanceled();
            }
            else if (args.Error != null)
            {
                tcs.TrySetException(args.Error);
            }
            else
            {
                return true;
            }

            return false;
        }

        public Task<Employee> GetEmployeeAsync(string employeeAlias)
        {
            TaskCompletionSource<Employee> tcs = new TaskCompletionSource<Employee>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetEmployeeCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result.ToModelEmployee());
                    }
                };
            client.GetEmployeeAsync(employeeAlias);

            return tcs.Task;
        }

        public Task<int> SaveEmployeeAsync(Employee employee)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.SaveEmployeeCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result);
                    }
                };
            client.SaveEmployeeAsync(WcfExpenseService.Employee.FromModelEmployee(employee));

            return tcs.Task;
        }

        public Task<List<SummaryItem>> GetSummaryItemsAsync(int employeeId)
        {
            TaskCompletionSource<List<SummaryItem>> tcs = new TaskCompletionSource<List<SummaryItem>>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetSummaryItemsCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result.ToModelSummaryItems());
                    }
                };
            client.GetSummaryItemsAsync(employeeId);

            return tcs.Task;
        }

        public Task<ExpenseReport> GetExpenseReportAsync(int expenseReportId)
        {
            TaskCompletionSource<ExpenseReport> tcs = new TaskCompletionSource<ExpenseReport>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetExpenseReportCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result.ToModelExpenseReport());
                    }
                };
            client.GetExpenseReportAsync(expenseReportId);

            return tcs.Task;
        }

        public Task<List<ExpenseReport>> GetExpenseReportsAsync(int employeeId)
        {
            TaskCompletionSource<List<ExpenseReport>> tcs = new TaskCompletionSource<List<ExpenseReport>>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetExpenseReportsCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result.ToModelExpenseReports());
                    }
                };
            client.GetExpenseReportsAsync(employeeId);

            return tcs.Task;
        }

        public Task<List<ExpenseReport>> GetExpenseReportsByStatusAsync(int employeeId, ExpenseReportStatus status)
        {
            TaskCompletionSource<List<ExpenseReport>> tcs = new TaskCompletionSource<List<ExpenseReport>>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetExpenseReportsByStatusCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result.ToModelExpenseReports());
                    }
                };
            client.GetExpenseReportsByStatusAsync(employeeId, (WcfExpenseService.ExpenseReportStatus)status);

            return tcs.Task;
        }

        public Task<List<ExpenseReport>> GetExpenseReportsForApprovalAsync(string employeeAlias)
        {
            TaskCompletionSource<List<ExpenseReport>> tcs = new TaskCompletionSource<List<ExpenseReport>>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetExpenseReportsForApprovalCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result.ToModelExpenseReports());
                    }
                };
            client.GetExpenseReportsForApprovalAsync(employeeAlias);

            return tcs.Task;
        }

        public Task<List<Charge>> GetChargesAsync(int expenseReportId)
        {
            TaskCompletionSource<List<Charge>> tcs = new TaskCompletionSource<List<Charge>>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetChargesCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result.ToModelCharges());
                    }
                };
            client.GetChargesAsync(expenseReportId);

            return tcs.Task;
        }

        public Task<Charge> GetChargeAsync(int chargeId)
        {
            TaskCompletionSource<Charge> tcs = new TaskCompletionSource<Charge>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetChargeCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result.ToModelCharge());
                    }
                };
            client.GetChargeAsync(chargeId);

            return tcs.Task;
        }

        public Task<List<Charge>> GetOutstandingChargesAsync(int employeeId)
        {
            TaskCompletionSource<List<Charge>> tcs = new TaskCompletionSource<List<Charge>>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetOutstandingChargesCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result.ToModelCharges());
                    }
                };
            client.GetOutstandingChargesAsync(employeeId);

            return tcs.Task;
        }

        public Task<decimal> GetAmountOwedToCreditCardAsync(int employeeId)
        {
            TaskCompletionSource<decimal> tcs = new TaskCompletionSource<decimal>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetAmountOwedToCreditCardCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result);
                    }
                };
            client.GetAmountOwedToCreditCardAsync(employeeId);

            return tcs.Task;
        }

        public Task<decimal> GetAmountOwedToEmployeeAsync(int employeeId)
        {
            TaskCompletionSource<decimal> tcs = new TaskCompletionSource<decimal>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.GetAmountOwedToEmployeeCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(e.Result);
                    }
                };
            client.GetAmountOwedToEmployeeAsync(employeeId);

            return tcs.Task;
        }

        public Task<int> SaveChargeAsync(Charge charge)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.SaveChargeCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        charge.ChargeId = e.Result;
                        tcs.TrySetResult(e.Result);
                    }
                };
            client.SaveChargeAsync(WcfExpenseService.Charge.FromModelCharge(charge));

            return tcs.Task;
        }

        public Task DeleteChargeAsync(int chargeId)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.DeleteChargeCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(null);
                    }
                };
            client.DeleteChargeAsync(chargeId);

            return tcs.Task;
        }

        public Task<int> SaveExpenseReportAsync(ExpenseReport expenseReport)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.SaveExpenseReportCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        expenseReport.ExpenseReportId = e.Result;
                        tcs.TrySetResult(e.Result);
                    }
                };
            client.SaveExpenseReportAsync(WcfExpenseService.ExpenseReport.FromModelExpenseReport(expenseReport));

            return tcs.Task;
        }

        public Task DeleteExpenseReportAsync(int expenseReportId)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.DeleteExpenseReportCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(null);
                    }
                };
            client.DeleteExpenseReportAsync(expenseReportId);

            return tcs.Task;
        }

        public Task ResetDataAsync()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            WcfExpenseService.ExpenseServiceClient client = this.CreateExpenseServiceClient();

            client.ResetDataCompleted +=
                (_, e) =>
                {
                    if (this.VerifyResult(e, tcs))
                    {
                        tcs.TrySetResult(null);
                    }
                };
            client.ResetDataAsync();

            return tcs.Task;
        }
    }
}
