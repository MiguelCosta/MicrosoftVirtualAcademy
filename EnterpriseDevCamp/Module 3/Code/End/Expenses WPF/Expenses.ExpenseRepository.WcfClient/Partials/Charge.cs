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

namespace Expenses.ExpenseRepository.WcfClient.WcfExpenseService
{
    public partial class Charge
    {
        public Expenses.Model.Charge ToModelCharge()
        {
            return
                new Model.Charge()
                {
                    AccountNumber = this.AccountNumber,
                    BilledAmount = this.BilledAmount,
                    Category = this.Category,
                    ChargeId = this.ChargeId,
                    Description = this.Description,
                    EmployeeId = this.EmployeeId,
                    ExpenseDate = this.ExpenseDate,
                    ExpenseReportId = this.ExpenseReportId,
                    ExpenseType = this.ExpenseType,
                    Location = this.Location,
                    Merchant = this.Merchant,
                    Notes = this.Notes,
                    ReceiptRequired = this.ReceiptRequired,
                    TransactionAmount = this.TransactionAmount,                    
                };
        }

        static public Charge FromModelCharge(Expenses.Model.Charge charge)
        {
            Charge newCharge =
                new Charge()
                {
                    AccountNumber = charge.AccountNumber,
                    BilledAmount = charge.BilledAmount,
                    ChargeId = charge.ChargeId,
                    Category = charge.Category,
                    Description = charge.Description,
                    EmployeeId = charge.EmployeeId,
                    ExpenseDate = charge.ExpenseDate,
                    ExpenseReportId = charge.ExpenseReportId,
                    ExpenseType = charge.ExpenseType,
                    Location = charge.Location,
                    Merchant = charge.Merchant,
                    Notes = charge.Notes,
                    ReceiptRequired = charge.ReceiptRequired,
                    TransactionAmount = charge.TransactionAmount,
                };

            return newCharge;
        }
    }
}

namespace Expenses.ExpenseRepository.WcfClient.WcfExpenseService.Extensions
{
    static public class ChargeExtensions
    {
        static public List<Expenses.Model.Charge> ToModelCharges(this IEnumerable<Charge> items)
        {
            return items.Select(item => (item == null) ? null : item.ToModelCharge()).ToList();
        }
    }
}
