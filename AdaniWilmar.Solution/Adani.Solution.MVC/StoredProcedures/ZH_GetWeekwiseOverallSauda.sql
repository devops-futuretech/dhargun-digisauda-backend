USE [AdaniDb]
GO
/****** Object:  StoredProcedure [dbo].[ZH_GetWeekwiseOverallSales]    Script Date: 3/5/2024 1:01:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create or ALTER  PROCEDURE [dbo].[ZH_GetWeekwiseOverallSauda]
(	
	@UserId bigint,
	@StartDate datetimeoffset,
	@EndDate datetimeoffset,
	@Status varchar(max)
)
AS 
BEGIN
CREATE TABLE #BdoTemp(BdoId BIGINT)
CREATE TABLE #DealerTemp(DealerId BIGINT)
Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings with(NOLOCK) where UserId=@UserId

 insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings with(NOLOCK)
 where ReportingToUserId=@UserId
 insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings with(NOLOCK)
 where UserId in (select BdoId from #BdoTemp)

 select 
  so.CreatedDate as Date,
  so.BidQuantity as Achievment
 from Saudas s with(NOLOCK)
 join SaudaOrders so with(NOLOCK) on s.Id=so.SaudaId
 join #UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId
 and ud.DistributionChannelId=s.DistributionChannelId and ud.DivisionId=s.DivisionId
 where Cast(so.CreatedDate as date) >= Cast(@StartDate as date)
 and Cast(so.CreatedDate as date) <= Cast(@EndDate as date)
 and s.UserId in (select DealerId from #DealerTemp)
 and so.StatusId in (Select Data from dbo.Split(@Status,','))
   drop table #BdoTemp
  drop table #DealerTemp
  drop table #UserDivision
End
