CREATE PROCEDURE [dbo].[GetBiddingWindowTotalAndRemaining]
		@BiddingWindowId bigint
AS
BEGIN
	SET NOCOUNT ON;
	IF @BiddingWindowId > 0
	BEGIN
		With Cte_SaudaBookedVolume As(
    Select
    so.OilTypeId,
    SUM(so.BidQuantity) as BookedVolumeCapacity,
    RemainingVolumeCapacity = (Select VolumeCapacity From BiddingWindowVolumeCapacities Where BiddingWindowid = @BiddingWindowId And OilTypeId = so.OilTypeId) - SUM(so.BidQuantity)
    From SaudaOrders so
    Where so.BiddingWindowid = @BiddingWindowId
    Group By so.OilTypeId
)
Select v.OilTypeId,v.VolumeCapacity as TotalVolumeCapacity,so.BookedVolumeCapacity,so.RemainingVolumeCapacity
From BiddingWindowVolumeCapacities v
Left Join Cte_SaudaBookedVolume so ON so.OilTypeId = v.OilTypeId
Where v.BiddingWindowid = @BiddingWindowId
END
END