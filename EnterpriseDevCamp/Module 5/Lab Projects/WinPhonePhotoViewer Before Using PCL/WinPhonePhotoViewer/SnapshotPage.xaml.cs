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

namespace WinPhonePhotoViewer
{
    public partial class SnapshotPage : PhoneApplicationPage
    {
        private SnapshotViewModel snapshotViewModel = new SnapshotViewModel();
        public SnapshotViewModel SnapshotViewModel
        {
            get { return this.snapshotViewModel; }
        }

        public SnapshotPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("snapshotId", out selectedIndex))
            {
                int snapshotId = int.Parse(selectedIndex);
                snapshotViewModel.GetSnapshot(Convert.ToInt32(snapshotId));
            }
        }


    }
}