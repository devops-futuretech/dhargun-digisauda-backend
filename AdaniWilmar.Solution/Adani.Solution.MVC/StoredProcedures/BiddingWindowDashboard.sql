/****** Object:  StoredProcedure [dbo].[BiddingWindowDashboard]    Script Date: 04-10-2019 16:11:27 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'BiddingWindowDashboard')
    BEGIN
        DROP  Procedure BiddingWindowDashboard
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BiddingWindowDashboard](
	@StatusId bigint,
	@SearchDate date
)
AS
BEGIN

--drop table #TempTable2
	Create TABLE #BiddingWindowOilTypeDetails (OilName varchar(1000),WindowName varchar(1000),BiddingWindowId bigint) 
	INSERT INTO #BiddingWindowOilTypeDetails (OilName,BiddingWindowId) 
	select  Distinct o.Name ,BW.Id
	from BiddingWindows bw With(NoLock)
	Left join SaudaOrders so With(NoLock) on so.BiddingWindowId = bw.Id
	Left Join BiddingWindowVolumeCapacities bwvc With(NoLock) on bw.Id = bwvc.BiddingWindowId
	Left Join OilTypes o With(NoLock)  on bwvc.OilTypeId = o.Id 
	where bw.StatusId = @StatusId  and CONVERT(varchar,bw.CreatedDate,111) = CONVERT(varchar, @SearchDate ,111)
	
	Create TABLE #BiddingWindowCustomerGroupDetails (CustomerGroups varchar(1000),BiddingWindowId bigint) 
	INSERT INTO #BiddingWindowCustomerGroupDetails (CustomerGroups,BiddingWindowId) 
	select c.Name,bw.Id
	from BiddingWindows bw With(NoLock)
	Left Join BiddingWindowCustomerGroups bwcg With(NoLock)  on  bw.Id = bwcg.BiddingWindowId
	Left Join CustomerGroups c With(NoLock) on bwcg.CustomerGroupId = c.Id


	--While (Select Count(*) From #TempTable) > 0
	--Begin
	
		Create TABLE #BiddingWindowDetailswithOilNames (OilNames varchar(1000),WindowName varchar(1000),BiddingWindowId bigint,CustomerGroups varchar(1000) ) 
		INSERT INTO #BiddingWindowDetailswithOilNames (BiddingWindowId,OilNames) 
		SELECT BiddingWindowId, 
			OilNames = STUFF(
                 (SELECT ',' + OilName FROM #BiddingWindowOilTypeDetails
				  As T2
				  WHERE T2.BiddingWindowId = T1.BiddingWindowId
				  FOR XML PATH ('')), 1, 1, ''
               ) 
		FROM #BiddingWindowOilTypeDetails  As T1
		GROUP BY BiddingWindowId	


		Create TABLE #BiddingWindowDetailsWithCustomerGroupNames (BiddingWindowId bigint,CustomerGroups varchar(1000) ) 
		INSERT INTO #BiddingWindowDetailsWithCustomerGroupNames (BiddingWindowId,CustomerGroups) 
		SELECT  BiddingWindowId,
			  CustomerGroups = STUFF(
                 (SELECT ',' + CustomerGroups FROM #BiddingWindowCustomerGroupDetails
				   As T2
				  WHERE T2.BiddingWindowId = T1.BiddingWindowId
				  FOR XML PATH ('')), 1, 1, ''
               ) 
		FROM #BiddingWindowCustomerGroupDetails as T1
		GROUP BY BiddingWindowId	
				
		Create TABLE #BiddingWindowTotalVolumeCapacity (TotalVolumeCapacity decimal(10,4),BiddingWindowId bigint) 
		INSERT INTO #BiddingWindowTotalVolumeCapacity (TotalVolumeCapacity,BiddingWindowId) 
		select sum(VolumeCapacity),BiddingWindowId from BiddingWindowVolumeCapacities  group by BiddingWindowId  
		
		Create TABLE #BiddingWindowBookedVolumeCapacity (BookedVolumeCapacity decimal(10,4),BiddingWindowId bigint) 
		INSERT INTO #BiddingWindowBookedVolumeCapacity (BookedVolumeCapacity,BiddingWindowId) 
		select sum(BidQuantityInMT),BiddingWindowId from SaudaBiddingCarts Where StatusId = 2 group by BiddingWindowId  

		Create TABLE #BiddingWindowSaudaBookedQuantity (BookedVolumeCapacity varchar(1000),BiddingWindowId bigint) 
		INSERT INTO #BiddingWindowSaudaBookedQuantity (BookedVolumeCapacity,BiddingWindowId) 
		select   CAST(Cast(b.BookedVolumeCapacity as INT) as varchar) + ' / ' + CAST(Cast(a.TotalVolumeCapacity as INT) as varchar), a.BiddingWindowId from #BiddingWindowTotalVolumeCapacity as a join #BiddingWindowBookedVolumeCapacity as b on a.BiddingWindowId =b.BiddingWindowId

		select 
			a.OilNames as OilTypes 
			,b.CustomerGroups as CustomerGroups,
			bw.Name as WindowName,
			CONVERT(varchar,Cast(bw.StartTime as Time),108) + ' - ' + CONVERT(varchar,Cast(bw.EndTime as Time),108) as WindowStartAndEndTime,
			CONVERT(varchar,Cast(bw.SaudaAllocationStartTime as Time),108) + ' - ' + CONVERT(varchar,Cast(bw.SaudaAllocationEndTime as Time),108) as SaudaAllocationStartAndEndTime,
			sb.BookedVolumeCapacity as SaudaBooked,
			bw.Id as BiddingWindowId
		from #BiddingWindowDetailswithOilNames as a 
		Left join #BiddingWindowDetailsWithCustomerGroupNames as b
		on a.BiddingWindowId = b.BiddingWindowId 
		Left join BiddingWindows as bw on bw.Id = a.BiddingWindowId 
		Left join #BiddingWindowSaudaBookedQuantity as sb on sb.BiddingWindowId = a.BiddingWindowId 
				
		DROP TABLE #BiddingWindowDetailswithOilNames
		DROP TABLE #BiddingWindowOilTypeDetails
		DROP TABLE #BiddingWindowCustomerGroupDetails
		DROP TABLE #BiddingWindowDetailsWithCustomerGroupNames
		DROP TABLE #BiddingWindowTotalVolumeCapacity
		DROP TABLE #BiddingWindowBookedVolumeCapacity
		DROP TABLE #BiddingWindowSaudaBookedQuantity
--End	
END


--EXEC BiddingWindowDashboard 4,'2019/12/21'
