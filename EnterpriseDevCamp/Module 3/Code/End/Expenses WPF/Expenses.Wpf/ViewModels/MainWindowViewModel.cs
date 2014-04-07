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
using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Expenses.Wpf
{
    public class MainWindowViewModel : ViewModelBase
    {
        public readonly IExpenseRepository Repository;

        public ViewModelBase CurrentViewModel
        {
            get { return this._currentViewModel; }
            set
            {
                if (this._currentViewModel == value) { return; }

                this._currentViewModel = value;
                this.NotifyOfPropertyChange(() => this.CurrentViewModel);
            }
        }
        private ViewModelBase _currentViewModel;

        public LogInControlViewModel LogInViewModel
        {
            get { return this._logInViewModel; }
            set
            {
                if (this._logInViewModel == value) { return; }

                this._logInViewModel = value;
                this.NotifyOfPropertyChange(() => this.LogInViewModel);
            }
        }
        private LogInControlViewModel _logInViewModel;

        public EmployeeViewModel EmployeeViewModel { get; set; }

        public bool ShowLogin
        {
            get { return this._showLogin; }
            set
            {
                if (this._showLogin == value) { return; }

                this._showLogin = value;
                this.NotifyOfPropertyChange(() => this.ShowLogin);
            }
        }
        private bool _showLogin;

        public readonly IViewService ViewService;

        public readonly NavigationService NavigationService;

        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        async Task OnRequestChangeUser()
        {
            string alias = (this.EmployeeViewModel.Alias == this.LogInViewModel.Alias) ? this.EmployeeViewModel.Manager : this.LogInViewModel.Alias;

            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.EmployeeViewModel.Employee = await this.Repository.GetEmployeeAsync(alias);
                    await this.ShowSummaryViewAsync();
                });
        }

        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
            set
            {
                this._isBusy = value;
                this.NotifyOfPropertyChange(() => this.IsBusy);
            }
        }
        private bool _isBusy;

        public ICommand CloseCommand
        {
            get
            {
                if (this._closeCommand == null)
                {
                    this._closeCommand = new RelayCommand(param => this.OnRequestClose());
                }
                return this._closeCommand;
            }
        }
        private ICommand _closeCommand;

        public ICommand ChangeUserCommand
        {
            get
            {
                if (this._changeUserCommand == null)
                {
                    this._changeUserCommand = new RelayCommand(async (_) => { await this.OnRequestChangeUser(); });
                }
                return this._changeUserCommand;
            }
        }
        private ICommand _changeUserCommand;

        public ICommand ShowChargesCommand { get; private set; }
        public ICommand NewChargeCommand { get; private set; }
        public ICommand ShowSavedReportsCommand { get; private set; }
        public ICommand ShowPendingReportsCommand { get; private set; }
        public ICommand ShowPastReportsCommand { get; private set; }
        public ICommand ResetDataCommand { get; private set; }
        public ICommand NewReportCommand { get; private set; }
        public ICommand ApproveReportsCommand { get; private set; }
        public ICommand SummaryViewCommand { get; private set; }

        public MainWindowViewModel()
        {
            // TODO: There might be a better way to do this.
            this.NavigationService = ServiceLocator.Current.GetService<INavigationService>() as NavigationService;
            this.Repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();
            this.ViewService.BusyChanged += ViewService_BusyChanged;
            this.EmployeeViewModel = ServiceLocator.Current.GetService<EmployeeViewModel>();

            this.LogInViewModel = new LogInControlViewModel(this, new RelayCommand(async (_) => await this.LogInAsync()));
            //NOTE: Automatically logging in hardcoded alias for now
            this.LogInViewModel.LogInCommand.Execute(this);

            this.NewChargeCommand = new RelayCommand((_) => this.NewCharge());
            this.ShowChargesCommand = new RelayCommand(async (_) => await this.ShowChargesAsync());
            this.ShowSavedReportsCommand = new RelayCommand(async (_) => await this.ShowSavedReportsAsync());
            this.ShowPendingReportsCommand = new RelayCommand(async (_) => await this.ShowPendingReportsAsync());
            this.ShowPastReportsCommand = new RelayCommand(async (_) => await this.ShowPastReportsAsync());
            this.ResetDataCommand = new RelayCommand(async (_) => await this.ResetDataAsync());
            this.NewReportCommand = new RelayCommand((_) => this.NewReport());
            this.ApproveReportsCommand = new RelayCommand(async (_) => await this.ShowReportsForApprovalAsync());
            this.SummaryViewCommand = new RelayCommand(async (_) => await this.ShowSummaryViewAsync());

            this.NavigationService.ShowChargeRequested += (_, ea) => { this.ShowCharge(ea.Data); };
            this.NavigationService.ShowChargesRequested += async (_, __) => { await this.ShowChargesAsync(); };
            this.NavigationService.ShowExpenseReportRequested += (_, ea) => { this.ShowExpenseReportAsync(ea.Data); };
            this.NavigationService.ShowPastExpenseReportsRequested += async (_, __) => { await this.ShowPastReportsAsync(); };
            this.NavigationService.ShowPendingExpenseReportsRequested += async (_, __) => { await this.ShowPendingReportsAsync(); };
            this.NavigationService.ShowSavedExpenseReportsRequested += async (_, __) => { await this.ShowSavedReportsAsync(); };

            //this.ShowLogin = true;
        }

        void ViewService_BusyChanged(object sender, EventArgs<bool> e)
        {
            this.IsBusy = e.Data;
        }

        public void ShowCharge(ChargeViewModel charge)
        {
            this.CurrentViewModel = charge;
        }

        public void NewCharge()
        {
            this.ShowCharge(
                new ChargeViewModel()
                {
                    EmployeeId = this.EmployeeViewModel.EmployeeId,
                });
        }

        public void NewReport()
        {
            ExpenseReportViewModel reportVM = new ExpenseReportViewModel()
            {
                Approver = this.EmployeeViewModel.Manager,
                EmployeeId = this.EmployeeViewModel.EmployeeId,
                ExpenseReportId = 0,
            };

            this.ShowExpenseReportAsync(reportVM);
        }

        public async Task LogInAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.EmployeeViewModel.Employee = await this.Repository.GetEmployeeAsync(this.LogInViewModel.Alias);
                    await this.ShowSummaryViewAsync();
                    this.ShowLogin = string.IsNullOrWhiteSpace(this.EmployeeViewModel.Alias);
                });
        }

        public async void ShowExpenseReportAsync(ExpenseReportViewModel expenseReportViewModel)
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    var editReportVM = new EditExpenseReportViewModel();
                    editReportVM.ExpenseReport = expenseReportViewModel;

                    AddChargesViewModel addChargesVM = new AddChargesViewModel();
                    await addChargesVM.LoadChargesAsync();
                    editReportVM.AddCharges = addChargesVM;

                    ExpenseReportChargesViewModel associatedChargesVM = new ExpenseReportChargesViewModel();
                    await associatedChargesVM.LoadChargesAsync(expenseReportViewModel.ExpenseReportId);
                    editReportVM.AssociatedCharges = associatedChargesVM;

                    this.CurrentViewModel = editReportVM;
                });
        }

        public async Task ShowReportsForApprovalAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    ApproveExpenseReportsViewModel approveViewModel = new ApproveExpenseReportsViewModel();
                    await approveViewModel.LoadReportsForApprovalAsync();
                    this.CurrentViewModel = approveViewModel;
                });
        }

        public async Task ShowChargesAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    ChargesViewModel chargesViewModel = new ChargesViewModel();
                    await chargesViewModel.LoadChargesAsync();
                    this.CurrentViewModel = chargesViewModel;
                });
        }

        public async Task ShowSummaryViewAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    SummaryItemsViewModel summaryViewModel = new SummaryItemsViewModel(this.EmployeeViewModel);
                    await summaryViewModel.GetSummaryItems();
                    this.CurrentViewModel = summaryViewModel;
                });
        }

        public async Task ShowSavedReportsAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    ExpenseReportsViewModel expenseReportsViewModel = new ExpenseReportsViewModel();
                    await expenseReportsViewModel.LoadSavedExpenseReportsAsync();
                    this.CurrentViewModel = expenseReportsViewModel;
                });
        }

        public async Task ShowPendingReportsAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    ExpenseReportsViewModel expenseReportsViewModel = new ExpenseReportsViewModel();
                    await expenseReportsViewModel.LoadPendingExpenseReportsAsync();
                    this.CurrentViewModel = expenseReportsViewModel;
                });
        }

        public async Task ShowPastReportsAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    ExpenseReportsViewModel expenseReportsViewModel = new ExpenseReportsViewModel();
                    await expenseReportsViewModel.LoadApprovedExpenseReportsAsync();
                    this.CurrentViewModel = expenseReportsViewModel;
                });
        }

        public async Task ResetDataAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    await this.Repository.ResetDataAsync();
                    await this.LogInAsync();
                    await this.ShowChargesAsync();
                });
        }
    }
}
