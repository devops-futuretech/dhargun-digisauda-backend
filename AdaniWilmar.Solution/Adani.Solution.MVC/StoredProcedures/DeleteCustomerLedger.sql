USE [AdaniDB]
GO

/****** Object:  StoredProcedure [dbo].[DeleteCustomerLedger]    Script Date: 05-09-2022 13:35:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[DeleteCustomerLedger]
	as
DECLARE @daysCount bigint

Set NOCOUNT OFF

BEGIN TRANSACTION


--Get Days Count

IF NOT EXISTS(Select 1 FROM Configurations Where [Name]= 'Customer Ledger Delete Days Count')
BEGIN
	ROLLBACK
	SELECT  'Failed, Days Count Not Found' as 'Message'      
	RETURN
END
ELSE
	Select @daysCount = [Value] From Configurations Where [Name]= 'Customer Ledger Delete Days Count'

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT 'Failed In Configurations' as 'Message'      
RETURN 
END 

IF(@daysCount > 0)
BEGIN
	DELETE FROM CustomerLedgers WHERE  CreatedDate <= DATEADD(d, -@daysCount, getdate())
END
ELSE
BEGIN
	SELECT 'Failed, Set the Days Count' as 'Message'
END

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT  'Failed in Delete Customer Ledger' as 'Message'     
RETURN 
END 
COMMIT

GO


