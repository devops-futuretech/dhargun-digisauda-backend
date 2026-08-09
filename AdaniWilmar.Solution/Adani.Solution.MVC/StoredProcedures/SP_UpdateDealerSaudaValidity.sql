USE [AdaniDB]
GO

/****** Object:  StoredProcedure [dbo].[SP_UpdateDealerSaudaValidity]    Script Date: 18-08-2022 17:19:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[SP_UpdateDealerSaudaValidity](
	@DealerCode nvarchar(100),
	--@SaudaBookingType nvarchar(100),
	@SaudaValidityPeriod bigint,
	--@SalesOrganizationCode char(100),
	--@DistributionChannelCode char(100),
	--@DivisionCode varchar(100),
	@ModifiedBy bigint
	)
	as
--DECLARE @DivisionId bigint, @SaudaBookingTypeId bigint
--DECLARE @SalesOrganizationId bigint
--DECLARE @DistributionChannelId bigint
set @DealerCode = ltrim(rtrim(@DealerCode))
--set @SaudaBookingType = ltrim(rtrim(@SaudaBookingType))
--set @DivisionCode = ltrim(rtrim(@DivisionCode))
--set @SalesOrganizationCode = ltrim(rtrim(@SalesOrganizationCode))
--set @DistributionChannelCode = ltrim(rtrim(@DistributionChannelCode))

Set NOCOUNT OFF

BEGIN TRANSACTION

----SalesOrganization begins

--IF NOT EXISTS(Select 1 FROM SalesOrganizations Where [Code]= @SalesOrganizationCode)
--BEGIN
--	ROLLBACK
--	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@SaudaValidityPeriod as SaudaValidityPeriod,@DivisionCode as DivisionCode, 'Failed, SalesOrganization Not Exist' as 'Message'      
--	RETURN
--END
--ELSE
--	Select @SalesOrganizationId = ID From SalesOrganizations Where [Code] = @SalesOrganizationCode

--IF @@ERROR <> 0 
--BEGIN     
--ROLLBACK  
--	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@SaudaValidityPeriod as SaudaValidityPeriod,@DivisionCode as DivisionCode, 'Failed In SalesOrganization' as 'Message'      
--RETURN 
--END 
----SalesOrganization Ends

----DistributionChannel begins

--IF NOT EXISTS(Select 1 FROM DistributionChannels Where [Code]= @DistributionChannelCode and SalesOrganizationId = @SalesOrganizationId)
--BEGIN
--	ROLLBACK
--	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@SaudaValidityPeriod as SaudaValidityPeriod,@DivisionCode as DivisionCode, 'Failed, DistributionChannel Not Exist' as 'Message'      
--	RETURN
--END
--ELSE
--	Select @DistributionChannelId = ID From DistributionChannels Where [Code] = @DistributionChannelCode and SalesOrganizationId = @SalesOrganizationId

--IF @@ERROR <> 0 
--BEGIN     
--ROLLBACK  
--	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@SaudaValidityPeriod as SaudaValidityPeriod,@DivisionCode as DivisionCode, 'Failed In DistributionChannel' as 'Message'      
--RETURN 
--END 
----DistributionChannel Ends

----Division begins
--IF NOT EXISTS(SELECT 1 FROM [Divisions] Where [Code]= @DivisionCode and DistributionChannelId = @DistributionChannelId and SalesOrganizationId = @SalesOrganizationId)
--BEGIN
--	ROLLBACK
--		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@SaudaValidityPeriod as SaudaValidityPeriod,@DivisionCode as DivisionCode,  'Failed, Division Not Exist' as 'Message'      
--	RETURN
--END
--ELSE
--	SELECT @DivisionId = Id From [Divisions]  Where [Code]= @DivisionCode  and DistributionChannelId = @DistributionChannelId and SalesOrganizationId = @SalesOrganizationId

--IF @@ERROR <> 0 
--BEGIN     
--	ROLLBACK  
--		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@SaudaValidityPeriod as SaudaValidityPeriod,@DivisionCode as DivisionCode,  'Failed In Division' as 'Message'      
--	RETURN 
--END 
----Division Ends

----SaudaBookingTypes begins
--IF EXISTS(SELECT 1 FROM [SaudaBookingTypes] Where [Name]= @SaudaBookingType)
--BEGIN
--	SELECT @SaudaBookingTypeId = Id From [SaudaBookingTypes] Where [Name]= @SaudaBookingType	
--END
--ELSE
--	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@SaudaValidityPeriod as SaudaValidityPeriod,@DivisionCode as DivisionCode,  'SaudaBookingType not exists' as 'Message'     
	
--IF @@ERROR <> 0 
--BEGIN     
--	ROLLBACK  
--		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@SaudaValidityPeriod as SaudaValidityPeriod,@DivisionCode as DivisionCode,  'Failed In SaudaBookingType' as 'Message'      
--	RETURN 
--END 
----SaudaBookingTypes Ends


IF EXISTS(SELECT 1 FROM [Users] u join [UserRoles] r on u.Id = r.UserId  Where u.Code= @DealerCode )
BEGIN
		
		   UPDATE u
			 SET [SaudaValidityPeriod] =  @SaudaValidityPeriod 
			 ,[ModifiedBy] = @ModifiedBy,[ModifiedDate] = getdate()
			 FROM [Users] u join [UserRoles] r on u.Id = r.UserId  Where u.Code= @DealerCode 

			 SELECT @DealerCode as DealerCode,@SaudaValidityPeriod as SaudaValidityPeriod, 'Record Updated' as 'Message' FROM [Users] u join [UserRoles] r on u.Id = r.UserId  Where u.Code= @DealerCode  
END
ELSE
BEGIN
	     SELECT @DealerCode as DealerCode,@SaudaValidityPeriod as SaudaValidityPeriod, 'Record not exists for particular details' as 'Message' 
END
	
COMMIT

GO


