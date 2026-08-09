
/****** Object:  StoredProcedure [dbo].[SP_Emami_SchemeCostsDetails]    Script Date: 04-10-2019 14:06:24 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_Emami_SchemeCostsDetails')
    BEGIN
        DROP  Procedure SP_Emami_SchemeCostsDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_Emami_SchemeCostsDetails]
(
	@ValidTo DateTime
)
As
Begin

	Create TABLE #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1 
	from
	SchemeCosts
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SchemeCosts
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  

	Create TABLE #SchemeCostsDetails (Vertical varchar(1000),OilType varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),PackGroup varchar(1000),State varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #SchemeCostsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,PackGroup,SkuName,SkuCode,State,ValidFrom,ValidTo)
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	V.Name as Vertical,
	ot.name as OilType,
	pg.Name as PackGroup,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	--Z.name as Zone,
	st.StateName as State,
	sc.ValidFrom,
	sc.ValidTo
	
	From SchemeCosts sc With(NoLock)
	Inner Join Verticals V With(NoLock) On sc.VerticalId = V.Id
	Inner Join OilTypes ot With(NoLock) On sc.OilTypeId = ot.Id
	Inner Join PackGroups pg With(NoLock) On sc.PackGroupId = pg.Id
	Inner Join Skus S With(NoLock) On sc.SkuId = S.Id
	Inner Join Zones Z With(NoLock) On sc.ZoneId = Z.Id
	Inner Join States st With(NoLock) On sc.StateId = st.Id
	Where (CONVERT(varchar, sc.ValidTo,111) = CONVERT(varchar, @ValidTo,111) and sc.IsActive = 1) or (CONVERT(varchar,sc.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and sc.IsActive = 1  )


	IF((select Count(*) from #SchemeCostsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,PackGroup,SkuName,SkuCode,State,ValidFrom,ValidTo from  #SchemeCostsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #SchemeCostsDetails

End

/****** Object:  StoredProcedure [dbo].[SP_Emami_RAMarginDetails]    Script Date: 04-10-2019 14:07:06 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_Emami_RAMarginDetails')
    BEGIN
        DROP  Procedure SP_Emami_RAMarginDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_Emami_RAMarginDetails]
(
	@ValidTo DateTime
)
As
Begin

	
	Create TABLE #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	RaMargins
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	RaMargins
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  
	

	Create TABLE #RaMarginsDetails (Vertical varchar(1000),OilType varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),PackGroup varchar(1000),Zone varchar(1000),State varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #RaMarginsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,SkuName,SkuCode,PackGroup,ValidFrom,ValidTo,Zone,State)
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	V.Name as Vertical,
	ot.name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	pg.Name as PackGroup,
	ra.ValidFrom,
	ra.ValidTo,
	Z.name as Zone,
	st.StateName as State
	
	From RaMargins ra With(NoLock)
	Inner Join Verticals V With(NoLock) On ra.VerticalId = V.Id
	Inner Join OilTypes ot With(NoLock) On ra.OilTypeId = ot.Id
	Inner Join Skus S With(NoLock) On ra.SkuId = S.Id
	Inner Join PackGroups pg With(NoLock) On ra.OilPackingTypeId = pg.Id
	Inner Join States st With(NoLock) On ra.StateId = st.Id
	Inner Join Zones Z With(NoLock) On ra.ZoneId = Z.Id
	Where (CONVERT(varchar, ra.ValidTo,111) = CONVERT(varchar, @ValidTo,111) and ra.IsActive = 1) or (CONVERT(varchar,ra.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and ra.IsActive = 1  )


	IF((select Count(*) from #RaMarginsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,SkuName,SkuCode,PackGroup,ValidFrom,ValidTo,Zone,State from  #RaMarginsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	   DROP TABLE #TommorrowExpiringDetails
	   DROP TABLE #DayAfterTommorrowExpiringDetails
	   DROP TABLE #RaMarginsDetails


End

/****** Object:  StoredProcedure [dbo].[SP_Emami_CushionMarginDetails]    Script Date: 04-10-2019 14:07:48 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_Emami_CushionMarginDetails')
    BEGIN
        DROP  Procedure SP_Emami_CushionMarginDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_Emami_CushionMarginDetails]
(
	@ValidTo DateTime
)
As
Begin


	Create TABLE #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1 
	from
	CushionMargins
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	CushionMargins
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  


	Create TABLE #CushionMarginsDetails (Vertical varchar(1000),OilType varchar(1000),PackGroup varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),Zone varchar(1000),State varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #CushionMarginsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,PackGroup,SkuName,SkuCode,Zone,State,ValidFrom,ValidTo) 
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	V.Name as Vertical,
	ot.name as OilType,
	pg.Name as PackGroup,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	Z.name as Zone,
	st.StateName as State,
	cm.ValidFrom,
	cm.ValidTo
	
	From CushionMargins cm With(NoLock)
	Inner Join Verticals V With(NoLock) On cm.VerticalId = V.Id
	Inner Join OilTypes ot With(NoLock) On cm.OilTypeId = ot.Id
	Inner Join Skus S With(NoLock) On cm.SkuId = S.Id
	Inner Join PackGroups pg With(NoLock) On cm.OilPackingTypeId = pg.Id
	Inner Join Zones Z With(NoLock) On cm.ZoneId = Z.Id
	Inner Join States st With(NoLock) On cm.StateId = st.Id
	Where ((CONVERT(varchar, cm.ValidTo,111) = CONVERT(varchar, @ValidTo,111)) and cm.IsActive = 1) or (CONVERT(varchar,cm.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and cm.IsActive = 1  )


	IF((select Count(*) from #CushionMarginsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,PackGroup,SkuName,SkuCode,Zone,State,ValidFrom,ValidTo from  #CushionMarginsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	 DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #CushionMarginsDetails


End



/****** Object:  StoredProcedure [dbo].[SP_Emami_ProfitMarginDetails]    Script Date: 04-10-2019 14:08:10 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_Emami_ProfitMarginDetails')
    BEGIN
        DROP  Procedure SP_Emami_ProfitMarginDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_Emami_ProfitMarginDetails]
(
	@ValidTo DateTime
)
As
Begin


	Create TABLE #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1 
	from
	ProfitMargins
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	ProfitMargins
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  


	Create TABLE #ProfitMarginsDetails (Vertical varchar(1000),OilType varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),PackGroup varchar(1000),State varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #ProfitMarginsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,PackGroup,SkuName,SkuCode,ValidFrom,ValidTo,State)
	Select (select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	V.Name as Vertical,
	ot.name as OilType,
	pg.Name as PackGroup,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	pm.ValidFrom,
	pm.ValidTo,
	st.StateName as State
	
	From ProfitMargins pm With(NoLock)
	Inner Join Verticals V With(NoLock) On pm.VerticalId = V.Id
	Inner Join OilTypes ot With(NoLock) On pm.OilTypeId = ot.Id
	Inner Join Skus S With(NoLock) On pm.SkuId = S.Id
	Inner Join PackGroups pg With(NoLock) On pm.OilPackingTypeId = pg.Id
	Inner Join States st With(NoLock) On pm.StateId = st.Id
	Where (CONVERT(varchar, pm.ValidTo,111) = CONVERT(varchar, @ValidTo,111) and pm.IsActive = 1) or (CONVERT(varchar,pm.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and pm.IsActive = 1  )


	IF((select Count(*) from #ProfitMarginsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,PackGroup,SkuName,SkuCode,ValidFrom,ValidTo,State from  #ProfitMarginsDetails
		END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	 DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #ProfitMarginsDetails
End



/****** Object:  StoredProcedure [dbo].[SP_Emami_LoadCapacityDetails]    Script Date: 04-10-2019 14:08:30 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_Emami_LoadCapacityDetails')
    BEGIN
        DROP  Procedure SP_Emami_LoadCapacityDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_Emami_LoadCapacityDetails]
(
	@ValidTo DateTime
)
As
Begin

	Create TABLE #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1 
	from
	LoadCapacityConversions
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	LoadCapacityConversions
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  

	Create TABLE #LoadCapacityConversionsDetails (Vertical varchar(1000),OilType varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),TransportMode varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #LoadCapacityConversionsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,SkuName,SkuCode,TransportMode,ValidFrom,ValidTo)
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	V.Name as Vertical,
	ot.name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	tm.Name as TransportMode,
	--Z.name as Zone,
	lcc.ValidFrom,
	lcc.ValidTo
	
	From LoadCapacityConversions lcc With(NoLock)
	Inner Join Verticals V With(NoLock) On lcc.VerticalId = V.Id
	Inner Join OilTypes ot With(NoLock) On lcc.OilTypeId = ot.Id
	Inner Join Skus S With(NoLock) On lcc.SkuId = S.Id
	Inner Join TransportModes tm With(NoLock) On lcc.TransportModeId = tm.Id
	Where (CONVERT(varchar, lcc.ValidTo,111) = CONVERT(varchar, @ValidTo,111) and lcc.IsActive = 1) or (CONVERT(varchar,lcc.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and lcc.IsActive = 1  )


	IF((select Count(*) from #LoadCapacityConversionsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,SkuName,SkuCode,TransportMode,ValidFrom,ValidTo from  #LoadCapacityConversionsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #LoadCapacityConversionsDetails
End



/****** Object:  StoredProcedure [dbo].[SP_Emami_HoneyCombDetails]    Script Date: 04-10-2019 14:08:55 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_Emami_HoneyCombDetails')
    BEGIN
        DROP  Procedure SP_Emami_HoneyCombDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_Emami_HoneyCombDetails]
(
	@ValidTo DateTime
)
As
Begin

	Create TABLE #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1 
	from
	HoneycombCosts
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	HoneycombCosts
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  

	Create TABLE #HoneycombCostsDetails (Vertical varchar(1000),OilType varchar(1000),Plant varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),SourceCode varchar(1000),Destination varchar(1000),TransportMode varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #HoneycombCostsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,SkuName,SkuCode,Plant,SourceCode,Destination,TransportMode,ValidFrom,ValidTo)	
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	V.Name as Vertical,
	ot.name as OilType,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	D.Name as Plant,
	D.Code as SourceCode,
	st.StateName as Destination,
	tm.Name as TransportMode,
	hcc.ValidFrom,
	hcc.ValidTo
	
	From HoneycombCosts hcc With(NoLock)
	Inner Join Depots D With(NoLock) On hcc.PlantId = D.Id
	Inner Join Verticals V With(NoLock) On hcc.VerticalId = V.Id
	Inner Join OilTypes ot With(NoLock) On hcc.OilTypeId = ot.Id
	Inner Join Skus S With(NoLock) On hcc.SkuId = S.Id
	Inner Join States st With(NoLock) On hcc.StateId = st.Id
	Inner Join TransportModes tm With(NoLock) On hcc.TransportModeId = tm.Id
	Where (CONVERT(varchar, hcc.ValidTo,111) = CONVERT(varchar, @ValidTo,111) and hcc.IsActive = 1) or (CONVERT(varchar,hcc.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and hcc.IsActive = 1  )


	IF((select Count(*) from #HoneycombCostsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,SkuName,SkuCode,Plant,SourceCode,Destination,TransportMode,ValidFrom,ValidTo from  #HoneycombCostsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #HoneycombCostsDetails
End

/****** Object:  StoredProcedure [dbo].[SP_Emami_GstMailDetails]    Script Date: 04-10-2019 14:09:15 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_Emami_GstMailDetails')
    BEGIN
        DROP  Procedure SP_Emami_GstMailDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_Emami_GstMailDetails]
(
	
	@ValidTo DateTime
)
As
Begin


	Create TABLE #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails (DayAfterTommorrowExpiringCount,Id) 
	select  DISTINCT count(*),1 
	from
	Gsts
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select DISTINCT count(*),1
	from
	Gsts
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  

	Create TABLE #gstDetails (Plant varchar(1000),FreightRoute varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #gstDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Plant,FreightRoute,ValidFrom,ValidTo) 
	Select
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	 D.Name as Plant,
	st.StateName,
	gst.ValidFrom,
	gst.ValidTo
	
	From Gsts gst With(NoLock)
	Inner Join Depots D With(NoLock) On gst.DepotId = D.Id
	Inner Join States st With(NoLock) On gst.DestinationStateId = st.Id
	Where (CONVERT(varchar, gst.ValidTo,111) = CONVERT(varchar, @ValidTo,111) and gst.IsActive = 1 ) or (CONVERT(varchar,gst.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and gst.IsActive = 1  ) 

	IF((select Count(*) from #gstDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Plant,FreightRoute,ValidFrom,ValidTo from  #gstDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	   DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #gstDetails
End



/****** Object:  StoredProcedure [dbo].[GetMaterialCostNotification]    Script Date: 04-10-2019 14:09:49 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetMaterialCostNotification')
    BEGIN
        DROP  Procedure GetMaterialCostNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetMaterialCostNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo datetime
	
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Insert statements for procedure here


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	MaterialCosts
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	MaterialCosts
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  

	Create TABLE #MaterialCostsDetails (Vertical varchar(1000),OilType varchar(1000),Plant varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #MaterialCostsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,Plant ,ValidFrom,ValidTo)
	SELECT  
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	 v.Name as Vertical , 
	o.Name as OilType , 
	d.Name as Plant , 
	m.ValidFrom as ValidFrom , 
	m.ValidTo as ValidTo
	
	From MaterialCosts m 
	Join Verticals v on m.VerticalId = v.Id 
	Join Depots d on m.PlantId = d.Id 
	Join OilTypes o on m.OilTypeId = o.Id
	Where (CONVERT(varchar,m.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and m.IsActive = 'true') or (CONVERT(varchar,m.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and m.IsActive = 1 )



	IF((select Count(*) from #MaterialCostsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,Plant ,ValidFrom,ValidTo from  #MaterialCostsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	    DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #MaterialCostsDetails
END


/****** Object:  StoredProcedure [dbo].[GetPackingCostsNotification]    Script Date: 04-10-2019 14:10:09 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetPackingCostsNotification')
    BEGIN
        DROP  Procedure GetPackingCostsNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure  [dbo].[GetPackingCostsNotification]
	@ValidTo datetime
AS
BEGIN
	
	SET NOCOUNT ON;


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1 
	from
	PackingCosts
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	PackingCosts
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  


	Create TABLE #PackingCostsDetails (Vertical varchar(1000),OilType varchar(1000),Plant varchar(1000),SkuCode varchar(1000),SkuName varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #PackingCostsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,Plant,SkuCode,SkuName,ValidFrom,ValidTo)
    SELECT
	(select DayAfterTommorrowExpiringCount from  #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	 v.Name as Vertical , 
	o.Name as OilType , 
	d.Name as Plant , 
	s.SkuCode as SkuCode,
	s.SkuName as SkuName,
	p.ValidFrom as ValidFrom , 
	p.ValidTo as ValidTo
	
	From PackingCosts p  
	Join Verticals v on p.VerticalId = v.Id 
	Join Depots d on p.PlantId = d.Id 
	Join OilTypes o on p.OilTypeId = o.Id 
	Join Skus s on p.SkuId = s.Id
	Where (CONVERT(varchar,p.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and p.IsActive = 'true') or (CONVERT(varchar,p.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and p.IsActive = 1  )



	IF((select Count(*) from #PackingCostsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,Plant,SkuCode,SkuName,ValidFrom,ValidTo from #PackingCostsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	   DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #PackingCostsDetails

END

/****** Object:  StoredProcedure [dbo].[GetPrimaryFreightNotification]    Script Date: 04-10-2019 14:10:32 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetPrimaryFreightNotification')
    BEGIN
        DROP  Procedure GetPrimaryFreightNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetPrimaryFreightNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo datetime
AS
BEGIN
	
	SET NOCOUNT ON;

	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	PrimaryFreights
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	PrimaryFreights
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  

	Create TABLE #PrimaryFreightsDetails (Vertical varchar(1000),DepotName varchar(1000),DepotCode varchar(1000),Plant varchar(1000),TransportMode varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #PrimaryFreightsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,DepotName,DepotCode,Plant,TransportMode,ValidFrom,ValidTo)
    SELECT 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	 v.Name as Vertical , 
	dp.Name as DepotName,
	dp.Code as DepotCode,
	d.Name as Plant , 
	t.Name as TransportMode,
	p.ValidFrom as ValidFrom , 
	p.ValidTo as ValidTo 
	
	From PrimaryFreights p 
	Join Verticals v on p.VerticalId = v.Id 
	Join Depots d  on p.PlantId = d.Id 
	Join TransportModes t   on p.TransportModeId = t.Id 
	Join Depots dp  on p.DepotId = dp.Id 
	Where (CONVERT(varchar,p.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and p.IsActive = 'true') or (CONVERT(varchar,p.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and p.IsActive = 1  
)


	IF((select Count(*) from #PrimaryFreightsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,DepotName,DepotCode,Plant,TransportMode,ValidFrom,ValidTo from #PrimaryFreightsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	   DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #PrimaryFreightsDetails
END


/****** Object:  StoredProcedure [dbo].[GetSecondaryFreightNotification]    Script Date: 04-10-2019 14:11:00 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSecondaryFreightNotification')
    BEGIN
        DROP  Procedure GetSecondaryFreightNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetSecondaryFreightNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo datetime
AS
BEGIN


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SecondaryFreights
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SecondaryFreights
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  


	Create TABLE #SecondaryFreightsDetails (Vertical varchar(1000),SourceName varchar(1000),SourceCode varchar(1000),Zone varchar(1000), State varchar(1000),FreightZoneName varchar(1000),FreightRouteName varchar(1000),TransportMode varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #SecondaryFreightsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,SourceName,SourceCode,Zone,State, FreightZoneName,FreightRouteName,TransportMode,ValidFrom,ValidTo)
	SELECT 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails)  as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	v.Name as Vertical , 
	dp.Name as SourceName ,
	dp.Code as SourceCode,
	z.Name as Zone,
	st.StateName as State,
	fz.Name as FreightZoneName,
	fr.Name as FreightRouteName, 
	t.Name as TransportMode,
	s.ValidFrom as ValidFrom , 
	s.ValidTo as ValidTo
	
	From SecondaryFreights s 
	Join Verticals v on s.VerticalId = v.Id 
	Join FreightZones fz on s.FreightZoneId = fz.Id
	Join FreightRoutes fr on s.FreightRouteId = fr.Id
	Join TransportModes t   on s.TransportModeId = t.Id 
	Join Depots dp  on s.DepotId = dp.Id 
	Join Zones z on s.ZoneId = z.Id
	Join States st on s.StateId = st.Id
	Where (CONVERT(varchar,s.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and s.IsActive = 'true') or (CONVERT(varchar,s.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and s.IsActive = 1 )


	IF((select Count(*) from #SecondaryFreightsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,SourceName,SourceCode,Zone,State, FreightZoneName,FreightRouteName,TransportMode,ValidFrom,ValidTo from #SecondaryFreightsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	   DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #SecondaryFreightsDetails
    
END



/****** Object:  StoredProcedure [dbo].[GetDepotCostNotification]    Script Date: 04-10-2019 14:11:39 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetDepotCostNotification')
    BEGIN
        DROP  Procedure GetDepotCostNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetDepotCostNotification] 
	-- Add the parameters for the stored procedure here
	@ValidTo datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1 
	from
	DepotCosts
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	DepotCosts
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  

	Create TABLE #DepotCostsDetails (Vertical varchar(1000),DepotName varchar(1000),DepotCode varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #DepotCostsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,DepotName,DepotCode,ValidFrom,ValidTo)
    SELECT Distinct
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	 v.Name as Vertical , 
	dp.Name as DepotName ,
	dp.Code as DepotCode,
	d.ValidFrom as ValidFrom , 
	d.ValidTo as ValidTo
	
	 From DepotCosts d 
	Join Verticals v on d.VerticalId = v.Id 
	Join Depots dp  on d.DepotId = dp.Id 
	Where (CONVERT(varchar,d.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and d.IsActive = 'true') or (CONVERT(varchar,d.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and d.IsActive = 1 )


	IF((select Count(*) from #DepotCostsDetails) > 0)
		BEGIN
			select Vertical,DepotName,DepotCode,ValidFrom,ValidTo,DayAfterTommorrowExpiringCount,TommorrowExpiringCount from  #DepotCostsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id 

	  END

	   DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #DepotCostsDetails
END


/****** Object:  StoredProcedure [dbo].[GetDetentionCostsNotification]    Script Date: 04-10-2019 14:12:05 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetDetentionCostsNotification')
    BEGIN
        DROP  Procedure GetDetentionCostsNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetDetentionCostsNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	DetentionCosts
	where 
	CONVERT(varchar,ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and IsActive = 1  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	DetentionCosts
	where 
	CONVERT(varchar,ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and IsActive = 1  

	Create TABLE #DetentionCostsDetails (Vertical varchar(1000),DepotName varchar(1000),DepotCode varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #DetentionCostsDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,DepotName,DepotCode,ValidFrom,ValidTo)
    SELECT  
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	 v.Name as Vertical , 
	dp.Name as DepotName ,
	dp.Code as DepotCode,
	d.ValidFrom as ValidFrom , 
	d.ValidTo as ValidTo
	
	From DetentionCosts d 
	Join Verticals v on d.VerticalId = v.Id 
	Join Depots dp  on d.DepotId = dp.Id 
	Where (CONVERT(varchar,d.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and d.IsActive = 'true') or (CONVERT(varchar,d.ValidTo,111) =  DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and d.IsActive = 1 ) 


	IF((select Count(*) from #DetentionCostsDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,DepotName,DepotCode,ValidFrom,ValidTo from  #DetentionCostsDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id 
	  END


	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #DetentionCostsDetails
END




/****** Object:  StoredProcedure [dbo].[GetVolumeUserBasedNotification]    Script Date: 04-10-2019 14:12:27 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetVolumeUserBasedNotification')
    BEGIN
        DROP  Procedure GetVolumeUserBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetVolumeUserBasedNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	VolumeDiscountUsers v 
	join  VolumeDiscountUserMappings vd on v.Id = vd.VolumeDiscountUserId
	join CustomerGroups c on vd.CustomerGroupId = c.Id 
	join Skus s on vd.SkuId = s.Id
	Join OilTypes o on s.OilTypeId = o.Id 
	join Users u on vd.CustomerId = u.Id 
	where 
	CONVERT(varchar,v.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) 

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	VolumeDiscountUsers v 
	join  VolumeDiscountUserMappings vd on v.Id = vd.VolumeDiscountUserId
	join CustomerGroups c on vd.CustomerGroupId = c.Id 
	join Skus s on vd.SkuId = s.Id
	Join OilTypes o on s.OilTypeId = o.Id 
	join Users u on vd.CustomerId = u.Id 
	where 
	CONVERT(varchar,v.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) 


	Create TABLE #VolumeDiscountUsersDetails (CustomerGroup varchar(1000) ,OilType varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),UserCode varchar(1000), UserName varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #VolumeDiscountUsersDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,CustomerGroup,SkuName,SkuCode,UserCode,UserName,OilType,ValidFrom,ValidTo)
	select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails ) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	c.Name as CustomerGroup,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	u.Code as UserCode,
	u.Name as UserName,
	o.Name as OilType,
	v.ValidFrom as ValidFrom,
	v.ValidTo as ValidTo
	from VolumeDiscountUsers v 
	join  VolumeDiscountUserMappings vd on v.Id = vd.VolumeDiscountUserId
	join CustomerGroups c on vd.CustomerGroupId = c.Id 
	join Skus s on vd.SkuId = s.Id
	Join OilTypes o on s.OilTypeId = o.Id 
	join Users u on vd.CustomerId = u.Id 
	Where (CONVERT(varchar,v.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,v.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111))  )

	IF((select Count(*) from #VolumeDiscountUsersDetails) > 0)
		BEGIN
			select  DayAfterTommorrowExpiringCount,TommorrowExpiringCounT,CustomerGroup,OilType,SkuName,SkuCode,UserCode,UserName,ValidFrom,ValidTo from #VolumeDiscountUsersDetails
		END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END
   	
	DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #VolumeDiscountUsersDetails

END




/****** Object:  StoredProcedure [dbo].[GetVolumeDiscountGeographyBasedNotification]    Script Date: 04-10-2019 14:12:48 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetVolumeDiscountGeographyBasedNotification')
    BEGIN
        DROP  Procedure GetVolumeDiscountGeographyBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetVolumeDiscountGeographyBasedNotification] 
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	

	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	VolumeDiscountGeographies g 
	join VolumeDiscountGeographyMappings gd on g.Id = gd.VolumeDiscountGeographyId
	join Skus s on gd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join Cities c on gd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	where 
	CONVERT(varchar,g.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) 

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	VolumeDiscountGeographies g 
	join VolumeDiscountGeographyMappings gd on g.Id = gd.VolumeDiscountGeographyId
	join Skus s on gd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join Cities c on gd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	where 
	CONVERT(varchar,g.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) 

	Create TABLE #VolumeDiscountGeographiesDetails (OilType varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),StateName varchar(1000), DistrictName varchar(1000),CityName varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #VolumeDiscountGeographiesDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,OilType,SkuName,SkuCode,StateName,DistrictName,CityName,ValidFrom,ValidTo)
	select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	o.Name as OilType ,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	st.StateName as StateName,
	d.DistrictName as DistrictName,
	c.CityName as CityName,
	g.ValidFrom as ValidFrom,
	g.ValidTo as ValidTo 
	from VolumeDiscountGeographies g 
	join VolumeDiscountGeographyMappings gd on g.Id = gd.VolumeDiscountGeographyId
	join Skus s on gd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join Cities c on gd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	Where (CONVERT(varchar,g.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,g.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) )

	IF((select Count(*) from #VolumeDiscountGeographiesDetails) > 0)
		BEGIN
			select  DayAfterTommorrowExpiringCount,TommorrowExpiringCount,OilType,SkuName,SkuCode,StateName,DistrictName,CityName,ValidFrom,ValidTo from #VolumeDiscountGeographiesDetails
		END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END
   

	DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #VolumeDiscountGeographiesDetails
    
END



/****** Object:  StoredProcedure [dbo].[GetSchemeDiscountUserBasedNotification]    Script Date: 04-10-2019 14:13:12 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSchemeDiscountUserBasedNotification')
    BEGIN
        DROP  Procedure GetSchemeDiscountUserBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetSchemeDiscountUserBasedNotification] 
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SchemeDiscountUsers s 
	join SchemeDiscountUserMappings sd on s.Id = sd.SchemeDiscountUserId
	join Skus sk on sd.SkuId = sk.Id
	join OilTypes o on sk.OilTypeId = o.Id 
	join Users u on sd.CustomerId = u.Id 
	where 
	CONVERT(varchar,s.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) 
	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SchemeDiscountUsers s 
	join SchemeDiscountUserMappings sd on s.Id = sd.SchemeDiscountUserId
	join Skus sk on sd.SkuId = sk.Id
	join OilTypes o on sk.OilTypeId = o.Id 
	join Users u on sd.CustomerId = u.Id 
	where 
	CONVERT(varchar,s.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) 


	Create TABLE #SchemeDiscountUsersDetails (SchemeName varchar(1000),OilType varchar(1000),Discount decimal(10,4),SkuCode varchar(1000),SkuName varchar(1000), UserCode varchar(1000),UserName varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #SchemeDiscountUsersDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,SchemeName,OilType,Discount,SkuCode,SkuName, UserCode,UserName,ValidFrom,ValidTo)
	select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	s.Name as SchemeName,
	o.Name as OilType,
	s.Discount as Discount,
	sk.SkuCode as SkuCode,
	sk.SkuName as SkuName,
	u.Code as UserCode,
	u.Name as UserName,
	s.ValidFrom as ValidFrom,
	s.ValidTo as ValidTo
	from SchemeDiscountUsers s 
	join SchemeDiscountUserMappings sd on s.Id = sd.SchemeDiscountUserId
	join Skus sk on sd.SkuId = sk.Id
	join OilTypes o on sk.OilTypeId = o.Id 
	join Users u on sd.CustomerId = u.Id 
	Where (CONVERT(varchar,s.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,s.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111))  )


	IF((select Count(*) from #SchemeDiscountUsersDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,SchemeName,OilType,Discount,SkuCode,SkuName, UserCode,UserName,ValidFrom,ValidTo from #SchemeDiscountUsersDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #SchemeDiscountUsersDetails

END


/****** Object:  StoredProcedure [dbo].[GetSchemeDiscountGeographyBasedNotification]    Script Date: 04-10-2019 14:13:42 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSchemeDiscountGeographyBasedNotification')
    BEGIN
        DROP  Procedure GetSchemeDiscountGeographyBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetSchemeDiscountGeographyBasedNotification] 
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	
	SET NOCOUNT ON;


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select count(*),1
	from SchemeDiscountGeographies s 
	join SchemeDiscountGeographyMappings sd on s.Id = sd.SchemeDiscountGeographyId
	join Skus sk on sd.SkuId = sk.Id
	join OilTypes o on sk.OilTypeId = o.Id 
	join Cities c on sd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	where 
	CONVERT(varchar,s.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111))  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SchemeDiscountGeographies s 
	join SchemeDiscountGeographyMappings sd on s.Id = sd.SchemeDiscountGeographyId
	join Skus sk on sd.SkuId = sk.Id
	join OilTypes o on sk.OilTypeId = o.Id 
	join Cities c on sd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	where 
	CONVERT(varchar,s.ValidTo,111) = CONVERT(varchar, @ValidTo ,111)

	Create TABLE #SchemeDiscountGeographiesDetails (SchemeName varchar(1000),OilType varchar(1000),Discount decimal(10,4),SkuCode varchar(1000),SkuName varchar(1000),StateName varchar(1000),DistrictName varchar(1000),CityName varchar(1000) ,ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #SchemeDiscountGeographiesDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,SchemeName,OilType,Discount,SkuCode,SkuName,StateName,DistrictName,CityName,ValidFrom,ValidTo)
    select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	s.Name as SchemeName,
	o.Name as OilType,
	s.Discount as Discount,
	sk.SkuCode as SkuCode,
	sk.SkuName as SkuName,
	st.StateName as StateName,
	d.DistrictName as DistrictName,
	c.CityName as CityName,
	s.ValidFrom as ValidFrom,
	s.ValidTo as ValidTo
	 from SchemeDiscountGeographies s 
	join SchemeDiscountGeographyMappings sd on s.Id = sd.SchemeDiscountGeographyId
	join Skus sk on sd.SkuId = sk.Id
	join OilTypes o on sk.OilTypeId = o.Id 
	join Cities c on sd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	Where (CONVERT(varchar,s.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,s.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) ) 

	IF((select Count(*) from #SchemeDiscountGeographiesDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,SchemeName,OilType,Discount,SkuCode,SkuName,StateName,DistrictName,CityName,ValidFrom,ValidTo from #SchemeDiscountGeographiesDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	   DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #SchemeDiscountGeographiesDetails
END


/****** Object:  StoredProcedure [dbo].[GetSkuDiscountVolumeBasedNotification]    Script Date: 04-10-2019 14:14:07 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSkuDiscountVolumeBasedNotification')
    BEGIN
        DROP  Procedure GetSkuDiscountVolumeBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetSkuDiscountVolumeBasedNotification]
	-- Add the parameters for the stored procedure here
		@ValidTo DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1 
	from
	SkuDiscountUsers sv 
	join SkuDiscountUserMappings svd on sv.Id = svd.SkuDiscountUserId
	join Skus s on svd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join CustomerGroups c on svd.CustomerGroupId = c.Id 
	join Users u on svd.CustomerId = u.Id
	where 
	CONVERT(varchar,sv.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) 

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SkuDiscountUsers sv 
	join SkuDiscountUserMappings svd on sv.Id = svd.SkuDiscountUserId
	join Skus s on svd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join CustomerGroups c on svd.CustomerGroupId = c.Id 
	join Users u on svd.CustomerId = u.Id
	where
	CONVERT(varchar,sv.ValidTo,111) = CONVERT(varchar, @ValidTo ,111)  

	Create TABLE #SkuDiscountUsersDetails (CustomerGroup varchar(1000),OilType varchar(1000),Discount decimal(10,4),SkuName varchar(1000),SkuCode varchar(1000),UserCode varchar(1000), UserName varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #SkuDiscountUsersDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,CustomerGroup,OilType,Discount,SkuName,SkuCode,UserCode,UserName,ValidFrom,ValidTo)
   select 
   (select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	c.Name as CustomerGroup,
	o.Name as OilType ,
	sv.Discount as Discount ,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	u.Code as UserCode,
	u.Name as UserName,
	sv.ValidFrom as ValidFrom,
	sv.ValidTo as ValidTo
	from SkuDiscountUsers sv 
	join SkuDiscountUserMappings svd on sv.Id = svd.SkuDiscountUserId
	join Skus s on svd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join CustomerGroups c on svd.CustomerGroupId = c.Id 
	join Users u on svd.CustomerId = u.Id
	Where (CONVERT(varchar,sv.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,sv.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) )

	IF((select Count(*) from #SkuDiscountUsersDetails) > 0)
		BEGIN
			select  DayAfterTommorrowExpiringCount,TommorrowExpiringCount,CustomerGroup,OilType,Discount,SkuName,SkuCode,UserCode,UserName,ValidFrom,ValidTo from #SkuDiscountUsersDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END
   	
	 DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #SkuDiscountUsersDetails
END



/****** Object:  StoredProcedure [dbo].[GetSkuDiscountGeographyBasedNotification]    Script Date: 04-10-2019 14:14:31 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSkuDiscountGeographyBasedNotification')
    BEGIN
        DROP  Procedure GetSkuDiscountGeographyBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetSkuDiscountGeographyBasedNotification] 
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1 
	from
	skuDiscountGeographies sg 
	join SkuDiscountGeographyMappings sgd on sg.Id = sgd.SkuDiscountGeographyId
	join Skus s on sgd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join Cities c on sgd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	where 
	CONVERT(varchar,sg.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111))  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	skuDiscountGeographies sg 
	join SkuDiscountGeographyMappings sgd on sg.Id = sgd.SkuDiscountGeographyId
	join Skus s on sgd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join Cities c on sgd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	where 
	CONVERT(varchar,sg.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) 

	Create TABLE #SkuDiscountGeographiesDetails (OilType varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),StateName varchar(1000), DistrictName varchar(1000),CityName varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #SkuDiscountGeographiesDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,OilType,SkuName,SkuCode,StateName,DistrictName, CityName,ValidFrom,ValidTo)
	Select
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	o.Name as OilType ,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	st.StateName as StateName,
	d.DistrictName as DistrictName,
	c.CityName as CityName,
	sg.ValidFrom as ValidFrom,
	sg.ValidTo as ValidTo
	from SkuDiscountGeographies sg 
	join SkuDiscountGeographyMappings sgd on sg.Id = sgd.SkuDiscountGeographyId
	join Skus s on sgd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join Cities c on sgd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	Where (CONVERT(varchar,sg.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,sg.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) )

	IF((select Count(*) from #SkuDiscountGeographiesDetails) > 0)
		BEGIN
			select  DayAfterTommorrowExpiringCount,TommorrowExpiringCount,OilType,SkuName,SkuCode,StateName,DistrictName, CityName,ValidFrom,ValidTo from #SkuDiscountGeographiesDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #SkuDiscountGeographiesDetails

END


/****** Object:  StoredProcedure [dbo].[GetGPUserBasedNotification]    Script Date: 04-10-2019 14:14:55 ******/
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGPUserBasedNotification')
    BEGIN
        DROP  Procedure GetGPUserBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetGPUserBasedNotification] 
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	GPBenefitUsers g
	join GPBenefitUserMappings gd on g.Id = gd.GPBenefitUserId
	join Skus s on gd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join Users u on gd.CustomerId = u.Id
	join CustomerGroups c on gd.CustomerGroupId = c.Id
	join BenefitTypes b on g.BenefitTypesId = b.Id
	where CONVERT(varchar,g.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) 

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	GPBenefitUsers g
	join GPBenefitUserMappings gd on g.Id = gd.GPBenefitUserId
	join Skus s on gd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join Users u on gd.CustomerId = u.Id
	join CustomerGroups c on gd.CustomerGroupId = c.Id
	join BenefitTypes b on g.BenefitTypesId = b.Id
	where 
	CONVERT(varchar,g.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) 

   Create TABLE #GPBenefitUsersDetails (CustomerGroup varchar(1000),OilType varchar(1000),Discount decimal(10,4),UserCode varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),UserName varchar(1000) ,BenefitType varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
   INSERT INTO #GPBenefitUsersDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,CustomerGroup,OilType,BenefitType,Discount,SkuCode,SkuName,UserCode,UserName,ValidFrom,ValidTo)
   Select 
   (select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails)  as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
    c.Name as CustomerGroup,
	o.Name as OilType ,
	b.Name as BenefitType,
	g.DiscountOrDays as Discount,
	s.SkuCode as SkuCode,
	s.SkuName as SkuName,
	u.Code as UserCode,
	u.Name as UserName,
	g.ValidFrom as ValidFrom,
	g.ValidTo as ValidTo
	from GPBenefitUsers g
	join GPBenefitUserMappings gd on g.Id = gd.GPBenefitUserId
	join Skus s on gd.SkuId = s.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join Users u on gd.CustomerId = u.Id
	join CustomerGroups c on gd.CustomerGroupId = c.Id
	join BenefitTypes b on g.BenefitTypesId = b.Id
	 Where (CONVERT(varchar,g.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,g.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)))

	IF((select Count(*) from #GPBenefitUsersDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,CustomerGroup,OilType,BenefitType,Discount,SkuCode,SkuName,UserCode,UserName,ValidFrom,ValidTo from  #GPBenefitUsersDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #GPBenefitUsersDetails

END

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGPGeographyBasedNotification')
    BEGIN
        DROP  Procedure GetGPGeographyBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetGPGeographyBasedNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	GPBenefitGeographies g 
	join GPBenefitGeographyMappings gd on g.Id = gd.GPBenefitGeographyId
	join Skus s on gd.SkuId = s.Id
	join Cities c on gd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	join Users u on gd.CustomerId = u.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join BenefitTypes b  on g.BenefitTypesId = b.Id
	join Verticals v on o.VerticalId = v.Id
	where 
	CONVERT(varchar,g.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) 

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	GPBenefitGeographies g 
	join GPBenefitGeographyMappings gd on g.Id = gd.GPBenefitGeographyId
	join Skus s on gd.SkuId = s.Id
	join Cities c on gd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	join Users u on gd.CustomerId = u.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join BenefitTypes b  on g.BenefitTypesId = b.Id
	join Verticals v on o.VerticalId = v.Id
	where 
	CONVERT(varchar,g.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) 


	Create TABLE #GPBenefitGeographiesDetails (Vertical varchar(1000),OilType varchar(1000),Discount decimal(10,4),Customer varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),StateName varchar(1000) ,DistrictName varchar(1000),CityName varchar(1000),BenefitType varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #GPBenefitGeographiesDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,BenefitType,Discount,Customer,SkuName,SkuCode,StateName,DistrictName,CityName,ValidFrom,ValidTo)
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	v.Name as Vertical,
    o.Name as OilType ,
	b.Name as BenefitType,
	g.DiscountOrDays as Discount,
	u.Name as Customer,
	s.SkuName as SkuName,
	s.SkuCode as SkuCode,
	st.StateName as StateName,
	d.DistrictName as DistrictName,
	c.CityName as CityName,
	g.ValidFrom as ValidFrom,
	g.ValidTo as ValidTo
	from GPBenefitGeographies g 
	join GPBenefitGeographyMappings gd on g.Id = gd.GPBenefitGeographyId
	join Skus s on gd.SkuId = s.Id
	join Cities c on gd.CityId = c.Id
	join Districts d on c.DistrictId = d.Id
	join States st on d.StateId = st.Id
	join Users u on gd.CustomerId = u.Id
	join OilTypes o on s.OilTypeId = o.Id 
	join BenefitTypes b  on g.BenefitTypesId = b.Id
	join Verticals v on o.VerticalId = v.Id
	 Where (CONVERT(varchar, g.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,g.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) )  

	IF((select Count(*) from #GPBenefitGeographiesDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,BenefitType,Discount,Customer,SkuName,SkuCode,StateName,DistrictName,CityName,ValidFrom,ValidTo from  #GPBenefitGeographiesDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id 
	  END

	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #GPBenefitGeographiesDetails


END
/****** Object:  StoredProcedure [dbo].[GetBaseGroupMarginNotification]    Script Date: 15-10-2019 11:50:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetBaseGroupMarginNotification')
    BEGIN
        DROP  Procedure GetBaseGroupMarginNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetBaseGroupMarginNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	 Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	BaseGroupMargins as b 
	join DerivedGroupMargins as d on b.Id = d.BaseGroupMarginId
	join OilTypes as o on b.OilTypeId = o.Id
	join PackGroups as p on b.PackGroupId = p.Id
	join CustomerGroups as c on d.CustomerGroupId = c.Id
	where 
	CONVERT(varchar, b.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111) ) and  b.IsActive = 1   


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	BaseGroupMargins as b 
	join DerivedGroupMargins as d on b.Id = d.BaseGroupMarginId
	join OilTypes as o on b.OilTypeId = o.Id
	join PackGroups as p on b.PackGroupId = p.Id
	join CustomerGroups as c on d.CustomerGroupId = c.Id
	where 
	CONVERT(varchar, b.ValidTo,111) = CONVERT(varchar,@ValidTo,111) and  b.IsActive = 1 



	Create TABLE #BaseGroupMarginDetails(OilType varchar(1000),PackGroup varchar(1000),CustomerGroup varchar(1000),Formula varchar(1000),Margin decimal(18,2),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #BaseGroupMarginDetails(DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Formula,Margin,CustomerGroup,OilType,PackGroup,ValidFrom,ValidTo)
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	d.Formula as Formula,
	d.Margin as Margin,
	c.Name as CustomerGroup,
	o.Name as OilType,
	p.Name as PackGroup,
	b.ValidFrom as ValidFrom,
	b.ValidTo as ValidTo
	from
	BaseGroupMargins as b 
	join DerivedGroupMargins as d on b.Id = d.BaseGroupMarginId
	join OilTypes as o on b.OilTypeId = o.Id
	join PackGroups as p on b.PackGroupId = p.Id
	join CustomerGroups as c on d.CustomerGroupId = c.Id
	Where (CONVERT(varchar, b.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and  b.IsActive = 1 ) or (CONVERT(varchar,b.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and  b.IsActive = 1 )  



	IF((select Count(*) from #BaseGroupMarginDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Formula,Margin,CustomerGroup,OilType,PackGroup,ValidFrom,ValidTo from  #BaseGroupMarginDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id 
	  END


	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #BaseGroupMarginDetails
END

/****** Object:  StoredProcedure [dbo].[GetCounterBidJumpsNotification]    Script Date: 15-10-2019 11:51:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetCounterBidJumpsNotification')
    BEGIN
        DROP  Procedure GetCounterBidJumpsNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetCounterBidJumpsNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	CounterBidJumps as c
	join OilTypes as o on c.OilTypeId = o.Id
	join PackGroups as p on c.PackGroupId = p.Id
	join Verticals as v on c.VerticalId = v.Id
	where 
	CONVERT(varchar, c.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and  c.IsActive = 1  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	CounterBidJumps as c
	join OilTypes as o on c.OilTypeId = o.Id
	join PackGroups as p on c.PackGroupId = p.Id
	join Verticals as v on c.VerticalId = v.Id
	where 
	CONVERT(varchar, c.ValidTo,111) = CONVERT(varchar,@ValidTo,111) and  c.IsActive = 1  



	Create TABLE #CounterBidJumpDetails (CounterBidJump decimal(18,2),OilType varchar(1000),Packgroup varchar(1000),Vertical varchar(1000) ,ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #CounterBidJumpDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,CounterBidJump,Vertical,OilType,packgroup,ValidFrom,ValidTo)
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	c.CounterbidJump,
	v.Name as Vertical,
    o.Name as OilType ,
	p.Name as Packgroup,
	c.ValidFrom as ValidFrom,
	c.ValidTo as ValidTo
	from
	CounterBidJumps as c
	join OilTypes as o on c.OilTypeId = o.Id
	join PackGroups as p on c.PackGroupId = p.Id
	join Verticals as v on c.VerticalId = v.Id
    Where (CONVERT(varchar, c.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and c.IsActive = 'true') or (CONVERT(varchar,c.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and c.IsActive = 1)  



	IF((select Count(*) from #CounterBidJumpDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,CounterBidJump,Vertical,OilType,packgroup,ValidFrom,ValidTo from  #CounterBidJumpDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id 
	  END
	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #CounterBidJumpDetails

END
/****** Object:  StoredProcedure [dbo].[GetGpJumpNotification]    Script Date: 15-10-2019 11:52:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetGpJumpNotification')
    BEGIN
        DROP  Procedure GetGpJumpNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetGpJumpNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	 Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	GuaranteePriceJumps as p
	join OilTypes as o on p.OilTypeId = o.Id
	join PackGroups as pg on p.PackGroupId = pg.Id
	join Verticals as v on p.VerticalId = v.Id
	where 
	CONVERT(varchar, p.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and  p.IsActive = 1  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	GuaranteePriceJumps as p
	join OilTypes as o on p.OilTypeId = o.Id
	join PackGroups as pg on p.PackGroupId = pg.Id
	join Verticals as v on p.VerticalId = v.Id
	where 
	CONVERT(varchar, p.ValidTo,111) = CONVERT(varchar,@ValidTo,111) and  p.IsActive = 1  

	Create TABLE #GpJumpDetails (StartRange bigint,EndRange bigint,OilType varchar(1000),Packgroup varchar(1000),Vertical varchar(1000) ,ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #GpJumpDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,packgroup,StartRange,EndRange,ValidFrom,ValidTo)
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	v.Name as Vertical,
    o.Name as OilType ,
	pg.Name as Packgroup,
	p.StartValue as StartRange,
	p.EndValue as EndRange,
	p.ValidFrom as ValidFrom,
	p.ValidTo as ValidTo
	from
	GuaranteePriceJumps as p
	join OilTypes as o on p.OilTypeId = o.Id
	join PackGroups as pg on p.PackGroupId = pg.Id
	join Verticals as v on p.VerticalId = v.Id
	Where (CONVERT(varchar, p.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and p.IsActive = 'true') or (CONVERT(varchar,p.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and p.IsActive = 1)  



	IF((select Count(*) from #GpJumpDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Vertical,OilType,packgroup,StartRange,EndRange,ValidFrom,ValidTo from  #GpJumpDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id 
	  END
	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #GpJumpDetails
    
END
/****** Object:  StoredProcedure [dbo].[GetPercentileNumberNotification]    Script Date: 15-10-2019 11:53:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetPercentileNumberNotification')
    BEGIN
        DROP  Procedure GetPercentileNumberNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetPercentileNumberNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	PercentileNumbers as p
	join PercentileNumberDetails as pd on p.Id = pd.PercentileNumberId
	join OilTypes as o on pd.OilTypeId = o.Id
	join PackGroups as pg on pd.PackGroupId = pg.Id
	where 
	CONVERT(varchar,p.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and p.IsActive = 1  


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	PercentileNumbers as p
	join PercentileNumberDetails as pd on p.Id = pd.PercentileNumberId
	join OilTypes as o on pd.OilTypeId = o.Id
	join PackGroups as pg on pd.PackGroupId = pg.Id
	where 
	CONVERT(varchar,p.ValidTo,111) = CONVERT(varchar,@ValidTo,111) and p.IsActive = 1  


	Create TABLE #PercentileNumberDetails (PercentileNumber bigint,OilType varchar(1000),Packgroup varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #PercentileNumberDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,PercentileNumber,OilType,packgroup,ValidFrom,ValidTo)
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	p.PercentileNumbers as PercentileNumber,
    o.Name as OilType ,
	pg.Name as Packgroup,
	p.ValidFrom as ValidFrom,
	p.ValidTo as ValidTo
	from
	PercentileNumbers as p
	join PercentileNumberDetails as pd on p.Id = pd.PercentileNumberId
	join OilTypes as o on pd.OilTypeId = o.Id
	join PackGroups as pg on pd.PackGroupId = pg.Id
	  
    Where (CONVERT(varchar, p.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) and p.IsActive = 'true') or (CONVERT(varchar,p.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) and p.IsActive = 1)  



	IF((select Count(*) from #PercentileNumberDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,PercentileNumber,OilType,packgroup,ValidFrom,ValidTo from  #PercentileNumberDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id 
	  END
	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #PercentileNumberDetails
END

/****** Object:  StoredProcedure [dbo].[GetRaNotificationDetails]    Script Date: 15-10-2019 11:54:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetRaNotificationDetails')
    BEGIN
        DROP  Procedure GetRaNotificationDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetRaNotificationDetails]
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	 Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	RaNotifications as n
	join RaNotificationDetails as nd on n.Id = nd.RaNotificationId
	join CustomerGroups as c on nd.CustomerGroupId = c.Id
	join Users as u on nd.DealerId = u.Id
	join Districts d on u.DistrictId = d.Id
	join States s on d.StateId = s.Id
	where 
	CONVERT(varchar, n.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) 


	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	RaNotifications as n
	join RaNotificationDetails as nd on n.Id = nd.RaNotificationId
	join CustomerGroups as c on nd.CustomerGroupId = c.Id
	join Users as u on nd.DealerId = u.Id
	join Districts d on u.DistrictId = d.Id
	join States s on d.StateId = s.Id
	where 
	CONVERT(varchar, n.ValidTo,111) = CONVERT(varchar,@ValidTo,111) 



	Create TABLE #RANotificationDetails(Sms bit,Email bit,InAppNotification bit,CustomerGroup varchar(1000),Customer varchar(1000),WindowVolumeCapacity varchar(1000),State varchar(1000),District varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint,RANotificationActionId bigint,RANotificationAction varchar(1000)) 
	INSERT INTO #RANotificationDetails(DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Sms,Email,InAppNotification,CustomerGroup,Customer,WindowVolumeCapacity,State,District,RANotificationActionId,ValidFrom,ValidTo,RANotificationAction)
	Select 
	(select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails) as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	n.SMS as Sms,
	n.Email as Email,
	n.InAppNotification as InAppNotification,
	c.Name as CustomerGroup,
	u.Name as Customer,
    nd.WindowVolumeCapacity as WindowVolumeCapacity,
	s.StateName as State,
	d.DistrictName as District,
	nd.NotificationActionId as RANotificationActionId,
	n.ValidFrom as ValidFrom,
	n.ValidTo as ValidTo,
	'' as RANotificationAction
	from
	RaNotifications as n
	join RaNotificationDetails as nd on n.Id = nd.RaNotificationId
	join CustomerGroups as c on nd.CustomerGroupId = c.Id
	join Users as u on nd.DealerId = u.Id
	join Districts d on u.DistrictId = d.Id
	join States s on d.StateId = s.Id
   Where (CONVERT(varchar, n.ValidTo,111) = CONVERT(varchar, @ValidTo ,111)) or (CONVERT(varchar,n.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)))  



	IF((select Count(*) from #RANotificationDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,Sms,Email,InAppNotification,CustomerGroup,Customer,WindowVolumeCapacity,State,District,RANotificationActionId,ValidFrom,ValidTo,RANotificationAction from  #RANotificationDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id 
	  END
	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #RANotificationDetails
END
/****** Object:  StoredProcedure [dbo].[GetSupriseBenefitUserBasedNotification]    Script Date: 15-10-2019 11:54:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSupriseBenefitUserBasedNotification')
    BEGIN
        DROP  Procedure GetSupriseBenefitUserBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetSupriseBenefitUserBasedNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SurpriseBenefitUsers as s
	join SurpriseBenefitUserMappings sd on s.Id = sd.SurpriseBenefitUserId
	join Skus sku on sd.SkuId = sku.Id
	join Users u on sd.CustomerId = u.Id
	join CustomerGroups c on sd.CustomerGroupId = c.Id
	join BenefitTypes b on s.BenefitTypesId = b.Id
	where CONVERT(varchar,s.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) 

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SurpriseBenefitUsers as s
	join SurpriseBenefitUserMappings sd on s.Id = sd.SurpriseBenefitUserId
	join Skus sku on sd.SkuId = sku.Id
	join Users u on sd.CustomerId = u.Id
	join CustomerGroups c on sd.CustomerGroupId = c.Id
	join BenefitTypes b on s.BenefitTypesId = b.Id
    where CONVERT(varchar,s.ValidTo,111) = CONVERT(varchar,@ValidTo,111)

   Create TABLE #SupriseBenefitUsersDetails (CustomerGroup varchar(1000),DiscountOrDays decimal(10,4),BidQuantityCase decimal(18,2),BidPriceBeforeDiscount decimal(18,2),TotalSaudaValidityDays bigint,BidPriceAfterDiscount decimal(18,2),UserCode varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),UserName varchar(1000) ,BenefitType varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint,SaudaValidity bigint) 
   INSERT INTO #SupriseBenefitUsersDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,BenefitType,DiscountOrDays,SkuCode,SkuName,UserCode,UserName,CustomerGroup,BidQuantityCase,BidPriceBeforeDiscount,BidPriceAfterDiscount,TotalSaudaValidityDays,SaudaValidity,ValidFrom,ValidTo)
   Select 
   (select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails)  as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	b.Name as BenefitType,
	s.DiscountOrDays as Discount,
    sku.SkuCode as SkuCode,
	sku.SkuName as SkuName,
	u.Code as UserCode,
	u.Name as UserName,
	c.Name as CustomerGroup,
	sd.BidQuantityCase as BidQuantityCase,
	sd.BidPriceBeforeDiscount as BidPriceBeforeDiscount,
	sd.BidPriceAfterDiscount as BidPriceAfterDiscount,
	sd.TotalSaudaValidityDays as TotalSaudaValidityDays,
	sd.SaudaValidityPeriod as SaudaValidity,
	s.ValidFrom as ValidFrom,
	s.ValidTo as ValidTo
	from
	SurpriseBenefitUsers as s
	join SurpriseBenefitUserMappings sd on s.Id = sd.SurpriseBenefitUserId
	join Skus sku on sd.SkuId = sku.Id
	join Users u on sd.CustomerId = u.Id
	join CustomerGroups c on sd.CustomerGroupId = c.Id
	join BenefitTypes b on s.BenefitTypesId = b.Id
	Where (CONVERT(varchar,s.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,s.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)))

	IF((select Count(*) from #SupriseBenefitUsersDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,BenefitType,DiscountOrDays,SkuCode,SkuName,UserCode,UserName,CustomerGroup,BidQuantityCase,BidPriceBeforeDiscount,BidPriceAfterDiscount,TotalSaudaValidityDays,SaudaValidity,ValidFrom,ValidTo from  #SupriseBenefitUsersDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #SupriseBenefitUsersDetails
END
/****** Object:  StoredProcedure [dbo].[GetSupriseBenefitGeographyBasedNotification]    Script Date: 15-10-2019 11:55:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSupriseBenefitGeographyBasedNotification')
    BEGIN
        DROP  Procedure GetSupriseBenefitGeographyBasedNotification
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetSupriseBenefitGeographyBasedNotification]
	-- Add the parameters for the stored procedure here
	@ValidTo DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   Create TABLE #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount bigint,Id bigint) 
	INSERT INTO #DayAfterTommorrowExpiringDetails(DayAfterTommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SurpriseBenefitGeographies as s
	join SurpriseBenefitGeographyMappings sd on s.Id = sd.SurpriseBenefitGeographyId
	join Skus sku on sd.SkuId = sku.Id
	join Users u on sd.CustomerId = u.Id
	join CustomerGroups c on sd.CustomerGroupId = c.Id
	join BenefitTypes b on s.BenefitTypesId = b.Id
	join Cities as city on sd.CityId = city.Id
	join Districts as d on city.DistrictId = d.Id
	join States as st on d.StateId = st.Id
	where CONVERT(varchar,s.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)) 

	Create TABLE #TommorrowExpiringDetails(Id bigint,TommorrowExpiringCount bigint) 
	INSERT INTO #TommorrowExpiringDetails(TommorrowExpiringCount,Id) 
	select  count(*),1
	from
	SurpriseBenefitGeographies as s
	join SurpriseBenefitGeographyMappings sd on s.Id = sd.SurpriseBenefitGeographyId
	join Skus sku on sd.SkuId = sku.Id
	join Users u on sd.CustomerId = u.Id
	join CustomerGroups c on sd.CustomerGroupId = c.Id
	join BenefitTypes b on s.BenefitTypesId = b.Id
	join Cities as city on sd.CityId = city.Id
	join Districts as d on city.DistrictId = d.Id
	join States as st on d.StateId = st.Id
    where CONVERT(varchar,s.ValidTo,111) = CONVERT(varchar,@ValidTo,111) 

   Create TABLE #SupriseBenefitGeographyDetails (CustomerGroup varchar(1000),DiscountOrDays decimal(10,4),BidQuantityCase decimal(18,2),BidPriceBeforeDiscount decimal(18,2),TotalSaudaValidityDays bigint,BidPriceAfterDiscount decimal(18,2),SaudaValidity bigint,State varchar(1000),District varchar(1000),City varchar(1000),UserCode varchar(1000),SkuName varchar(1000),SkuCode varchar(1000),UserName varchar(1000) ,BenefitType varchar(1000),ValidFrom DateTime,ValidTo DateTime,DayAfterTommorrowExpiringCount bigint,TommorrowExpiringCount bigint) 
   INSERT INTO  #SupriseBenefitGeographyDetails (DayAfterTommorrowExpiringCount,TommorrowExpiringCount,BenefitType,DiscountOrDays,SkuCode,SkuName,UserCode,UserName,CustomerGroup,BidQuantityCase,BidPriceBeforeDiscount,BidPriceAfterDiscount,TotalSaudaValidityDays,SaudaValidity,City,District,State,ValidFrom,ValidTo)
   Select 
   (select DayAfterTommorrowExpiringCount from #DayAfterTommorrowExpiringDetails)  as DayAfterTommorrowExpiringCount,
	(select TommorrowExpiringCount from  #TommorrowExpiringDetails) as TommorrowExpiringCount,
	b.Name as BenefitType,
	s.DiscountOrDays as Discount,
    sku.SkuCode as SkuCode,
	sku.SkuName as SkuName,
	u.Code as UserCode,
	u.Name as UserName,
	c.Name as CustomerGroup,
	sd.BidQuantityCase as BidQuantityCase,
	sd.BidPriceBeforeDiscount as BidPriceBeforeDiscount,
	sd.BidPriceAfterDiscount as BidPriceAfterDiscount,
	sd.TotalSaudaValidityDays as TotalSaudaValidityDays,
	sd.SaudaValidityPeriod as SaudaValidity,
	st.StateName as State,
	d.DistrictName as District,
	c.Name as City,
	s.ValidFrom as ValidFrom,
	s.ValidTo as ValidTo
	from
	SurpriseBenefitGeographies as s
	join SurpriseBenefitGeographyMappings sd on s.Id = sd.SurpriseBenefitGeographyId
	join Skus sku on sd.SkuId = sku.Id
	join Users u on sd.CustomerId = u.Id
	join CustomerGroups c on sd.CustomerGroupId = c.Id
	join BenefitTypes b on s.BenefitTypesId = b.Id
	join Cities as city on sd.CityId = city.Id
	join Districts as d on city.DistrictId = d.Id
	join States as st on d.StateId = st.Id
	Where (CONVERT(varchar,s.ValidTo,111) = CONVERT(varchar, @ValidTo ,111) ) or (CONVERT(varchar,s.ValidTo,111) = DATEADD(day,1,CONVERT(varchar,@ValidTo,111)))

	IF((select Count(*) from  #SupriseBenefitGeographyDetails) > 0)
		BEGIN
			select DayAfterTommorrowExpiringCount,TommorrowExpiringCount,BenefitType,DiscountOrDays,SkuCode,SkuName,UserCode,UserName,CustomerGroup,BidQuantityCase,BidPriceBeforeDiscount,BidPriceAfterDiscount,TotalSaudaValidityDays,SaudaValidity,City,District,State,ValidFrom,ValidTo from   #SupriseBenefitGeographyDetails
	  END
	ELSE
		BEGIN
			select a.DayAfterTommorrowExpiringCount,b.TommorrowExpiringCount  from #DayAfterTommorrowExpiringDetails as a join #TommorrowExpiringDetails as b on a.Id = b.Id
	  END

	  DROP TABLE #TommorrowExpiringDetails
	  DROP TABLE #DayAfterTommorrowExpiringDetails
	  DROP TABLE #SupriseBenefitGeographyDetails
END
