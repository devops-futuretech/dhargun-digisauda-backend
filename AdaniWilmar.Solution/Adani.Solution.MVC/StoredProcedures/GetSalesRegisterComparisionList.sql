USE [AdaniDB]
GO

/****** Object:  StoredProcedure [dbo].[GetSalesRegisterComparisionList]    Script Date: 25-07-2022 08:52:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetSalesRegisterComparisionList]  
 @StartDate DateTime,  
 @EndDate DateTime,  
 @VerticalId BigInt,
 @SalesOrganizationId bigint,
 @DistributionChannelId bigint
  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 Select    
SR.BillingType,  
SR.Contractnumber,  
SR.SalesOrderNo As DONumber,  
SR.BillNumber,  
( CASE WHEN SR.BillingDate = null THEN '' ELSE SR.BillingDate END) AS BillingDate,  
SR.QuantityCase,  
SR.QuantityMT,  
SR.OilTypeDesc,  
SR.BillToParty ,  
SR.BillToPartyDescription,  
SR.StateofShipparty,  
I.BillingDocument As InvBillNumber,  
ID.QuantityInCase As InvQuantityInCase  
From SalesRegisters SR  
Inner Join Invoices I On I.BillingDocument = SR.BillNumber  
Inner Join InvoiceDetails ID On I.Id = ID.InvoiceId And SR.MaterialCode = ID.MaterialNumber  And SR.QuantityCase = ID.QuantityInCase
Inner Join SaudaOrders SO On SO.Id = ID.SaudaOrderId And SR.Contractnumber = SO.SaudaNumber
Inner Join OilTypes OT On ID.OilTypeId = OT.Id  
where Convert(date,SR.BillingDate) Between Convert(date,@StartDate) and Convert(date,@EndDate)  
And ((@VerticalId = 0 And 1 = 1) Or (@VerticalId > 0 And OT.DivisionId = @VerticalId And OT.SalesOrganizationId=@SalesOrganizationId And OT.DistributionChannelId=@DistributionChannelId))
order by SR.BillingDate 
     
END
GO


