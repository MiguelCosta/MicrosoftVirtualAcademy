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
using System.Linq;
using System.Text;
using Expenses.ExpenseRepository.WcfClient.WcfExpenseService.Extensions;

namespace Expenses.ExpenseRepository.WcfClient.WcfExpenseService
{
    public partial class Employee
    {
        public Expenses.Model.Employee ToModelEmployee()
        {
            return
                new Model.Employee()
                {
                    Alias = this.Alias,
                    EmployeeId = this.EmployeeId,
                    Manager = this.Manager,
                    Name = this.Name,                    
                };
        }

        static public Employee FromModelEmployee(Expenses.Model.Employee employee)
        {
            return
               new Employee()
               {
                   Alias = employee.Alias,
                   EmployeeId = employee.EmployeeId,
                   Manager = employee.Manager,
                   Name = employee.Name,
               };
        }
    }
}
