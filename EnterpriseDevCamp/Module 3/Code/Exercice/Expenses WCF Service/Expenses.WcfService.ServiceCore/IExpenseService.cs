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
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Expenses.WcfService.ServiceCore
{
    [ServiceContract]
    public interface IExpenseService
    {
        [OperationContract]
        Employee GetEmployee(string employeeAlias);

        [OperationContract]
        int SaveEmployee(Employee employee);

        [OperationContract]
        List<SummaryItem> GetSummaryItems(int employeeId);

        [OperationContract]
        ExpenseReport GetExpenseReport(int expenseReportId);

        [OperationContract]
        List<ExpenseReport> GetExpenseReportsForApproval(string employeeAlias);

        [OperationContract]
        List<ExpenseReport> GetExpenseReports(int employeeId);

        [OperationContract]
        List<ExpenseReport> GetExpenseReportsByStatus(int employeeId, ExpenseReportStatus status);

        [OperationContract]
        List<Charge> GetCharges(int expenseReportId);

        [OperationContract]
        Charge GetCharge(int chargeId);

        [OperationContract]
        List<Charge> GetOutstandingCharges(int employeeId);

        [OperationContract]
        decimal GetAmountOwedToCreditCard(int employeeId);

        [OperationContract]
        decimal GetAmountOwedToEmployee(int employeeId);

        [OperationContract]
        int SaveCharge(Charge charge);

        [OperationContract]
        void DeleteCharge(int chargeId);
        
        [OperationContract]
        int SaveExpenseReport(ExpenseReport expenseReport);

        [OperationContract]
        void DeleteExpenseReport(int expenseReportId);

        [OperationContract]
        void ResetData();
    }
}