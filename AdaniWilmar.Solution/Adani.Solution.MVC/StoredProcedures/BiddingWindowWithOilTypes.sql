CREATE PROCEDURE [dbo].[BiddingWindowWithOilTypes](
	@BiddingWindowId bigint
)	
AS
BEGIN 
 
	Create TABLE #BiddingWindowOilTypeDetails (OilName varchar(1000),TotalVolumeCapacity decimal(10,4),BiddingWindowId bigint,OilTypeId bigint) 
	INSERT INTO #BiddingWindowOilTypeDetails (OilName,TotalVolumeCapacity,BiddingWindowId,OilTypeId) 
	select  Distinct o.Name,bwvc.VolumeCapacity,BW.Id,bwvc.OilTypeId
	from BiddingWindows bw With(NoLock)
	Left Join BiddingWindowVolumeCapacities bwvc With(NoLock) on bw.Id = bwvc.BiddingWindowId
	Left Join OilTypes o With(NoLock)  on bwvc.OilTypeId = o.Id 
	where bw.Id = @BiddingWindowId
	
	Create TABLE #BiddingWindowBookedCapacityDetails (BookedVolumeCapacity decimal(10,4),OilTypeId bigint) 
	INSERT INTO #BiddingWindowBookedCapacityDetails (BookedVolumeCapacity,OilTypeId) 
	select sum(BidQuantity),OilTypeId
	from SaudaOrders  
	where BiddingwindowId = @BiddingWindowId
	group by OilTypeId

	select 
			a.OilName as OilTypeName,
			b.BookedVolumeCapacity as BookedVolumeCapacity,
			a.TotalVolumeCapacity as TotalVolumeCapacity
		from  #BiddingWindowOilTypeDetails as a 
		Left join #BiddingWindowBookedCapacityDetails as b
		on a.OilTypeId = b.OilTypeId 
		  
		DROP TABLE #BiddingWindowBookedCapacityDetails
		DROP TABLE #BiddingWindowOilTypeDetails
		END	

