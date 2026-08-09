GO

/****** Object:  StoredProcedure [dbo].[SP_AddOpenCreditMasters]    Script Date: 12/19/2022 4:54:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_AddOpenCreditMasters]
 
    @OpenCreditMaster UDTT_OpenCreditMaster READONLY
 
AS
 
BEGIN
 
    INSERT INTO UserCreditMasters(
	   [UserId]
      ,[SalesOrgId]
      ,[DistChnlId]
      ,[DivisionId]
      ,[CreditLimit]
      ,[CreditExposure]
      ,[OpenOrders]
      ,[DeliveryValue]
      ,[BillingDocumentValue]
      ,[AvailableCreditLimit]     
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate],
	  [Isactive],
	  [IsSAPData],
	  [SalesValue],
	  [TotalReceivable]
      ,[SaudaDepC]
      ,[SecDepH]
      ,[BankGuarM]
      ,[AdvanceA]
      ,[DueToday]
      ,[TomorrowsDue]
      ,[Overdue]
      ,[NotDue]
      ,[NextIntRev]
      ,[Blocked]
      ,[TotalLimit]
      ,[IndividLimit])
 
    SELECT [UserId]
      ,[SalesOrgId]
      ,[DistChnlId]
      ,[DivisionId]
      ,[CreditLimit]
      ,[CreditExposure]
      ,[OpenOrders]
      ,[DeliveryValue]
      ,[BillingDocumentValue]
      ,[AvailableCreditLimit]     
      ,[CreatedBy]
      ,GetDate()
      ,[ModifiedBy]
      ,GetDate(),1,1,0,0,0,0,0,0,0,0,0,0,'','',0,0 FROM @OpenCreditMaster
 
END
 
GO


