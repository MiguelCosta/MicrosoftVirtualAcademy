CREATE TABLE [dbo].[ExpenseReports] (
    [ExpenseReportId]  INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeId]       INT            NOT NULL,
    [Amount]           SMALLMONEY     NOT NULL,
    [Purpose]          NVARCHAR (50)  NOT NULL,
    [Approver]         NVARCHAR (25)  NOT NULL,
    [CostCenter]       INT            NOT NULL,
    [Notes]            NVARCHAR (250) NOT NULL,
    [DateSubmitted]    DATE           NULL,
    [Status]           INT            NOT NULL,
    [DateResolved]     DATE           NULL,
    [OwedToCreditCard] SMALLMONEY     NOT NULL,
    [OwedToEmployee]   SMALLMONEY     NOT NULL,
    [DateSaved]        DATE           NULL,
    CONSTRAINT [PK_ExpenseReports] PRIMARY KEY CLUSTERED ([ExpenseReportId] ASC),
    CONSTRAINT [FK_ExpenseReports_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([EmployeeId])
);





