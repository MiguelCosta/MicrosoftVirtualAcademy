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

using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Expenses.WindowsPhone
{
    public class ViewModelContentControl : ContentControl
    {
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            this.ContentTemplate = App.Current.Resources[newContent.GetType().Name] as DataTemplate;

            //if (newContent.GetType() == typeof(ExpenseReportsViewModel))
            //{
            //    this.ContentTemplate = App.Current.Resources["ExpenseReportsViewModel"] as DataTemplate;
            //}
            //else if (newContent.GetType() == typeof(ApproveExpenseReportsViewModel))
            //{
            //    this.ContentTemplate = App.Current.Resources["ApproveExpenseReportsViewModel"] as DataTemplate;
            //}
            //else if (newContent.GetType() == typeof(LoginControlViewModel))
            //{
            //    this.ContentTemplate = App.Current.Resources["LoginControlViewModel"] as DataTemplate;
            //}
        }
    }
}
