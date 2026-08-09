CREATE PROCEDURE [dbo].[RAMaterialCostExport]
	-- Add the parameters for the stored procedure here

	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId int,
	@IsActiveStatus int
	
AS
BEGIN
	
	SET NOCOUNT ON;
	if(@IsActiveStatus = 2 ) --Active records
	Select v.Name as Vertical,
	 ot.Name as OilType,
	 d.Name as Plant,
	 mc.RatePerMt as RateOrMT,
	 mc.ValidFrom as ValidFrom,
	 mc.ValidTo as ValidTo,
	 mc.IsActive as Status,
	 mc.IsPublished as Published
	 From RAMaterialCosts mc 
	 Join Depots d on d.Id = mc.PlantId
	 Join Verticals v on v.Id = mc.VerticalId
	 Join OilTypes ot on ot.Id = mc.OiltypeId 
     Where CAST(@StartDate as date) <= CAST(mc.CreatedDate as date) and CAST(mc.CreatedDate as date) <= CAST(@EndDate as date) and (mc.VerticalId = @VerticalId or @VerticalId = 0) and mc.IsActive = 1
	 ORDER By mc.CreatedDate desc
	 else if(@IsActiveStatus = 3 ) --InActive records
	 Select v.Name as Vertical,
	 ot.Name as OilType,
	 d.Name as Plant,
	 mc.RatePerMt as RateOrMT,
	 mc.ValidFrom as ValidFrom,
	 mc.ValidTo as ValidTo,
	 mc.IsActive as Status,
	 mc.IsPublished as Published
	 From RAMaterialCosts mc 
	 Join Depots d on d.Id = mc.PlantId
	 Join Verticals v on v.Id = mc.VerticalId
	 Join OilTypes ot on ot.Id = mc.OiltypeId 
     Where CAST(@StartDate as date) <= CAST(mc.CreatedDate as date) and CAST(mc.CreatedDate as date) <= CAST(@EndDate as date) and (mc.VerticalId = @VerticalId or @VerticalId = 0) and mc.IsActive = 0
	 ORDER By mc.CreatedDate desc
	else
	Select v.Name as Vertical,
	 ot.Name as OilType,
	 d.Name as Plant,
	 mc.RatePerMt as RateOrMT,
	 mc.ValidFrom as ValidFrom,
	 mc.ValidTo as ValidTo,
	 mc.IsActive as Status,
	 mc.IsPublished as Published
	 From RAMaterialCosts mc 
	 Join Depots d on d.Id = mc.PlantId
	 Join Verticals v on v.Id = mc.VerticalId
	 Join OilTypes ot on ot.Id = mc.OiltypeId 
     Where CAST(@StartDate as date) <= CAST(mc.CreatedDate as date) and CAST(mc.CreatedDate as date) <= CAST(@EndDate as date) and (mc.VerticalId = @VerticalId or @VerticalId = 0)
	 ORDER By mc.CreatedDate desc
END