USE [AdaniDB]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdateDealerSaudaLimit]    Script Date: 19-08-2022 10:53:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[SP_UpdateDealerSaudaLimit](
	@DealerCode nvarchar(100),
	@SalesOrganizationCode char(100),
	@DistributionChannelCode char(100),
	@DivisionCode varchar(100),
	@ModifiedBy bigint,
	@SaudaLimit decimal(18,2)
	)
	as
DECLARE @DivisionId bigint
DECLARE @SalesOrganizationId bigint
DECLARE @DistributionChannelId bigint

set @DealerCode = ltrim(rtrim(@DealerCode))
set @DivisionCode = ltrim(rtrim(@DivisionCode))
set @SalesOrganizationCode = ltrim(rtrim(@SalesOrganizationCode))
set @DistributionChannelCode = ltrim(rtrim(@DistributionChannelCode))


Set NOCOUNT OFF

BEGIN TRANSACTION



--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where [Code]= @SalesOrganizationCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Failed, SalesOrganization Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @SalesOrganizationId = ID From SalesOrganizations Where [Code] = @SalesOrganizationCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Failed In SalesOrganization' as 'Message'      
RETURN 
END 
--SalesOrganization Ends

--DistributionChannel begins

IF NOT EXISTS(Select 1 FROM DistributionChannels Where [Code]= @DistributionChannelCode and SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Failed, DistributionChannel Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DistributionChannelId = ID From DistributionChannels Where [Code] = @DistributionChannelCode and SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Failed In DistributionChannel' as 'Message'      
RETURN 
END 
--DistributionChannel Ends

--Division begins
IF NOT EXISTS(SELECT 1 FROM [Divisions] Where [Code]= @DivisionCode and DistributionChannelId = @DistributionChannelId and SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit,  'Failed, Division Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @DivisionId = Id From [Divisions] Where [Code]= @DivisionCode  and DistributionChannelId = @DistributionChannelId and SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit,  'Failed In Division' as 'Message'      
	RETURN 
END 
--Division Ends

----SaudaBookingTypes begins
--IF EXISTS(SELECT 1 FROM [SaudaBookingTypes] Where [Name]= @SaudaBookingType)
--BEGIN
--	SELECT @SaudaBookingTypeId = Id From [SaudaBookingTypes] Where [Name]= @SaudaBookingType	
--END
--ELSE
--	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit,  'SaudaBookingType not exists' as 'Message'     
	
--IF @@ERROR <> 0 
--BEGIN     
--	ROLLBACK  
--		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit,  'Failed In SaudaBookingType' as 'Message'      
--	RETURN 
--END 
----SaudaBookingTypes Ends


IF EXISTS(SELECT 1 FROM [Users] u join [UserRoles] r on u.Id = r.UserId join UserDivisionMappings udm on udm.UserId = u.Id Where u.Code= @DealerCode and udm.SalesOrganizationId = @SalesOrganizationId and udm.DistributionChannelId = @DistributionChannelId and udm.DivisionId = @DivisionId )
BEGIN
		UPDATE udm
			 SET udm.[SaudaLimit] = @SaudaLimit
			 ,udm.[ModifiedBy] = @ModifiedBy,udm.[ModifiedDate] = getdate()
			 FROM [Users] u join [UserRoles] r on u.Id = r.UserId join UserDivisionMappings udm on udm.UserId = u.Id Where u.Code= @DealerCode and udm.SalesOrganizationId = @SalesOrganizationId and udm.DistributionChannelId = @DistributionChannelId and udm.DivisionId = @DivisionId 

			 SELECT  @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Record Updated' as 'Message' FROM [Users] u join [UserRoles] r on u.Id = r.UserId join UserDivisionMappings udm on udm.UserId = u.Id Where u.Code= @DealerCode and udm.SalesOrganizationId = @SalesOrganizationId and udm.DistributionChannelId = @DistributionChannelId and udm.DivisionId = @DivisionId 
END
ELSE
BEGIN
	     SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Record not exists for particular details' as 'Message' 
END
	
COMMIT

GO


