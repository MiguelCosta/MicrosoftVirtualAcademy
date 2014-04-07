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
    public class OtherCategoryToStringConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            string result = string.Empty;

            if (value != null)
            {
                switch (System.Convert.ToInt32(value))
                {
                    case 742001:
                        result = "Cell Phone";
                        break;
                    case 740000:
                        result = "Computer Services";
                        break;
                    case 728004:
                        result = "Computer Supplies & Equipment";
                        break;
                    case 7210020:
                        result = "Employee Morale";
                        break;
                    case 728002:
                        result = "General Supplies";
                        break;
                    case 742002:
                        result = "Home Broadband";
                        break;
                    case 742000:
                        result = "Phone/Fax";
                        break;
                    case 752000:
                        result = "Postage";
                        break;
                    case 722004:
                        result = "Recruit Meals";
                        break;
                    case 722003:
                        result = "Recruit Travel";
                        break;
                    case 722005:
                        result = "Recruit Other";
                        break;
                    case 728007:
                        result = "Reference Material";
                        break;
                    case 725001:
                        result = "Training";
                        break;
                    case 742016:
                        result = "Travel Broadband";
                        break;
                    default:
                        result = "";
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
                    case "Admin Services":
                        result = 710008;
                        break;
                    case "Cell Phone":
                        result = 742001;
                        break;
                    case "Computer Services":
                        result = 740000;
                        break;
                    case "Computer Supplies & Equipment":
                        result = 728004;
                        break;
                    case "Employee Morale":
                        result = 7210020;
                        break;
                    case "General Supplies":
                        result = 728002;
                        break;
                    case "Home Broadband":
                        result = 742002;
                        break;
                    case "Phone/Fax":
                        result = 742000;
                        break;
                    case "Postage":
                        result = 752000;
                        break;
                    case "Recruit Meals":
                        result = 722004;
                        break;
                    case "Recruit Travel":
                        result = 722003;
                        break;
                    case "Recruit Other":
                        result = 722005;
                        break;
                    case "Reference Material":
                        result = 728007;
                        break;
                    case "Training":
                        result = 725001;
                        break;
                    case "Travel Broadband":
                        result = 742016;
                        break;
                    default:
                        result = 0;
                        break;
                }
            }

            return result;
        }
    }
}
