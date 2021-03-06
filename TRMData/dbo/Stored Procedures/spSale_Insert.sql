﻿CREATE PROCEDURE [dbo].[spSale_Insert]
	@Id int output,
	@UserId nvarchar(128),
	@SaleDate datetime2(7),
	@SubTotal money,
	@Tax money,
	@Total money
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Sale](UserId, SaleDate, SubTotal, Tax, Total)
	VALUES (@UserId, @SaleDate, @SubTotal, @Tax, @Total);

	--SELECT @Id = @@Identity;
	SELECT @Id = Scope_Identity();

	--INSERT INTO dbo.SaleDetail(SaleId, ProductId, Quantity, PurchasePrice, Tax)
	--SELECT @Id, ProductId, Quantity, PurchasePrice, Tax
	--FROM @TVP
END