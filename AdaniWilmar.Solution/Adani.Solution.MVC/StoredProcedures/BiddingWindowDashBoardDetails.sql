/****** Object:  StoredProcedure [dbo].[BiddingWindowDashboardDetails]    Script Date: 04-10-2019 16:12:36 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'BiddingWindowDashboardDetails')
    BEGIN
        DROP  Procedure BiddingWindowDashboardDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BiddingWindowDashboardDetails]
	-- Add the parameters for the stored procedure here
	@BiddingWindowId bigint
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @OilTypes varchar(1000), @CustomerGroups  varchar(1000),@TotalVolumeCapacity decimal(10,4),@BookedQuantity decimal(10,4),@SaudaBookedQuantity varchar(1000)

	Create TABLE #BiddingWindowOilTypeDetails (OilName varchar(1000),BiddingWindowId bigint) 
	INSERT INTO #BiddingWindowOilTypeDetails (OilName,BiddingWindowId) 
	select  Distinct o.Name ,BW.Id
	from BiddingWindows bw With(NoLock)
	Left Join BiddingWindowVolumeCapacities bwvc With(NoLock) on bw.Id = bwvc.BiddingWindowId
	Left Join OilTypes o With(NoLock)  on bwvc.OilTypeId = o.Id 
	where bw.Id = @BiddingWindowId
	
	Create TABLE #BiddingWindowCustomerGroupDetails (CustomerGroups varchar(1000),BiddingWindowId bigint) 
	INSERT INTO #BiddingWindowCustomerGroupDetails (CustomerGroups,BiddingWindowId) 
	select Distinct c.Name,bw.Id
	from BiddingWindows bw With(NoLock)
	Left Join BiddingWindowCustomerGroups bwcg With(NoLock)  on  bw.Id = bwcg.BiddingWindowId
	Left Join CustomerGroups c With(NoLock) on bwcg.CustomerGroupId = c.Id
	where  bw.Id = @BiddingWindowId

	
	set @OilTypes=''
	SELECT  @OilTypes = STUFF((
            SELECT ',' + OilName
            FROM #BiddingWindowOilTypeDetails
            FOR XML PATH ('')
            ), 1, 1, '')
	FROM #BiddingWindowOilTypeDetails

	set @CustomerGroups=''
	SELECT  @CustomerGroups = STUFF((
            SELECT ',' + CustomerGroups
            FROM #BiddingWindowCustomerGroupDetails
            FOR XML PATH ('')
            ), 1, 1, '')
	FROM #BiddingWindowCustomerGroupDetails

		
		select  @TotalVolumeCapacity =  sum(VolumeCapacity) 
		from BiddingWindowVolumeCapacities  
		where  BiddingWindowId = @BiddingWindowId
		select @BookedQuantity = sum(BidQuantityInMT) 
		from SaudaBiddingCarts 
		where  BiddingWindowId = @BiddingWindowId
		AND StatusId = 2

		SET @SaudaBookedQuantity =  Cast(Cast(@BookedQuantity as INT) as varchar)  + ' / ' +  Cast(CAST(@TotalVolumeCapacity as INT) as varchar)


	select  @OilTypes as OilTypes,
	@CustomerGroups as CustomerGroups,
	Name as WindowName,
	CONVERT(varchar,Cast(StartTime as Time),108) + ',' + CONVERT(varchar,Cast(EndTime as Time),108) as WindowStartAndEndTime,
	CONVERT(varchar,Cast(SaudaAllocationStartTime as Time),108) + ',' + CONVERT(varchar,Cast(SaudaAllocationEndTime as Time),108) as SaudaAllocationStartAndEndTime,
	Id as BiddingWindowId,
	@SaudaBookedQuantity as SaudaBooked
	from  BiddingWindows 
	where Id = @BiddingWindowId
		

	DROP TABLE 	#BiddingWindowOilTypeDetails
	DROP TABLE 	#BiddingWindowCustomerGroupDetails
	
END


--EXEC BiddingWindowDashboardDetails 237
