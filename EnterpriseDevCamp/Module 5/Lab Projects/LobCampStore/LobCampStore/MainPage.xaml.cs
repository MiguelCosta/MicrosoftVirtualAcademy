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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Expenses.Model;
using Expenses.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LobCampStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<Charge> _charges;
        public MainPage()
        {
            this.InitializeComponent();

            // 1. Get our service authorizer from the ServiceLocator.
            WinRtServiceAuthorizer winRtServiceAuthorizer =
                ServiceLocator.Current.GetService<WinRtServiceAuthorizer>();

            this.Loaded +=
                (_, __) =>
                {
                    // 2. Once the page loads, navigate the WebBrowser to the login.
                    this._webBrowser.Navigate(winRtServiceAuthorizer.LoginUri);
                };

            this._webBrowser.NavigationStarting +=
                (_, e) =>
                {
                    // 3. As the user navigates through the login process, we'll have 
                    // the WinRtServiceAuthorizer look for the code needed to complete 
                    // the process.
                    winRtServiceAuthorizer.ProcessUri(e.Uri);
                };

            winRtServiceAuthorizer.TokenReceived +=
                async (_, __) =>
                {

                    // 4. When we complete the token process, hide the WebBrowser.
                    this._webBrowser.Visibility = Visibility.Collapsed;

                    // 5. Next, we'll get our repository.
                    IExpenseRepository repository =
                        ServiceLocator.Current.GetService<IExpenseRepository>();

                    // 6. Make a request for the "rogreen" user.
                    Employee employee = await repository.GetEmployeeAsync("rogreen");

                    // 7. Get the outstanding charges and save them in the app state.
                    this._charges =
                     await repository.GetOutstandingChargesAsync(employee.EmployeeId);

                    // 8. Bind the list to our UI.
                    this._chargesListBox.ItemsSource = this._charges;
                };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string chargeId = (sender as Button).CommandParameter.ToString();

            Charge charge =
                this._charges.First(item => item.ChargeId.ToString() == chargeId);

            this.Frame.Navigate(typeof(EditChargePage), charge);
        }
    }
}
