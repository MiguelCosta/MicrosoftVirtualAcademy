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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LobCampPhone.Resources;
using Expenses.Model;
using Expenses.ViewModel;

namespace LobCampPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // 1. Get our service authorizer from the ServiceLocator.
            Wp8ServiceAuthorizer wp8ServiceAuthorizer =
                ServiceLocator.Current.GetService<Wp8ServiceAuthorizer>();

            this.Loaded +=
                (_, __) =>
                {
                    // 2. Once the page loads, navigate the WebBrowser to the login.
                    this._webBrowser.Navigate(wp8ServiceAuthorizer.LoginUri);
                };

            this._webBrowser.Navigating +=
                (_, e) =>
                {
                    // 3. As the user navigates through the login process, we'll have 
                    // the Wp8ServiceAuthorizer look for the code needed to complete 
                    // the process.
                    wp8ServiceAuthorizer.ProcessUri(e.Uri);
                };

            wp8ServiceAuthorizer.TokenReceived +=
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
                    PhoneApplicationService.Current.State["charges"] =
                     await repository.GetOutstandingChargesAsync(employee.EmployeeId);

                    // 8. Bind the list to our UI.
                    this._chargesListBox.ItemsSource =
                     PhoneApplicationService.Current.State["charges"] as List<Charge>;
                };

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string chargeId = (sender as Button).CommandParameter.ToString();

            this.NavigationService.Navigate(
                new Uri(
                    string.Format("/EditChargePage.xaml?chargeId={0}", chargeId),
                    UriKind.Relative));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}