/****** Object:  StoredProcedure [dbo].[BiddingWindowStateWiseAcceptedDetails]    Script Date: 14-11-2019 11:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BiddingWindowStateWiseAcceptedDetails]
	-- Add the parameters for the stored procedure here
	@BiddingWindowId bigint,
	@AcceptedStatus bigint,
	@StateId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Create TABLE #BiddingWindowStateWiseAcceptedCount(OilTypes varchar(1000),StateId bigint,StatusId bigint) 
	INSERT INTO #BiddingWindowStateWiseAcceptedCount(OilTypes,StateId,StatusId) 
	select distinct d.Name as OilTypes ,b.StateId as StateId,a.StatusId as  StatusId
	from SaudaBiddingCarts as a 
	join Users as b on a.DealerId = b.Id 
    join OilTypes as d on a.OilTypeId = d.Id
	where a.BiddingWindowId = @BiddingWindowId
	group by b.StateId,d.Name,a.StatusId
	--group by a.OilTypeId,c.StateName


	select OilTypes,COUNT(StatusId) as ApprovedCount  from #BiddingWindowStateWiseAcceptedCount 
	where StatusId = @AcceptedStatus and StateId = @StateId
	group by OilTypes


	DROP TABLE #BiddingWindowStateWiseAcceptedCount

END
