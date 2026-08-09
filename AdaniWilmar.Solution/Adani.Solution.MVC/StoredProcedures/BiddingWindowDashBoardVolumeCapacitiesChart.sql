/****** Object:  StoredProcedure [dbo].[BiddingWindowDashboardVolumeCapacitiesChart]    Script Date: 04-10-2019 16:12:54 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'BiddingWindowDashboardVolumeCapacitiesChart')
    BEGIN
        DROP  Procedure BiddingWindowDashboardVolumeCapacitiesChart
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BiddingWindowDashboardVolumeCapacitiesChart] 
	-- Add the parameters for the stored procedure here
	@BiddingWindowId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @BiddingWindowId > 0
	BEGIN
		Create TABLE #BiddingWindowBiddedQuantity (BiddedQuantity decimal(10,4) ,OilTypeId bigint) 
		INSERT INTO #BiddingWindowBiddedQuantity (BiddedQuantity,OilTypeId) 
		select sum(BidQuantityInMT),OilTypeId
		from SaudaBiddingCarts 
		where BiddingWindowId = @BiddingWindowId 
		AND StatusId = 2
		group by OiltypeId 



		Create TABLE #BiddingWindowTotalVolmeCapacity (TotalVolumerCapacity decimal(10,4) ,OilTypeId bigint) 
		INSERT INTO #BiddingWindowTotalVolmeCapacity (TotalVolumerCapacity,OilTypeId) 
		select Sum(VolumeCapacity),OilTypeId
		from BiddingWindowVolumeCapacities 
		where BiddingWindowId = @BiddingWindowId 
		group by OiltypeId 

		--select o.Name as OilName
		--,  b.TotalVolumerCapacity -a.BiddedQuantity as RemainingVolumeCapacity 
		--, b.TotalVolumerCapacity as TotalVolumeCapacity 
		--from #BiddingWindowBiddedQuantity as a 
		--Left join #BiddingWindowTotalVolmeCapacity as b 
		--on a.OilTypeId = b.OilTypeId 
		--Left join OilTypes as o  
		--on a.OilTypeId = O.Id

		select 
		o.Name as OilName,
		CAST((b.TotalVolumerCapacity - a.BiddedQuantity) as INT) as RemainingVolumeCapacity, 
		CAST(b.TotalVolumerCapacity as INT) as TotalVolumeCapacity,
		CAST(a.BiddedQuantity as INT) as BookedVolumeCapacity
		from #BiddingWindowTotalVolmeCapacity as b 
		Left join #BiddingWindowBiddedQuantity as a
		on a.OilTypeId = b.OilTypeId 
		Left join OilTypes as o  
		on b.OilTypeId = O.Id

		DROP TABLE #BiddingWindowBiddedQuantity
		DROP TABLE #BiddingWindowTotalVolmeCapacity
		
		
	END
END

--EXEC BiddingWindowDashboardVolumeCapacitiesChart 237
