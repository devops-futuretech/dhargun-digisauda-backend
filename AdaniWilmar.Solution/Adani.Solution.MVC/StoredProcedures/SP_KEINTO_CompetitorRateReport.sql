USE [AdaniDB]
GO

/****** Object:  StoredProcedure [dbo].[SP_KEINTO_CompetitorRateReport]    Script Date: 29-08-2022 09:35:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[SP_KEINTO_CompetitorRateReport]
(
@StartDate DateTime,
@EndDate DateTime,
@VerticalId bigint,
@SalesOrganizationId bigint,
@DistributionChannelId bigint
)
As
Begin
Select * Into #temp 
From(
Select U.StateId,S.StateName, CA.SkuId,SKU.SkuName As Product, HQ.Name As NameOfMarket,CT.Name As Competitor,CAD.SaudaRate As PTD, CAD.MarketOperatingPrice As MOP
From CompetitorAnalysis CA With(NoLock)
Inner Join CompetitorAnalysisDetails CAD With(NoLock) On CA.Id = CAD.CompetitorAnalysisId
Inner Join Competitors CT With(NoLock) On CT.Id = CAD.CompetitorId
Inner Join SKUs SKU With(NoLock) On CA.SkuId = SKU.Id
Inner Join Users U With(NoLock) On CA.CreatedBy = U.Id
Inner Join States S With(NoLock) On U.StateId = S.Id
Inner Join Headquarters HQ With(NoLock) On HQ.Id = U.HeadquartersId
Where ( (Convert(date,CA.CreatedDate) >= Convert(date,@StartDate)) or (Convert(date,CA.CreatedDate) <= Convert(date,@EndDate))) and (sku.DivisionId = @VerticalId or sku.DivisionId > 0) And (sku.SalesOrganizationId=@SalesOrganizationId or sku.SalesOrganizationId > 0) And (sku.DistributionChannelId=@DistributionChannelId or sku.DistributionChannelId > 0)
) as A

Declare @tempColumn Table (ColumnName Nvarchar(Max))

Insert Into @tempColumn Select 'StateId' As ColumnName
Insert Into @tempColumn Select 'ProductId' As ColumnName
Insert Into @tempColumn Select 'State' As ColumnName
Insert Into @tempColumn Select 'Product' As ColumnName
Insert Into @tempColumn Select 'Name Of Market' As ColumnName

Insert Into @tempColumn
Select Distinct Competitor + ' - PTD' As ColumnName From #temp 
Union All
Select Distinct Competitor + ' - MOP' As ColumnName From #temp 
Order By  ColumnName Asc

Select Distinct Competitor From #temp 


Select * From @tempColumn 

Select * From #temp 
Drop Table #temp
End

GO


