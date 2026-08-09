/****** Object:  StoredProcedure [dbo].[MaterialCostExport]    Script Date: 12-09-2019 15:51:26 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'MaterialCostExport')
    BEGIN
        DROP  Procedure MaterialCostExport
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MaterialCostExport]
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
	 From MaterialCosts mc 
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
	 From MaterialCosts mc 
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
	 From MaterialCosts mc 
	 Join Depots d on d.Id = mc.PlantId
	 Join Verticals v on v.Id = mc.VerticalId
	 Join OilTypes ot on ot.Id = mc.OiltypeId 
     Where CAST(@StartDate as date) <= CAST(mc.CreatedDate as date) and CAST(mc.CreatedDate as date) <= CAST(@EndDate as date) and (mc.VerticalId = @VerticalId or @VerticalId = 0)
	 ORDER By mc.CreatedDate desc
END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PackingCostExport]

	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId int,
	@IsActiveStatus int
AS
BEGIN
	
	SET NOCOUNT ON;

	if(@IsActiveStatus = 2) --Active records
	Select v.Name as Vertical
	,ot.Name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	d.Name as Plant,
	pc.ActualPackingCost as ActualPackingCost,
	pc.SalesPackingCost as SalesPackingCost,
	pc.ValidFrom as ValidFrom,
	pc.ValidTo as ValidTo,
	pc.IsActive as Status,
	pc.IsPublished as Published
	from PackingCosts pc
	Join Verticals v on v.Id = pc.VerticalId 
	Join OilTypes ot on ot.Id = pc.OilTypeId
	Join Skus s on s.Id = pc.SkuId
	Join Depots d on d.Id = pc.PlantId
	Where CAST(@Startdate as date) <= CAST(pc.CreatedDate as date) and CAST(pc.CreatedDate as date) <= CAST(@Enddate as date) and (pc.VerticalId = @VerticalId or @VerticalId = 0) and pc.IsActive = 1
	ORDER By pc.CreatedDate desc
	else if(@IsActiveStatus = 3) --InActive records
	Select v.Name as Vertical
	,ot.Name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	d.Name as Plant,
	pc.ActualPackingCost as ActualPackingCost,
	pc.SalesPackingCost as SalesPackingCost,
	pc.ValidFrom as ValidFrom,
	pc.ValidTo as ValidTo,
	pc.IsActive as Status,
	pc.IsPublished as Published
	from PackingCosts pc
	Join Verticals v on v.Id = pc.VerticalId 
	Join OilTypes ot on ot.Id = pc.OilTypeId
	Join Skus s on s.Id = pc.SkuId
	Join Depots d on d.Id = pc.PlantId
	Where CAST(@Startdate as date) <= CAST(pc.CreatedDate as date) and CAST(pc.CreatedDate as date) <= CAST(@Enddate as date) and (pc.VerticalId = @VerticalId or @VerticalId = 0) and pc.IsActive = 0
	ORDER By pc.CreatedDate desc
	else
	Select v.Name as Vertical
	,ot.Name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	d.Name as Plant,
	pc.ActualPackingCost as ActualPackingCost,
	pc.SalesPackingCost as SalesPackingCost,
	pc.ValidFrom as ValidFrom,
	pc.ValidTo as ValidTo,
	pc.IsActive as Status,
	pc.IsPublished as Published
	from PackingCosts pc
	Join Verticals v on v.Id = pc.VerticalId 
	Join OilTypes ot on ot.Id = pc.OilTypeId
	Join Skus s on s.Id = pc.SkuId
	Join Depots d on d.Id = pc.PlantId
	Where CAST(@Startdate as date) <= CAST(pc.CreatedDate as date) and CAST(pc.CreatedDate as date) <= CAST(@Enddate as date) and (pc.VerticalId = @VerticalId or @VerticalId = 0)
	ORDER By pc.CreatedDate desc
END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PrimaryFreightExport] 
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime,	
	@VerticalId int,
	@IsActiveStatus int
AS
BEGIN
	
	SET NOCOUNT ON;
  if(@IsActiveStatus = 2) -- Active records
   select v.Name as Vertical,
   dp.Name as Plant,
   d.Name  as DepotRakeName,
   d.Code as DepotRakeCode,
   tm.Name as TransportMode,
   pf.LoadCapacity as TruckCapacity,
   v.Name as BusinessVertical,
   pf.ActualFreight as ActualFreightInMT,
   pf.SalesFreight as SalesFreightInMT,
   pf.ValidFrom  as ValidFrom,
   pf.ValidTo as ValidTo,
   pf.IsActive as Status,
   pf.IsPublished as Published
   From PrimaryFreights pf 
	Join Depots dp on dp.Id = pf.PlantId
	Join Depots d on d.Id = pf.DepotId 
	Join Verticals v on v.Id = pf.VerticalId 
	Join TransportModes tm on tm.Id = pf.TransportModeId 
	Where CAST(@Startdate as date) <= CAST(pf.CreatedDate as date) and CAST(pf.CreatedDate as date) <= CAST(@Enddate as date) and (pf.VerticalId = @VerticalId or @VerticalId = 0) and pf.IsActive = 1
	 ORDER By pf.CreatedDate desc
  else  if(@IsActiveStatus = 3) -- InActive records
   select v.Name as Vertical,
   dp.Name as Plant,
   d.Name  as DepotRakeName,
   d.Code as DepotRakeCode,
   tm.Name as TransportMode,
   pf.LoadCapacity as TruckCapacity,
   v.Name as BusinessVertical,
   pf.ActualFreight as ActualFreightInMT,
   pf.SalesFreight as SalesFreightInMT,
   pf.ValidFrom  as ValidFrom,
   pf.ValidTo as ValidTo,
   pf.IsActive as Status,
   pf.IsPublished as Published
   From PrimaryFreights pf 
	Join Depots dp on dp.Id = pf.PlantId
	Join Depots d on d.Id = pf.DepotId 
	Join Verticals v on v.Id = pf.VerticalId 
	Join TransportModes tm on tm.Id = pf.TransportModeId 
	Where CAST(@Startdate as date) <= CAST(pf.CreatedDate as date) and CAST(pf.CreatedDate as date) <= CAST(@Enddate as date) and (pf.VerticalId = @VerticalId or @VerticalId = 0) and pf.IsActive = 0
	 ORDER By pf.CreatedDate desc
	else 
   select v.Name as Vertical,
   dp.Name as Plant,
   d.Name  as DepotRakeName,
   d.Code as DepotRakeCode,
   tm.Name as TransportMode,
   pf.LoadCapacity as TruckCapacity,
   v.Name as BusinessVertical,
   pf.ActualFreight as ActualFreightInMT,
   pf.SalesFreight as SalesFreightInMT,
   pf.ValidFrom  as ValidFrom,
   pf.ValidTo as ValidTo,
   pf.IsActive as Status,
   pf.IsPublished as Published
   From PrimaryFreights pf 
	Join Depots dp on dp.Id = pf.PlantId
	Join Depots d on d.Id = pf.DepotId 
	Join Verticals v on v.Id = pf.VerticalId 
	Join TransportModes tm on tm.Id = pf.TransportModeId 
	Where CAST(@Startdate as date) <= CAST(pf.CreatedDate as date) and CAST(pf.CreatedDate as date) <= CAST(@Enddate as date) and (pf.VerticalId = @VerticalId or @VerticalId = 0) 
	 ORDER By pf.CreatedDate desc

END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SecondaryFreightExport]
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId int,
	@IsActiveStatus int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if(@IsActiveStatus = 2) --Active records
	select v.Name as Vertical,
	d.Name as SourceName,
	d.Code as SourceCode,
	z.Name as Zone,
	s.StateName as State,
	fz.Name as FreightZone,
	fr.Name as FreightRoute,
	sf.ActualFreight as ActualFreightInMT,
	sf.SalesFreight as SalesFreightInMT,
	tm.Name as TransportMode,
	sf.Capacity as TruckCapacity,
	v.Name as BusinessVertical,
	sf.ValidFrom as ValidFrom,
	sf.ValidTo as ValidTo,
	sf.IsActive as Status,
	sf.IsPublished as Published
	from SecondaryFreights sf 
   Join Depots d on d.Id = sf.DepotId
   Join FreightZones fz on fz.Id = sf.FreightZoneId
   Join FreightRoutes fr on fr.Id = sf.FreightRouteId 
   Join TransportModes tm on tm.Id = sf.TransportModeId 
   Join Verticals v on v.Id = sf.VerticalId
   Join Zones z on z.Id = sf.ZoneId
   Join States s on s.Id = sf.StateId 
   Where CAST(@Startdate as date) <= CAST(sf.CreatedDate as date) and CAST(sf.CreatedDate as date) <= CAST(@Enddate as date) and (sf.VerticalId = @VerticalId or @VerticalId = 0) and sf.IsActive = 1
    ORDER By sf.CreatedDate desc 
  else if(@IsActiveStatus = 3) --InActive records
	select v.Name as Vertical,
	d.Name as SourceName,
	d.Code as SourceCode,
	z.Name as Zone,
	s.StateName as State,
	fz.Name as FreightZone,
	fr.Name as FreightRoute,
	sf.ActualFreight as ActualFreightInMT,
	sf.SalesFreight as SalesFreightInMT,
	tm.Name as TransportMode,
	sf.Capacity as TruckCapacity,
	v.Name as BusinessVertical,
	sf.ValidFrom as ValidFrom,
	sf.ValidTo as ValidTo,
	sf.IsActive as Status,
	sf.IsPublished as Published
	from SecondaryFreights sf 
   Join Depots d on d.Id = sf.DepotId
   Join FreightZones fz on fz.Id = sf.FreightZoneId
   Join FreightRoutes fr on fr.Id = sf.FreightRouteId 
   Join TransportModes tm on tm.Id = sf.TransportModeId 
   Join Verticals v on v.Id = sf.VerticalId
   Join Zones z on z.Id = sf.ZoneId
   Join States s on s.Id = sf.StateId 
   Where CAST(@Startdate as date) <= CAST(sf.CreatedDate as date) and CAST(sf.CreatedDate as date) <= CAST(@Enddate as date) and (sf.VerticalId = @VerticalId or @VerticalId = 0) and sf.IsActive = 0
    ORDER By sf.CreatedDate desc
	else
	select v.Name as Vertical,
	d.Name as SourceName,
	d.Code as SourceCode,
	z.Name as Zone,
	s.StateName as State,
	fz.Name as FreightZone,
	fr.Name as FreightRoute,
	sf.ActualFreight as ActualFreightInMT,
	sf.SalesFreight as SalesFreightInMT,
	tm.Name as TransportMode,
	sf.Capacity as TruckCapacity,
	v.Name as BusinessVertical,
	sf.ValidFrom as ValidFrom,
	sf.ValidTo as ValidTo,
	sf.IsActive as Status,
	sf.IsPublished as Published
	from SecondaryFreights sf 
   Join Depots d on d.Id = sf.DepotId
   Join FreightZones fz on fz.Id = sf.FreightZoneId
   Join FreightRoutes fr on fr.Id = sf.FreightRouteId 
   Join TransportModes tm on tm.Id = sf.TransportModeId 
   Join Verticals v on v.Id = sf.VerticalId
   Join Zones z on z.Id = sf.ZoneId
   Join States s on s.Id = sf.StateId 
   Where CAST(@Startdate as date) <= CAST(sf.CreatedDate as date) and CAST(sf.CreatedDate as date) <= CAST(@Enddate as date) and (sf.VerticalId = @VerticalId or @VerticalId = 0) 
    ORDER By sf.CreatedDate desc

END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DepotCostExport] 
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId int,
	@IsActiveStatus int
AS
BEGIN
	SET NOCOUNT ON;
	if(@IsActiveStatus = 2) --Active records
	Select v.name as Vertical,
	d.Name as DepotRakeName,
	d.Code as DepotRakeCode,
	dc.RatePerMt as CostOrMT,
	v.Name as BusinessVertical,
	dc.ValidFrom as ValidFrom,
	dc.ValidTo as ValidTo,
	dc.IsActive as Status,
	dc.IsPublished as Published
	From DepotCosts dc
   Join Depots d on d.Id = dc.DepotId
   Join Verticals v on v.Id = dc.VerticalId
   Where CAST(@Startdate as date) <= CAST(dc.CreatedDate as date) and CAST(dc.CreatedDate as date) <= CAST(@Enddate as date) and (dc.VerticalId = @VerticalId or @VerticalId = 0) and dc.IsActive = 1
   ORDER By dc.CreatedDate desc
   else if(@IsActiveStatus = 3)--InActive records
	Select v.name as Vertical,
	d.Name as DepotRakeName,
	d.Code as DepotRakeCode,
	dc.RatePerMt as CostOrMT,
	v.Name as BusinessVertical,
	dc.ValidFrom as ValidFrom,
	dc.ValidTo as ValidTo,
	dc.IsActive as Status,
	dc.IsPublished as Published
	From DepotCosts dc
   Join Depots d on d.Id = dc.DepotId
   Join Verticals v on v.Id = dc.VerticalId
   Where CAST(@Startdate as date) <= CAST(dc.CreatedDate as date) and CAST(dc.CreatedDate as date) <= CAST(@Enddate as date) and (dc.VerticalId = @VerticalId or @VerticalId = 0) and dc.IsActive = 0
   ORDER By dc.CreatedDate desc
   else
	Select v.name as Vertical,
	d.Name as DepotRakeName,
	d.Code as DepotRakeCode,
	dc.RatePerMt as CostOrMT,
	v.Name as BusinessVertical,
	dc.ValidFrom as ValidFrom,
	dc.ValidTo as ValidTo,
	dc.IsActive as Status,
	dc.IsPublished as Published
	From DepotCosts dc
   Join Depots d on d.Id = dc.DepotId
   Join Verticals v on v.Id = dc.VerticalId
   Where CAST(@Startdate as date) <= CAST(dc.CreatedDate as date) and CAST(dc.CreatedDate as date) <= CAST(@Enddate as date) and (dc.VerticalId = @VerticalId or @VerticalId = 0) 
   ORDER By dc.CreatedDate desc
END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DetentionCostExport] 
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId int,
	@IsActiveStatus int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if(@IsActiveStatus = 2) --Active records
	Select v.name as Vertical,
	d.Name as DepotRakeName
	,d.Code as DepotRakeCode,
	dc.RatePerMt as CostOrMT,
	v.Name as BusinessVertical,
	dc.ValidFrom as ValidFrom,
	dc.ValidTo as ValidTo,
	dc.IsActive as Status,
	dc.IsPublished as Published
	From DetentionCosts dc 
    Join Depots d on d.Id = dc.DepotId
	Join Verticals v on v.Id = dc.VerticalId 
	Where CAST(@Startdate as date) <= CAST(dc.CreatedDate as date) and CAST(dc.CreatedDate as date) <= CAST(@Enddate as date) and (dc.VerticalId = @VerticalId or @VerticalId = 0) and dc.IsActive = 1
	ORDER By dc.CreatedDate desc
	else if(@IsActiveStatus = 3) --InActive records
	Select v.name as Vertical,
	d.Name as DepotRakeName
	,d.Code as DepotRakeCode,
	dc.RatePerMt as CostOrMT,
	v.Name as BusinessVertical,
	dc.ValidFrom as ValidFrom,
	dc.ValidTo as ValidTo,
	dc.IsActive as Status,
	dc.IsPublished as Published
	From DetentionCosts dc 
    Join Depots d on d.Id = dc.DepotId
	Join Verticals v on v.Id = dc.VerticalId 
	Where CAST(@Startdate as date) <= CAST(dc.CreatedDate as date) and CAST(dc.CreatedDate as date) <= CAST(@Enddate as date) and (dc.VerticalId = @VerticalId or @VerticalId = 0) and dc.IsActive = 0
	ORDER By dc.CreatedDate desc
	else
	Select v.name as Vertical,
	d.Name as DepotRakeName
	,d.Code as DepotRakeCode,
	dc.RatePerMt as CostOrMT,
	v.Name as BusinessVertical,
	dc.ValidFrom as ValidFrom,
	dc.ValidTo as ValidTo,
	dc.IsActive as Status,
	dc.IsPublished as Published
	From DetentionCosts dc 
    Join Depots d on d.Id = dc.DepotId
	Join Verticals v on v.Id = dc.VerticalId 
	Where CAST(@Startdate as date) <= CAST(dc.CreatedDate as date) and CAST(dc.CreatedDate as date) <= CAST(@Enddate as date) and (dc.VerticalId = @VerticalId or @VerticalId = 0) 
	ORDER By dc.CreatedDate desc
END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CushionMarginExport]
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId bigint,
	@IsActiveStatus int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

 if(@IsActiveStatus = 2) --Active records
	select v.Name as Vertical,
	ot.Name as OilType,
	pg.Name as BPOrCPWise,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	z.Name as Zone,
	st.StateName as State,
	cm.RatePerMt as PricePerMT,
	cm.ValidFrom as ValidFrom,
	cm.ValidTo as ValidTo,
	cm.IsActive as Status,
	cm.IsPublished as Published
	From CushionMargins cm
	 Join Verticals v on v.Id = cm.VerticalId
	 Join OilTypes ot on ot.id =cm.OilTypeId 
	 Join PackGroups pg on pg.Id = cm.OilPackingTypeId 
	 Join Skus s on s.Id = cm.SkuId
	 Join Zones z on z.Id = cm.ZoneId 
	 Join States st on st.Id = cm.StateId 
	 Where CAST(@Startdate as date) <= CAST(cm.CreatedDate as date) and CAST(cm.CreatedDate as date) <= CAST(@Enddate as date)  and (cm.VerticalId = @VerticalId or @VerticalId = 0) and cm.IsActive = 1 
	 ORDER By cm.CreatedDate desc 
  ELSE IF(@IsActiveStatus = 3) --InActive records
    select v.Name as Vertical,
	ot.Name as OilType,
	pg.Name as BPOrCPWise,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	z.Name as Zone,
	st.StateName as State,
	cm.RatePerMt as PricePerMT,
	cm.ValidFrom as ValidFrom,
	cm.ValidTo as ValidTo,
	cm.IsActive as Status,
	cm.IsPublished as Published
	From CushionMargins cm
	 Join Verticals v on v.Id = cm.VerticalId
	 Join OilTypes ot on ot.id =cm.OilTypeId 
	 Join PackGroups pg on pg.Id = cm.OilPackingTypeId 
	 Join Skus s on s.Id = cm.SkuId
	 Join Zones z on z.Id = cm.ZoneId 
	 Join States st on st.Id = cm.StateId 
	 Where CAST(@Startdate as date) <= CAST(cm.CreatedDate as date) and CAST(cm.CreatedDate as date) <= CAST(@Enddate as date)  and (cm.VerticalId = @VerticalId or @VerticalId = 0) and cm.IsActive = 0
	 ORDER By cm.CreatedDate desc 
	ELSE 
    select v.Name as Vertical,
	ot.Name as OilType,
	pg.Name as BPOrCPWise,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	z.Name as Zone,
	st.StateName as State,
	cm.RatePerMt as PricePerMT,
	cm.ValidFrom as ValidFrom,
	cm.ValidTo as ValidTo,
	cm.IsActive as Status,
	cm.IsPublished as Published
	From CushionMargins cm
	 Join Verticals v on v.Id = cm.VerticalId
	 Join OilTypes ot on ot.id =cm.OilTypeId 
	 Join PackGroups pg on pg.Id = cm.OilPackingTypeId 
	 Join Skus s on s.Id = cm.SkuId
	 Join Zones z on z.Id = cm.ZoneId 
	 Join States st on st.Id = cm.StateId 
	 Where CAST(@Startdate as date) <= CAST(cm.CreatedDate as date) and CAST(cm.CreatedDate as date) <= CAST(@Enddate as date)  and (cm.VerticalId = @VerticalId or @VerticalId = 0) 
	 ORDER By cm.CreatedDate desc 
END

 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ProfitMarginExport]
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId bigint,
	@IsActiveStatus int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

 if(@IsActiveStatus = 2) --Active records
	Select v.Name as Vertical,
	ot.Name as OilType,
	pg.Name as BPOrCPWise,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	pm.RatePerMt as MarginOrMT,
	st.StateName as State,
	pm.ValidFrom as ValidFrom,
	pm.ValidTo as ValidTo,
	pm.IsActive as Status,
	pm.IsPublished as Published
	From ProfitMargins pm 
    Join Verticals v on v.Id = pm.VerticalId
	Join OilTypes ot on ot.Id = pm.OilTypeId 
	Join PackGroups pg on pg.Id = pm.OilPackingTypeId 
	Join skus s on s.id = pm.SkuId 
	Join States st on st.Id = pm.StateId 
	 Where CAST(@Startdate as date) <= CAST(pm.CreatedDate as date) and CAST(pm.CreatedDate as date) <= CAST(@Enddate as date) and (pm.VerticalId = @VerticalId or @VerticalId = 0) and pm.IsActive = 1
	  ORDER By pm.CreatedDate desc
  ELSE IF(@IsActiveStatus = 3) --InActive records
	  Select v.Name as Vertical,
	ot.Name as OilType,
	pg.Name as BPOrCPWise,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	pm.RatePerMt as MarginOrMT,
	st.StateName as State,
	pm.ValidFrom as ValidFrom,
	pm.ValidTo as ValidTo,
	pm.IsActive as Status,
	pm.IsPublished as Published
	From ProfitMargins pm 
    Join Verticals v on v.Id = pm.VerticalId
	Join OilTypes ot on ot.Id = pm.OilTypeId 
	Join PackGroups pg on pg.Id = pm.OilPackingTypeId 
	Join skus s on s.id = pm.SkuId 
	Join States st on st.Id = pm.StateId 
	 Where CAST(@Startdate as date) <= CAST(pm.CreatedDate as date) and CAST(pm.CreatedDate as date) <= CAST(@Enddate as date) and (pm.VerticalId = @VerticalId or @VerticalId = 0) and pm.IsActive = 0
	  ORDER By pm.CreatedDate desc
   ELSE 
	  Select v.Name as Vertical,
	ot.Name as OilType,
	pg.Name as BPOrCPWise,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	pm.RatePerMt as MarginOrMT,
	st.StateName as State,
	pm.ValidFrom as ValidFrom,
	pm.ValidTo as ValidTo,
	pm.IsActive as Status,
	pm.IsPublished as Published
	From ProfitMargins pm 
    Join Verticals v on v.Id = pm.VerticalId
	Join OilTypes ot on ot.Id = pm.OilTypeId 
	Join PackGroups pg on pg.Id = pm.OilPackingTypeId 
	Join skus s on s.id = pm.SkuId 
	Join States st on st.Id = pm.StateId 
	 Where CAST(@Startdate as date) <= CAST(pm.CreatedDate as date) and CAST(pm.CreatedDate as date) <= CAST(@Enddate as date) and (pm.VerticalId = @VerticalId or @VerticalId = 0) 
	  ORDER By pm.CreatedDate desc
END



/****** Object:  StoredProcedure [dbo].[SchemeCostExport]    Script Date: 12-09-2019 15:55:31 ******/
SET ANSI_NULLS ON
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SchemeCostExport]
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId bigint,
	@IsActiveStatus int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if(@IsActiveStatus = 2) --Active records
	select v.name as Vertical,
	ot.name as OilType,
	pg.Name as PackGroup,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	sc.RatePerMt as CostperMT,
	st.StateName as State,
	sc.ValidFrom as ValidFrom,
	sc.ValidTo as ValidTo, 
	sc.IsActive as Status,
	sc.IsPublished as Published
	From SchemeCosts sc 
	 Join Verticals v on v.Id = sc.VerticalId 
	 Join OilTypes ot on ot.Id = sc.OilTypeId
	 Join PackGroups pg on pg.Id = sc.PackGroupId 
	 Join skus s on s.id = sc.SkuId
	 Join States st on st.Id = sc.StateId
	 Where CAST(@Startdate as date) <= CAST(sc.CreatedDate as date) and CAST(sc.CreatedDate as date) <= CAST(@Enddate as date) and (sc.VerticalId = @VerticalId or @VerticalId = 0) and sc.IsActive = 1 
	 ORDER By sc.CreatedDate desc 
  ELSE IF(@IsActiveStatus = 3) --InActive records
    select v.name as Vertical,
	ot.name as OilType,
	pg.Name as PackGroup,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	sc.RatePerMt as CostperMT,
	st.StateName as State,
	sc.ValidFrom as ValidFrom,
	sc.ValidTo as ValidTo, 
	sc.IsActive as Status,
	sc.IsPublished as Published
	From SchemeCosts sc 
	 Join Verticals v on v.Id = sc.VerticalId 
	 Join OilTypes ot on ot.Id = sc.OilTypeId
	 Join PackGroups pg on pg.Id = sc.PackGroupId 
	 Join skus s on s.id = sc.SkuId
	 Join States st on st.Id = sc.StateId
	 Where CAST(@Startdate as date) <= CAST(sc.CreatedDate as date) and CAST(sc.CreatedDate as date) <= CAST(@Enddate as date) and (sc.VerticalId = @VerticalId or @VerticalId = 0) and sc.IsActive = 0
	 ORDER By sc.CreatedDate desc 
  ELSE 
    select v.name as Vertical,
	ot.name as OilType,
	pg.Name as PackGroup,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	sc.RatePerMt as CostperMT,
	st.StateName as State,
	sc.ValidFrom as ValidFrom,
	sc.ValidTo as ValidTo, 
	sc.IsActive as Status,
	sc.IsPublished as Published
	From SchemeCosts sc 
	 Join Verticals v on v.Id = sc.VerticalId 
	 Join OilTypes ot on ot.Id = sc.OilTypeId
	 Join PackGroups pg on pg.Id = sc.PackGroupId 
	 Join skus s on s.id = sc.SkuId
	 Join States st on st.Id = sc.StateId
	 Where CAST(@Startdate as date) <= CAST(sc.CreatedDate as date) and CAST(sc.CreatedDate as date) <= CAST(@Enddate as date) and (sc.VerticalId = @VerticalId or @VerticalId = 0) 
	 ORDER By sc.CreatedDate desc 
