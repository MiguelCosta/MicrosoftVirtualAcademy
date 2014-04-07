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

namespace Expenses.Model
{
    public class ExpenseReport
    {
        public decimal Amount { get; set; }
        public string Approver { get; set; }
        public int CostCenter { get; set; }
        public Nullable<DateTime> DateResolved { get; set; }
        public Nullable<DateTime> DateSaved { get; set; }
        public Nullable<DateTime> DateSubmitted { get; set; }
        public int EmployeeId { get; set; }
        public int ExpenseReportId { get; set; }
        public decimal OwedToCreditCard { get; set; }
        public decimal OwedToEmployee { get; set; }
        public string Notes { get; set; }
        public string Purpose { get; set; }
        public ExpenseReportStatus Status { get; set; }
    }

}
