USE [AdaniDB2]
GO

/****** Object:  StoredProcedure [dbo].[SP_Emami_SKUWisePremiumAmountReport]    Script Date: 25-07-2022 16:28:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Emami_SKUWisePremiumAmountReport]
(
	--@FromDate DateTime,
	--@ToDate DateTime,
	@VerticalId BigInt,
	@SalesOrganizationId bigint,
	@DistributionChannelId bigint
)
As
Begin
	Select
	V.[Name] AS Divisions,
	sku.SkuCode,
	sku.SkuName,
	sku.PremiumAmount
	From Skus sku With(NoLock)
	Inner Join Divisions V With(NoLock) On sku.DivisionId = V.Id
	Where ((@VerticalId = 0 And 1 = 1) Or (@VerticalId <> 0 And sku.DivisionId = @VerticalId And sku.SalesOrganizationId=@SalesOrganizationId And sku.DistributionChannelId=@DistributionChannelId))
	And sku.IsActive = 1
	--And Convert(date,sku.CreatedDate) >= Convert(date,@FromDate) And Convert(date,sku.CreatedDate) <= Convert(date,@ToDate)
	Order By sku.CreatedDate Desc
End
GO


