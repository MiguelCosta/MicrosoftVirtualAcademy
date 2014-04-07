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
using System.Windows.Input;

namespace Expenses.WindowsStore
{
    public class MainWindowViewModel : ViewModelBase
    {
        public readonly IExpenseRepository Repository;

        const string Alias = "rogreen";

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

        public LoginViewModel LoginViewModel
        {
            get { return this._loginViewModel; }
            set
            {
                this._loginViewModel = value;
                this.NotifyOfPropertyChange(() => this.LoginViewModel);
            }
        }
        private LoginViewModel _loginViewModel;

        public EmployeeViewModel EmployeeViewModel { get; set; }

        public bool ShowLogin
        {
            get { return this._showLogin; }
            set
            {
                this._showLogin = value;
                this.NotifyOfPropertyChange(() => this.ShowLogin);
            }
        }
        private bool _showLogin;

        public bool LoggedIn
        {
            get { return this._loggedIn; }
            set
            {
                this._loggedIn = value;
                this.ShowLogin = !this._loggedIn;
                this.NotifyOfPropertyChange(() => this.LoggedIn);
            }
        }
        private bool _loggedIn;

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

        public ICommand ShowChargesCommand { get; private set; }
        public ICommand NewChargeCommand { get; private set; }
        public ICommand ShowSavedReportsCommand { get; private set; }
        public ICommand ShowPendingReportsCommand { get; private set; }
        public ICommand ShowPastReportsCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand NewReportCommand { get; private set; }
        public ICommand ApproveReportsCommand { get; private set; }

        public WinRtServiceAuthorizer ServiceAuthorizer
        {
            get { return this._serviceAuthorizer; }
            set
            {
                this._serviceAuthorizer = value;
                this.NotifyOfPropertyChange(() => this.ServiceAuthorizer);
            }
        }
        private WinRtServiceAuthorizer _serviceAuthorizer;

        public MainWindowViewModel()
        {
            // TODO: There might be a better way to do this.
            this.NavigationService = ServiceLocator.Current.GetService<INavigationService>() as NavigationService;
            this.Repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this.ServiceAuthorizer = ServiceLocator.Current.GetService<WinRtServiceAuthorizer>();
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();
            this.ViewService.BusyChanged += ViewService_BusyChanged;
            this.EmployeeViewModel = ServiceLocator.Current.GetService<EmployeeViewModel>();
            
            this.NewChargeCommand = new RelayCommand((_) => this.NewCharge());
            this.ShowChargesCommand = new RelayCommand(async (_) => await this.ShowCharges());
            this.ShowSavedReportsCommand = new RelayCommand(async (_) => await this.ShowSavedReports());
            this.ShowPendingReportsCommand = new RelayCommand(async (_) => await this.ShowPendingReports());
            this.ShowPastReportsCommand = new RelayCommand((_) => this.ShowPastReports());
            this.ResetCommand = new RelayCommand(async (_) => await this.ResetUserData());
            this.NewReportCommand = new RelayCommand((_) => this.NewReport());
            this.ApproveReportsCommand = new RelayCommand(async (_) => await this.ShowReportsForApproval());

            this.NavigationService.ShowChargesRequested += async (_, __) => { await this.ShowCharges(); };
            this.NavigationService.ShowExpenseReportRequested += (_, ea) => { this.ShowExpenseReport(ea.Data); };
            this.NavigationService.ShowPendingExpenseReportsRequested += async (_, __) => { await this.ShowPendingReports(); };
            this.NavigationService.ShowSavedExpenseReportsRequested += async (_, __) => { await this.ShowSavedReports(); };

            this.LoggedIn = false;
            this.LoginViewModel = new LoginViewModel(ServiceLocator.Current.GetService<WinRtServiceAuthorizer>(), new RelayCommand(async (_) => await this.LogInAsync()));
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

            this.ShowExpenseReport(reportVM);
        }
        
        public async Task LogInAsync()
        {
            try
            {
                this.IsBusy = true;
                this.EmployeeViewModel.Employee = await this.Repository.GetEmployeeAsync(MainWindowViewModel.Alias);
                await this.ShowSummaryAsync();
                this.LoggedIn = !string.IsNullOrWhiteSpace(this.EmployeeViewModel.Alias);
            }
            catch (Exception e)
            {
                this.ViewService.ShowError(e.Message);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public void ShowExpenseReport(ExpenseReportViewModel expenseReportViewModel)
        {
            //var editReportVM = new EditExpenseReportViewModel();
            //editReportVM.ExpenseReport = expenseReportViewModel;

            //AddChargesViewModel addChargesVM = new AddChargesViewModel();
            //addChargesVM.LoadChargesAsync();
            //editReportVM.AddCharges = addChargesVM;

            //ExpenseReportChargesViewModel associatedChargesVM = new ExpenseReportChargesViewModel();
            //associatedChargesVM.LoadChargesAsync(expenseReportViewModel.ExpenseReportId);
            //editReportVM.AssociatedCharges = associatedChargesVM;

            //this.CurrentViewModel = editReportVM;
        }

        public async Task ShowReportsForApproval()
        {
            ApproveExpenseReportsViewModel approveViewModel = new ApproveExpenseReportsViewModel();
            await approveViewModel.LoadReportsForApprovalAsync();
            this.CurrentViewModel = approveViewModel;
        }

        public async Task ShowCharges()
        {
            ChargesViewModel chargesViewModel = new ChargesViewModel();
            await chargesViewModel.LoadChargesAsync();
            this.CurrentViewModel = chargesViewModel;
        }

        public async Task ShowSavedReports()
        {
            ExpenseReportsViewModel expenseReportsViewModel = new ExpenseReportsViewModel();
            await expenseReportsViewModel.LoadSavedExpenseReportsAsync();
            this.CurrentViewModel = expenseReportsViewModel;
        }

        public async Task ShowPendingReports()
        {
            ExpenseReportsViewModel expenseReportsViewModel = new ExpenseReportsViewModel();
            await expenseReportsViewModel.LoadPendingExpenseReportsAsync();
            this.CurrentViewModel = expenseReportsViewModel;
        }

        public void ShowPastReports()
        {
            this.NavigationService.ShowPastExpenseReports();
        }

        public async Task ShowSummaryAsync()
        {
            SummaryViewModel summaryViewModel = new SummaryViewModel(this.EmployeeViewModel);
            await summaryViewModel.LoadAsync();
            this.CurrentViewModel = summaryViewModel;
        }

        public async Task ResetUserData()
        {
            await Repository.ResetDataAsync();
        }
    }
}
