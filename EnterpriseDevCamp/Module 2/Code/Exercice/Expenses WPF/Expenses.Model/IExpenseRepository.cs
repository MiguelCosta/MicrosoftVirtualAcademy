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
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Model
{
    public interface IExpenseRepository
    {
        Task<Employee> GetEmployeeAsync(string employeeAlias);
        Task<int> SaveEmployeeAsync(Employee employee);
        Task<List<SummaryItem>> GetSummaryItemsAsync(int employeeId);
        Task<ExpenseReport> GetExpenseReportAsync(int expenseReportId);
        Task<List<ExpenseReport>> GetExpenseReportsAsync(int employeeId);
        Task<List<ExpenseReport>> GetExpenseReportsByStatusAsync(int employeeId, ExpenseReportStatus status);
        Task<List<ExpenseReport>> GetExpenseReportsForApprovalAsync(string employeeAlias);
        Task<List<Charge>> GetChargesAsync(int expenseReportId);
        Task<Charge> GetChargeAsync(int chargeId);
        Task<List<Charge>> GetOutstandingChargesAsync(int employeeId);
        Task<decimal> GetAmountOwedToCreditCardAsync(int employeeId);
        Task<decimal> GetAmountOwedToEmployeeAsync(int employeeId);
        Task<int> SaveChargeAsync(Charge charge);
        Task DeleteChargeAsync(int chargeId);
        Task<int> SaveExpenseReportAsync(ExpenseReport expenseReport);
        Task DeleteExpenseReportAsync(int expenseReportId);
        Task ResetDataAsync();

    }
}
