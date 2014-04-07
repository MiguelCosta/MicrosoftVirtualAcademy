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
using Expenses.ExpenseRepository.WcfClient.WcfExpenseService.Extensions;

namespace Expenses.ExpenseRepository.WcfClient.WcfExpenseService
{
    public partial class ExpenseReport
    {
        public Expenses.Model.ExpenseReport ToModelExpenseReport()
        {
            return
                new Model.ExpenseReport()
                {
                    Amount = this.Amount,
                    Approver = this.Approver,
                    CostCenter = this.CostCenter,
                    DateResolved = this.DateResolved,
                    DateSaved = this.DateSaved,
                    DateSubmitted = this.DateSubmitted,
                    EmployeeId = this.EmployeeId,
                    ExpenseReportId = this.ExpenseReportId,
                    Notes = this.Notes,
                    OwedToCreditCard = this.OwedToCreditCard,
                    OwedToEmployee = this.OwedToEmployee,
                    Purpose = this.Purpose,
                    Status = (Expenses.Model.ExpenseReportStatus) this.Status,
                };
        }

        static public ExpenseReport FromModelExpenseReport(Expenses.Model.ExpenseReport expenseReport)
        {
            return
                   new ExpenseReport()
                   {
                       Amount = expenseReport.Amount,
                       Approver = expenseReport.Approver,
                       CostCenter = expenseReport.CostCenter,
                       DateResolved = expenseReport.DateResolved,
                       DateSaved = expenseReport.DateSaved,
                       DateSubmitted = expenseReport.DateSubmitted,
                       EmployeeId = expenseReport.EmployeeId,
                       ExpenseReportId = expenseReport.ExpenseReportId,
                       Notes = expenseReport.Notes,
                       OwedToCreditCard = expenseReport.OwedToCreditCard,
                       OwedToEmployee = expenseReport.OwedToEmployee,
                       Purpose = expenseReport.Purpose,
                       Status = (int) expenseReport.Status,                       
                   };
        }
    }
}

namespace Expenses.ExpenseRepository.WcfClient.WcfExpenseService.Extensions
{
    static public class ExpenseReportExtensions
    {
        static public List<Expenses.Model.ExpenseReport> ToModelExpenseReports(this IEnumerable<ExpenseReport> items)
        {
            return items.Select(item => (item == null) ? null : item.ToModelExpenseReport()).ToList();
        }
    }
}
