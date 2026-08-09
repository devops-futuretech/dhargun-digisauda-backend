CREATE PROCEDURE [dbo].[BiddingWindowStatusStateWiseCount]
	-- Add the parameters for the stored procedure here
	@BiddingWindowId bigint,
	@AcceptedStatus bigint,
	@RejectedStatus bigint,
	@PendingStatus  bigint,
	@StateId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;   


		CREATE TABLE #BiddingWindowStatusOilTypeWise(StatusId bigint,OiltypeId bigint,OilType varchar(50),AcceptedCount bigint,PendingCount bigint,RejectedCount bigint,BidQuantityAccepted decimal(18,4),BidQuantityRejected decimal(18,4),BidQuantityPending decimal(18,4)) 
		INSERT INTO #BiddingWindowStatusOilTypeWise(StatusId,OiltypeId,OilType,AcceptedCount,RejectedCount,PendingCount,BidQuantityAccepted,BidQuantityRejected,BidQuantityPending) 

		SELECT 
			@AcceptedStatus,
			sb.OiltypeId,
			ot.Name as OilName,
			Count(OilTypeId) as AcceptedCount,
			0,
			0,
			SUM(BidQuantityInMT) as BidQuantityAccepted,
			0,
			0
		FROM SaudaBiddingCarts sb
		JOIN Users u ON u.Id = sb.DealerId
		JOIN OilTypes ot ON ot.Id = sb.OilTypeId
		WHERE BiddingWindowId = @BiddingWindowId 
		AND StatusId = @AcceptedStatus 
		AND u.StateId = @StateId
		GROUP BY sb.OilTypeId,ot.Name		
		
		DECLARE @TotalCount bigint = (SELECT SUM(AcceptedCount) FROM #BiddingWindowStatusOilTypeWise)
		DECLARE @TotalVolumeBooked Decimal(18,4) = (SELECT SUM(BidQuantityAccepted) FROM #BiddingWindowStatusOilTypeWise)

		select 
			a.StatusId,
			a.OiltypeId,
			a.OilType,		
			@TotalCount AS TotalBidding,
			CAST(@TotalVolumeBooked as INT) AS TotalVolume,
			SUM(a.AcceptedCount) AS ApprovedCount,
			CAST(SUM(a.BidQuantityAccepted) as INT) AS BidQuantityAccepted
		from #BiddingWindowStatusOilTypeWise AS a
		GROUP BY a.StatusId,a.OiltypeId,a.OilType

		DROP TABLE #BiddingWindowStatusOilTypeWise
END


--EXEC BiddingWindowStatusStateWiseCount 237,2,3,1,40