/****** Object:  StoredProcedure [dbo].[GetPendingContracts]    Script Date: 09-08-2022 13:14:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[GetPendingContracts]   
 @LoginUserId BIGINT,  
 @RoleId BIGINT,  
 @DivisionId BIGINT,  
 @SalesOrgId BIGINT,  
 @DistChannelId BIGINT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;   
  
 CREATE TABLE #DealerIdsTemp(DealerId BIGINT)   
  
 IF(@RoleId = 12) -- NH  
  BEGIN  
   INSERT INTO #DealerIdsTemp(DealerId)  
   Select DISTINCT cus.Id as DealerId  
   From Users nh  
   INNER JOIN Users zh ON nh.Id = zh.ReportingToId  
   INNER JOIN UserCustomerMappings ucm ON ucm.UserId = zh.Id  
   INNER JOIN Users cus ON ucm.CustomerId = cus.Id  
   Where nh.ReportingToId = @LoginUserId   
   --And cus.SaudaBookingtypeId = 1  
   And zh.Id IS NOT NULL  
   And ucm.CustomerId IS NOT NULL  
  END  
 ELSE IF(@RoleId = 9) -- ZH  
  BEGIN  
   INSERT INTO #DealerIdsTemp(DealerId)  
   Select DISTINCT cus.Id as DealerId From Users StateTrader  
   INNER JOIN UserCustomerMappings ucm ON ucm.UserId = StateTrader.Id  
   INNER JOIN Users cus ON ucm.CustomerId = cus.Id  
   Where StateTrader.ReportingToId = @LoginUserId --ANd cus.SaudaBookingtypeId = 1  
  END  
 ELSE IF(@RoleId = 7) --StateTrader  
  BEGIN  
   INSERT INTO #DealerIdsTemp(DealerId)  
   Select DISTINCT cus.Id as DealerId   
   From UserCustomerMappings ucm   
   JOIN Users cus ON ucm.CustomerId = cus.Id  
   Where ucm.UserId = @LoginUserId --And cus.SaudaBookingtypeId = 1  
  END
  ELSE -- Admin  
  BEGIN  
   INSERT INTO #DealerIdsTemp(DealerId)  
   Select u.Id as DealerId From Users u   
   Join UserRoles ur on u.Id = ur.UserId Join Roles r on ur.RoleId = r.Id  
   Where ur.RoleId = 5 --AND u.SaudaBookingTypeId = 1  
  END   
    
    SELECT Distinct P.Id  
      ,P.SaudaOrderId  
      ,P.SaudaNumber   
      ,sorg.Name as SalesOrganization
	  ,dt.Name as DistributionChannel  
      ,v.Name as Division  
      ,( CASE WHEN ContractValidTo = null THEN '' ELSE Convert(nvarchar(50),ContractValidTo,103) END) AS ContractValidTo  
      ,CustomerCode  
      ,CustomerName
      ,MaterialCode  
      ,BasicRate  
      ,PendingQuantityInCase  
      ,SaudaQuantity  
	  ,( CASE WHEN P.CreatedDate = null THEN '' ELSE Convert(nvarchar(50),P.CreatedDate,103) END) AS CreatedDate
   from 
   PendingContracts as P  
   LEFT JOIN Users u ON P.CustomerCode = u.Code  
   --LEFT JOIN Depots d ON d.Code = P.PlantCode  
   Left Join Skus sku On Sku.SkuCode = P.MaterialCode  
   LEFT JOIN OilTypes ot ON ot.Id = sku.OilTypeId  
  -- Inner join PackGroups pg on sku.PackGroupId = pg.Id
   Left Join Divisions v on p.DivisionId = v.Id
   Left Join SalesOrganizations sorg on p.SalesOrgId = sorg.Id
   Left Join DistributionChannels dt on p.DistChnlId = dt.Id
  -- Inner Join Saudas so on so.SaudaNumber = p.SaudaNumber
 --  Left Join UserCustomerMappings as ucm on ucm.CustomerId = u.Id 
  -- Left Join Users as StateTrader on StateTrader.Id = ucm.UserId
 --  left join UserRoles as userrole on userrole.UserId = StateTrader.ID
   WHERE u.Id IN (SELECT DealerId FROM #DealerIdsTemp) 
   AND ((@DivisionId = 0) Or (@DivisionId > 0 And p.DivisionId = @DivisionId )) 
   AND ((@DistChannelId = 0) Or (@DistChannelId > 0 And p.DistChnlId = @DistChannelId ))  
   AND ((@SalesOrgId = 0) Or (@SalesOrgId > 0 And p.SalesOrgId = @SalesOrgId ))
   AND p.DistChnlId = sku.DistributionChannelId 
   and p.SalesOrgId = sku.SalesOrganizationId
   and p.DivisionId = sku.DivisionId 
  -- and userrole.ROleId = 7
     
   DROP TABLE #DealerIdsTemp  
END  