END


/****** Object:  StoredProcedure [dbo].[LoadCapacityExport]    Script Date: 12-09-2019 15:56:04 ******/
SET ANSI_NULLS ON
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LoadCapacityExport] 
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId bigint,
	@IsActiveStatus int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if(@IsActiveStatus = 2) --Active records
	select v.name as Vertical,
	ot.name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	tm.Name as TransportMode,
	lcc.LoadCapacity as TruckCapacity,
	lcc.LoadQuantity as SalesTruckLoadQuantity,
	lcc.ActualLoadQuantity as ActualTruckLoadQuantity,
	lcc.ValidFrom as ValidFrom,
	lcc.ValidTo as ValidTo,
	lcc.IsActive as Status,
	lcc.IsPublished as Published
	From LoadCapacityConversions lcc 
   Join Verticals v on v.Id = lcc.VerticalId 
    Join OilTypes ot on ot.Id = lcc.OilTypeId 
	Join Skus s on s.Id = lcc.SkuId 
	Join TransportModes tm on tm.Id = lcc.TransportModeId 
	 Where CAST(@Startdate as date) <= CAST(lcc.CreatedDate as date) and CAST(lcc.CreatedDate as date) <= CAST(@Enddate as date) and  (lcc.VerticalId = @VerticalId or @VerticalId = 0) and lcc.IsActive = 1
	  ORDER By lcc.CreatedDate desc
  ELSE IF(@IsActiveStatus = 3) --InActive records
    select v.name as Vertical,
	ot.name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	tm.Name as TransportMode,
	lcc.LoadCapacity as TruckCapacity,
	lcc.LoadQuantity as SalesTruckLoadQuantity,
	lcc.ActualLoadQuantity as ActualTruckLoadQuantity,
	lcc.ValidFrom as ValidFrom,
	lcc.ValidTo as ValidTo,
	lcc.IsActive as Status,
	lcc.IsPublished as Published
	From LoadCapacityConversions lcc 
   Join Verticals v on v.Id = lcc.VerticalId 
    Join OilTypes ot on ot.Id = lcc.OilTypeId 
	Join Skus s on s.Id = lcc.SkuId 
	Join TransportModes tm on tm.Id = lcc.TransportModeId 
	 Where CAST(@Startdate as date) <= CAST(lcc.CreatedDate as date) and CAST(lcc.CreatedDate as date) <= CAST(@Enddate as date)   and  (lcc.VerticalId = @VerticalId or @VerticalId = 0) and lcc.IsActive = 0
	  ORDER By lcc.CreatedDate desc
	ELSE 
    select v.name as Vertical,
	ot.name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	tm.Name as TransportMode,
	lcc.LoadCapacity as TruckCapacity,
	lcc.LoadQuantity as SalesTruckLoadQuantity,
	lcc.ActualLoadQuantity as ActualTruckLoadQuantity,
	lcc.ValidFrom as ValidFrom,
	lcc.ValidTo as ValidTo,
	lcc.IsActive as Status,
	lcc.IsPublished as Published
	From LoadCapacityConversions lcc 
   Join Verticals v on v.Id = lcc.VerticalId 
    Join OilTypes ot on ot.Id = lcc.OilTypeId 
	Join Skus s on s.Id = lcc.SkuId 
	Join TransportModes tm on tm.Id = lcc.TransportModeId 
	 Where CAST(@Startdate as date) <= CAST(lcc.CreatedDate as date) and CAST(lcc.CreatedDate as date) <= CAST(@Enddate as date)   and  (lcc.VerticalId = @VerticalId or @VerticalId = 0) 
	  ORDER By lcc.CreatedDate desc
