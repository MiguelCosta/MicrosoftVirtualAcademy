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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Expenses.WcfService.ServiceCore;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Expenses.WcfService.Tests
{
    [TestClass]
    public class EmptyStartTests
    {
        private TransactionScope _transactionScope;

        private Random _random = new Random();

        [TestInitialize()]
        public void TestInitialize()
        {
            this._transactionScope = new TransactionScope();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            this._transactionScope.Dispose();
        }

        private Employee AddRandomEmployee()
        {
            Employee employee =
                new Employee()
                {
                    Alias = this._random.Next().ToString(),
                    Manager = this._random.Next().ToString(),
                    Name = this._random.Next().ToString(),
                };

            ExpenseService service = new ExpenseService();
            service.SaveEmployee(employee);
            return employee;
        }

        private Charge AddRandomCharge(int employeeId)
        {
            Charge charge =
                new Charge()
                {
                    AccountNumber = this._random.Next(),
                    BilledAmount = this._random.Next(1000),
                    Category = this._random.Next(),
                    Description = this._random.Next().ToString(),
                    EmployeeId = employeeId,
                    ExpenseDate = DateTime.Today,
                    ExpenseType = this._random.Next(),
                    Location = this._random.Next().ToString(),
                    Merchant = this._random.Next().ToString(),
                    Notes = this._random.Next().ToString(),
                    ReceiptRequired = false,
                    TransactionAmount = this._random.Next(1000),
                };

            ExpenseService service = new ExpenseService();
            service.SaveCharge(charge);
            return charge;
        }

        [TestMethod]
        public void Create_Employee_WithDefaultData()
        {
            string alias = "rogreen";
            ExpenseService service = new ExpenseService();
            Employee employee = service.GetEmployee(alias);

            Assert.IsNotNull(employee);
            Assert.AreEqual(alias, employee.Alias);
            Assert.AreNotEqual(0, service.GetCharges(employee.EmployeeId));
            Assert.AreNotEqual(0, service.GetExpenseReports(employee.EmployeeId));
        }

        [TestMethod]
        public void Create_Employee()
        {
            string alias = "rogreen";

            Employee employee =
                new Employee()
                {
                    Alias = alias,
                    Manager = "notrogreen",
                    Name = "Robert Green",
                };

            ExpenseService service = new ExpenseService();

            int employeeId = service.SaveEmployee(employee);
            Assert.AreNotEqual(0, employeeId);
            Assert.AreEqual(employeeId, employee.EmployeeId);

            Employee dbData = service.GetEmployee(alias);
            Assert.AreEqual(alias, dbData.Alias);

            alias = "rogreen2";
            employee.Alias = alias;
            service.SaveEmployee(employee);

            dbData = service.GetEmployee(alias);
            Assert.AreEqual(alias, dbData.Alias);
        }

        [TestMethod]
        public void Create_Charge()
        {
            Employee employee = this.AddRandomEmployee();

            Charge charge =
                new Charge()
                {
                    AccountNumber = 0,
                    BilledAmount = 1,
                    Category = 2,
                    Description = "3",
                    EmployeeId = employee.EmployeeId,
                    ExpenseDate = new DateTime(2013, 9, 28),
                    ExpenseType = (int) ChargeType.Business,
                    Location = "4",
                    Merchant = "5",
                    Notes = "6",
                    ReceiptRequired = true,
                    TransactionAmount = 7,
                };

            ExpenseService service = new ExpenseService();

            int chargeId = service.SaveCharge(charge);

            Assert.AreNotEqual(0, chargeId);
            Assert.AreEqual(chargeId, charge.ChargeId);
        }

        [TestMethod]
        public void Create_ExpenseReport()
        {
            Employee employee = this.AddRandomEmployee();
            Charge charge1 = this.AddRandomCharge(employee.EmployeeId);
            Charge charge2 = this.AddRandomCharge(employee.EmployeeId);
            Charge charge3 = this.AddRandomCharge(employee.EmployeeId);

            ExpenseReport expenseReport =
                new ExpenseReport()
                {
                    Amount = 1,
                    Approver = "2",
                    CostCenter = 3,
                    EmployeeId = employee.EmployeeId,
                    Notes = "4",
                    Purpose = "5",
                    Status = 6,                    
                };

            ExpenseService service = new ExpenseService();
            service.SaveExpenseReport(expenseReport);
            Assert.AreNotEqual(0, expenseReport.ExpenseReportId);

            charge1.ExpenseReportId = expenseReport.ExpenseReportId;
            service.SaveCharge(charge1);
            charge2.ExpenseReportId = expenseReport.ExpenseReportId;
            service.SaveCharge(charge2);
            charge3.ExpenseReportId = expenseReport.ExpenseReportId;
            service.SaveCharge(charge3);

            List<Charge> charges = service.GetCharges(expenseReport.ExpenseReportId);
            Assert.AreEqual(3, charges.Count);
            Assert.AreEqual(charge1.ChargeId, charges[0].ChargeId);
            Assert.AreEqual(charge2.ChargeId, charges[1].ChargeId);
            Assert.AreEqual(charge3.ChargeId, charges[2].ChargeId);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void Create_Employee_WithDuplicateAlias()
        {
            Employee employee = this.AddRandomEmployee();
            employee.EmployeeId = 0;
            ExpenseService service = new ExpenseService();
            service.SaveEmployee(employee);
        }
    }
}
