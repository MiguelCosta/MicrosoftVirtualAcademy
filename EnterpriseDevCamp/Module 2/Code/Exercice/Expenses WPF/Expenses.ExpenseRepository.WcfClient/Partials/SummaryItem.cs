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
    public partial class SummaryItem
    {
        public Expenses.Model.SummaryItem ToModelSummaryItem()
        {
            return
                new Model.SummaryItem()
                {
                    Amount = this.Amount,
                    Date = this.Date.GetValueOrDefault(DateTime.MinValue),
                    Description = this.Description,
                    Id = this.Id,
                    ItemType = (Expenses.Model.ItemType) this.ItemType,
                    Submitter = this.Submitter,
                };
        }

        static public SummaryItem FromModelSummaryItem(Expenses.Model.SummaryItem summaryItem)
        {
            return
                new SummaryItem()
                {
                    Amount = summaryItem.Amount,
                    Date = summaryItem.Date,
                    Description = summaryItem.Description,
                    Id = summaryItem.Id,
                    ItemType = (ItemType) summaryItem.ItemType,
                    Submitter = summaryItem.Submitter,
                };            
        }
    }
}

namespace Expenses.ExpenseRepository.WcfClient.WcfExpenseService.Extensions
{
    static public class SummaryItemExtensions
    {
        static public List<Expenses.Model.SummaryItem> ToModelSummaryItems(this IEnumerable<SummaryItem> items)
        {
            return items.Select(item => (item == null) ? null : item.ToModelSummaryItem()).ToList();
        }
    }
}
