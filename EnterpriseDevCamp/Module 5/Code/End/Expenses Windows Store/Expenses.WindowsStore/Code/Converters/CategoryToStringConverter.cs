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
using Windows.UI.Xaml.Data;

namespace Expenses.WindowsStore
{
    public class CategoryToStringConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            string result = string.Empty;

            if (value != null)
            {
                switch (System.Convert.ToInt32(value))
                {
                    case 1:
                        result = "Airfare";
                        break;
                    case 2:
                        result = "Car Rental";
                        break;
                    case 3:
                        result = "Conference/Seminar";
                        break;
                    case 4:
                        result = "Entertainment";
                        break;
                    case 5:
                        result = "Gifts";
                        break;
                    case 6:
                        result = "Hotel";
                        break;
                    case 7:
                        result = "Mileage";
                        break;
                    case 8:
                        result = "Other";
                        break;
                    case 9:
                        result = "Other Travel & Lodging";
                        break;
                    case 10:
                        result = "T&E Meals";
                        break;
                    default:
                        result = "Other";
                        break;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            int result = 0;

            if (value != null)
            {
                switch (value.ToString())
                {
                    case "Airfare":
                        result = 1;
                        break;
                    case "Car Rental":
                        result = 2;
                        break;
                    case "Conference/Seminar":
                        result = 3;
                        break;
                    case "Entertainment":
                        result = 4;
                        break;
                    case "Gifts":
                        result = 5;
                        break;
                    case "Hotel":
                        result = 6;
                        break;
                    case "Mileage":
                        result = 7;
                        break;
                    case "Other":
                        result = 8;
                        break;
                    case "Other Travel & Lodging":
                        result = 9;
                        break;
                    case "T&E Meals":
                        result = 10;
                        break;
                    default:
                        result = 8;
                        break;
                }
            }

            return result;
        }
    }
}
