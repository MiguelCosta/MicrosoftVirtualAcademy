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

using Expenses.Model;
using Expenses.ViewModel;
using Expenses.WindowsStore.Common;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Expenses.WindowsStore
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ChargePage : Page
    {
        private ChargeViewModel charge = null;
        private NavigationHelper navigationHelper;

        string billedAmountTextBoxText = string.Empty;
        string transactionAmountTextBoxText = string.Empty;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public ChargePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            if (e.Parameter == null)
            {
                charge = new ChargeViewModel();
                charge.ExpenseType = Convert.ToInt32(ExpenseItemType.Personal);

                ExpenseDatePicker.Visibility = Visibility.Visible;
                MerchantTextBox.Visibility = Visibility.Visible;
                LocationTextBox.Visibility = Visibility.Visible;
                BilledAmountTextBox.Visibility = Visibility.Visible;
                TransactionAmountTextBox.Visibility = Visibility.Visible;
                DeleteChargeButton.Visibility = Visibility.Collapsed;

                this.DataContext = charge;
                ExpenseTypeTextBlock.Text = "Personal charge";
                this.pageTitle.Text = "New Charge";
            }
            else
            {
                charge = (ChargeViewModel)e.Parameter;
                this.DataContext = charge;
                CategoriesComboBox.SelectedItem =
                    Helpers.GetCategoryName(charge.Category);
                if (CategoriesComboBox.SelectedItem.ToString() == "Other")
                {
                    OtherCategoriesComboBox.SelectedItem =
                        Helpers.GetOtherCategoryName(charge.AccountNumber);
                }
                ExpenseDatePicker.Date = charge.ExpenseDate;
                SnappedExpenseDatePicker.Date = charge.ExpenseDate;

                this.pageTitle.Text = string.Format("{0} on {1:d} for {2:C}",
                    charge.Merchant, charge.ExpenseDate, charge.BilledAmount);

                if (charge.ExpenseType == Convert.ToInt32(ExpenseItemType.Personal))
                {
                    ExpenseDatePicker.Visibility = Visibility.Visible;
                    MerchantTextBox.Visibility = Visibility.Visible;
                    LocationTextBox.Visibility = Visibility.Visible;
                    BilledAmountTextBox.Visibility = Visibility.Visible;
                    TransactionAmountTextBox.Visibility = Visibility.Visible;
                    DeleteChargeButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ExpenseDateTextBlock.Text = charge.ExpenseDate.ToString("d");
                    ExpenseDateTextBlock.Visibility = Visibility.Visible;
                    MerchantTextBlock.Visibility = Visibility.Visible;
                    LocationTextBlock.Visibility = Visibility.Visible;
                    BilledAmountTextBlock.Visibility = Visibility.Visible;
                    TransactionAmountTextBlock.Visibility = Visibility.Visible;
                }

            }
            // Register handler for DataRequested events for sharing
            //dataTransferManager = DataTransferManager.GetForCurrentView();
            //dataTransferManager.DataRequested += new TypedEventHandler<
            //    DataTransferManager, DataRequestedEventArgs>(OnDataRequested);

            charge.IsDirty = false;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesComboBox.SelectedItem == null)
            {
                return;
            }
            string category = CategoriesComboBox.SelectedItem.ToString();
            charge.Category = Helpers.GetCategoryNumber(category);

            if (category == "Other")
            {
                OtherCategoriesComboBox.IsEnabled = true;
            }
            else
            {
                OtherCategoriesComboBox.IsEnabled = false;
            }
            CategoryErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void OtherCategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OtherCategoriesComboBox.SelectedItem == null)
            {
                return;
            }
            string otherCategory = OtherCategoriesComboBox.SelectedItem.ToString();
            charge.AccountNumber = Helpers.GetOtherCategoryNumber(otherCategory);
            OtherCategoryErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void BilledAmountTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            BilledAmountTextBox.SelectAll();
        }

        private void BilledAmountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            billedAmountTextBoxText = BilledAmountTextBox.Text;
            if (BilledAmountTextBox.Text.Substring(0, 1) == "$")
            {
                billedAmountTextBoxText = BilledAmountTextBox.Text.Substring(1);
            }

            if (Convert.ToDecimal(billedAmountTextBoxText) > 0M)
            {
                BilledAmountErrorTextBlock.Visibility = Visibility.Collapsed;
            }

            charge.ReceiptRequired =
                (BilledAmountTextBox.Text != string.Empty &&
                 Convert.ToDecimal(billedAmountTextBoxText) > 75M);
        }

        private void TransactionAmountTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TransactionAmountTextBox.SelectAll();
        }

        private void TransactionAmountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            transactionAmountTextBoxText = TransactionAmountTextBox.Text;
            if (TransactionAmountTextBox.Text.Substring(0, 1) == "$")
            {
                transactionAmountTextBoxText = TransactionAmountTextBox.Text.Substring(1);
            }

            if (Convert.ToDecimal(transactionAmountTextBoxText) > 0M)
            {
                TransactionAmountErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void MerchantTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MerchantTextBox.Text != string.Empty)
            {
                MerchantErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void DescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DescriptionTextBox.Text != string.Empty)
            {
                DescriptionErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void ExpenseDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ExpenseDatePicker.Date != SnappedExpenseDatePicker.Date)
            {
                SnappedExpenseDatePicker.Date = ExpenseDatePicker.Date;
            }
        }

        private void SnappedExpenseDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ExpenseDatePicker.Date != SnappedExpenseDatePicker.Date)
            {
                ExpenseDatePicker.Date = SnappedExpenseDatePicker.Date;
            }
        }

        private void PopulateCategories()
        {
            CategoriesComboBox.Items.Clear();
            CategoriesComboBox.Items.Add("Airfare");
            CategoriesComboBox.Items.Add("Car Rental");
            CategoriesComboBox.Items.Add("Conference/Seminar");
            CategoriesComboBox.Items.Add("Entertainment");
            CategoriesComboBox.Items.Add("Gifts");
            CategoriesComboBox.Items.Add("Hotel");
            CategoriesComboBox.Items.Add("Mileage");
            CategoriesComboBox.Items.Add("Other");
            CategoriesComboBox.Items.Add("Other Travel & Lodging");
            CategoriesComboBox.Items.Add("T&E Meals");
        }

        private void PopulateOtherCategories()
        {
            OtherCategoriesComboBox.Items.Clear();
            OtherCategoriesComboBox.Items.Add("Admin Services");
            OtherCategoriesComboBox.Items.Add("Cell Phone");
            OtherCategoriesComboBox.Items.Add("Computer Services");
            OtherCategoriesComboBox.Items.Add("Computer Supplies & Equipment");
            OtherCategoriesComboBox.Items.Add("Employee Morale");
            OtherCategoriesComboBox.Items.Add("General Supplies");
            OtherCategoriesComboBox.Items.Add("Home Broadband");
            OtherCategoriesComboBox.Items.Add("Phone/Fax");
            OtherCategoriesComboBox.Items.Add("Postage");
            OtherCategoriesComboBox.Items.Add("Recruit Meals");
            OtherCategoriesComboBox.Items.Add("Recruit Travel");
            OtherCategoriesComboBox.Items.Add("Recruit Other");
            OtherCategoriesComboBox.Items.Add("Reference Material");
            OtherCategoriesComboBox.Items.Add("Training");
            OtherCategoriesComboBox.Items.Add("Travel Broadband");
        }

    }
}
