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
    public class DoubleToCurrencyStringConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            string result = string.Format("{0:C}", 0);

            if (value is double)
            {
                double amount = (double)value;
                result = string.Format("{0:C}", amount);
            }
            else if (value is decimal)
            {
                decimal amount = (decimal)value;
                result = string.Format("{0:C}", amount);
            }

            return result;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            if (value is double)
            {
                return System.Convert.ToDouble(value);
            }
            else if (value is decimal)
            {
                return System.Convert.ToDecimal(value);
            }
            return System.Convert.ToDecimal(value);
        }
    }
}
