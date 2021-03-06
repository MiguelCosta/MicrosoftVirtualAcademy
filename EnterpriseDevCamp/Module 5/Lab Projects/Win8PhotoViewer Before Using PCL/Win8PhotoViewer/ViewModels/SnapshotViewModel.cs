﻿// ----------------------------------------------------------------------------------
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
using System.Text;
using System.Threading.Tasks;
using Win8PhotoViewer.Models;
using Win8PhotoViewer.Services;

namespace Win8PhotoViewer.ViewModels
{
    public class SnapshotViewModel : ViewModelBase
    {
        public Snapshot Snapshot { get; private set; }

        public SnapshotViewModel()
        {
        }

        public void GetSnapshot(int snapshotId)
        {
            var snapShotService = new SnapshotService();
            this.Snapshot = snapShotService.GetSnapshot(snapshotId);
        }
    }
}
