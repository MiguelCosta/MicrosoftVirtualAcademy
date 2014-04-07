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

//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses;
using Expenses.Model;

namespace Expenses.ViewModel
{
    public class SummaryItemsViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<SortType> SortTypes { get; set; }

        public ObservableCollection<GroupInfoList<object>> GroupedSummaryItems
        {
            get { return this._groupedSummaryItems; }
            set
            {
                this._groupedSummaryItems = value;
                this.NotifyOfPropertyChange(() => this.GroupedSummaryItems);
            }
        }

        public ObservableCollection<SummaryItemViewModel> SummaryItems
        {
            get { return this._summaryItems; }
            set
            {
                this._summaryItems = value;
                this.NotifyOfPropertyChange(() => this.SummaryItems);
            }
        }

        public SortType SortType
        {
            get { return _sortType; }
            set
            {
                if (this._sortType == value) { return; }

                this._sortType = value;
                this.NotifyOfPropertyChange(() => this.SortType);

                //update the grouping because the sort/filter type changed
                this.GroupItems();
            }
        }
                
        public int NumberOfCharges
        {
            get { return this._numberOfCharges; }

            set
            {
                if (this._numberOfCharges == value) { return; }

                this._numberOfCharges = value;
                this.NotifyOfPropertyChange(() => this.NumberOfCharges);
            }
        }
        private int _numberOfCharges = 0;
                
        public decimal AmountOfCharges
        {
            get
            { return this._amountOfCharges; }

            set
            {
                if (this._amountOfCharges == value) { return; }

                this._amountOfCharges = value;
                this.NotifyOfPropertyChange(() => this.AmountOfCharges);
            }
        }
        private decimal _amountOfCharges = 0M;

        public int NumberOfChargesLT30Days
        {
            get { return this._numberOfChargesLT30Days; }
            set
            {
                if (this._numberOfChargesLT30Days == value) { return; }

                this._numberOfChargesLT30Days = value;
                this.NotifyOfPropertyChange(() => this.NumberOfChargesLT30Days);
            }
        }
        private int _numberOfChargesLT30Days = 0;
                
        public decimal AmountOfChargesLT30Days
        {
            get { return this._amountOfChargesLT30Days; }
            set
            {
                if (this._amountOfChargesLT30Days == value) { return; }

                this._amountOfChargesLT30Days = value;
                this.NotifyOfPropertyChange(() => this.AmountOfChargesLT30Days);
            }
        }
        private decimal _amountOfChargesLT30Days = 0M;

        public int NumberOfCharges30To44Days
        {
            get { return this._numberOfCharges30To44Days; }
            set
            {
                if (this._numberOfCharges30To44Days == value) { return; }

                this._numberOfCharges30To44Days = value;
                this.NotifyOfPropertyChange(() => this.NumberOfCharges30To44Days);
            }
        }
        private int _numberOfCharges30To44Days = 0;

        public decimal AmountOfCharges30To44Days
        {
            get { return this._amountOfCharges30To44Days; }
            set
            {
                if (this._amountOfCharges30To44Days == value) { return; }

                this._amountOfCharges30To44Days = value;
                this.NotifyOfPropertyChange(() => this.AmountOfCharges30To44Days);
            }
        }
        private decimal _amountOfCharges30To44Days = 0M;

        public int NumberOfChargesGT45Days
        {
            get { return this._numberOfChargesGT45Days; }

            set
            {
                if (this._numberOfChargesGT45Days == value) { return; }

                this._numberOfChargesGT45Days = value;
                this.NotifyOfPropertyChange(() => this.NumberOfChargesGT45Days);
            }
        }
        private int _numberOfChargesGT45Days = 0;

        public decimal AmountOfChargesGT45Days
        {
            get { return this._amountOfChargesGT45Days; }

            set
            {
                if (this._amountOfChargesGT45Days == value) { return; }

                this._amountOfChargesGT45Days = value;
                this.NotifyOfPropertyChange(() => this.AmountOfChargesGT45Days);
            }
        }
        private decimal _amountOfChargesGT45Days = 0M;

