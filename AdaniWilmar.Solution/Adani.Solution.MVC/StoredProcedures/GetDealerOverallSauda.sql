USE [AdaniDB]
GO
/****** Object:  StoredProcedure [dbo].[GetSaudaDataExport]    Script Date: 3/5/2024 11:10:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create or Alter  PROCEDURE [dbo].[GetDealerOverallSauda]
(	
	@CustomerId bigint,
	@UserId bigint,
	@StartDate datetime,
	@EndDate datetime,
	@Status varchar(max)
)
AS 
BEGIN


SET NOCOUNT ON;   

CREATE TABLE #DealerTemp(DealerId BIGINT)
Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

if(@CustomerId>0)
begin
insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
 where UserId=@UserId and CustomerId=@CustomerId
end
else
begin
 insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
 where UserId=@UserId
end

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
 and so.StatusId in (select Data from dbo.Split(@Status,','))
  drop table #DealerTemp
  drop table #UserDivision

END;

