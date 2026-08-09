USE [AdaniDB]
GO

/****** Object:  StoredProcedure [dbo].[SetDistributionChannel]    Script Date: 18-08-2022 12:59:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SetDistributionChannel](
	@SalesOrganizationCode varchar(100),
	@LoginUserId bigint,
	@Name varchar(100),
    @SAPCode varchar(100),
	@IsActive bit
    )
	as
DECLARE @SalesOrganizationId bigint
set @Name = ltrim(rtrim(@Name))
set @SAPCode = ltrim(rtrim(@SAPCode))
set @LoginUserId = ltrim(rtrim(@LoginUserId))
set @IsActive = ltrim(rtrim(@IsActive))

Set NOCOUNT OFF

BEGIN TRANSACTION


--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where [Code]= @SalesOrganizationCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@Name as Name, @SAPCode as SAPCode,@IsActive as IsActive, 'Failed, SalesOrganization Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @SalesOrganizationId = ID From SalesOrganizations Where [Code] = @SalesOrganizationCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@Name as Name, @SAPCode as SAPCode,@IsActive as IsActive, 'Failed In SalesOrganization' as 'Message'      
RETURN 
END 
--SalesOrganization Ends

IF EXISTS(Select 1 FROM DistributionChannels Where [Name] = @Name and [SalesOrganizationId]=@SalesOrganizationId)
BEGIN
    SELECT @SalesOrganizationCode as SalesOrganizationCode,@Name as Name, @SAPCode as SAPCode,@IsActive as IsActive, 'Failed, Name Exists' as 'Message'
END
ELSE IF EXISTS(Select 1 FROM DistributionChannels Where [Code] = @SAPCode and [SalesOrganizationId]=@SalesOrganizationId)
BEGIN
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@Name as Name, @SAPCode as SAPCode,@IsActive as IsActive, 'Failed, Code Exists' as 'Message'
END
ELSE
BEGIN
	INSERT INTO DistributionChannels(Name,[Code],CreatedBy,IsActive,CreatedDate,SalesOrganizationId) VALUES (@Name, @SAPCode, @LoginUserId, @IsActive, getdate(),@SalesOrganizationId);
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@Name as Name, @SAPCode as SAPCode,@IsActive as IsActive, 'Success' as 'Message' 
END

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @Name as Name, @SAPCode as SAPCode,@IsActive as IsActive, 'Failed in Distribution Channel' as 'Message'     
RETURN 
END 
COMMIT

GO


