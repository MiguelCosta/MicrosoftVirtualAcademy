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

namespace Expenses.WindowsStore
{
    public static class Helpers
    {
        public static string GetCategoryName(int categoryNumber)
        {
            string categoryName;
            switch (categoryNumber)
            {
                case 1:
                    categoryName = "Airfare";
                    break;
                case 2:
                    categoryName = "Car Rental";
                    break;
                case 3:
                    categoryName = "Conference/Seminar";
                    break;
                case 4:
                    categoryName = "Entertainment";
                    break;
                case 5:
                    categoryName = "Gifts";
                    break;
                case 6:
                    categoryName = "Hotel";
                    break;
                case 7:
                    categoryName = "Mileage";
                    break;
                case 8:
                    categoryName = "Other";
                    break;
                case 9:
                    categoryName = "Other Travel & Lodging";
                    break;
                case 10:
                    categoryName = "T&E Meals";
                    break;
                default:
                    categoryName = "Other";
                    break;
            }
            return categoryName;
        }

        public static int GetCategoryNumber(string categoryName)
        {
            int categoryNumber;
            switch (categoryName)
            {
                case "Airfare":
                    categoryNumber = 1;
                    break;
                case "Car Rental":
                    categoryNumber = 2;
                    break;
                case "Conference/Seminar":
                    categoryNumber = 3;
                    break;
                case "Entertainment":
                    categoryNumber = 4;
                    break;
                case "Gifts":
                    categoryNumber = 5;
                    break;
                case "Hotel":
                    categoryNumber = 6;
                    break;
                case "Mileage":
                    categoryNumber = 7;
                    break;
                case "Other":
                    categoryNumber = 8;
                    break;
                case "Other Travel & Lodging":
                    categoryNumber = 9;
                    break;
                case "T&E Meals":
                    categoryNumber = 10;
                    break;
                default:
                    categoryNumber = 8;
                    break;
            }
            return categoryNumber;
        }

        public static int GetOtherCategoryNumber(string otherCategoryName)
        {
            int otherCategoryNumber;
            switch (otherCategoryName)
            {
                case "Admin Services":
                    otherCategoryNumber = 740008;
                    break;
                case "Cell Phone":
                    otherCategoryNumber = 742001;
                    break;
                case "Computer Services":
                    otherCategoryNumber = 740000;
                    break;
                case "Computer Supplies & Equipment":
                    otherCategoryNumber = 728004;
                    break;
                case "Employee Morale":
                    otherCategoryNumber = 7210020;
                    break;
                case "General Supplies":
                    otherCategoryNumber = 728002;
                    break;
                case "Home Broadband":
                    otherCategoryNumber = 742002;
                    break;
                case "Phone/Fax":
                    otherCategoryNumber = 742000;
                    break;
                case "Postage":
                    otherCategoryNumber = 752000;
                    break;
                case "Recruit Meals":
                    otherCategoryNumber = 722004;
                    break;
                case "Recruit Travel":
                    otherCategoryNumber = 722003;
                    break;
                case "Recruit Other":
                    otherCategoryNumber = 722005;
                    break;
                case "Reference Material":
                    otherCategoryNumber = 728007;
                    break;
                case "Training":
                    otherCategoryNumber = 725001;
                    break;
                case "Travel Broadband":
                    otherCategoryNumber = 742016;
                    break;
                default:
                    otherCategoryNumber = 0;
                    break;
            }
            return otherCategoryNumber;
        }

        public static string GetOtherCategoryName(int otherCategoryNumber)
        {
            string otherCategoryName;
            switch (otherCategoryNumber)
            {
                case 740008:
                    otherCategoryName = "Admin Services";
                    break;
                case 742001:
                    otherCategoryName = "Cell Phone";
                    break;
                case 740000:
                    otherCategoryName = "Computer Services";
                    break;
                case 728004:
                    otherCategoryName = "Computer Supplies & Equipment";
                    break;
                case 7210020:
                    otherCategoryName = "Employee Morale";
                    break;
                case 728002:
                    otherCategoryName = "General Supplies";
                    break;
                case 742002:
                    otherCategoryName = "Home Broadband";
                    break;
                case 742000:
                    otherCategoryName = "Phone/Fax";
                    break;
                case 752000:
                    otherCategoryName = "Postage";
                    break;
                case 722004:
                    otherCategoryName = "Recruit Meals";
                    break;
                case 722003:
                    otherCategoryName = "Recruit Travel";
                    break;
                case 722005:
                    otherCategoryName = "Recruit Other";
                    break;
                case 728007:
                    otherCategoryName = "Reference Material";
                    break;
                case 725001:
                    otherCategoryName = "Training";
                    break;
                case 742016:
                    otherCategoryName = "Travel Broadband";
                    break;
                default:
                    otherCategoryName = "Missing";
                    break;
            }
            return otherCategoryName;
        }

    }
}
