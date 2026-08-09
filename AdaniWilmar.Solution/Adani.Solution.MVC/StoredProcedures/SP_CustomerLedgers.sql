GO

/****** Object:  StoredProcedure [dbo].[SP_CustomerLedgers]    Script Date: 12/19/2022 4:55:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[SP_CustomerLedgers]
 
    @CustomerLedger UDTT_CustomerLedgers READONLY
 
AS
 
BEGIN
 
    INSERT INTO CustomerLedgers(
	   [Reference]
      ,[PostingDate]
      ,[DueDate]
      ,[DocumentType]
      ,[Balance]
      ,[UserId]
      ,[UserCode]
      ,[CompanyCode]
      ,[Currency]
      ,[Credit]
      ,[Debit]
      ,[CreatedBy]
      ,[CreatedDate])
 
    SELECT [Reference]
      ,convert(datetime,PostingDate,105)
      ,convert(datetime,DueDate,105)
      ,[DocumentType]
      ,[Balance]
      ,[UserId]
      ,[UserCode]
      ,[CompanyCode]
      ,[Currency]
      ,[Credit]
      ,[Debit]
      ,[CreatedBy]
      ,GETDATE() FROM @CustomerLedger
 
END
 
GO


