/****** Object:  StoredProcedure [dbo].[BiddingWindowOilWiseStatusCount]    Script Date: 14-11-2019 10:59:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BiddingWindowOilWiseStatusCount]
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


		Create TABLE #BiddingWindowStatusOilTypeWise(OilTypeId bigint,AcceptedCount bigint,PendingCount bigint,RejectedCount bigint,BidQuantityAccepted decimal(18,4),BidQuantityRejected decimal(18,4),BidQuantityPending decimal(18,4)) 
		INSERT INTO #BiddingWindowStatusOilTypeWise(OilTypeId,AcceptedCount,RejectedCount,PendingCount,BidQuantityAccepted,BidQuantityRejected,BidQuantityPending) 

		select 
		OilTypeId,
		Count(OilTypeId) as AcceptedCount,
		0,
		0,
		SUM(BidQuantityInMT) as BidQuantityAccepted,
		0,
		0
		from SaudaBiddingCarts 
		where BiddingWindowId = @BiddingWindowId and StatusId = @AcceptedStatus 
		Group By OilTypeId		
		
		INSERT INTO #BiddingWindowStatusOilTypeWise (OilTypeId,PendingCount,AcceptedCount,RejectedCount,BidQuantityAccepted,BidQuantityRejected,BidQuantityPending) 

		select 
		OilTypeId,
		Count(OilTypeId) as PendingCount,
		0,
		0,
		0,		
		0,
		SUM(BidQuantityInMT) as BidQuantityRejected
		from SaudaBiddingCarts 
		where BiddingWindowId = @BiddingWindowId and StatusId = @PendingStatus
		Group By OilTypeId


        
		INSERT INTO #BiddingWindowStatusOilTypeWise (OilTypeId,RejectedCount,AcceptedCount,PendingCount,BidQuantityAccepted,BidQuantityRejected,BidQuantityPending) 

		select 
		OilTypeId,
		Count(OilTypeId) as RejectedCount,
		0,
		0,
		0,
		SUM(BidQuantityInMT) as BidQuantityPending,
		0		
		from SaudaBiddingCarts 
		where BiddingWindowId = @BiddingWindowId and StatusId = @RejectedStatus
		Group By OilTypeId


		select b.Name as OilTypes,
		Sum(a.AcceptedCount) as ApprovedCount,
		CAST(Sum(a.BidQuantityAccepted) as INT) as BidQuantityAccepted,

		Sum(a.RejectedCount) as RejectedCount,
		CAST(Sum(a.BidQuantityRejected) as INT) as BidQuantityRejected,

		Sum(a.PendingCount) as PendingCount,
		CAST(Sum(a.BidQuantityPending) as INT) as BidQuantityPending

		from #BiddingWindowStatusOilTypeWise as a join OilTypes as b on a.OilTypeId = b.Id
		group by a.OilTypeId,b.Name


		DROP TABLE #BiddingWindowStatusOilTypeWise
END



--EXEC BiddingWindowOilWiseStatusCount 237,2,3,1

