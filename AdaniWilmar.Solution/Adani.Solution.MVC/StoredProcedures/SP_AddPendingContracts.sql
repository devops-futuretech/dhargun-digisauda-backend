SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


 

CREATE PROCEDURE [dbo].[SP_AddPendingContracts]

    @PendingContracts UDTT_PendingContracts READONLY

AS

BEGIN

    INSERT INTO PendingContracts([UserId]
      ,[SaudaOrderId]
      ,[SaudaNumber]
      ,[MaterialCode]
      ,[CustomerCode]
      ,[CustomerName]
      ,[ContractValidTo]
	  ,[ContractValidFrom]
      ,[BasicRate]
      ,[PendingQuantityInCase]
      ,[SaudaQuantity]
      ,[SalesOrgId]
      ,[DistChnlId]
      ,[DivisionId]
      ,[TotalValue]
      ,[IsSaudaExtended]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate])

    SELECT [UserId]
      ,[SaudaOrderId]
      ,[SaudaNumber]
      ,[MaterialCode]
      ,[CustomerCode]
      ,[CustomerName]
      ,convert(datetime,[ContractValidTo],105)
	  ,convert(datetime,[ContractValidFrom],105)
      ,[BasicRate]
      ,[PendingQuantityInCase]
      ,[SaudaQuantity]
      ,[SalesOrgId]
      ,[DistChnlId]
      ,[DivisionId]
      ,[TotalValue]
      ,[IsSaudaExtended]
      ,[CreatedBy]
      ,GetDate()
      ,[ModifiedBy]
      ,GetDate() FROM @PendingContracts

END

GO


