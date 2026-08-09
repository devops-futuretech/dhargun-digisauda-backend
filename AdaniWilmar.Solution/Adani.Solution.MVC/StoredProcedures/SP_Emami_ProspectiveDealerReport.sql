/****** Object:  StoredProcedure [dbo].[SP_Emami_ProspectiveDealerReport]    Script Date: 27-11-2019 18:51:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_Emami_ProspectiveDealerReport]
(
	@FromDate Datetime,
	@Todate Datetime,
	@BDOIds Nvarchar(Max),
	@ZHIds Nvarchar(Max)
)
As
Begin
	--Declare 
	--@FromDate Datetime = '2019-11-27',
	--@Todate Datetime = '2019-11-27',
	--@BDOIds Nvarchar(Max) = '',
	--@ZHIds Nvarchar(Max) = '27'


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
			Select Id From Users Where OrganizationReportingToId In (Select Id From @ZHList)
		End
	End
	Insert Into @DealerList
	Select CustomerId From UserCustomerMappings Where UserId In (Select Id From @BDOList) 

	Select 
		PD.CreatedDate As [Date],
		PD.Name As ProspectName,
		PD.MobileNumber,
		PD.Email,
		PD.Address,
		PD.ProspectiveSales,
		PD.ProspectiveInterestLevel,
		PD.BusinessPotentialPeryear
	From ProspectiveDealers PD
	Where PD.DealerId In (Select Id From @DealerList)
	And Convert(date,PD.CreatedDate) >= Convert(date,@FromDate) And Convert(date,PD.CreatedDate) <= Convert(date,@Todate)
End

