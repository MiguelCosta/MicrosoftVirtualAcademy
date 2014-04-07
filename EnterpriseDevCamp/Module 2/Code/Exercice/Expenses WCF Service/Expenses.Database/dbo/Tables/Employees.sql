CREATE TABLE [dbo].[Employees] (
    [EmployeeId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Alias]      NVARCHAR (25) NOT NULL,
    [Manager]    NVARCHAR (25) NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([EmployeeId] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Employees]
    ON [dbo].[Employees]([Alias] ASC);