        public DateTime BeginDateOfCharges
        {
            get { return this._beginDateOfCharges; }
            set
            {
                if (this._beginDateOfCharges == value) { return; }

                this._beginDateOfCharges = value;
                this.NotifyOfPropertyChange(() => this.BeginDateOfCharges);
            }
        }
        private DateTime _beginDateOfCharges;

        public DateTime EndDateOfCharges
        {
            get { return this._endDateOfCharges; }
            set
            {
                if (this._endDateOfCharges == value) { return; }

                this._endDateOfCharges = value;
                this.NotifyOfPropertyChange(() => this.EndDateOfCharges);
            }
        }
        private DateTime _endDateOfCharges;

        private int _numberOfSavedReports = 0;
        public int NumberOfSavedReports
        {
            get
            { return _numberOfSavedReports; }

            set
            {
                if (_numberOfSavedReports == value)
                { return; }

                _numberOfSavedReports = value;
                this.NotifyOfPropertyChange(() => this.NumberOfSavedReports);
            }
        }

        private decimal _amountOfSavedReports = 0M;
        public decimal AmountOfSavedReports
        {
            get
            { return _amountOfSavedReports; }

            set
            {
                if (_amountOfSavedReports == value)
                { return; }

                _amountOfSavedReports = value;
                this.NotifyOfPropertyChange(() => this.AmountOfSavedReports);
            }
        }

        private int _numberOfPendingReports = 0;
        public int NumberOfPendingReports
        {
            get
            { return _numberOfPendingReports; }

            set
            {
                if (_numberOfPendingReports == value)
                { return; }

                _numberOfPendingReports = value;
                this.NotifyOfPropertyChange(() => this.NumberOfPendingReports);
            }
        }

        private decimal _amountOfPendingReports = 0M;
        public decimal AmountOfPendingReports
        {
            get
            { return _amountOfPendingReports; }

            set
            {
                if (_amountOfPendingReports == value)
                { return; }

                _amountOfPendingReports = value;
                this.NotifyOfPropertyChange(() => this.AmountOfPendingReports);
            }
        }

        private int _numberOfReportsNeedingApproval = 0;
        public int NumberOfReportsNeedingApproval
        {
            get
            { return _numberOfReportsNeedingApproval; }

            set
            {
                if (_numberOfReportsNeedingApproval == value)
                { return; }

                _numberOfReportsNeedingApproval = value;
                this.NotifyOfPropertyChange(() => this.NumberOfReportsNeedingApproval);
            }
        }

        private decimal _amountOfReportsNeedingApproval = 0M;
        public decimal AmountOfReportsNeedingApproval
        {
            get
            { return _amountOfReportsNeedingApproval; }

            set
            {
                if (_amountOfReportsNeedingApproval == value)
                { return; }

                _amountOfReportsNeedingApproval = value;
                this.NotifyOfPropertyChange(() => this.AmountOfReportsNeedingApproval);
            }
        }


        #endregion

        private SortType _sortType = SortType.ItemType;
        private ObservableCollection<GroupInfoList<object>> _groupedSummaryItems;
        private ObservableCollection<SummaryItemViewModel> _summaryItems;
        decimal _amountOwedToCreditCard = 0M;
        decimal _amountOwedToEmployee = 0M;
        private readonly IExpenseRepository _repository;
        private readonly EmployeeViewModel _employeeViewModel;

        private SummaryItemsViewModel() { }

        public SummaryItemsViewModel(EmployeeViewModel employeeViewModel)
        {
            this._employeeViewModel = employeeViewModel;
            this._repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this.GroupedSummaryItems = new ObservableCollection<GroupInfoList<object>>();
            this.SortTypes = new ObservableCollection<SortType>();
            this.SortTypes.Add(SortType.Age);
            this.SortTypes.Add(SortType.Amount);
        }

