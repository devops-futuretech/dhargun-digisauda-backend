IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_Emami_DepotCostReport')
    BEGIN
        DROP  Procedure SP_Emami_DepotCostReport
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure SP_Emami_DepotCostReport
(
	@FromDate DateTime,
	@ToDate DateTime,
	@VerticalId BigInt = 0
)
As
Begin
	Declare @UOMId Int = 0
	Declare @RelationalUOMId Int = 0

	Select @UOMId = Id From Uoms Where Name = 'MT'
	Select @RelationalUOMId = Id From Uoms Where Name = 'NOS'

	Select
	 DC.CreatedDate As DateOfUpload, 
	 D.Name As DepotName,
	 D.Code As DepotCode,
	 sku.SkuCode As MaterialCode,
	 sku.SkuName As MaterialDescription,
	 ST.StateName,
	 --Case When sku.UOMId = 2 Then OT.LitreConversion / (sku.Quantity * 1) Else 1000 / (sku.Quantity * 1) End 
	 --0 As DepotCostPerCase,
	 (Select Convert(decimal(18,2),(DC.RatePerMt / ConversionFactor)) From SkuUomMappings Where SkuId = sku.Id And UOMId = @UOMId And RelationUomId = @RelationalUOMId) As DepotCostPerCase,
	 DC.RatePerMt As DepotCostPerMT
	From DepotCosts DC With(NoLock)
	Inner Join Depots D With(NoLock) On DC.DepotId = D.Id And D.IsPlant = 0
	Inner Join Verticals V With(NoLock) On DC.VerticalId = V.Id
	Inner Join Skus sku With(NoLock) On 1 = 1
	Inner Join OilTypes OT With(NoLock) On sku.OilTypeId = OT.Id
	Inner Join Depots DT With(NoLock) On DT.Id = DC.DepotId
	Inner Join States ST With(NoLock) On DT.StateId = ST.Id
	Where ((@VerticalId = 0 And 1 = 1) Or (@VerticalId <> 0 And DC.VerticalId = @VerticalId))
	And DC.IsActive = 1
	And Convert(date,DC.CreatedDate) >= Convert(date,@FromDate) And Convert(date,DC.CreatedDate) <= Convert(date,@ToDate)
	Order By DC.CreatedDate Desc
End

