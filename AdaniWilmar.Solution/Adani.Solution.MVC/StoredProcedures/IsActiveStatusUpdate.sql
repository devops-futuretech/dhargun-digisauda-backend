/****** Object:  StoredProcedure [dbo].[IsActiveStatusUpdate]    Script Date: 09-03-2020 10:20:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[IsActiveStatusUpdate]
	
AS
BEGIN
	

	update CounterBidJumps set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo < GETDATE() and  IsActive = 1
	update BaseGroupMargins set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update PercentileNumbers set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update GuaranteePriceJumps set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update MaterialCosts set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update PackingCosts set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update PrimaryFreights set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update SecondaryFreights set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update DepotCosts set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update DetentionCosts set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update HoneycombCosts set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo < GETDATE() and  IsActive = 1
	update SchemeCosts set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update ProfitMargins set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo < GETDATE() and  IsActive = 1
	update CushionMargins set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo < GETDATE() and  IsActive = 1
	update LoadCapacityConversions set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo  < GETDATE() and  IsActive = 1
	update RaMargins set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo < GETDATE() and  IsActive = 1
	update IngredientCosts set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo < GETDATE() and  IsActive = 1
    update SkuIngrediantPlants set IsActive = 0 ,ModifiedBy = 1,ModifiedDate = GETDATE() where ValidTo < GETDATE() and  IsActive = 1

END
