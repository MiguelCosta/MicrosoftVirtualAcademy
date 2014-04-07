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
using System.Data.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Expenses.WcfService.ServiceCore
{
    public class ExpenseService : IExpenseService
    {
        public Employee GetEmployee(string employeeAlias)
        {            
            using (DbDataContext db = new DbDataContext())
            {
                Employee employee = db.Employees.FirstOrDefault(item => item.Alias == employeeAlias);

                // This is a hack to create fresh data for a new employee.
                if (employee == null)
                {
                    employee = this.CreateNewEmployee(employeeAlias);
                }

                return employee;
            }
        }

        public int SaveEmployee(Employee employee)
        {
            using (DbDataContext db = new DbDataContext())
            {
                Employee dbEmployee;
                if (employee.EmployeeId == 0)
                {
                    dbEmployee = employee;
                    db.Employees.InsertOnSubmit(dbEmployee);
                }
                else
                {
                    dbEmployee = db.Employees.Single(item => item.EmployeeId == employee.EmployeeId);
                    dbEmployee.Alias = employee.Alias;
                    dbEmployee.Manager = employee.Manager;
                    dbEmployee.Name = employee.Name;
                }
                db.SubmitChanges();
                return dbEmployee.EmployeeId;
            }            
        }

        public List<SummaryItem> GetSummaryItems(int employeeId)
        {
            using (DbDataContext db = new DbDataContext())
            {
                Employee employee = db.Employees.FirstOrDefault(item => item.EmployeeId == employeeId);
                if (employee == null)
                {
                    throw new ArgumentException("Employee not found");
                }

                List<SummaryItem> summaryItems = new List<SummaryItem>();

                summaryItems.AddRange(
                    this.GetOutstandingCharges(employeeId).Select(
                        item =>
                            new SummaryItem()
                            {
                                Amount = item.TransactionAmount,
                                Date = item.ExpenseDate,
                                Description = (!string.IsNullOrWhiteSpace(item.Location)) ? string.Format("{0} ({1})", item.Merchant, item.Location) : item.Merchant,
                                Id = item.ChargeId,
                                ItemType = ItemType.Charge,
                                Submitter = employee.Name,
                            }));

                summaryItems.AddRange(
                    this.GetExpenseReportsByStatus(employeeId, ExpenseReportStatus.Saved).Select(
                        item =>
                            new SummaryItem()
                            {
                                Amount = item.Amount,
                                Date = item.DateSaved,
                                Description = item.Purpose,
                                Id = item.ExpenseReportId,
                                ItemType = ItemType.SavedReport,
                                Submitter = employee.Name,
                            }));

                summaryItems.AddRange(
                    this.GetExpenseReportsByStatus(employeeId, ExpenseReportStatus.Pending).Select(
                        item =>
                            new SummaryItem()
                            {
                                Amount = item.Amount,
                                Date = item.DateSubmitted,
                                Description = item.Purpose,
                                Id = item.ExpenseReportId,
                                ItemType = ItemType.PendingReport,
                                Submitter = employee.Name,
                            }));

                summaryItems.AddRange(
                    this.GetExpenseReportsByStatus(employeeId, ExpenseReportStatus.Approved).Select(
                        item =>
                            new SummaryItem()
                            {
                                Amount = item.Amount,
                                Date = item.DateSaved,
                                Description = item.Purpose,
                                Id = item.ExpenseReportId,
                                ItemType = ItemType.ApprovedReport,
                                Submitter = employee.Name,
                            }));


                summaryItems.AddRange(
                         this.GetExpenseReportsForApproval(employee.Alias).Select(
                             item =>
                                 new SummaryItem()
                                 {
                                     Amount = item.Amount,
                                     Date = item.DateSubmitted,
                                     Description = string.Format("{0} ({1})", item.Purpose, this.GetEmployee(item.EmployeeId).Name),
                                     Id = item.ExpenseReportId,
                                     ItemType = ItemType.UnresolvedReport,
                                     Submitter = employee.Name,
                                 }));

                return summaryItems;
            }
        }

        public ExpenseReport GetExpenseReport(int expenseReportId)
        {
            using (DbDataContext db = new DbDataContext())
            {
                return db.ExpenseReports.FirstOrDefault(item => item.ExpenseReportId == expenseReportId);
            }
        }

        public List<ExpenseReport> GetExpenseReports(int employeeId)
        {
            using (DbDataContext db = new DbDataContext())
            {
                return db.ExpenseReports.Where(item => item.EmployeeId == employeeId).ToList();
            }
        }

        public List<ExpenseReport> GetExpenseReportsByStatus(int employeeId, ExpenseReportStatus status)
        {
            using (DbDataContext db = new DbDataContext())
            {
                return db.ExpenseReports.Where(item => ((item.EmployeeId == employeeId) && (item.Status == (int)status))).ToList();
            }
        }

        public List<Charge> GetOutstandingCharges(int employeeId)
        {
            using (DbDataContext db = new DbDataContext())
            {
                return db.Charges.Where(item => ((item.EmployeeId == employeeId) && (item.ExpenseReportId == null))).ToList();
            }
        }
        

        public List<Charge> GetCharges(int expenseReportId)
        {
            using (DbDataContext db = new DbDataContext())
            {
                return db.Charges.Where(item => item.ExpenseReportId == expenseReportId).ToList();
            }
        }

        public Charge GetCharge(int chargeId)
        {
            using (DbDataContext db = new DbDataContext())
            {
                return db.Charges.FirstOrDefault(item => item.ChargeId == chargeId);
            }
        }

        public decimal GetAmountOwedToCreditCard(int employeeId)
        {
            return this.GetExpenseReportsByStatus(employeeId, ExpenseReportStatus.Pending).Sum(item => item.OwedToCreditCard);
        }

        public decimal GetAmountOwedToEmployee(int employeeId)
        {
            return this.GetExpenseReportsByStatus(employeeId, ExpenseReportStatus.Pending).Sum(item => item.OwedToEmployee);
        }

        public int SaveCharge(Charge charge)
        {
            using (DbDataContext db = new DbDataContext())
            {
                Charge dbCharge;
                if (charge.ChargeId == 0)
                {
                    dbCharge = charge;
                    db.Charges.InsertOnSubmit(dbCharge);
                }
                else
                {
                    dbCharge = db.Charges.Single(item => item.ChargeId == charge.ChargeId);
                    dbCharge.AccountNumber = charge.AccountNumber;
                    dbCharge.BilledAmount = charge.BilledAmount;
                    dbCharge.Category = charge.Category;
                    dbCharge.Description = charge.Description;
                    dbCharge.EmployeeId = charge.EmployeeId;
                    dbCharge.ExpenseDate = charge.ExpenseDate;
                    dbCharge.ExpenseReportId = charge.ExpenseReportId;
                    dbCharge.ExpenseType = charge.ExpenseType;
                    dbCharge.Location = charge.Location;
                    dbCharge.Merchant = charge.Merchant;
                    dbCharge.Notes = charge.Notes;
                    dbCharge.ReceiptRequired = charge.ReceiptRequired;
                    dbCharge.TransactionAmount = charge.TransactionAmount;
                }
                db.SubmitChanges();
                return dbCharge.ChargeId;
            }            
        }

        public void DeleteCharge(int chargeId)
        {
            using (DbDataContext db = new DbDataContext())
            {
                db.Charges.DeleteAllOnSubmit(db.Charges.Where(item => item.ChargeId == chargeId));
                db.SubmitChanges();
            }
        }

        public int SaveExpenseReport(ExpenseReport expenseReport)
        {
            using (DbDataContext db = new DbDataContext())
            {
                ExpenseReport dbExpenseReport;
                if (expenseReport.ExpenseReportId == 0)
                {
                    dbExpenseReport = expenseReport;
                    db.ExpenseReports.InsertOnSubmit(dbExpenseReport);
                }
                else
                {
                    dbExpenseReport = db.ExpenseReports.Single(item => item.ExpenseReportId == expenseReport.ExpenseReportId);
                    dbExpenseReport.Amount = expenseReport.Amount;
                    dbExpenseReport.Approver = expenseReport.Approver;
                    dbExpenseReport.CostCenter = expenseReport.CostCenter;
                    dbExpenseReport.DateResolved = expenseReport.DateResolved;
                    dbExpenseReport.DateSaved = expenseReport.DateSaved;
                    dbExpenseReport.DateSubmitted = expenseReport.DateSubmitted;
                    dbExpenseReport.EmployeeId = expenseReport.EmployeeId;
                    dbExpenseReport.Notes = expenseReport.Notes;
                    dbExpenseReport.OwedToCreditCard = expenseReport.OwedToCreditCard;
                    dbExpenseReport.OwedToEmployee = expenseReport.OwedToEmployee;
                    dbExpenseReport.Purpose = expenseReport.Purpose;
                    dbExpenseReport.Status = expenseReport.Status;
                }
                db.SubmitChanges();
                return dbExpenseReport.ExpenseReportId;
            }
        }

        public void DeleteExpenseReport(int expenseReportId)
        {
            using (DbDataContext db = new DbDataContext())
            {
                db.ExpenseReports.DeleteAllOnSubmit(db.ExpenseReports.Where(item => item.ExpenseReportId == expenseReportId));
                db.SubmitChanges();
            }
        }

        public void ResetData()
        {
            using (DbDataContext db = new DbDataContext())
            {
                db.Charges.DeleteAllOnSubmit(db.Charges);
                db.ExpenseReports.DeleteAllOnSubmit(db.ExpenseReports);
                db.Employees.DeleteAllOnSubmit(db.Employees);
                db.SubmitChanges();
            }
        }

        private Employee GetEmployee(int employeeId)
        {
            using (DbDataContext db = new DbDataContext())
            {
                return db.Employees.FirstOrDefault(item => item.EmployeeId == employeeId);
            }
        }

        public List<ExpenseReport> GetExpenseReportsForApproval(string employeeAlias)
        {
            using (DbDataContext db = new DbDataContext())
            {
                return db.ExpenseReports.Where(item => ((item.Approver == employeeAlias) && (item.Status == (int)ExpenseReportStatus.Pending))).ToList();
            }
        }

        private Employee CreateNewEmployee(string alias)
        {
            string managerAlias = "manager";

            if (string.Compare(alias, "rogreen", true) != 0)
            {
                managerAlias = "rogreen";
            }

            Employee employee =
                new Employee()
                {
                    Alias = alias,
                    Charges = new EntitySet<Charge>(),
                    ExpenseReports = new EntitySet<ExpenseReport>(),
                    Manager = managerAlias,
                    Name = "New Employee",
                };

            employee.Charges.Add(
                new Charge()
                {
                    AccountNumber = 723000,
                    BilledAmount = 200M,
                    Category = (int)CategoryType.Hotel,
                    Description = "REF# 27438948",
                    ExpenseDate = DateTime.Today.AddDays(-45),
                    ExpenseType = (int)ChargeType.Business,
                    Location = "San Francisco, CA",
                    Merchant = "Northwind Inn",
                    Notes = string.Empty,
                    ReceiptRequired = true,
                    TransactionAmount = 200M,
                });

            employee.Charges.Add(
                new Charge()
                {
                    AccountNumber = 723000,
                    BilledAmount = 40,
                    Category = (int)CategoryType.OtherTravelAndLodging,
                    Description = "REF# 77384751",
                    ExpenseDate = DateTime.Today.AddDays(-20),
                    ExpenseType = (int)ChargeType.Business,
                    Location = "Seattle, WA",
                    Merchant = "Contoso Taxi",
                    Notes = string.Empty,
                    ReceiptRequired = false,
                    TransactionAmount = 40,
                });

            employee.Charges.Add(
                new Charge()
                {
                    AccountNumber = 723000,
                    BilledAmount = 67,
                    Category = (int)CategoryType.TEMeals,
                    Description = "REF# 33748563",
                    ExpenseDate = DateTime.Today.AddDays(-8),
                    ExpenseType = (int)ChargeType.Business,
                    Location = "Seattle, WA",
                    Merchant = "Fourth Coffee",
                    Notes = string.Empty,
                    ReceiptRequired = false,
                    TransactionAmount = 12,
                });

            employee.Charges.Add(
                new Charge()
                {
                    AccountNumber = 723000,
                    BilledAmount = 17,
                    Category = (int)CategoryType.TEMeals,
                    Description = "REF# 33748876",
                    ExpenseDate = DateTime.Today.AddDays(-4),
                    ExpenseType = (int)ChargeType.Business,
                    Location = "Seattle, WA",
                    Merchant = "Fourth Coffee",
                    Notes = string.Empty,
                    ReceiptRequired = false,
                    TransactionAmount = 15,
                });

            ExpenseReport expenseReport = 
                new ExpenseReport()
                {
                    Amount = 640M,
                    Approver = managerAlias,
                    CostCenter = 50992,
                    DateSubmitted = DateTime.Today.AddDays(-7),
                    Notes = (managerAlias == "rogreen") ? "Kim Akers" : "Visit to Blue Yonder Airlines",
                    OwedToCreditCard = 450M,
                    OwedToEmployee = 0M,
                    Purpose = "Visit to Blue Yonder Airlines",
                    Status = (int)ExpenseReportStatus.Pending,
                };
            employee.ExpenseReports.Add(expenseReport);

            expenseReport =
                new ExpenseReport()
                {
                    Amount = 450M,
                    Approver = managerAlias,
                    CostCenter = 50992,
                    DateSubmitted = DateTime.Today.AddDays(-7),
                    Notes = (managerAlias == "rogreen") ? "Kim Akers" : "Visit to Tailspin Toys",
                    OwedToCreditCard = 450M,
                    OwedToEmployee = 0M,
                    Purpose = "Visit to Tailspin Toys",
                    Status = (int)ExpenseReportStatus.Pending,
                };
            employee.ExpenseReports.Add(expenseReport);
            
            expenseReport.Charges.Add(
                new Charge()
                {
                    AccountNumber = 723000,
                    BilledAmount = 350M,
                    Category = (int)CategoryType.AirFare,
                    Description = "Airfare to San Francisco",    
                    Employee = employee,
                    ExpenseDate = DateTime.Today.AddDays(-60),
                    ExpenseType = (int)ChargeType.Business,
                    Location = "Chicago, IL",
                    Merchant = "Blue Yonder Airlines",
                    Notes = string.Empty,
                    ReceiptRequired = true,
                    TransactionAmount = 350M,
                });

            expenseReport.Charges.Add(
                new Charge()
                {
                    AccountNumber = 723000,
                    BilledAmount = 50,
                    Category = (int)CategoryType.OtherTravelAndLodging,
                    Description = "Cab from airport",
                    Employee = employee,
                    ExpenseDate = DateTime.Today.AddDays(-45),
                    ExpenseType = (int)ChargeType.Business,
                    Location = "San Francisco, CA",
                    Merchant = "Contoso Taxi",
                    Notes = string.Empty,
                    ReceiptRequired = false,
                    TransactionAmount = 50,
                });

            expenseReport.Charges.Add(
                new Charge()
                {
                    AccountNumber = 723000,
                    BilledAmount = 50,
                    Category = (int)CategoryType.OtherTravelAndLodging,
                    Description = "Cab to airport",
                    Employee = employee,
                    ExpenseDate = DateTime.Today.AddDays(-45),
                    ExpenseType = (int)ChargeType.Business,
                    Location = "San Francisco, CA",
                    Merchant = "Contoso Taxi",
                    Notes = string.Empty,
                    ReceiptRequired = false,
                    TransactionAmount = 50,
                });
                        
            // Add a year of every other month customer visits
            int x = -75;
            for (int i = 1; i <= 6; i++)
            {
                expenseReport = 
                    new ExpenseReport()
                    {
                        Amount = 850M,
                        Approver = managerAlias,
                        CostCenter = 50992,
                        DateSubmitted = DateTime.Today.AddDays(x - 5),
                        DateResolved = DateTime.Today.AddDays(x),
                        Notes = "Visit to Tailspin Toys",
                        OwedToCreditCard = 850M,
                        OwedToEmployee = 0M,
                        Purpose = "Customer visit",
                        Status = (int)ExpenseReportStatus.Approved,
                    };
                employee.ExpenseReports.Add(expenseReport);

                expenseReport.Charges.Add(
                    new Charge()
                    {
                        AccountNumber = 723000,
                        BilledAmount = 350M,
                        Category = (int)CategoryType.AirFare,
                        Description = "Airfare to Chicago",
                        Employee = employee,
                        ExpenseDate = DateTime.Today.AddDays(x - 15),
                        ExpenseType = (int)ChargeType.Business,
                        Location = "Chicago, IL",
                        Merchant = "Blue Yonder Airlines",
                        Notes = string.Empty,
                        ReceiptRequired = true,
                        TransactionAmount = 350M,
                    });

                expenseReport.Charges.Add(
                    new Charge()
                    {
                        AccountNumber = 723000,
                        BilledAmount = 50M,
                        Category = (int)CategoryType.OtherTravelAndLodging,
                        Description = "Cab from airport",
                        Employee = employee,
                        ExpenseDate = DateTime.Today.AddDays(x - 5),
                        ExpenseType = (int)ChargeType.Business,
                        Location = "Chicago, IL",
                        Merchant = "Contoso Taxi",
                        Notes = string.Empty,
                        ReceiptRequired = false,
                        TransactionAmount = 50M,
                    });

                expenseReport.Charges.Add(
                    new Charge()
                    {
                        AccountNumber = 723000,
                        BilledAmount = 50M,
                        Category = (int)CategoryType.OtherTravelAndLodging,
                        Description = "Cab to airport",
                        Employee = employee,
                        ExpenseDate = DateTime.Today.AddDays(x - 3),
                        ExpenseType = (int)ChargeType.Business,
                        Location = "Chicago, IL",
                        Merchant = "Contoso Taxi",
                        Notes = string.Empty,
                        ReceiptRequired = false,
                        TransactionAmount = 50M,
                    });

                expenseReport.Charges.Add(
                    new Charge()
                    {
                        AccountNumber = 723000,
                        BilledAmount = 400M,
                        Category = (int)CategoryType.Hotel,
                        Description = "2 nights hotel",
                        Employee = employee,
                        ExpenseDate = DateTime.Today.AddDays(x - 3),
                        ExpenseType = (int)ChargeType.Business,
                        Location = "Chicago, IL",
                        Merchant = "Northwind Inn",
                        Notes = string.Empty,
                        ReceiptRequired = true,
                        TransactionAmount = 400M,
                    });

                x -= 60;
            }

            // Add 18 months of cell phone charges
            x = -30;
            for (int i = 1; i <= 18; i++)
            {
                expenseReport =
                    new ExpenseReport()
                    {
                        Amount = 850M,
                        Approver = managerAlias,
                        CostCenter = 50992,
                        DateSubmitted = DateTime.Today.AddDays(x - 5),
                        DateResolved = DateTime.Today.AddDays(x),
                        Notes = "",
                        OwedToCreditCard = 0,
                        OwedToEmployee = 50M,
                        Purpose = "Last month's cell phone",
                        Status = (int)ExpenseReportStatus.Approved,
                    };
                employee.ExpenseReports.Add(expenseReport);

                expenseReport.Charges.Add(
                    new Charge()
                    {
                        AccountNumber = 742000,
                        BilledAmount = 50M,
                        Category = (int)CategoryType.OtherTravelAndLodging,
                        Description = "Cell phone bill",
                        Employee = employee,
                        ExpenseDate = DateTime.Today.AddDays(x - 10),
                        ExpenseType = (int)ChargeType.Personal,
                        Location = "Seattle, WA",
                        Merchant = "The Phone Company",
                        Notes = string.Empty,
                        ReceiptRequired = true,
                        TransactionAmount = 50M,
                    });

                x -= 30;
            }

            using (DbDataContext db = new DbDataContext())
            {
                db.Employees.InsertOnSubmit(employee);
                db.SubmitChanges();
            }

            this.GetEmployee("kimakers");

            return employee;
        }
    }
}