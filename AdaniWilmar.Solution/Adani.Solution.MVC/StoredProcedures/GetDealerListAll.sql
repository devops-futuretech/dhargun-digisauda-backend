USE [AdaniDB]
GO
/****** Object:  StoredProcedure [dbo].[GetDealerListAll]    Script Date: 2/19/2024 7:22:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[GetDealerListAll]    Script Date: 06-01-2023 14:46:03 ******/
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
Create or alter  PROCEDURE [dbo].[GetDealerListAll]   
 @LoginUserId BIGINT,  
 @RoleId BIGINt
AS  
BEGIN  
   SET NOCOUNT ON;
CREATE TABLE #DealerIdsTemp(DealerId BIGINT) 
IF(@RoleId = 12) -- NH  
BEGIN  
INSERT INTO #DealerIdsTemp(DealerId)  
Select DISTINCT cus.Id as DealerId  
From UserReportingToMappings zh with(NOLOCK)
INNER JOIN UserReportingToMappings bdo with(NOLOCK) ON zh.UserId = bdo.ReportingToUserId  
INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
Where zh.ReportingToUserId = @LoginUserId
END  
ELSE IF(@RoleId = 9) -- ZH  
BEGIN  
INSERT INTO #DealerIdsTemp(DealerId)  
Select DISTINCT 
cus.Id as DealerId From UserReportingToMappings bdo  
INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
Where bdo.ReportingToUserId = @LoginUserId 
END  
ELSE IF(@RoleId = 7) --BDO  
BEGIN  
INSERT INTO #DealerIdsTemp(DealerId)  
Select DISTINCT cus.Id as DealerId   
From UserCustomerMappings ucm with(NOLOCK) 
JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
Where ucm.UserId = @LoginUserId 
END
ELSE -- Admin  
BEGIN  
INSERT INTO #DealerIdsTemp(DealerId)  
Select u.Id as DealerId From Users u with(NOLOCK)
Join UserRoles ur with(NOLOCK) on u.Id = ur.UserId 
Join Roles r with(NOLOCK) on ur.RoleId = r.Id  
Where ur.RoleId = 5 
END  
   

   
Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

if(@RoleId = 1)
begin
	insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) Select SalesOrganizationId,DistributionChannelId,Id as DivisionId from Divisions 
end
else
begin
	insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
	select SalesOrganizationId,DistributionChannelId,DivisionId 
	from UserDivisionMappings 
	where UserId=@LoginUserId
end

   select distinct
   u.Id,
   u.StateId,
   u.Code as EmployeeCode,
   u.SaudaBookingTypeId,
   (u.Name+'-'+IsNull(c.CityName,'')+'-'+Isnull(s.StateName,'')+'-'+u.Code) as EmployeeName
  from   
  Users u
  join UserDivisionMappings udiv on u.Id=udiv.UserId
  join #UserDivision ud on ud.SalesOrganizationId=udiv.SalesOrganizationId and ud.DistributionChannelId=udiv.DistributionChannelId and ud.DivisionId=udiv.DivisionId
	left join Cities c on u.CityId=c.Id
	left join States s on u.StateId=s.Id
	left join UserRoles ur on u.Id=ur.UserId
  where 
   u.Id IN (SELECT DealerId FROM #DealerIdsTemp) 
 

 
DROP TABLE #DealerIdsTemp  
DROP TABLE #UserDivision  
END







