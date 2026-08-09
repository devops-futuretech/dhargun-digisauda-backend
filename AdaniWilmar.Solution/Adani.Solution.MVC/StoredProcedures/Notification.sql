/****** Object:  StoredProcedure [dbo].[GetVolumeCapacityDetails]    Script Date: 25-09-2019 11:02:59 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetVolumeCapacityDetails')
    BEGIN
        DROP  Procedure GetVolumeCapacityDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetVolumeCapacityDetails]
	-- Add the parameters for the stored procedure here
	@BiddingWoindowId bigint,
	@OilTypeId bigint
AS
DECLARE @TotalVolumeCapacity decimal, @BiddedVolumeCapacity decimal
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

	SET NOCOUNT ON;

	IF @BiddingWoindowId > 0
	BEGIN
		
			Create TABLE #VolumeCapacities (OilTypeId bigint,TotalVolumeCapacity decimal(10,4),BiddingWindowId bigint) 
			INSERT INTO #VolumeCapacities (OilTypeId,TotalVolumeCapacity,BiddingWindowId) 
			select OilTypeId,VolumeCapacity,BiddingWindowId from BiddingWindowVolumeCapacities where BiddingWindowId = @BiddingWoindowId and  OilTypeId = @OilTypeId
		

	IF exists(select 1 from #VolumeCapacities)
		begin
		
			Create TABLE #SaudaBookedQuantity (OilTypeId bigint,UsedVolumeCapacity decimal(10,4)) 
			INSERT INTO #SaudaBookedQuantity (OilTypeId,UsedVolumeCapacity)
			select OilTypeId, Sum(BidQuantity) from  SaudaOrders where SaudaBookingTypeId = 2 and BiddingWindowId = @BiddingWoindowId Group by OilTypeId
		end
	
   select CASE 
       WHEN t1.UsedVolumeCapacity < t.TotalVolumeCapacity 
		   THEN		   
		   (t1.UsedVolumeCapacity / t.TotalVolumeCapacity) * 100 
			ELSE 0
       END as UsedPercentage, 
	   o.Name as OilName, 
	   b.Name as WindowName,
	   t.TotalVolumeCapacity,
	   b.StartTime as StartTime,
	   b.EndTime as EndTime,
	   t.TotalVolumeCapacity-t1.UsedVolumeCapacity as RemainingVolumeCapacity
	   from #VolumeCapacities t 
	   join #SaudaBookedQuantity t1 on t.OilTypeId = t1.OilTypeId 
	   join BiddingWindows b on t.BiddingWindowId = b.Id 
	   join OilTypes o on t.OilTypeId = o.Id

	  RETURN
	END

	DROP TABLE #VolumeCapacities
	DROP TABLE #SaudaBookedQuantity
	
END