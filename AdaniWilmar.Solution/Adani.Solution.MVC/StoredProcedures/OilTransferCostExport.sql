CREATE PROCEDURE [dbo].[OilTransferCostExport]	@StartDate DateTime,	@EndDate DateTime,	@VerticalId bigint,	@IsActiveStatus intAS BEGIN  IF(@IsActiveStatus = 2) --Active records	SELECT  
	v.Name AS Vertical ,
	sku.SkuName AS Sku,
	o.Name AS OilType,
	d.Name AS SourcePlant,
	de.Name AS DestinationPlant,
	ot.RatePerMt,
	CONVERT(VARCHAR,ot.ValidFrom,103) AS ValidFrom,
	CONVERT(VARCHAR,ot.ValidTo,103) AS ValidTo,
	ot.IsActive as Status
	FROM OilTransferCosts ot
	JOIN Verticals v ON ot.VerticalId = v.Id
	JOIN OilTypes o ON o.id = ot.OilTypeId
	JOIN Depots d on d.Id = ot.SourceId
	JOIN Depots de on de.Id = ot.DestinationId
	JOIN Skus sku on sku.Id = ot.SkuId
	WHERE CAST(@Startdate as date) <= CAST(ot.CreatedDate as date) and CAST(ot.CreatedDate as date) <= CAST(@Enddate as date) and (ot.VerticalId = @VerticalId or @VerticalId = 0) and ot.IsActive = 1
	ORDER By ot.CreatedDate DESC  ELSE IF(@IsActiveStatus = 3) --InActive records   SELECT  
	v.Name AS Vertical ,
	sku.SkuName AS Sku,
	o.Name AS OilType,
	d.Name AS SourcePlant,
	de.Name AS DestinationPlant,
	ot.RatePerMt,
	CONVERT(VARCHAR,ot.ValidFrom,103) AS ValidFrom,
	CONVERT(VARCHAR,ot.ValidTo,103) AS ValidTo,
	ot.IsActive as Status
	FROM OilTransferCosts ot
	JOIN Verticals v ON ot.VerticalId = v.Id
	JOIN OilTypes o ON o.id = ot.OilTypeId
	JOIN Depots d on d.Id = ot.SourceId
	JOIN Depots de on de.Id = ot.DestinationId
	JOIN Skus sku on sku.Id = ot.SkuId
	WHERE CAST(@Startdate as date) <= CAST(ot.CreatedDate as date) and CAST(ot.CreatedDate as date) <= CAST(@Enddate as date) and (ot.VerticalId = @VerticalId or @VerticalId = 0) and ot.IsActive = 0
	ORDER By ot.CreatedDate DESC  ELSE    SELECT  
	v.Name AS Vertical ,
	sku.SkuName AS Sku,
	o.Name AS OilType,
	d.Name AS SourcePlant,
	de.Name AS DestinationPlant,
	ot.RatePerMt,
	CONVERT(VARCHAR,ot.ValidFrom,103) AS ValidFrom,
	CONVERT(VARCHAR,ot.ValidTo,103) AS ValidTo,
	ot.IsActive as Status
	FROM OilTransferCosts ot
	JOIN Verticals v ON ot.VerticalId = v.Id
	JOIN OilTypes o ON o.id = ot.OilTypeId
	JOIN Depots d on d.Id = ot.SourceId
	JOIN Depots de on de.Id = ot.DestinationId
	JOIN Skus sku on sku.Id = ot.SkuId
	WHERE CAST(@Startdate as date) <= CAST(ot.CreatedDate as date) and CAST(ot.CreatedDate as date) <= CAST(@Enddate as date) and (ot.VerticalId = @VerticalId or @VerticalId = 0) 
	ORDER By ot.CreatedDate DESCEND;--EXEC OilTransferCostExport '2020-03-24','2020-04-04'