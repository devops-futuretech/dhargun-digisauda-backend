USE [AdaniTryed]
GO

/****** Object:  StoredProcedure [dbo].[SetUserDepotMapping]    Script Date: 10-06-2022 12:01:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SetUserDepotMapping](
	@UserCode char(100),
	@DepotCode varchar(100),
	@IsDealer int,
	@DivisionCode nvarchar(100),
	@SalesOrganizationCode char(100),
	@DistributionChannelCode char(100)
    )as
DECLARE @UserID int, @DepotID int,@DivisionId bigint
DECLARE @SalesOrganizationId bigint
DECLARE @DistributionChannelId bigint
set @UserCode = ltrim(rtrim(@UserCode))
set @DepotCode = ltrim(rtrim(@DepotCode))
set @DivisionCode = ltrim(rtrim(@DivisionCode))
set @SalesOrganizationCode = ltrim(rtrim(@SalesOrganizationCode))
set @DistributionChannelCode = ltrim(rtrim(@DistributionChannelCode))

Set NOCOUNT OFF

BEGIN TRANSACTION

--Depot begins
IF NOT EXISTS(Select 1 FROM Depots Where [Code]= @DepotCode)
BEGIN
		ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Depot Not Exist' as 'Message'     
		RETURN 
END
ELSE
    Select @DepotID = ID From Depots Where Code= @DepotCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Failed In Depot' as 'Message'      
RETURN 
END 
--Depot Ends

--Vertical begins
IF NOT EXISTS(Select 1 FROM [Divisions] Where [Code]= @DivisionCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer,  'Failed, Vertical Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DivisionId = Id From [Divisions] Where [Code]= @DivisionCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer,  'Failed In Vertical' as 'Message'      
RETURN 
END 
--Vertical Ends

--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where [SAPCode]= @SalesOrganizationCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Failed, SalesOrganization Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @SalesOrganizationId = ID From SalesOrganizations Where [SAPCode] = @SalesOrganizationCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Failed In SalesOrganization' as 'Message'      
RETURN 
END 
--SalesOrganization Ends

--DistributionChannel begins

IF NOT EXISTS(Select 1 FROM DistributionChannels Where [SAPCode]= @DistributionChannelCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Failed, DistributionChannel Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DistributionChannelId = ID From DistributionChannels Where [SAPCode] = @DistributionChannelCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Failed In DistributionChannel' as 'Message'      
RETURN 
END 
--DistributionChannel Ends


--User begins
IF NOT EXISTS(Select 1 FROM Users u, UserRoles r,UserDivisionMappings ud Where [Code]= @UserCode and ud.DivisionId = @DivisionId and ud.UserId=u.Id and  ((@IsDealer = 1 and r.RoleId = 5) or (@IsDealer = 0 and r.RoleId = 6)) )
BEGIN
		ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'User Not Exist' as 'Message'     
		RETURN 
END
ELSE
    Select @UserID = u.Id From Users u, UserRoles r,UserDivisionMappings ud Where [Code]= @UserCode and ud.DivisionId = @DivisionId and ud.UserId=u.Id and  ((@IsDealer = 1 and r.RoleId = 5) or (@IsDealer = 0 and r.RoleId = 6))

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Failed In Depot' as 'Message'      
RETURN 
END 
--User Ends


--UserDepotMapping begins
IF NOT EXISTS(Select 1 FROM UserDepotMappings d Where d.UserId = @UserID and d.DepotId = @DepotID)
BEGIN
		INSERT INTO UserDepotMappings(UserId,DepotId,IsSAPData,CreatedBy,CreatedDate) VALUES (@UserID,@DepotID,0, 1, getdate());
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Success' as 'Message' 
END
ELSE
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Record Exists' as 'Message' 

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode,@UserCode as UserCode,@DepotCode as DepotCode,@IsDealer as IsDealer, 'Failed In UserDepotMappings' as 'Message'     
RETURN 
END 
--UserDepotMapping Ends

COMMIT

GO

