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
using Expenses.Model;
using Expenses.ViewModel;

namespace LobCampPhone
{
    public partial class EditChargePage : PhoneApplicationPage
    {
        public EditChargePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string chargeId = this.NavigationContext.QueryString["chargeId"];
            List<Charge> charges =
                 PhoneApplicationService.Current.State["charges"] as List<Charge>;
            this.DataContext =
                 charges.First(item => item.ChargeId.ToString() == chargeId);
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            await ServiceLocator.Current.GetService<IExpenseRepository>()
                  .SaveChargeAsync(this.DataContext as Charge);
            this.NavigationService.GoBack();
        }

    }
}