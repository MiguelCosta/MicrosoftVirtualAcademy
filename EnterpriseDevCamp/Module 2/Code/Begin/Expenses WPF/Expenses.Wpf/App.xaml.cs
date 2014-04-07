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

using Expenses.ExpenseRepository.WcfClient;
using Expenses.Model;
using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Expenses.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();

            string url = ConfigurationManager.AppSettings["expenseServiceUrl"];

            ServiceLocator.Current.SetService<IExpenseRepository>(new WcfClientExpenseRepository(url));
            ServiceLocator.Current.SetService<IViewService>(new ViewService());
            ServiceLocator.Current.SetService<INavigationService>(new NavigationService());
            ServiceLocator.Current.SetService<EmployeeViewModel>(new EmployeeViewModel());

            MainWindowViewModel mainViewModel = new MainWindowViewModel();
            mainWindow.DataContext = mainViewModel;

            EventHandler handler = null;
            handler = delegate
            {
                mainViewModel.RequestClose -= handler;
                mainWindow.Close();
            };
            mainViewModel.RequestClose += handler;

            mainViewModel.ViewService.BusyChanged +=
                (_, ea) =>
                {
                    mainWindow.Cursor = (ea.Data) ? Cursors.Wait : Cursors.Arrow;
                };

            mainWindow.Show();
        }
    }
}
