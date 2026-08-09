USE [AdaniDB]
GO

/****** Object:  StoredProcedure [dbo].[Sp_Report_SaudaExecutionAudit]    Script Date: 24-07-2022 16:12:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sp_Report_SaudaExecutionAudit]
(
	@StartDate DateTime,
	@EndDate DateTime,
	@VerticalId BigInt,
	@PlantId BigInt,
	@SalesOrganizationId bigint,
	@DistributionChannelId bigint
)
As
Begin
	Select 
	SO.SaudaId As AppBookingId,
	Sku.SkuCode,
	D.Name As Plant,
	div.Name As Division,
	SO.SaudaNumber As SaudaNumber,
	U.Name As SaudaBookedBy,
	SO.CreatedDate As SaudaBookingDate,
	convert(char(10), SO.CreatedDate, 108) As SaudaBookingTime,
	TT.CreatedDate As TradeTicketDate,
	convert(char(10), TT.CreatedDate, 108) As TradeTicketTime,
	SO.SaudaTTAttachedDate,
	convert(char(10), SO.SaudaTTAttachedDate, 108) As SaudaTTAttachedTime,
	SO.CreatedDate As SaudaCreationDate,
	convert(char(10), SO.CreatedDate, 108) As SaudaCreationTime,
	SO.SaudaReleaseDate,
	convert(char(10), SO.SaudaReleaseDate, 108) As SaudaReleaseTime,
	--Case 
	--	When DATEDIFF(minute,SO.CreatedDate,SO.SaudaReleaseDate) > 0 Then dbo.MinutesToDuration(DATEDIFF(minute,SO.CreatedDate,SO.SaudaReleaseDate))
	--	Else '' 
	--End As TimeGapSaudabookingandrelease
	Case 
		When DATEDIFF(minute,SO.CreatedDate,SO.SaudaReleaseDate) > 0 Then Convert(nvarchar(100),DATEDIFF(minute,SO.CreatedDate,SO.SaudaReleaseDate)) 
		Else '' 
	End As TimeGapSaudabookingandrelease
	From SaudaOrders SO With(NoLock)
	Inner Join Skus SKU With(NoLock) On SO.SkuId = SKU.Id
	Left Join TradeTickets TT With(NoLock) On TT.TradeTicketNumber = SO.TradeTicketNumber  
	Left Join Depots D With(NoLock) On SO.PlantId = D.Id And D.IsPlant = 1 And D.StorageTypeId = 1
	Inner Join Users U With(NoLock) on SO.CreatedBy = U.Id
	Inner Join Divisions div With(NoLock) On SKU.DivisionId = div.Id
	Where  (Convert(date,SO.CreatedDate) Between Convert(date,@StartDate) And Convert(date,@EndDate))
	And ((@VerticalId = 0) Or (@VerticalId > 0 And SKU.DivisionId = @VerticalId And SKU.SalesOrganizationId=@SalesOrganizationId And SKU.DistributionChannelId=@DistributionChannelId))
	And ((@PlantId = 0) Or (@PlantId > 0 And SO.PlantId = @PlantId))
End

GO


