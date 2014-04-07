CREATE TABLE [dbo].[Charges] (
    [ChargeId]          INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeId]        INT            NOT NULL,
    [ExpenseReportId]   INT            CONSTRAINT [DF_ExpenseItems_ReportNumber] DEFAULT ((0)) NULL,
    [ExpenseDate]       DATE           NOT NULL,
    [Merchant]          NVARCHAR (50)  NOT NULL,
    [Location]          NVARCHAR (50)  NOT NULL,
    [BilledAmount]      SMALLMONEY     NOT NULL,
    [TransactionAmount] SMALLMONEY     NOT NULL,
    [Description]       NVARCHAR (250) NOT NULL,
    [Category]          INT            NOT NULL,
    [AccountNumber]     INT            NOT NULL,
    [ReceiptRequired]   BIT            NOT NULL,
    [Notes]             NVARCHAR (250) NOT NULL,
    [ExpenseType]       INT            CONSTRAINT [DF_ExpenseItems_Type] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Charges] PRIMARY KEY CLUSTERED ([ChargeId] ASC),
    CONSTRAINT [FK_Charges_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([EmployeeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Charges_ExpenseReports] FOREIGN KEY ([ExpenseReportId]) REFERENCES [dbo].[ExpenseReports] ([ExpenseReportId]) ON DELETE CASCADE
);



