CREATE PROCEDURE [dbo].[BiddingWindowStatusWiseCount]
	-- Add the parameters for the stored procedure here
	@BiddingWindowId bigint,
	@AcceptedStatus bigint,
	@RejectedStatus bigint,
	@PendingStatus  bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;   


		Create TABLE #BiddingWindowStatusOilTypeWise(StatusId bigint,AcceptedCount bigint,PendingCount bigint,RejectedCount bigint,BidQuantityAccepted decimal(18,4),BidQuantityRejected decimal(18,4),BidQuantityPending decimal(18,4)) 
		INSERT INTO #BiddingWindowStatusOilTypeWise(StatusId,AcceptedCount,RejectedCount,PendingCount,BidQuantityAccepted,BidQuantityRejected,BidQuantityPending) 

		select @AcceptedStatus,
		Count(OilTypeId) as AcceptedCount,
		0,
		0,
		SUM(BidQuantityInMT) as BidQuantityAccepted,
		0,
		0
		from SaudaBiddingCarts 
		where BiddingWindowId = @BiddingWindowId and StatusId = @AcceptedStatus 
		Group By OilTypeId		
		
		INSERT INTO #BiddingWindowStatusOilTypeWise (StatusId,PendingCount,AcceptedCount,RejectedCount,BidQuantityAccepted,BidQuantityRejected,BidQuantityPending) 

		select @PendingStatus,
		Count(OilTypeId) as PendingCount,
		0,
		0,
		0,		
		0,
		SUM(BidQuantityInMT) as BidQuantityRejected
		from SaudaBiddingCarts 
		where BiddingWindowId = @BiddingWindowId and StatusId = @PendingStatus
		Group By OilTypeId


        
		INSERT INTO #BiddingWindowStatusOilTypeWise (StatusId,RejectedCount,AcceptedCount,PendingCount,BidQuantityAccepted,BidQuantityRejected,BidQuantityPending) 

		select @RejectedStatus,
		Count(OilTypeId) as RejectedCount,
		0,
		0,
		0,
		SUM(BidQuantityInMT) as BidQuantityPending,
		0		
		from SaudaBiddingCarts 
		where BiddingWindowId = @BiddingWindowId and StatusId = @RejectedStatus
		Group By OilTypeId


		select a.StatusId,
		(Sum(a.AcceptedCount) + Sum(a.RejectedCount) + Sum(a.PendingCount)) as TotalBidding,
		CAST((Sum(a.BidQuantityAccepted) + Sum(a.BidQuantityRejected) + Sum(a.BidQuantityPending)) as INT) as TotalVolume,
		Sum(a.AcceptedCount) as ApprovedCount,
		CAST(Sum(a.BidQuantityAccepted) as INT) as BidQuantityAccepted,

		Sum(a.RejectedCount) as RejectedCount,
		CAST(Sum(a.BidQuantityRejected) as INT) as BidQuantityRejected,

		Sum(a.PendingCount) as PendingCount,
		CAST(Sum(a.BidQuantityPending) as INT) as BidQuantityPending

		from #BiddingWindowStatusOilTypeWise as a
		GROUP BY a.StatusId
		


		DROP TABLE #BiddingWindowStatusOilTypeWise
END



--EXEC BiddingWindowStatusWiseCount 237,2,3,1
