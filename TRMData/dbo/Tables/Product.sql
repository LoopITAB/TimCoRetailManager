﻿CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProductName] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(100) NOT NULL, 
    [RetailPrice] MONEY NOT NULL, 
    [QuantityInStock] INT NOT NULL DEFAULT 1, 
    [CreateDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [LastModified] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [IsTaxable] BIT NOT NULL DEFAULT 1 
)
