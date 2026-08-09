


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[SetDivision](
	@SalesOrganizationCode varchar(100),
	@DistributionChannelCode varchar(100),
	@SalesDocumentType nvarchar(100),
	@SalesOrderDocumentType nvarchar(100),
	@LoginUserId bigint,
	@Name varchar(100),
    @Code varchar(100),
	@IsActive bit,
	@ZPR4 bit
    )
	as
DECLARE @SalesOrganizationId bigint
DECLARE @DistributionChannelId bigint
set @Name = ltrim(rtrim(@Name))
set @Code = ltrim(rtrim(@Code))
set @LoginUserId = ltrim(rtrim(@LoginUserId))
set @IsActive = ltrim(rtrim(@IsActive))
set @SalesOrganizationCode = ltrim(rtrim(@SalesOrganizationCode))
set @DistributionChannelCode = ltrim(rtrim(@DistributionChannelCode))

Set NOCOUNT OFF

BEGIN TRANSACTION


--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where [Code]= @SalesOrganizationCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@Name as Name, @Code as Code,@IsActive as IsActive,@SalesDocumentType as SalesDocumentType,@SalesOrderDocumentType as SalesOrderDocumentType,@ZPR4 as ZPR4, 'Failed, SalesOrganization Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @SalesOrganizationId = ID From SalesOrganizations Where [Code] = @SalesOrganizationCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@Name as Name, @Code as Code,@IsActive as IsActive,@SalesDocumentType as SalesDocumentType,@SalesOrderDocumentType as SalesOrderDocumentType,@ZPR4 as ZPR4, 'Failed In SalesOrganization' as 'Message'      
RETURN 
END 
--SalesOrganization Ends


--DistributionChannel begins

IF NOT EXISTS(Select 1 FROM DistributionChannels Where [Code]= @DistributionChannelCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@Name as Name, @Code as Code,@IsActive as IsActive,@SalesDocumentType as SalesDocumentType,@SalesOrderDocumentType as SalesOrderDocumentType,@ZPR4 as ZPR4, 'Failed, DistributionChannel Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DistributionChannelId = ID From DistributionChannels Where [Code] = @DistributionChannelCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@Name as Name, @Code as Code,@IsActive as IsActive,@SalesDocumentType as SalesDocumentType,@SalesOrderDocumentType as SalesOrderDocumentType,@ZPR4 as ZPR4, 'Failed In DistributionChannel' as 'Message'      
RETURN 
END 
--DistributionChannel Ends



IF EXISTS(Select 1 FROM Divisions Where [Name] = @Name and [SalesOrganizationId]=@SalesOrganizationId and [DistributionChannelId] =@DistributionChannelId)
BEGIN
    SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@Name as Name, @Code as Code,@IsActive as IsActive,@SalesDocumentType as SalesDocumentType,@SalesOrderDocumentType as SalesOrderDocumentType,@ZPR4 as ZPR4, 'Failed, Name Exists' as 'Message'
END
ELSE IF EXISTS(Select 1 FROM Divisions Where [Code] = @Code and [SalesOrganizationId]=@SalesOrganizationId and [DistributionChannelId] =@DistributionChannelId)
BEGIN
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@Name as Name, @Code as Code,@IsActive as IsActive,@SalesDocumentType as SalesDocumentType,@SalesOrderDocumentType as SalesOrderDocumentType,@ZPR4 as ZPR4, 'Failed, Code Exists' as 'Message'
END
ELSE
BEGIN
	INSERT INTO Divisions(Name,Code,CreatedBy,IsActive,CreatedDate,SalesOrganizationId,DistributionChannelId,SalesDocumentType,SalesOrderDocumentType,ZPR4) VALUES (@Name, @Code, @LoginUserId, @IsActive, getdate(),@SalesOrganizationId,@DistributionChannelId,@SalesDocumentType,@SalesOrderDocumentType,@ZPR4);
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@Name as Name, @Code as Code,@IsActive as IsActive,@SalesDocumentType as SalesDocumentType,@SalesOrderDocumentType as SalesOrderDocumentType,@ZPR4 as ZPR4, 'Success' as 'Message' 
END

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@Name as Name, @Code as Code,@IsActive as IsActive,@SalesDocumentType as SalesDocumentType,@SalesOrderDocumentType as SalesOrderDocumentType,@ZPR4 as ZPR4, 'Failed in Divisions Channel' as 'Message'     
RETURN 
END 
COMMIT
