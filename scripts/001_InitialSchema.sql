-- Initial schema for the CCMSApp Users table.
-- Run this script against your Azure SQL database before starting the API.

IF OBJECT_ID(N'[dbo].[Users]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Users]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [Email] NVARCHAR(256) NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        [UpdatedAt] DATETIME2 NULL,
        CONSTRAINT [UQ_Users_Email] UNIQUE ([Email])
    );

    CREATE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users]([Email]);
    CREATE NONCLUSTERED INDEX [IX_Users_Name] ON [dbo].[Users]([Name]);
END
GO