        public async Task<ObservableCollection<SummaryItemViewModel>> GetSummaryItems()
        {
            List<SummaryItem> items = await this._repository.GetSummaryItemsAsync(this._employeeViewModel.EmployeeId);

            var summaryItems = from item in items
                               select new SummaryItemViewModel()
                               {
                                   Id = item.Id,
                                   Amount = item.Amount,
                                   Date = Convert.ToDateTime(item.Date),
                                   Description = item.Description,
                                   Submitter = item.Submitter,
                                   ItemType = item.ItemType,
                               };
            this.SummaryItems = new ObservableCollection<SummaryItemViewModel>(summaryItems);

            this.NumberOfCharges =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge
                 select item).Count();
            this.AmountOfCharges =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge
                 select Convert.ToDecimal(item.Amount)).Sum();
            this.NumberOfChargesLT30Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days < 30
                 select item).Count();
            this.AmountOfChargesLT30Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days < 30
                 select Convert.ToDecimal(item.Amount)).Sum();
            this.NumberOfCharges30To44Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days >= 30
                 && DateTime.Today.Subtract(item.Date).Days < 45
                 select item).Count();
            this.AmountOfCharges30To44Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days >= 30
                 && DateTime.Today.Subtract(item.Date).Days < 45
                 select Convert.ToDecimal(item.Amount)).Sum();
            this.NumberOfChargesGT45Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days >= 45
                 select item).Count();
            this.AmountOfChargesGT45Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days >= 45
                 select Convert.ToDecimal(item.Amount)).Sum();

            if (this.NumberOfCharges > 0)
            {
                this.BeginDateOfCharges = (from item in this.SummaryItems
                                      where item.ItemType == ItemType.Charge
                                      select item.Date).Min();
                this.EndDateOfCharges = (from item in this.SummaryItems
                                    where item.ItemType == ItemType.Charge
                                    select item.Date).Max();
            }

            this.NumberOfSavedReports = (from item in this.SummaryItems
                                    where item.ItemType == ItemType.SavedReport
                                    select item).Count();
            this.AmountOfSavedReports = Convert.ToDecimal(
                (from item in summaryItems
                 where item.ItemType == ItemType.SavedReport
                 select item.Amount).Sum());

            this.NumberOfPendingReports = (from item in this.SummaryItems
                                      where item.ItemType == ItemType.PendingReport
                                      select item).Count();
            this.AmountOfPendingReports = Convert.ToDecimal(
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.PendingReport
                 select item.Amount).Sum());

            var expenseReportsViewModel = new ExpenseReportsViewModel();
            this._amountOwedToCreditCard = await this._repository.GetAmountOwedToCreditCardAsync(this._employeeViewModel.EmployeeId);
            this._amountOwedToEmployee = await this._repository.GetAmountOwedToEmployeeAsync(this._employeeViewModel.EmployeeId);

            this.NumberOfReportsNeedingApproval = (from item in this.SummaryItems
                                              where item.ItemType == ItemType.UnresolvedReport
                                              select item).Count();
            this.AmountOfReportsNeedingApproval = Convert.ToDecimal(
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.UnresolvedReport
                 select item.Amount).Sum());

            GroupItems();

            return this.SummaryItems;
        }

        public void GroupItems()
        {
            switch (this.SortType)
            {
                case SortType.Age:
                    GroupItemsByTypeAndAge();
                    break;
                case SortType.Amount:
                    GroupItemsByTypeAndAmount();
                    break;
                case SortType.Submitter:
                    GroupItemsBySubmitter();
                    break;
                case SortType.ItemType:
                    GroupItemsByTypeAndAge();
                    break;
                default:
                    break;
            }
        }

        private void GroupItemsByTypeAndAge()
        {
            var query = from item in this.SummaryItems
                        orderby item.ItemType, item.Date ascending
                        group item by item.ItemType into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            this.GroupedSummaryItems.Clear();
            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                string itemTypeName = null;
                ItemType itemType = (ItemType)group.GroupName;
                switch (itemType)
                {
                    case ItemType.Charge:
                        itemTypeName = "Charges";

                        string beginMonth = this.BeginDateOfCharges.ToString("M").Substring(0, 3);
                        int beginDay = this.BeginDateOfCharges.Day;
                        int beginYear = this.BeginDateOfCharges.Year;
                        string endMonth = this.EndDateOfCharges.ToString("M").Substring(0, 3);
                        int endDay = this.EndDateOfCharges.Day;
                        int endYear = this.EndDateOfCharges.Year;

                        if (beginYear == endYear)
                        {
                            items.Details1 =
                                string.Format("{0} {1} - {2} {3}, {4}",
                                beginMonth, beginDay, endMonth, endDay, endYear);
                        }
                        else
                        {
                            items.Details1 =
                                string.Format("{0} {1}, {2} - {3} {4}, {5}",
                                beginMonth, beginDay, beginYear, endMonth, endDay, endYear);
                        }

                        items.Details2 = string.Format("{0} new for {1:C}",
                            this.NumberOfChargesLT30Days.ToString(),
                            this.AmountOfChargesLT30Days);
                        items.Details3 = string.Format("{0} old for {1:C}",
                            this.NumberOfCharges30To44Days.ToString(),
                            this.AmountOfCharges30To44Days);
                        items.Details4 = string.Format("{0} late for {1:C}",
                            this.NumberOfChargesGT45Days.ToString(),
                            this.AmountOfChargesGT45Days);
                        break;
                    case ItemType.SavedReport:
                        itemTypeName = "Saved Reports";
                        break;
                    case ItemType.PendingReport:
                        itemTypeName = "Pending Reports";
                        items.Details1 = string.Format("{0:C} owed to employee",
                            this._amountOwedToEmployee);
                        items.Details2 = string.Format("{0:C} owed to card",
                            this._amountOwedToCreditCard);
                        break;
                    case ItemType.UnresolvedReport:
                        itemTypeName = "Needing Approval";
                        break;
                    case ItemType.ApprovedReport:
                        itemTypeName = "Past Reports";
                        break;
                    default:
                        break;
                }
                items.Key = itemTypeName;
                items.Amount = System.Convert.ToDecimal(group.Amount);
                if (group.Amount == 0)
                {
                    items.Summary = "You have none";
                    items.Details1 = string.Empty;
                    items.Details2 = string.Empty;
                    items.Details3 = string.Empty;
                    items.Details4 = string.Empty;
                }
                else
                {
                    items.Summary = string.Format("{0} for {1:C}",
                        group.Count, items.Amount);
                }
                items.ImportList(group.Items.ToArray());
                if (!(itemTypeName == "Needing Approval" && !this._employeeViewModel.IsManager))
                {
                    this.GroupedSummaryItems.Add(items);
                }
            }
        }

        internal GroupInfoList<object> GroupItemsBySubmitter()
        {
            var query = from item in this.SummaryItems
                        orderby item.Submitter
                        group item by item.Submitter into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                if (group.Count > 0)
                {
                    items.Key = group.GroupName;
                    items.Summary = string.Format("{0} for {1:C}",
                        items.Count, items.Amount);
                }
                else
                {
                    items.Key = group.GroupName;
                }
                items.Amount = System.Convert.ToDecimal(group.Amount);
                foreach (object item in items)
                {
                    items.Add(item);
                }
                groups.Add(items);
            }

            return groups;
        }


        private void GroupItemsByTypeAndAmount()
        {
            var query = from item in SummaryItems
                        orderby item.ItemType, item.Amount descending
                        group item by item.ItemType into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            this.GroupedSummaryItems.Clear();
            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                string itemTypeName = null;
                ItemType itemType = (ItemType)group.GroupName;
                switch (itemType)
                {
                    case ItemType.Charge:
                        itemTypeName = "Charges";
                        break;
                    case ItemType.SavedReport:
                        itemTypeName = "Saved Reports";
                        break;
                    case ItemType.PendingReport:
                        itemTypeName = "Pending Reports";
                        break;
                    case ItemType.UnresolvedReport:
                        itemTypeName = "Needing Approval";
                        break;
                    default:
                        break;
                }
                items.Key = string.Format(
                    "{0} ({1:C})", itemTypeName, group.Amount);
                items.Amount = System.Convert.ToDecimal(group.Amount);
                items.ImportList(group.Items.ToArray());
                this.GroupedSummaryItems.Add(items);
            }
        }

    }
}