END


/****** Object:  StoredProcedure [dbo].[RaMarginExport]    Script Date: 12-09-2019 15:56:32 ******/
SET ANSI_NULLS ON
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RaMarginExport]
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId bigint,
	@IsActiveStatus int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if(@IsActiveStatus = 2) --Active records
	select v.name as Vertical,
	ot.name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	pg.Name as BPOrCPWise,
	ram.RatePerMt as PricePerMT,
	ram.ValidFrom as ValidFrom,
	ram.ValidTo as ValidTo,
	z.Name as Zone,
	st.StateName as State,
	ram.IsActive as Status,
	ram.IsPublished as Published
	From RaMargins ram
	Join Verticals v on v.Id = ram.VerticalId
	 Join OilTypes ot on ot.Id = ram.OilTypeId
	 Join Skus s on s.Id = ram.SkuId 
	 Join PackGroups pg on pg.Id = ram.OilPackingTypeId 
	 Join Zones z on z.Id = ram.ZoneId 
	 Join States st on st.Id = ram.StateId
	  Where CAST(@Startdate as date) <= CAST(ram.CreatedDate as date) and CAST(ram.CreatedDate as date) <= CAST(@Enddate as date) and (ram.VerticalId = @VerticalId or @VerticalId = 0) and ram.IsActive = 1
	  ORDER By ram.CreatedDate desc 
	else if(@IsActiveStatus = 3) --InActive records
	select v.name as Vertical,
	ot.name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	pg.Name as BPOrCPWise,
	ram.RatePerMt as PricePerMT,
	ram.ValidFrom as ValidFrom,
	ram.ValidTo as ValidTo,
	z.Name as Zone,
	st.StateName as State,
	ram.IsActive as Status,
	ram.IsPublished as Published
	From RaMargins ram
	Join Verticals v on v.Id = ram.VerticalId
	 Join OilTypes ot on ot.Id = ram.OilTypeId
	 Join Skus s on s.Id = ram.SkuId 
	 Join PackGroups pg on pg.Id = ram.OilPackingTypeId 
	 Join Zones z on z.Id = ram.ZoneId 
	 Join States st on st.Id = ram.StateId
	  Where CAST(@Startdate as date) <= CAST(ram.CreatedDate as date) and CAST(ram.CreatedDate as date) <= CAST(@Enddate as date) and (ram.VerticalId = @VerticalId or @VerticalId = 0) and ram.IsActive = 0
	  ORDER By ram.CreatedDate desc 
  else 
	select v.name as Vertical,
	ot.name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	pg.Name as BPOrCPWise,
	ram.RatePerMt as PricePerMT,
	ram.ValidFrom as ValidFrom,
	ram.ValidTo as ValidTo,
	z.Name as Zone,
	st.StateName as State,
	ram.IsActive as Status,
	ram.IsPublished as Published
	From RaMargins ram
	Join Verticals v on v.Id = ram.VerticalId
	 Join OilTypes ot on ot.Id = ram.OilTypeId
	 Join Skus s on s.Id = ram.SkuId 
	 Join PackGroups pg on pg.Id = ram.OilPackingTypeId 
	 Join Zones z on z.Id = ram.ZoneId 
	 Join States st on st.Id = ram.StateId
	  Where CAST(@Startdate as date) <= CAST(ram.CreatedDate as date) and CAST(ram.CreatedDate as date) <= CAST(@Enddate as date) and (ram.VerticalId = @VerticalId or @VerticalId = 0)
	  ORDER By ram.CreatedDate desc 
END
Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
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