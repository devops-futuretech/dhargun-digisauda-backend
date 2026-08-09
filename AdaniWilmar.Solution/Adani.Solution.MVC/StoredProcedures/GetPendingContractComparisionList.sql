USE [AdaniDB]
GO

/****** Object:  StoredProcedure [dbo].[GetPendingContractComparisionList]    Script Date: 25-07-2022 07:25:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPendingContractComparisionList]
	-- Add the parameters for the stored procedure here
	@VerticalId BigInt,
	@SalesOrganizationId bigint,
	@DistributionChannelId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
Select 
	PC.CustomerCode As SAPDealerCode,
	PC.CustomerName As SAPDealerName,
	PC.BrokerCode As SAPBrokerCode,
	PC.SaudaNumber As SAPContractNumber,
	PC.SaudaDate As SAPContractDate,
	PC.MaterialCode As SAPMaterialCode,
	PC.MaterialDescription1 As SAPMaterialDescription,
	PC.MaterialGroupDescription2 As SAPOilType,
	PC.SaudaQuantity As SAPContractQuantity,
	PC.DespatchQty As SAPDespatchQuantity,
	PC.PendingQuantityInCase As SAPPendingQuantity,
	PC.PendingQuantityInMT As SAPPendingQuantityMT,
	U.Code As DealerCode,
	U.Name As DealerName,
	BR.Code As BrokerCode,
	SO.SaudaNumber As ContractNumber,
	SO.CreatedDate As ContractDate,
	sku.SkuCode As MaterialCode,
	sku.SkuName As MaterialDescription,
	OT.Name As OilType,
	SO.BidQuantityCase As ContractQuantity,
Sum(SOLR.LiftingQuantityCase) As DespatchQuantity,
	SO.BidQuantityCase - Sum(SOLR.LiftingQuantityCase)  As PendingQuantity,
	SO.BidQuantity - Sum(SOLR.LiftingQuantity) As PendingQuantityMT,
	'' As Status,
	'' As ActionToTaken
	From PendingContracts PC
	Inner Join SaudaOrders SO On PC.SaudaOrderId = SO.Id
	Inner Join Saudas S On SO.SaudaId = S.Id
	Inner Join Users U On S.UserId = U.Id
	Inner Join Skus sku On SO.SkuId = sku.Id
	Inner Join OilTypes OT On OT.Id = SO.OilTypeId
	Inner Join SaudaOrderLiftingRequestMappings SOLR On SOLR.SaudaOrderId = SO.Id
	Left Join Users BR On SO.BrokerId = BR.Id
	Where ((@VerticalId = 0) Or (@VerticalId > 0 And sku.DivisionId = @VerticalId And sku.SalesOrganizationId=@SalesOrganizationId And sku.DistributionChannelId=@DistributionChannelId))
	And SOLR.StatusId <> 14
	Group By 
	PC.CustomerCode,
	PC.CustomerName,
	PC.BrokerCode,
	PC.SaudaNumber,
	PC.SaudaDate,
	PC.MaterialCode,
	PC.MaterialDescription1,
	PC.MaterialGroupDescription2,
	PC.SaudaQuantity,
	PC.DespatchQty,
	PC.PendingQuantityInCase,
	PC.PendingQuantityInMT,
	U.Code,
	U.Name,
	BR.Code,
	SO.SaudaNumber,
	SO.CreatedDate,
	sku.SkuCode,
	sku.SkuName,
	OT.Name,
	SO.BidQuantityCase,
	SO.BidQuantity
END

GO


