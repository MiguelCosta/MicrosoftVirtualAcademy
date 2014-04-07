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

//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections;

namespace Expenses.ViewModel
{
    public class ChargeViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        #region Properties

        private int _chargeId = 0;
        public int ChargeId
        {
            get
            { return _chargeId; }

            set
            {
                if (_chargeId == value)
                { return; }

                _chargeId = value;
                this.NotifyOfPropertyChange(() => this.ChargeId);
            }
        }

        private int employeeId = 0;
        public int EmployeeId
        {
            get
            { return employeeId; }

            set
            {
                if (employeeId == value)
                { return; }

                employeeId = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.EmployeeId);
            }
        }

        private int? _expenseReportId;
        public int? ExpenseReportId
        {
            get
            { return _expenseReportId; }

            set
            {
                if (_expenseReportId == value)
                { return; }

                _expenseReportId = value;
                this.NotifyOfPropertyChange(() => this.ExpenseReportId);
            }
        }

        private DateTime expenseDate = DateTime.Today;
        public DateTime ExpenseDate
        {
            get
            {
                return expenseDate;
            }

            set
            {
                if (expenseDate == value)
                { return; }

                expenseDate = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.ExpenseDate);
            }
        }

        private string merchant = string.Empty;
        public string Merchant
        {
            get
            { return merchant; }

            set
            {
                if (merchant == value)
                { return; }

                merchant = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.Merchant);
            }
        }

        private string location = string.Empty;
        public string Location
        {
            get
            { return location; }

            set
            {
                if (location == value)
                { return; }

                location = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.Location);
            }
        }

        private decimal billedAmount = 0M;
        public decimal BilledAmount
        {
            get
            { return billedAmount; }

            set
            {
                if (billedAmount == value)
                { return; }

                billedAmount = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.BilledAmount);
            }
        }

        private decimal transactionAmount = 0M;
        public decimal TransactionAmount
        {
            get
            { return transactionAmount; }

            set
            {
                if (transactionAmount == value)
                { return; }

                transactionAmount = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.TransactionAmount);
            }
        }

        private string description = string.Empty;
        public string Description
        {
            get
            { return description; }

            set
            {
                if (description == value)
                { return; }

                description = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.Description);
            }
        }

        public string FullCategoryPath
        {
            get
            {
                if (!this.IsOtherCategory)
                {
                    return this.CategoryName;
                }

                return string.Format("{0}/{1}", this.CategoryName, this.OtherCategoryName);
            }
        }

        public int Category
        {
            get
            {
                if (this.CategoryName != this.GetCategoryName((int)CategoryType.Other))
                {
                    return this.GetCategory(this.CategoryName);
                }

                return this.GetOtherCategory(this.OtherCategoryName);
            }
            set
            {
                if (this._category == value) { return; }

                this._category = value;

                string categoryName;
                if (value == -1) //not selected (NULL in DB)
                {
                    return;
                }
                else if (ChargeViewModel.CategoryTypeLookup.TryGetValue((CategoryType)this._category, out categoryName))
                {
                    this.CategoryName = categoryName;
                }
                else
                {
                    this.CategoryName = this.GetCategoryName((int)CategoryType.Other);
                    this.OtherCategoryName = this.GetOtherCategoryName(this._category);
                }
            }
        }
        private int _category = -1;

        public string CategoryName
        {
            get 
            { 
                return this._categoryName; 
            }
            set
            {
                if (this._categoryName == value) { return; }

                this._categoryName = value;
                isDirty = true;

                this.ShowOtherCategories = this.IsOtherCategory;
                this.NotifyOfPropertyChange(() => this.CategoryName);
                this.NotifyOfPropertyChange(() => this.OtherCategoryName);
                this.NotifyOfPropertyChange(() => this.FullCategoryPath);
            }
        }
        private string _categoryName;

        public string OtherCategoryName
        {
            get { return this._otherCategoryName; }
            set
            {
                if (this._otherCategoryName == value) { return; }

                this._otherCategoryName = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.OtherCategoryName);
                this.NotifyOfPropertyChange(() => this.FullCategoryPath);
            }
        }
        private string _otherCategoryName;
                
        private int accountNumber = 0;
        public int AccountNumber
        {
            get
            { return accountNumber; }

            set
            {
                if (accountNumber == value)
                { return; }

                accountNumber = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.AccountNumber);
            }
        }

        private bool receiptRequired = false;
        public bool ReceiptRequired
        {
            get
            { return receiptRequired; }

            set
            {
                if (receiptRequired == value)
                { return; }

                receiptRequired = value;
                this.NotifyOfPropertyChange(() => this.ReceiptRequired);
            }
        }

        private bool isLate = false;
        public bool IsLate
        {
            get
            { return isLate; }

            set
            {
                if (isLate == value)
                { return; }

                isLate = value;
                this.NotifyOfPropertyChange(() => this.IsLate);
            }
        }

        private string foregroundColor;
        public string ForegroundColor
        {
            get
            {
                return foregroundColor;
            }

            set
            {
                foregroundColor = value;
                this.NotifyOfPropertyChange(() => this.ForegroundColor);
            }
        }

        private int expenseType = 0;
        public int ExpenseType
        {
            get
            { return expenseType; }

            set
            {
                if (expenseType == value)
                { return; }

                expenseType = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.ExpenseType);
            }
        }

        private string notes = string.Empty;
        public string Notes
        {
            get
            { return notes; }

            set
            {
                if (notes == value)
                { return; }

                notes = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.Notes);
            }
        }

        private bool isDirty = false;
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }

            set
            {
                isDirty = value;
                this.NotifyOfPropertyChange(() => this.IsDirty);
            }
        }

        #endregion "Properties"

        public bool CanDelete
        {
            get { return this._canDelete; }
            set
            {
                if (this._canDelete == value) { return; }

                this._canDelete = value;
                this.NotifyOfPropertyChange(() => this.CanDelete);
            }
        }
        private bool _canDelete;

        public bool ShowOtherCategories
        {
            get { return this._showOtherCategories; }
            set
            {
                if (this._showOtherCategories == value) { return; }

                this._showOtherCategories = value;
                this.NotifyOfPropertyChange(() => this.ShowOtherCategories);
            }
        }
        private bool _showOtherCategories;

        public Charge Charge
        {
            set
            {
                this.AccountNumber = value.AccountNumber;
                this.BilledAmount = value.BilledAmount;
                this.CanDelete = (value.ChargeId != 0);
                this.Category = value.Category;
                this.ChargeId = value.ChargeId;
                this.Description = value.Description;
                this.EmployeeId = value.EmployeeId;
                this.ExpenseDate = value.ExpenseDate;
                this.ExpenseReportId = value.ExpenseReportId;
                this.ExpenseType = value.ExpenseType;
                this.Location = value.Location;
                this.Merchant = value.Merchant;
                this.ReceiptRequired = value.ReceiptRequired;
                this.TransactionAmount = value.TransactionAmount;
                this.Notes = value.Notes;
            }
        }

        public ICommand SaveChargeCommand { get; private set; }

        public ICommand DeleteChargeCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private readonly IExpenseRepository _repository;
        private readonly INavigationService NavigationService;
        private readonly IViewService ViewService;

        public ChargeViewModel()
        {
            this._repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this.NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();

            this.DeleteChargeCommand = new RelayCommand(
                async (_) =>
                {
                    if (!(await this.ViewService.ConfirmAsync("Delete Charge", "Are you sure you want to delete this charge?")))
                    {
                        return;
                    }

                    await this.DeleteAsync();
                    this.NavigationService.ShowCharges();
                });

            this.SaveChargeCommand = new RelayCommand(
                async (_) =>
                {
                    await this.SaveAsync();
                    this.NavigationService.ShowCharges();
                });

            this.CancelCommand = new RelayCommand(
                (_) =>
                {
                    this.NavigationService.ShowCharges();
                });
        }

        public async Task LoadAsync(int chargeId)
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Charge = await this._repository.GetChargeAsync(chargeId);
                });
        }

        public async Task SaveAsync()
        {
            // Convert charge to Charge 
            var charge = new Charge()
            {
                AccountNumber = this.AccountNumber,
                BilledAmount = this.BilledAmount,
                Category = this.Category,
                ChargeId = this.ChargeId,
                Description = this.Description,
                EmployeeId = this.EmployeeId,
                ExpenseDate = this.ExpenseDate,
                ExpenseReportId = this.ExpenseReportId,
                ExpenseType = this.ExpenseType,
                Location = this.Location,
                Merchant = this.Merchant,
                ReceiptRequired = this.ReceiptRequired,
                TransactionAmount = this.TransactionAmount,
                Notes = this.Notes
            };

            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    await this._repository.SaveChargeAsync(charge);
                });
        }

        public async Task DeleteAsync()
        {
            await this.ViewService.ExecuteBusyActionAsync(
                async () =>
                {
                    await this._repository.DeleteChargeAsync(this.ChargeId);
                });
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errors = new List<string>();

            // TODO: Can't use built-in data annotations or else we lose support for Windows Phone.
            // However, we should implement some quick & dirty attributes eventually. For now, this is the only
            // place we do a lot of validation, so we can live with it until we get the chance to clean it up.

            if (Utilities.GetPropertyName(() => this.Description) == propertyName)
            {
                if (!Utilities.ValidateRequiredString(this.Description))
                {
                    errors.Add("Description is required");
                }
                if (!Utilities.ValidateStringLength(this.Description, 32))
                {
                    errors.Add("Description cannot be greater than 32 characters");
                }
            }

            if (Utilities.GetPropertyName(() => this.Location) == propertyName)
            {
                if (!Utilities.ValidateRequiredString(this.Location))
                {
                    errors.Add("Location is required");
                }
                if (!Utilities.ValidateStringLength(this.Location, 32))
                {
                    errors.Add("Location cannot be greater than 32 characters");
                }
            }

            if (Utilities.GetPropertyName(() => this.Merchant) == propertyName)
            {
                if (!Utilities.ValidateRequiredString(this.Merchant))
                {
                    errors.Add("Merchant is required");
                }
                if (!Utilities.ValidateStringLength(this.Merchant, 32))
                {
                    errors.Add("Merchant cannot be greater than 32 characters");
                }
            }

            if (Utilities.GetPropertyName(() => this.BilledAmount) == propertyName)
            {
                if (!Utilities.ValidatePositive(this.BilledAmount))
                {
                    errors.Add("Billed Amount must be a positive number");
                }
            }

            if (Utilities.GetPropertyName(() => this.TransactionAmount) == propertyName)
            {
                if (!Utilities.ValidatePositive(this.TransactionAmount))
                {
                    errors.Add("Transaction Amount must be a positive number");
                }
            }

            if (Utilities.GetPropertyName(() => this.CategoryName) == propertyName)
            {
                if (!Utilities.ValidateRequiredString(this.CategoryName))
                {
                    errors.Add("Category is required");
                }
            }

            if (Utilities.GetPropertyName(() => this.OtherCategoryName) == propertyName)
            {
                if ((this.CategoryName != null) && this.GetCategory(this.CategoryName) == (int)CategoryType.Other)
                {
                    if (!Utilities.ValidateRequiredString(this.OtherCategoryName))
                    {
                        errors.Add("Other Category is required");
                    }
                }
            }

            return errors;
        }

        public bool HasErrors
        {
            get
            {
                string[] properties =
                    new string[]
                    {
                        Utilities.GetPropertyName(() => this.BilledAmount),
                        Utilities.GetPropertyName(() => this.Description),
                        Utilities.GetPropertyName(() => this.Location),
                        Utilities.GetPropertyName(() => this.Merchant),
                        Utilities.GetPropertyName(() => this.TransactionAmount),
                    };

                foreach (string property in properties)
                {
                    if (this.GetErrors(property).Cast<string>().Count() > 0) { return true; }
                }

                return false;
            }
        }

        private bool IsOtherCategory
        {
            get
            {
                return (this.GetCategory(this.CategoryName) == (int)CategoryType.Other);
            }
        }

        static private Dictionary<CategoryType, string> CategoryTypeLookup
        {
            get
            {
                if (ChargeViewModel._categoryTypeLookup == null)
                {
                    ChargeViewModel._categoryTypeLookup =
                        new Dictionary<CategoryType, string>()
                        {
                           {CategoryType.AirFare, "Airfare"},
                           {CategoryType.CarRental, "Car Rental"},
                           {CategoryType.ConferenceSeminar, "Conference Seminar"},
                           {CategoryType.Entertainment, "Entertainment"},
                           {CategoryType.Gifts, "Gifts"},
                           {CategoryType.Hotel, "Hotel"},
                           {CategoryType.Mileage, "Mileage"},
                           {CategoryType.Other, "Other"},
                           {CategoryType.OtherTravelAndLodging, "Other Travel & Lodging"},
                           {CategoryType.TEMeals, "T&E Meals"},
                        };

                    // In case we forgot to encode any.
                    foreach (CategoryType categoryType in Enum.GetValues(typeof(CategoryType)))
                    {
                        if (!ChargeViewModel._categoryTypeLookup.ContainsKey(categoryType))
                        {
                            throw new InvalidOperationException(string.Format("Unknown CategoryType: {0}", categoryType));
                        }
                    }
                }

                return ChargeViewModel._categoryTypeLookup;
            }
        }
        static private Dictionary<CategoryType, string> _categoryTypeLookup;

        private string GetCategoryName(int category)
        {
            if (ChargeViewModel.CategoryTypeLookup.ContainsKey((CategoryType) category))
            {
                return ChargeViewModel.CategoryTypeLookup[(CategoryType)category];
            }

            return string.Format("Other/{0}", ChargeViewModel.OtherCategoryTypeLookup[(OtherCategoryType)category]);
        }

        private int GetCategory(string categoryName)
        {
            if (categoryName == null)
            { 
                return -1; 
            }
            return (int)ChargeViewModel.CategoryTypeLookup.First(item => item.Value == categoryName).Key;
        }

        public string[] CategoryTypes
        {
            get
            {
                return ChargeViewModel.CategoryTypeLookup.Values.OrderBy(item => item).ToArray();
            }
        }

        static private Dictionary<OtherCategoryType, string> OtherCategoryTypeLookup
        {
            get
            {
                if (ChargeViewModel._otherCategoryTypeLookup == null)
                {
                    ChargeViewModel._otherCategoryTypeLookup =
                        new Dictionary<OtherCategoryType, string>()
                        {
                           {OtherCategoryType.AdminServices, "Administrative Services"},
                           {OtherCategoryType.CellPhone, "Cell Phone"},
                           {OtherCategoryType.ComputerServices, "Computer Services"},
                           {OtherCategoryType.ComputerSuppliesEquipment, "Computer Supplies & Equipment"},
                           {OtherCategoryType.EmployeeMorale, "Employee Morale"},
                           {OtherCategoryType.GeneralSupplies, "General Supplies"},
                           {OtherCategoryType.HomeBroadband, "Home Broadband"},
                           {OtherCategoryType.PhoneFax, "Phone/Fax"},
                           {OtherCategoryType.Postage, "Postage"},
                           {OtherCategoryType.RecruitMeals, "Recruiting Meals"},
                           {OtherCategoryType.RecruitOther, "Recruiting Other"},
                           {OtherCategoryType.RecruitTravel, "Recruiting Travel"},
                           {OtherCategoryType.ReferenceMaterial, "Reference Materials"},
                           {OtherCategoryType.Training, "Training"},
                           {OtherCategoryType.TravelBroadband, "Travel Broadband"},
                        };

                    // In case we forgot to encode any.
                    foreach (OtherCategoryType otherCategoryType in Enum.GetValues(typeof(OtherCategoryType)))
                    {
                        if (!ChargeViewModel._otherCategoryTypeLookup.ContainsKey(otherCategoryType))
                        {
                            throw new InvalidOperationException(string.Format("Unknown OtherCategoryType: {0}", otherCategoryType));
                        }
                    }
                }

                return ChargeViewModel._otherCategoryTypeLookup;
            }
        }
        static private Dictionary<OtherCategoryType, string> _otherCategoryTypeLookup;

        private string GetOtherCategoryName(int otherCategory)
        {
            return ChargeViewModel.OtherCategoryTypeLookup[(OtherCategoryType)otherCategory];
        }

        private int GetOtherCategory(string otherCategoryName)
        {
            return (int)ChargeViewModel.OtherCategoryTypeLookup.First(item => item.Value == otherCategoryName).Key;
        }

        public string[] OtherCategoryTypes
        {
            get
            {
                return ChargeViewModel.OtherCategoryTypeLookup.Values.OrderBy(item => item).ToArray();
            }
        }

    }

}
