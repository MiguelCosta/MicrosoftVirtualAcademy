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

namespace Expenses.Wpf
{
    /// <summary>
    /// This attached behavior is meant for use with DataGrid.
    /// </summary>
    public static class SelectedItemsBehavior
    {
        public static readonly DependencyProperty SelectedItemsChangedHandlerProperty = 
            DependencyProperty.RegisterAttached("SelectedItemsChangedHandler", typeof(RelayCommand), typeof(SelectedItemsBehavior), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemsChangedHandlerChanged)));

        public static RelayCommand GetSelectedItemsChangedHandler(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (RelayCommand)element.GetValue(SelectedItemsChangedHandlerProperty);
        }

        public static void SetSelectedItemsChangedHandler(DependencyObject element, RelayCommand value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(SelectedItemsChangedHandlerProperty, value);
        }

        private static void OnSelectedItemsChangedHandlerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)d;

            if (e.OldValue == null && e.NewValue != null)
            {
                dataGrid.SelectionChanged += ItemsControl_SelectionChanged;
            }

            if (e.OldValue != null && e.NewValue == null)
            {
                dataGrid.SelectionChanged -= ItemsControl_SelectionChanged;
            }
        }

        public static void ItemsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            RelayCommand itemsChangedHandler = GetSelectedItemsChangedHandler(dataGrid);

            itemsChangedHandler.Execute(dataGrid.SelectedItems);
        }
    }
}
