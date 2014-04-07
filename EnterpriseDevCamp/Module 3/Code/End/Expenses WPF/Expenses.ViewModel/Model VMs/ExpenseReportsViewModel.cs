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
using Expenses.Model;
using System.Windows.Input;

namespace Expenses.ViewModel
{
    public class ExpenseReportsViewModel : ViewModelBase
    {
        public ObservableCollection<SortType> SortTypes { get; set; }

        public ObservableCollection<GroupInfoList<object>> GroupedExpenseReports
        {
            get { return this._groupedExpenseReports; }
            set
            {
                this._groupedExpenseReports = value;
                this.NotifyOfPropertyChange(() => this.GroupedExpenseReports);
            }
        }
        private ObservableCollection<GroupInfoList<object>> _groupedExpenseReports;

        public ObservableCollection<ExpenseReportViewModel> ExpenseReports
        {
            get { return this._expenseReports; }
            set
            {
                this._expenseReports = value;
                this.NotifyOfPropertyChange(() => this.ExpenseReports);
            }
        }
        private ObservableCollection<ExpenseReportViewModel> _expenseReports;

        public SortType SortType
        {
            get { return this._sortType; }
            set
            {
                if (this._sortType == value) { return; }

                _sortType = value;
                this.NotifyOfPropertyChange(() => this.SortType);

                //update the grouping because the sort/filter type changed
                this.GroupExpenseReports();
            }
        }
        private SortType _sortType = SortType.ItemType;

        public readonly IExpenseRepository _repository;
        public readonly INavigationService NavigationService;
        public readonly EmployeeViewModel EmployeeViewModel;
        private readonly IViewService ViewService;

        public ICommand ViewReportCommand { get; private set; }
        

        public ExpenseReportsViewModel()
        {
            this.EmployeeViewModel = ServiceLocator.Current.GetService<EmployeeViewModel>();
            this.NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this._repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();

            this._expenseReports = new ObservableCollection<ExpenseReportViewModel>();
            this._groupedExpenseReports = new ObservableCollection<GroupInfoList<object>>();
            this.SortTypes = new ObservableCollection<SortType>();
            this.SortTypes.Add(SortType.Age);
            this.SortTypes.Add(SortType.Amount);

            this.ViewReportCommand = new RelayCommand(
                (report) =>
                {
                    this.ViewReport(report as ExpenseReportViewModel);
                });
        }

        private void ViewReport(ExpenseReportViewModel reportViewModel)
        {
            this.NavigationService.ShowExpenseReport(reportViewModel);   
        }

        protected void Load(IEnumerable<ExpenseReport> expenseReports)
        {
            this.ExpenseReports.Clear();
            foreach(ExpenseReport expenseReport in expenseReports)
            {
                this.ExpenseReports.Add(new ExpenseReportViewModel(expenseReport));
            }
        }

        public async Task LoadAllExpenseReportsAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Load(await this._repository.GetExpenseReportsAsync(this.EmployeeViewModel.EmployeeId));
                });
        }

        public async Task LoadSavedExpenseReportsAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Load(await this._repository.GetExpenseReportsByStatusAsync(this.EmployeeViewModel.EmployeeId, ExpenseReportStatus.Saved));
                });
        }

        public async Task LoadPendingExpenseReportsAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Load(await this._repository.GetExpenseReportsByStatusAsync(this.EmployeeViewModel.EmployeeId, ExpenseReportStatus.Pending));
                });
        }

        public async Task LoadApprovedExpenseReportsAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Load(await this._repository.GetExpenseReportsByStatusAsync(this.EmployeeViewModel.EmployeeId, ExpenseReportStatus.Approved));
                    GroupApprovedExpenseReportsByAge();
                });
        }

        public void GroupExpenseReports()
        {
            switch (this.SortType)
            {
                case SortType.Category:
                    GroupExpenseReportsByCategory();
                    break;
                case SortType.Age:
                    GroupExpenseReportsByAge();
                    break;
                case SortType.Amount:
                    GroupExpenseReportsByAmount();
                    break;
                case SortType.Submitter:
                    GroupExpenseReportsBySubmitter();
                    break;
                case SortType.ItemType:
                    GroupExpenseReportsByAge();
                    break;
                default:
                    break;
            }
        }

        internal void GroupApprovedExpenseReportsByAge()
        {
            var query = from expenseReport in this.ExpenseReports
                        orderby expenseReport.DateResolved descending
                        group expenseReport by ((DateTime)expenseReport.DateResolved).Year into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            this.GroupedExpenseReports.Clear();
            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                items.Key = group.GroupName;
                items.Amount = System.Convert.ToDecimal(group.Amount);
                items.Summary = string.Format("{0} for {1:C}",
                    group.Count, items.Amount);
                items.ImportList(group.Items.ToArray());
                this.GroupedExpenseReports.Add(items);
            }
        }

        internal GroupInfoList<object> GroupExpenseReportsByCategory()
        {
            var query = from expenseReport in ExpenseReports
                        orderby expenseReport.Status
                        group expenseReport by expenseReport.Status into g
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
                var groupedExpenses = new GroupInfoList<object>();
                string statusTypeName = null;
                ExpenseReportStatus statusType = (ExpenseReportStatus)group.GroupName;
                switch (statusType)
                {
                    case ExpenseReportStatus.Approved:
                        statusTypeName = "Approved";
                        break;
                    case ExpenseReportStatus.Pending:
                        statusTypeName = "Pending";
                        break;
                    case ExpenseReportStatus.Saved:
                        statusTypeName = "Saved";
                        break;
                    default:
                        break;
                }
                groupedExpenses.Key = string.Format(
                    "{0} ({1} items for {2:C})", statusTypeName, group.Count, group.Amount);
                groupedExpenses.Amount = System.Convert.ToDecimal(group.Amount);
                foreach (var item in groupedExpenses)
                {
                    groupedExpenses.Add(item);
                }
                groups.Add(groupedExpenses);
            }

            return groups;
        }

        internal void GroupExpenseReportsByAge()
        {
            var query = from expenseReport in ExpenseReports
                        orderby expenseReport.DateResolved descending
                        group expenseReport by ((DateTime)expenseReport.DateResolved).Year into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            this.GroupedExpenseReports.Clear();
            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                items.Key = group.GroupName;
                items.Amount = System.Convert.ToDecimal(group.Amount);
                items.Summary = string.Format("{0} for {1:C}",
                    group.Count, items.Amount);
                items.ImportList(group.Items.ToArray());
                this.GroupedExpenseReports.Add(items);
            }
        }

        internal void GroupExpenseReportsByAmount()
        {
        }

        internal GroupInfoList<object> GroupExpenseReportsBySubmitter()
        {
            var query = from expenseReport in ExpenseReports
                        orderby expenseReport.Status
                        group expenseReport by expenseReport.EmployeeId into g
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
                var groupedExpenses = new GroupInfoList<object>();
                groupedExpenses.Key = string.Format(
                    "{0} ({1} items for {2:C})", group.GroupName, group.Count, group.Amount);
                groupedExpenses.Amount = System.Convert.ToDecimal(group.Amount);
                foreach (var item in groupedExpenses)
                {
                    groupedExpenses.Add(item);
                }
                groups.Add(groupedExpenses);
            }

            return groups;
        }

    }

}
