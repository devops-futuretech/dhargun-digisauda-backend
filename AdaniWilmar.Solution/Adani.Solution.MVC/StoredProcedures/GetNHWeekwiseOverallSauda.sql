USE [AdaniPiloatDB]
GO
/****** Object:  StoredProcedure [dbo].[GetSaudaDataExport]    Script Date: 3/5/2024 11:10:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE or ALTER    PROCEDURE [dbo].[GetNHWeekwiseOverallSauda]
(	
	@UserId bigint,
	@StartDate datetime,
	@EndDate datetime,
	@Status varchar(max)
)
AS 
BEGIN


SET NOCOUNT ON;   

CREATE TABLE #ZHTemp(ZHId BIGINT)
CREATE TABLE #BdoTemp(BdoId BIGINT)
CREATE TABLE #DealerTemp(DealerId BIGINT)
Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings with(NOLOCK) where UserId=@UserId

insert into #ZHTemp(ZHId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId
 insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings with(NOLOCK)
 where ReportingToUserId in (select ZHId from #ZHTemp)
 insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings with(NOLOCK)
 where UserId in (select BdoId from #BdoTemp) group by CustomerId
  
	Select So.*, S.UserId Into #temptable from Saudas s with(nolock)
	Inner Join #UserDivision Ud on S.SalesOrganizationId = Ud.SalesOrganizationId and s.DistributionChannelId = Ud.DistributionChannelId
	and s.DivisionId = Ud.DivisionId
	Join SaudaOrders So with(nolock) on s.Id = So.SaudaId
	Where CAST(So.CreatedDate as date) >= CAST(@StartDate as date)
	and CAST(So.CreatedDate as date) <= CAST(@EndDate as date)
	and So.StatusId in (Select data from dbo.Split(@Status, ','))
	Select t.CreatedDate as date, t.BidQuantity as Achievment from #temptable t
	join #DealerTemp D on t.UserId = D.DealerId

	Drop table #temptable
  drop table #BdoTemp
  drop table #DealerTemp
  drop table #UserDivision
  drop table #ZHTemp
END;

