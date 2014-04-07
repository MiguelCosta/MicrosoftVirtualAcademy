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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win8PhotoViewer.Models;

namespace Win8PhotoViewer.Services
{
    public class SnapshotService
    {
        static ObservableCollection<Snapshot> listOfSnapshots;

        public ObservableCollection<Snapshot> GetSnapshots()
        {
            listOfSnapshots = new ObservableCollection<Snapshot>();

            // Add sample data at first, then use to connect
            //   to real data later.
            listOfSnapshots.Add(new Snapshot()
            {
                ID = 0,
                Image = "/Photos/Image1.jpg",
                Comment = "Still hungry!",
                DateTaken = DateTime.Now,
                Location = "Marcy Mountain"
            });
            listOfSnapshots.Add(new Snapshot()
            {
                ID = 1,
                Image = "/Photos/Image2.jpg",
                Comment = "It took a lot of stings to get this one.",
                DateTaken = DateTime.Now,
                Location = "Algonquin Peak"
            });
            listOfSnapshots.Add(new Snapshot()
            {
                ID = 2,
                Image = "/Photos/Image3.jpg",
                Comment = "Beautiful lake view! What a great day.",
                DateTaken = DateTime.Now,
                Location = "Whitaker Lake"
            });
            listOfSnapshots.Add(new Snapshot()
            {
                ID = 3,
                Image = "/Photos/Image4.jpg",
                Comment = "A burning flower of power",
                DateTaken = DateTime.Now,
                Location = "Haystack"
            });
            listOfSnapshots.Add(new Snapshot()
            {
                ID = 4,
                Image = "/Photos/Image5.jpg",
                Comment = "Berrrrup... I can't do the voice right, you had to be there.",
                DateTaken = DateTime.Now,
                Location = "Skylight Molehill"
            });
            listOfSnapshots.Add(new Snapshot()
            {
                ID = 5,
                Image = "/Photos/Image6.jpg",
                Comment = "Lily pad with the rare Marshall flower",
                DateTaken = DateTime.Now,
                Location = "Whiteface Pond"
            });
            listOfSnapshots.Add(new Snapshot()
            {
                ID = 6,
                Image = "/Photos/Image7.jpg",
                Comment = "Never found out what kind of flower this is.",
                DateTaken = DateTime.Now,
                Location = "Dix Valley"
            });
            listOfSnapshots.Add(new Snapshot()
            {
                ID = 7,
                Image = "/Photos/Image8.jpg",
                Comment = "I totally found him this way and did not in any way pose this photo.",
                DateTaken = DateTime.Now,
                Location = "Gray Skies Ranch"
            });
            listOfSnapshots.Add(new Snapshot()
            {
                ID = 8,
                Image = "/Photos/Image9.jpg",
                Comment = "Then some sort of flying rat in a blue hat attacked me.",
                DateTaken = DateTime.Now,
                Location = "Iroquois Peak"
            });
            return listOfSnapshots;
        }

        public Snapshot GetSnapshot(int snapshotId)
        {
            Snapshot snapshot = (from _snapshot in listOfSnapshots
                                 where _snapshot.ID == snapshotId
                                 select _snapshot).First();
            return snapshot;
        }
    }
}
