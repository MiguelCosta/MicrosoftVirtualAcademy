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
using WinPhonePhotoViewer.ViewModels;
using WinPhonePhotoViewer.Models;

namespace WinPhonePhotoViewer
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MainViewModel mainViewModel = new MainViewModel();
        public MainViewModel MainViewModel
        {
            get { return this.mainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private void SnapsnotLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri(
                "/SnapshotPage.xaml?snapshotId=" +
                (SnapsnotLongListSelector.SelectedItem as Snapshot).ID, UriKind.Relative));
        }
    }
}