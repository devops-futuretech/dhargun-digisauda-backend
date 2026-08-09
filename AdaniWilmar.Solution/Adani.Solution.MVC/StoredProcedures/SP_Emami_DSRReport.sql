USE [AdaniDB]
GO

/****** Object:  StoredProcedure [dbo].[SP_Emami_DSRReport]    Script Date: 24-07-2022 21:17:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Emami_DSRReport]
(
	@FromDate Datetime,
	@Todate Datetime,
	@BDOIds Nvarchar(Max),
	@ZHIds Nvarchar(Max),
	@VerticalId bigint,
	@SalesOrganizationId bigint,
	@DistributionChannelId bigint
)
As
Begin
	--Declare 
	--@FromDate Datetime = '2019-11-27',
	--@Todate Datetime = '2019-11-27',
	--@BDOIds Nvarchar(Max) = '28',
	--@ZHIds Nvarchar(Max) = ''


	Declare @BDOList Table(Id BigInt)
	Declare @ZHList Table(Id BigInt)
	Declare @DealerList Table(Id BigInt)

	Insert Into @BDOList
	Select Data From dbo.Split(@BDOIds,',')

	Insert Into @ZHList
	Select Data From dbo.Split(@ZHIds,',')

	If Exists(Select Top 1 1 From @ZHList Where Id != 0)
	Begin
		If Not Exists(Select Top 1 1 From @BDOList Where Id != 0)
		Begin
			Insert Into @BDOList
			Select Id From Users Where ReportingToId In (Select Id From @ZHList)
		End
	End
	Insert Into @DealerList
	Select CustomerId From UserCustomerMappings Where UserId In (Select Id From @BDOList) 

	Select 
		PSR.CreatedDate As [Date],
		U.Name As [DealerName],
		SO.SaudaNumber As [PendingSaudaNO],
		PSR.Remarks As [PendingSaudaNORemarks],
		MS.Title As [MarketScenarioTitle],
		MS.Remarks As [MarketScenarioRemarks],
		BC.Name As [CompetitorName],
		BCS.SkuName As [ProductName],
		BCS.QuanityPerMt As Qty,
		BCS.Price As [Rate]
	From PendingSaudaRemarks PSR
	Inner Join Users U On U.Id = PSR.DealerId
	Inner Join UserDivisionMappings ud On U.Id=ud.UserId
	Inner Join MarketScenarios MS On PSR.DealerId = MS.DealerId
	Inner Join BdoCompetitors BC On PSR.DealerId = BC.DealerId
	Left Join BdoCompetitorSkus BCS On BC.Id = BCS.BdoCompetitorId
	Left Join SaudaOrders SO On PSR.SaudaId = SO.Id
	Where PSR.DealerId In (Select Id From @DealerList)
	And BC.UserType = 1
	And Convert(date,PSR.CreatedDate) >= Convert(date,@FromDate) And Convert(date,PSR.CreatedDate) <= Convert(date,@Todate)
	And Convert(date,MS.CreatedDate) >= Convert(date,@FromDate) And Convert(date,MS.CreatedDate) <= Convert(date,@Todate)
	And Convert(date,BC.CreatedDate) >= Convert(date,@FromDate) And Convert(date,BC.CreatedDate) <= Convert(date,@Todate)
	And (ud.DivisionId = @VerticalId or @VerticalId = 0) And ud.SalesOrganizationId=@SalesOrganizationId And ud.DistributionChannelId=@DistributionChannelId
	--and (u.[DivisionId] = @VerticalId or @VerticalId = 0)
--And PSR.Remarks = 'Need lifting maximum'
End
GO


