CREATE PROCEDURE [dbo].[BiddingWindowDashboardDetailsInGrid]
	@StatusId bigint,
	@SearchDate date
AS
BEGIN 
 
	Create TABLE #BiddingWindowCustomerGroupDetails (CustomerGroups varchar(1000),BiddingWindowId bigint) 
	INSERT INTO #BiddingWindowCustomerGroupDetails (CustomerGroups,BiddingWindowId) 
	select c.Name,bw.Id
	from BiddingWindows bw With(NoLock)
	Left Join BiddingWindowCustomerGroups bwcg With(NoLock)  on  bw.Id = bwcg.BiddingWindowId
	Left Join CustomerGroups c With(NoLock) on bwcg.CustomerGroupId = c.Id

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
	
	Create TABLE #BiddingWindowBookedVolumeCapacity (BookedVolumeCapacity decimal(10,4),BiddingWindowId bigint) 
	INSERT INTO #BiddingWindowBookedVolumeCapacity (BookedVolumeCapacity,BiddingWindowId) 
	select sum(BidQuantity),BiddingWindowId from SaudaOrders group by BiddingWindowId  

	Create TABLE #BiddingWindowStatus (BiddingWindowId bigint,StatusName varchar(1000)) 
	INSERT INTO #BiddingWindowStatus (BiddingWindowId,StatusName) 
	select b.Id,bs.Name from BiddingWindows as b With(NoLock)
	Left join BiddingWindowStatus as bs on b.StatusId = bs.Id

	Create TABLE #BiddingWindowPlantName (BiddingWindowId bigint,PlantName varchar(1000)) 
	INSERT INTO #BiddingWindowPlantName (BiddingWindowId,PlantName) 
	select Distinct s.BiddingwindowId,d.Name from SaudaOrders as s join Depots as d
	on s.PlantId = d.Id 
	where d.IsPlant = 1

	select 
	b.Id as BiddingWindowId,
	b.Name as WindowName,
	CONVERT(varchar(15),Cast(b.StartTime as Time), 100) + ' - ' +
	CONVERT(varchar(15),Cast(b.EndTime as Time), 100) as WindowStartAndEndTime,
	bcgd.CustomerGroups as CustomerGroups,
	bvc.BookedVolumeCapacity as BookedVolumeCapacity,
	bstatus.StatusName as WindowStatusName,
	bp.PlantName as PlantName
	from BiddingWindows as b With(NoLock)
	left join #BiddingWindowDetailsWithCustomerGroupNames as bcgd With(NoLock)
	on b.Id = bcgd.BiddingWindowId
	left join #BiddingWindowBookedVolumeCapacity as bvc With(NoLock) on b.Id = bvc.BiddingWindowId
	left join #BiddingWindowStatus as bstatus With(NoLock) on b.Id = bstatus.BiddingWindowId
	left join #BiddingWindowPlantName as bp With(NoLock) on b.Id = bp.BiddingWindowId
	where b.StatusId = @StatusId and Cast(b.CreatedDate as date) = Cast(@SearchDate as Date)

	DROP TABLE #BiddingWindowCustomerGroupDetails
	DROP TABLE #BiddingWindowDetailsWithCustomerGroupNames
	DROP TABLE #BiddingWindowBookedVolumeCapacity
	DROP TABLE #BiddingWindowStatus	
	DROP TABLE #BiddingWindowPlantName
END