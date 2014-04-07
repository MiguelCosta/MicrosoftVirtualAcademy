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
    public class Charge
    {
        public int AccountNumber { get; set; }
        public decimal BilledAmount { get; set; }
        public int ChargeId { get; set; }
        public int Category { get; set; }
        public string Description { get; set; }
        public int EmployeeId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Nullable<int> ExpenseReportId { get; set; }
        public int ExpenseType { get; set; }
        public string Location { get; set; }
        public string Merchant { get; set; }
        public string Notes { get; set; }
        public bool ReceiptRequired { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
