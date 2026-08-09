/*********************************************************************************************************************************************************************/
Use [AdaniWilmar]
GO
/*********************************************************************************************************************************************************************/
ALTER Procedure [dbo].[SP_UpdateBrokerCallRecordingDetails](
	@BrokerCode nvarchar(100),
	@SaudaBookingType nvarchar(100),
	@SalesOrganizationCode nvarchar(100),
	@DistributionChannelCode nvarchar(100),
	@DivisionCode nvarchar(100),
	@ModifiedBy bigint,
	@AdditionalMobileNumber nvarchar(100),
	@ContactPersonName nvarchar(100),
	@ActiveForCallToCustomers bit
	)
	as
DECLARE @DivisionId bigint, @SalesOrganizationId bigint, @DistributionChannelId bigint, @SaudaBookingTypeId bigint

set @BrokerCode = ltrim(rtrim(@BrokerCode))
set @SaudaBookingType = ltrim(rtrim(@SaudaBookingType))
set @DivisionCode = ltrim(rtrim(@DivisionCode))
set @SalesOrganizationCode = ltrim(rtrim(@SalesOrganizationCode))
set @DistributionChannelCode = ltrim(rtrim(@DistributionChannelCode))


Set NOCOUNT OFF

BEGIN TRANSACTION



--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where SAPCode= @SalesOrganizationCode)
BEGIN
	ROLLBACK
		SELECT @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers,  'Failed, SalesOrganization Not Exist' as 'Message'     
	RETURN
END
ELSE
	Select @SalesOrganizationId = ID From SalesOrganizations Where SAPCode = @SalesOrganizationCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
		SELECT @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers,  'Failed In SalesOrganization' as 'Message'        
RETURN 
END 
--SalesOrganization Ends

--DistributionChannel begins

IF NOT EXISTS(Select 1 FROM DistributionChannels Where SAPCode= @DistributionChannelCode AND SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
		SELECT @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers,  'Failed, DistributionChannel Not Exist' as 'Message'   
	RETURN
END
ELSE
	Select @DistributionChannelId = ID From DistributionChannels Where SAPCode = @DistributionChannelCode AND SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
		SELECT @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers,  'Failed In DistributionChannel' as 'Message'      
RETURN 
END 
--DistributionChannel Ends

--Division begins
IF NOT EXISTS(Select 1 FROM Divisions Where [Code]= @DivisionCode AND DistributionChannelId = @DistributionChannelId)
BEGIN
	ROLLBACK
		SELECT @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers,  'Failed, Division Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DivisionId = ID From Divisions Where [Code] = @DivisionCode AND DistributionChannelId = @DistributionChannelId

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers,  'Failed In Division' as 'Message'      
	RETURN 
END 
--Division Ends

--SaudaBookingTypes begins
IF EXISTS(SELECT 1 FROM [SaudaBookingTypes] Where [Name]= @SaudaBookingType)
BEGIN
	SELECT @SaudaBookingTypeId = Id From [SaudaBookingTypes] Where [Name]= @SaudaBookingType	
END
ELSE
	SELECT @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers,  'SaudaBookingType not exists' as 'Message'     
	
IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers,  'Failed In SaudaBookingType' as 'Message'      
	RETURN 
END 
--SaudaBookingTypes Ends


IF EXISTS(SELECT 1 FROM [Users] u, [UserRoles] r Where u.Code= @BrokerCode and u.DivisionId = @DivisionId and u.SaudaBookingTypeId = @SaudaBookingTypeId)
BEGIN
		UPDATE [dbo].[Users]
			 SET [AdditionalMobileNumber] = @AdditionalMobileNumber,[ContactPersonName] = @ContactPersonName ,[IsActiveForCall] = @ActiveForCallToCustomers
			 ,[ModifiedBy] = @ModifiedBy,[ModifiedDate] = getdate()
			 Where Code= @BrokerCode and DivisionId = @DivisionId and SaudaBookingTypeId = @SaudaBookingTypeId

			 SELECT  @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers, 'Record Updated' as 'Message' FROM [Users] Where Code= @BrokerCode and DivisionId = @DivisionId and SaudaBookingTypeId = @SaudaBookingTypeId
END
ELSE
BEGIN
	     SELECT @BrokerCode as BrokerCode,@SaudaBookingType as SaudaBookingType,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode ,@AdditionalMobileNumber as AdditionalMobileNumber,@ContactPersonName as ContactPersonName,@ActiveForCallToCustomers as ActiveForCallToCustomers, 'Record not exists for particular details' as 'Message' 
END
	
COMMIT
GO

/*********************************************************************************************************************************************************************/

ALTER Procedure [dbo].[SetDepot](
	@Name varchar(100),
	@Code varchar(100),
	@Email varchar(100),
	@Zone varchar(100),
	@CityName varchar(100),
	@DistrictName varchar(100),
	@TerritoryName varchar(100),
    @StateName  varchar(100),
	@Pincode decimal,
	@Address varchar(100),
	@IsActive bit,
	@CreatedBy Bigint,
	@StorageTypeId int
    )
	as
DECLARE @DistrictId int, @StateId int, @TerritoryId int, @CityId int,@ZoneId int
set @DistrictName = ltrim(rtrim(@DistrictName))
set @StateName = ltrim(rtrim(@StateName))
set @CityName = ltrim(rtrim(@CityName))
set @TerritoryName = ltrim(rtrim(@TerritoryName))
Set NOCOUNT OFF

BEGIN TRANSACTION

--Zone begins
IF NOT EXISTS(SELECT 1 FROM Zones WHERE [Name]= @Zone)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Zone Not Exist' as 'Message'     
	RETURN 
END
ELSE
    SELECT @ZoneId = ID FROM Zones WHERE [Name] = @Zone

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In Zone' as 'Message'      
	RETURN 
END 
--Zone Ends

--State begins
IF NOT EXISTS(Select 1 FROM States Where [StateName]= @StateName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'State Not Exist' as 'Message'      
	RETURN
END
ELSE
    Select @StateId = Id From States Where StateName = @StateName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In State' as 'Message'      
RETURN 
END 


IF NOT EXISTS(SELECT 1 FROM States s,ZoneStateMappings zs,Zones z WHERE z.Id = zs.ZoneId and s.Id = zs.StateId and s.Id = @StateId and z.Id = @ZoneId and  s.StateName= @StateName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'State not mapped with Zone' as 'Message'     
	RETURN 
END
ELSE
    SELECT @StateId = s.Id FROM States s,ZoneStateMappings zs,Zones z WHERE z.Id = zs.ZoneId and s.Id = zs.StateId and s.Id = @StateId and z.Id = @ZoneId and  s.StateName= @StateName

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In State' as 'Message'      
	RETURN 
END 
--State Ends

--Territory Begins
IF NOT EXISTS(SELECT 1 FROM States s, Territories t WHERE t.StateId = s.Id and t.StateId = @StateId and t.[Name] = @TerritoryName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed, Territory Not Exist' as 'Message' 
	RETURN
END
ELSE
BEGIN
    SELECT @TerritoryId = t.id FROM States s, Territories t WHERE t.StateId = s.Id and t.StateId = @StateId and t.[Name] = @TerritoryName
END

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In Territory' as 'Message' 
RETURN 
END 
--Territory Ends

--District Begins
IF NOT EXISTS(SELECT 1 FROM Territories t, Districts d  WHERE d.TerritoryId = t.Id and d.TerritoryId = @TerritoryId and d.DistrictName = @DistrictName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed, District Not Exist' as 'Message' 
	RETURN
END
ELSE
BEGIN
    SELECT @DistrictId = d.id FROM Territories s, Districts d WHERE d.TerritoryId = s.Id and d.TerritoryId = @TerritoryId and d.DistrictName = @DistrictName
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In District' as 'Message' 
	RETURN 
END 
--District Ends


--City Begins
IF NOT EXISTS(SELECT 1 FROM Cities t, Districts d WHERE t.DistrictId = d.Id and t.DistrictId = @DistrictId and t.CityName = @CityName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed, City Not Exist' as 'Message' 
	RETURN
END
ELSE
BEGIN
    SELECT @CityId = t.id FROM Cities t, Districts d WHERE t.DistrictId = d.Id and t.DistrictId = @DistrictId and t.CityName = @CityName
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In City' as 'Message' 
	RETURN 
END 
--City Ends

--Plant begins
IF NOT EXISTS(SELECT 1 FROM Depots WHERE IsPlant = 0 and (Code = @Code or [Name] = @Name and Email = @Email))
BEGIN
	INSERT INTO Depots(ZoneId,Code, Name, Email, StateId, TerritoryId, CityId, Pincode, Location, DistrictId,IsPlant, CreatedBy, IsActive, CreatedDate,IsSAPData,IsSAPDataSyncOrNot,StorageTypeId,DepotId) VALUES (@ZoneId,@Code, @Name, @Email, @StateId,@TerritoryId,@CityId,@Pincode,@Address,@DistrictId,0, @CreatedBy, @IsActive, getdate(),0,0,@StorageTypeId,0);
	SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Success' as 'Message' 
END
ELSE
	SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Record Exists' as 'Message' 

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In Plant' as 'Message'     
	RETURN 
END 
--Cities Ends

COMMIT

GO

/*********************************************************************************************************************************************************************/

ALTER Procedure [dbo].[SetMaterialType](
	
	@SalesOrganizationCode nvarchar(100),
	@DistributionChannelCode nvarchar(100),
	@DivisionCode nvarchar(100),
	@MaterialType nvarchar(100),
	@IsActive bit,
	@CreatedBy bigint 
	)

	as
DECLARE @SalesOrganizationId bigint,@DistributionChannelId bigint,@DivisionId bigint

set @SalesOrganizationCode = ltrim(rtrim(@SalesOrganizationCode))
set @DistributionChannelCode = ltrim(rtrim(@DistributionChannelCode))
set @DivisionCode = ltrim(rtrim(@DivisionCode))
set @MaterialType = ltrim(rtrim(@MaterialType))


Set NOCOUNT OFF
BEGIN TRANSACTION


--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where SAPCode= @SalesOrganizationCode)
BEGIN
	ROLLBACK
				SELECT @DivisionCode as DivisionCode,@MaterialType as MaterialType,@IsActive as IsActive, 'Failed, SalesOrganizations Not Exist' as 'Message'        
	RETURN
END
ELSE
			Select @SalesOrganizationId = ID From SalesOrganizations Where SAPCode = @SalesOrganizationCode  

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
				SELECT @DivisionCode as DivisionCode,@MaterialType as MaterialType,@IsActive as IsActive, 'Failed, SalesOrganizations Not Exist' as 'Message'       
RETURN 
END 
--SalesOrganization Ends

--DistributionChannel begins

IF NOT EXISTS(Select 1 FROM DistributionChannels Where SAPCode= @DistributionChannelCode AND SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
				SELECT @DivisionCode as DivisionCode,@MaterialType as MaterialType,@IsActive as IsActive, 'Failed, DistributionChannel Not Exist' as 'Message'       
	RETURN
END
ELSE
	Select @DistributionChannelId = ID From DistributionChannels Where SAPCode = @DistributionChannelCode AND SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	Select @DistributionChannelId = ID From DistributionChannels Where SAPCode = @DistributionChannelCode AND SalesOrganizationId = @SalesOrganizationId    
RETURN 
END 
--DistributionChannel Ends

--Division begins
IF NOT EXISTS(SELECT 1 FROM Divisions WHERE [Code]= @DivisionCode)
BEGIN
	ROLLBACK
		SELECT @DivisionCode as DivisionCode,@MaterialType as MaterialType,@IsActive as IsActive, 'Failed, Division Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @DivisionId = Id From Divisions WHERE [Code] = @DivisionCode AND DistributionChannelId = @DistributionChannelId AND SalesOrganizationId = @SalesOrganizationId  

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @DivisionCode as DivisionCode,@MaterialType as MaterialType,@IsActive as IsActive, 'Failed, Division Not Exist' as 'Message'       
	RETURN 
END 
--Division Ends

--MaterialType begins
IF (@MaterialType is null or @MaterialType = '')

	SELECT @DivisionCode as DivisionCode,@MaterialType as MaterialType,@IsActive as IsActive, 'Failed, Material type is null or empty' as 'Message'  
 
--MaterialType Ends


IF NOT EXISTS(SELECT 1 FROM [MaterialTypes] WHERE [DivisionId]= @DivisionId and [Name]=@MaterialType and [IsActive]=@IsActive)
BEGIN
	INSERT INTO [MaterialTypes]([SalesOrganizationId],[DistributionChannelId],[DivisionId],[Name],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])
           VALUES (@SalesOrganizationId,@DistributionChannelId,@DivisionId,@MaterialType,@IsActive,@CreatedBy,getdate(),0,'0001-01-01');

	SELECT @DivisionCode as DivisionCode,@MaterialType as MaterialType,@IsActive as IsActive, 'Success , MaterialType added' as 'Message'    
END
else
BEGIN
SELECT @DivisionCode as DivisionCode,@MaterialType as MaterialType,@IsActive as IsActive, 'Failed, MaterialType already exists' as 'Message'    
END

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @DivisionCode as DivisionCode,@MaterialType as MaterialType,@IsActive as IsActive, 'Failed in MaterialType' as 'Message'    
RETURN 
END 
COMMIT
GO

/*********************************************************************************************************************************************************************/

ALTER Procedure [dbo].[SetOilType](
    @DivisionCode char(100),
	@SalesOrganizationCode char(100),
	@DistributionChannelCode char(100),
	@Name  varchar(100),
	@LitreConversion decimal(18,2),
	@IsActive bit,
	@IsRasoi bit = 0,
	@CreatedBy bigint
    )
as
DECLARE @DivisionId bigint
DECLARE @SalesOrganizationId bigint
DECLARE @DistributionChannelId bigint
set @DivisionCode = ltrim(rtrim(@DivisionCode))

Set NOCOUNT OFF
BEGIN TRANSACTION

--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where SAPCode= @SalesOrganizationCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Failed, SalesOrganization Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @SalesOrganizationId = ID From SalesOrganizations Where SAPCode = @SalesOrganizationCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Failed In SalesOrganization' as 'Message'      
RETURN 
END 
--SalesOrganization Ends

--DistributionChannel begins

IF NOT EXISTS(Select 1 FROM DistributionChannels Where SAPCode= @DistributionChannelCode AND SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Failed, DistributionChannel Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DistributionChannelId = ID From DistributionChannels Where SAPCode = @DistributionChannelCode AND SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Failed In DistributionChannel' as 'Message'      
RETURN 
END 
--DistributionChannel Ends


--Division begins

IF NOT EXISTS(Select 1 FROM Divisions Where [Code]= @DivisionCode AND DistributionChannelId = @DistributionChannelId AND SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Failed, Division Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DivisionId = ID FROM Divisions Where [Code] = @DivisionCode AND DistributionChannelId = @DistributionChannelId AND SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Failed In Division' as 'Message'      
RETURN 
END 
--Division Ends




--Insert Oiltype
IF @IsRasoi=1
BEGIN
	DECLARE @HBCDivisionId BIGINT
	Select @HBCDivisionId = Id FROM Divisions Where [Name] = 'HBC'
	
	IF @HBCDivisionId!=@DivisionId
	BEGIN
		SET @IsRasoi=0
	END
END

IF NOT EXISTS (SELECT 1 FROM OilTypes s WHERE s.DivisionId = @DivisionId and s.[Name] = @Name and s.SalesOrganizationId=@SalesOrganizationId and s.DistributionChannelId=@DistributionChannelId)
BEGIN
     Insert into OilTypes(DivisionId,[Name],LitreConversion,IsActive,CreatedBy,CreatedDate,[IsRasoi],SalesOrganizationId,DistributionChannelId) values(@DivisionId,@Name,@LitreConversion,@IsActive,@CreatedBy,GETDATE(),@IsRasoi,@SalesOrganizationId,@DistributionChannelId)
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Success' as 'Message' 
	END
ELSE
BEGIN
	UPDATE [dbo].[OilTypes] SET [Name] = @Name,[SalesOrganizationId]=@SalesOrganizationId,[DistributionChannelId]=@DistributionChannelId,[DivisionId] = @DivisionId,[LitreConversion] = @LitreConversion,[IsActive] = @IsActive,[ModifiedBy] = @CreatedBy
	,[ModifiedDate] = GETDATE(),[IsRasoi] = @IsRasoi WHERE DivisionId = @DivisionId and [Name] = @Name

	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Record Updated' as 'Message' 
END
	--SELECT @DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Failed, OilType Exists' as 'Message' 
IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DivisionCode as DivisionCode, @Name as [Name],@LitreConversion as LitreConversion,@IsActive as IsActive, 'Failed in OilType Insert' as 'Message' 
RETURN 
END 

COMMIT

GO

/*********************************************************************************************************************************************************************/

ALTER Procedure [dbo].[SetPlant](
	@Name varchar(100),
	@Code varchar(100),
	@Email varchar(100),
	@Zone varchar(100),
	@CityName varchar(100),
	@DistrictName varchar(100),
	@TerritoryName varchar(100),
    @StateName  varchar(100),
	@Pincode decimal,
	@Address varchar(100),
	@IsActive bit,
	@CreatedBy bigint,
	@StorageTypeId int
    )
	as
DECLARE @DistrictId int, @StateId int, @TerritoryId int, @CityId int,@ZoneId int
set @DistrictName = ltrim(rtrim(@DistrictName))
set @StateName = ltrim(rtrim(@StateName))
set @CityName = ltrim(rtrim(@CityName))
set @TerritoryName = ltrim(rtrim(@TerritoryName))
Set NOCOUNT OFF

BEGIN TRANSACTION

--Zone begins
IF NOT EXISTS(SELECT 1 FROM Zones WHERE [Name]= @Zone)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Zone Not Exist' as 'Message'     
	RETURN 
END
ELSE
    SELECT @ZoneId = ID FROM Zones WHERE [Name] = @Zone

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In Zone' as 'Message'      
	RETURN 
END 
--Zone Ends

--State begins
IF NOT EXISTS(Select 1 FROM States Where [StateName]= @StateName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'State Not Exist' as 'Message'      
	RETURN
END
ELSE
    Select @StateId = Id From States Where StateName = @StateName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In State' as 'Message'      
RETURN 
END 


IF NOT EXISTS(SELECT 1 FROM States s,ZoneStateMappings zs,Zones z WHERE z.Id = zs.ZoneId and s.Id = zs.StateId and s.Id = @StateId and z.Id = @ZoneId and  s.StateName= @StateName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'State not mapped with Zone' as 'Message'     
	RETURN 
END
ELSE
    SELECT @StateId = s.Id FROM States s,ZoneStateMappings zs,Zones z WHERE z.Id = zs.ZoneId and s.Id = zs.StateId and s.Id = @StateId and z.Id = @ZoneId and  s.StateName= @StateName

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In State' as 'Message'      
	RETURN 
END 
--State Ends

--Territory Begins
IF NOT EXISTS(SELECT 1 FROM States s, Territories t WHERE t.StateId = s.Id and t.StateId = @StateId and t.[Name] = @TerritoryName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed, Territory Not Exist' as 'Message' 
	RETURN
END
ELSE
BEGIN
    SELECT @TerritoryId = t.id FROM States s, Territories t WHERE t.StateId = s.Id and t.StateId = @StateId and t.[Name] = @TerritoryName
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In Territory' as 'Message' 
	RETURN 
END 
--Territory Ends


--District Begins
IF NOT EXISTS(SELECT 1 FROM Territories t, Districts d  WHERE d.TerritoryId = t.Id and d.TerritoryId = @TerritoryId and d.DistrictName = @DistrictName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed, District Not Exist' as 'Message' 
	RETURN
END
ELSE
BEGIN
    SELECT @DistrictId = d.id FROM Territories s, Districts d WHERE d.TerritoryId = s.Id and d.TerritoryId = @TerritoryId and d.DistrictName = @DistrictName
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In District' as 'Message' 
	RETURN 
END 
--District Ends


--City Begins
IF NOT EXISTS(SELECT 1 FROM Cities t, Districts d WHERE t.DistrictId = d.Id and t.DistrictId = @DistrictId and t.CityName = @CityName)
BEGIN
	ROLLBACK
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed, City Not Exist' as 'Message' 
	RETURN
END
ELSE
BEGIN
    SELECT @CityId = t.id FROM Cities t, Districts d WHERE t.DistrictId = d.Id and t.DistrictId = @DistrictId and t.CityName = @CityName
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
	SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In City' as 'Message' 
	RETURN 
END 
--City Ends

--Plant begins
IF NOT EXISTS(SELECT 1 FROM Depots d WHERE d.IsPlant = 1 and (d.Code = @Code or d.[Name] = @Name and d.Email = @Email))
BEGIN
	INSERT INTO Depots(ZoneId,Code, Name, Email, StateId, TerritoryId, CityId, Pincode, Location, DistrictId,IsPlant, CreatedBy, IsActive, CreatedDate,IsSAPData,IsSAPDataSyncOrNot,StorageTypeId,DepotId) VALUES (@ZoneId,@Code, @Name, @Email, @StateId,@TerritoryId,@CityId,@Pincode,@Address,@DistrictId,1, @CreatedBy, @IsActive, getdate(),0,0,@StorageTypeId,0);
	SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Success' as 'Message' 
END
ELSE
	SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Record Exists' as 'Message' 

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Zone as 'Zone',@Name as 'Name',@Code as Code,@Email as Email,@Pincode as Pincode,@Address as 'Address',@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive, 'Failed In Plant' as 'Message'     
	RETURN 
END 
--Plant Ends

COMMIT

GO

/*********************************************************************************************************************************************************************/

--Modified on - 07/07/2022
--Modification - SaudaLimit to UserDivisionMapping table

ALTER Procedure [dbo].[SetCustomerMaster](
	@Code nvarchar(100),
	@Name nvarchar(100),
	@MobileNumber nvarchar(100),
	@Email nvarchar(100),
	@SaudaValidityPeriod int,
	@SaudaLimit decimal(18,2),
	@GSTN nvarchar(100),
	@PlantTruckCapacity nvarchar(max),
	@DepotTruckCapacity nvarchar(max),
	@IncoTerms nvarchar(100),
	@TransportMode nvarchar(100),
	@SaudaBookingType nvarchar(100),
	@ZoneName nvarchar(100),
	@StateName nvarchar(100),
	@TerritoryName nvarchar(100),
	@DistrictName nvarchar(100),
	@CityName nvarchar(100),
	@Pincode nvarchar(100),
	@Address nvarchar(100),
	--@IsSelf nvarchar(100),
	--@IsBroker nvarchar(100),
	@BrokerCode nvarchar(100),
	@IsActive bit,
	@CreatedBy bigint,
	@RoleId bigint,
	@SalesOrganizationCode nvarchar(100),
	@DistributionChannelCode nvarchar(100),
	@DivisionCode nvarchar(100),
	@PlantCode varchar(100),
	@DepotCode varchar(100),
	@UserCode varchar(100),
	@Password nvarchar(100),
	@EncryptedPassword nvarchar(max),
	@ShipToPartyCode nvarchar(max),
	@CustomerGroupFiveName nvarchar(max)
    )

	as
DECLARE @TransportModeId bigint, @SaudaBookingTypeId bigint, @ZoneId bigint, @StateId bigint, @TerritoryId bigint, @DistrictId bigint,@CityId int,@BDOId bigint,
@BrokerId bigint,@CustomerId bigint, @ShipToPartyId bigint,@SalesOrganizationId bigint,@DistributionChannelId bigint,@DivisionId bigint,
@DealerRoleId bigint,@ShipToPartyRoleId bigint,@BrokerRoleId bigint,@BDORoleId bigint,
@DepotId bigint, @PlantId bigint , @CustomerGroupFiveId bigint

DECLARE @Item varchar(max)
DECLARE @position INT
DECLARE @Loop BIT
DECLARE @DepotCodeString varchar(100)
DECLARE @PlantCodeString varchar(100)
DECLARE @ShipToPartyCodeString varchar(100)

SET @Code = ltrim(rtrim(@Code))
SET @Name = ltrim(rtrim(@Name))
SET @MobileNumber = ltrim(rtrim(@MobileNumber))
SET @Email = ltrim(rtrim(@Email))
SET @GSTN = ltrim(rtrim(@GSTN))
SET @PlantTruckCapacity = ltrim(rtrim(@PlantTruckCapacity))
SET @IncoTerms = ltrim(rtrim(@IncoTerms))
SET @TransportMode = ltrim(rtrim(@TransportMode))
SET @SaudaBookingType = ltrim(rtrim(@SaudaBookingType))
SET @ZoneName = ltrim(rtrim(@ZoneName))
SET @StateName = ltrim(rtrim(@StateName))
SET @TerritoryName = ltrim(rtrim(@TerritoryName))
SET @DistrictName = ltrim(rtrim(@DistrictName))
SET @CityName = ltrim(rtrim(@CityName))
SET @Pincode = ltrim(rtrim(@Pincode))
SET @Address = ltrim(rtrim(@Address))
SET @BrokerCode = ltrim(rtrim(@BrokerCode))
SET @SalesOrganizationCode = ltrim(rtrim(@SalesOrganizationCode))
SET @DistributionChannelCode = ltrim(rtrim(@DistributionChannelCode))
SET @DivisionCode = ltrim(rtrim(@DivisionCode))
SET @BrokerId=0
SET @UserCode = ltrim(rtrim(@UserCode))
SET @Password = ltrim(rtrim(@Password))
SET @EncryptedPassword = ltrim(rtrim(@EncryptedPassword))
SET @ShipToPartyCodeString= ltrim(rtrim(@ShipToPartyCode))
SET @CustomerGroupFiveName= ltrim(rtrim(@CustomerGroupFiveName))




SELECT @BDORoleId=Id FROM Roles WHERE [Name]='StateTrader'
SELECT @DealerRoleId=Id FROM Roles WHERE [Name]='Dealer'
SELECT @ShipToPartyRoleId=Id FROM Roles WHERE [Name]='ShipToParty'
SELECT @BrokerRoleId=Id FROM Roles WHERE [Name]='Broker'

Set NOCOUNT OFF

BEGIN TRANSACTION

--PlantTruckCapacity begins
IF @PlantTruckCapacity is null 
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, PlantTruckCapacity is invalid' as 'Message'      
	RETURN
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  	 
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In PlantTruckCapacity' as 'Message'    
	RETURN 
END 
--PlantTruckCapacity Ends

--DepotTruckCapacity begins
IF @DepotTruckCapacity is null 
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, DepotTruckCapacity is invalid' as 'Message'      
	RETURN
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  	 
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In DepotTruckCapacity' as 'Message'    
	RETURN 
END 
--DepotTruckCapacity Ends

--TransportMode begins
IF NOT EXISTS(SELECT 1 FROM [TransportModes] WHERE [Name]= @TransportMode)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, TransportMode Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @TransportModeId = Id FROM [TransportModes] WHERE [Name] = @TransportMode

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  	 
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In TransportMode' as 'Message'    
	RETURN 
END 
--TransportMode Ends


--Zone begins
IF NOT EXISTS(SELECT 1 FROM [Zones] WHERE [Name]= @ZoneName)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, Zone Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @ZoneId = Id FROM [Zones] WHERE [Name] = @ZoneName

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  	 
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In Zone' as 'Message'    
	RETURN 
END 
--Zone Ends

--State begins
IF NOT EXISTS(Select 1 FROM States Where [StateName]= @StateName)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'State Not Exist' as 'Message'      
	RETURN
END
ELSE
    Select @StateId = Id From States Where StateName = @StateName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In State' as 'Message'      
RETURN 
END 


IF NOT EXISTS(SELECT 1 FROM States s,ZoneStateMappings zs,Zones z WHERE z.Id = zs.ZoneId and s.Id = zs.StateId and s.Id = @StateId and z.Id = @ZoneId and  s.StateName= @StateName)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'State not mapped with Zone' as 'Message'     
	RETURN 
END
ELSE
    SELECT @StateId = s.Id FROM States s,ZoneStateMappings zs,Zones z WHERE z.Id = zs.ZoneId and s.Id = zs.StateId and s.Id = @StateId and z.Id = @ZoneId and  s.StateName= @StateName

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In State' as 'Message'      
	RETURN 
END 
--State Ends

--Territory Begins
IF NOT EXISTS(SELECT 1 FROM States s, Territories t WHERE t.StateId = s.Id and t.StateId = @StateId and t.[Name] = @TerritoryName)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, Territory Not Exist' as 'Message' 
	RETURN
END
ELSE
	BEGIN
		SELECT @TerritoryID = d.id FROM States s, Territories d WHERE d.StateId = s.Id and d.StateId = @StateId and d.[Name] = @TerritoryName
	END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In Territory' as 'Message' 
	RETURN 
END 
--Territory Ends

--District Begins
IF NOT EXISTS(SELECT 1 FROM States s, Territories t, Districts d WHERE d.StateId = s.Id and d.StateId = @StateId and d.TerritoryId=t.Id and d.TerritoryId=@TerritoryId and d.DistrictName = @DistrictName)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, District Not Exist' as 'Message' 
	RETURN
END
ELSE
	BEGIN
		SELECT @DistrictID = d.id FROM States s, Territories t, Districts d WHERE d.StateId = s.Id and d.StateId = @StateId and d.TerritoryId=t.Id and d.TerritoryId=@TerritoryId and d.DistrictName = @DistrictName
	END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In District' as 'Message' 
	RETURN 
END 
--District Ends


--Cities begins
IF NOT EXISTS(SELECT 1 FROM Districts,Territories, Cities, States WHERE [CityName]= @CityName and Cities.DistrictId = @DistrictID and Cities.TerritoryId = @TerritoryID and  Cities.DistrictId = Districts.Id and Cities.TerritoryId = Territories.Id and Districts.StateId = @StateId and Districts.StateId = States.Id)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, City Not Exist' as 'Message' 
	RETURN
END
ELSE
	BEGIN
		SELECT @CityId = Cities.Id FROM Districts,Territories, Cities, States WHERE [CityName]= @CityName and Cities.DistrictId = @DistrictID and Cities.TerritoryId = @TerritoryID and  Cities.DistrictId = Districts.Id and Cities.TerritoryId = Territories.Id and Districts.StateId = @StateId and Districts.StateId = States.Id
	END
IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In City' as 'Message'     
	RETURN 
END 
--Cities Ends


--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where [SAPCode]= @SalesOrganizationCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, SalesOrganization Not Exist' as 'Message'        
	RETURN
END
ELSE
	Select @SalesOrganizationId = ID From SalesOrganizations Where [SAPCode] = @SalesOrganizationCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In SalesOrganization' as 'Message'      
	RETURN      
RETURN 
END 
--SalesOrganization Ends

--DistributionChannel begins

IF NOT EXISTS(Select 1 FROM DistributionChannels Where [SAPCode]= @DistributionChannelCode and SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, DistributionChannel Not Exist' as 'Message'        
	RETURN
END
ELSE
	Select @DistributionChannelId = ID From DistributionChannels Where [SAPCode] = @DistributionChannelCode and SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In DistributionChannel' as 'Message'      
	RETURN    
RETURN 
END 
--DistributionChannel Ends

--Division begins
IF NOT EXISTS(SELECT 1 FROM [Divisions] WHERE [Code]= @DivisionCode and DistributionChannelId = @DistributionChannelId and SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, Division Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @DivisionId = Id FROM [Divisions] Where [Code]= @DivisionCode and DistributionChannelId = @DistributionChannelId and SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In Division' as 'Message'      
	RETURN 
END 
--Division Ends

--SaudaBookingTypes begins
IF NOT EXISTS(SELECT 1 FROM [SaudaBookingTypes] WHERE [Name]= @SaudaBookingType)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, SaudaBookingType Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @SaudaBookingTypeId = Id FROM [SaudaBookingTypes] WHERE [Name]= @SaudaBookingType

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In SaudaBookingType' as 'Message'      
	RETURN 
END 
--SaudaBookingTypes Ends


--Broker begins
IF EXISTS(SELECT 1 FROM Users, UserRoles r WHERE [Code]= @BrokerCode and  r.RoleId = @BrokerRoleId and [DivisionId] = @DivisionId)
--BEGIN
--	ROLLBACK
--		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', 'Broker Not Exist' as 'Message'     
--	RETURN 
--END
--ELSE
    SELECT @BrokerId = Users.Id FROM Users,UserRoles r WHERE [Code]= @BrokerCode and r.RoleId = @BrokerRoleId and [DivisionId] = @DivisionId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In Broker' as 'Message'      
RETURN 
END 
--Broker Ends

--StateTrader begins
IF EXISTS(SELECT 1 FROM Users, UserRoles r WHERE [Code]= @UserCode and r.RoleId = @BDORoleId and [DivisionId] = @DivisionID)
--BEGIN
--	ROLLBACK
--		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', 'StateTrader User Not Exist' as 'Message'     
--	RETURN 
--END
--ELSE
    SELECT @BDOId = Users.Id FROM Users,UserRoles r WHERE [Code]= @UserCode and r.RoleId = @BDORoleId and [DivisionId] = @DivisionID

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In User' as 'Message'      
	RETURN 
END 
--StateTrader Ends

--CustomerGroupFive Begins

IF NOT EXISTS(SELECT 1 FROM [CustomerGroupFives] WHERE [GroupName]= @CustomerGroupFiveName)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed, CustomerGroupFive Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT  @CustomerGroupFiveId = Id FROM [CustomerGroupFives] WHERE [GroupName] =  @CustomerGroupFiveName

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  	 
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In CustomerGroupFive' as 'Message'    
	RETURN  
END 

 --CustomerGroupFive Ends




IF NOT EXISTS(SELECT 1 FROM [Users] u, [UserRoles] r WHERE (u.Code= @Code and r.RoleId = @RoleId and r.UserId = u.Id) or (u.MobileNumber=@MobileNumber and r.RoleId = @RoleId and r.UserId = u.Id))
BEGIN
	
	INSERT INTO [Users]([Name],[Code],[Email],[MobileNumber],[GSTN],[TransportModeId],[SaudaBookingTypeId]
		,[SaudaValidityPeriod],[SaudaLimit],[IsSelf],[IsBroker],[CustomerGroup],[ZoneId],[StateId],[TerritoryId],[DistrictId],[CityId],[Pincode],[Address1]
		,[IsActive],[IsActiveForCall],[CreatedBy],[CreatedDate],[IsSAPData],[IsSAPDataSyncOrNot],[SapStatusId],[ApprovedBy],[IsBlacklisted]
		,[Password],[PasswordModifiedDate],[CustomerGroupFiveId]
		,[Loadability],[DepotLoadability])
        VALUES (@Name,@Code,@Email,@MobileNumber,@GSTN,@TransportModeId,@SaudaBookingTypeId
		   ,@SaudaValidityPeriod,0,0,0,'01',@ZoneId,@StateId,@TerritoryId,@DistrictId,@CityId,@Pincode,@Address
		   ,@Isactive,0,@CreatedBy,getdate(),0,0,0,0,0,@EncryptedPassword,Getdate(),@CustomerGroupFiveId,0,0);
	
	Declare @Id BigInt = 0
	SELECT @Id = SCOPE_IDENTITY()

	--UserRoles begins
		IF NOT EXISTS(SELECT 1 FROM UserRoles WHERE UserId = @Id)
		BEGIN
			INSERT INTO [dbo].[UserRoles]([UserId],[RoleId],[IsSAPData],[CreatedBy],[CreatedDate])
				VALUES (@Id,@RoleId,0,@CreatedBy,getdate());
		END
	--UserRoles Ends

	--UserDivisionMapping begins
	IF NOT EXISTS(SELECT 1 FROM UserDivisionMappings WHERE UserId = @Id And SalesOrganizationId = @SalesOrganizationId and DistributionChannelId = @DistributionChannelId and DivisionId = @DivisionId)
		BEGIN
			INSERT INTO [dbo].[UserDivisionMappings]([UserId],[SalesOrganizationId],[DistributionChannelId],[DivisionId],SaudaLimit,[CreatedBy],[CreatedDate])
				VALUES (@Id,@SalesOrganizationId,@DistributionChannelId,@DivisionId,@SaudaLimit,@CreatedBy,getdate());
		END
		ELSE
		BEGIN
			UPDATE [dbo].[UserDivisionMappings]
			SET SaudaLimit = @SaudaLimit WHERE UserId = @Id And SalesOrganizationId = @SalesOrganizationId and DistributionChannelId = @DistributionChannelId and DivisionId = @DivisionId
		END
	--UserDivisionMapping Ends


	SELECT @CustomerId = u.Id FROM [Users] u, [UserRoles] r WHERE u.[Code]= @Code and u.[MobileNumber]=@MobileNumber and r.RoleId = @RoleId and r.UserId = u.Id

	if @CustomerId>0
	BEGIN

		--PlantTruckCapacities

	Create Table #TempPlantTruckCapacities(
			TruckCapacity decimal(18,2)
	)
	INSERT INTO  #TempPlantTruckCapacities(TruckCapacity)
	SELECT * FROM STRING_SPLIT(@PlantTruckCapacity,',')


DECLARE @COUNTER BIGINT = 0;
DECLARE @MAX BIGINT = (SELECT COUNT(*) FROM #TempPlantTruckCapacities)
DECLARE @VALUE decimal(18,2);

WHILE @COUNTER < @MAX
BEGIN

SET @VALUE = (SELECT TruckCapacity FROM
      (SELECT (ROW_NUMBER() OVER (ORDER BY (SELECT NULL))) [index] , TruckCapacity from #TempPlantTruckCapacities) R 
       ORDER BY R.[index] OFFSET @COUNTER 
       ROWS FETCH NEXT 1 ROWS ONLY);

       INSERT INTO [dbo].[CustomerTruckCapacityMappings]([UserId],[CreatedBy],[CreatedDate],[StorageTypeId],[TruckCapacity])
		Select @Id,@CreatedBy,getdate(),1,@VALUE


SET @COUNTER = @COUNTER + 1

END

Drop table #TempPlantTruckCapacities

  --DepotTruckCapacities

	Create Table #TempDepotTruckCapacities(
			TruckCapacity decimal(18,2)
	)
	INSERT INTO  #TempDepotTruckCapacities(TruckCapacity)
	SELECT * FROM STRING_SPLIT(@DepotTruckCapacity,',')


DECLARE @Start BIGINT = 0;
DECLARE @End BIGINT = (SELECT COUNT(*) FROM #TempDepotTruckCapacities)
DECLARE @TruckCapacity decimal(18,2);

WHILE @Start < @End
BEGIN

SET @TruckCapacity = (SELECT TruckCapacity FROM
      (SELECT (ROW_NUMBER() OVER (ORDER BY (SELECT NULL))) [index] , TruckCapacity from #TempDepotTruckCapacities) R 
       ORDER BY R.[index] OFFSET @Start 
       ROWS FETCH NEXT 1 ROWS ONLY);

       INSERT INTO [dbo].[CustomerTruckCapacityMappings]([UserId],[CreatedBy],[CreatedDate],[StorageTypeId],[TruckCapacity])
		SELECT @Id,@CreatedBy,getdate(),2,@TruckCapacity


SET @Start = @Start + 1

END

Drop table #TempDepotTruckCapacities

	--Insert UserIncoTerms
	INSERT INTO [dbo].[UserIncoTerms]([UserId],[CreatedBy],[CreatedDate],[IncoTermsId])
		SELECT @CustomerId,@CreatedBy,getdate(),Id FROM IncoTerms 
			JOIN STRING_SPLIT(@IncoTerms, ',') 
			ON value = [Name];  

	--UserCustomerMapping begins
	IF @BrokerId is not null and @BrokerId!=0
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM UserCustomerMappings WHERE UserId = @BrokerId and CustomerId = @CustomerId )
		BEGIN			
			INSERT INTO [dbo].[UserCustomerMappings]([UserId],[CustomerId],[CreatedBy],[CreatedDate]) VALUES (@BrokerId,@CustomerId,@CreatedBy,getdate());
		END
	END
	
	IF @BDOId is not null AND @BDOId!=0
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM UserCustomerMappings WHERE UserId = @BDOId and CustomerId =@CustomerId )
		BEGIN
			INSERT INTO UserCustomerMappings(UserId,CustomerId,CreatedBy,CreatedDate) VALUES (@BDOId,@CustomerId, @CreatedBy, getdate());
		END		
	END	
	--UserCustomerMapping Ends

	--UserDepotMapping begins		
		
		SET @DepotCodeString=@DepotCode
		-- Depot Code - Make sure we enter the loop, even if there's only one item
		IF(right(@DepotCodeString,1) <> ',' and Len(@DepotCodeString)>0)
		BEGIN
			Set @DepotCodeString = @DepotCodeString + ','
		END 		
		SET @Loop = CASE WHEN LEN(@DepotCodeString) > 0 THEN 1 ELSE 0 END
		WHILE (SELECT @Loop) = 1
		BEGIN
			SELECT @Position = CHARINDEX(',', @DepotCodeString, 1)
		
			IF(@Position > 0)
			BEGIN
				SELECT @Item = SUBSTRING(@DepotCodeString, 1, @Position - 1)
				SELECT @DepotCodeString = SUBSTRING(@DepotCodeString, @Position + 1, LEN(@DepotCodeString) - @Position)
				
				SELECT @DepotId = Id FROM Depots WHERE [Code]= @Item and IsPlant=0

				IF(@DepotId <> 0)
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM UserDepotMappings WHERE UserId = @CustomerId and DepotId = @DepotId)
					BEGIN
						INSERT INTO UserDepotMappings(UserId,DepotId,IsSAPData,CreatedBy,CreatedDate) VALUES (@CustomerId,@DepotId,0, @CreatedBy, getdate());
					END
					--ELSE
					--	BEGIN
					--		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password',  'Failed, Customer code already mapped' as 'Message'
					--	END
				END
			END
			ELSE
				BEGIN
					SELECT @Item = @DepotCodeString
					SELECT @Loop = 0
				END
			END	
				
		-- Plant Code - Make sure we enter the loop, even if there's only one item
		SET @PlantCodeString=@PlantCode				
		IF(right(@PlantCodeString,1) <> ',' and Len(@PlantCodeString)>0)
		BEGIN
			Set @PlantCodeString = @PlantCodeString + ','
		END 		
		SET @Loop = CASE WHEN LEN(@PlantCodeString) > 0 THEN 1 ELSE 0 END
		WHILE (SELECT @Loop) = 1
		BEGIN
			SELECT @Position = CHARINDEX(',', @PlantCodeString, 1)
		
			IF(@Position > 0)
			BEGIN
				SELECT @Item = SUBSTRING(@PlantCodeString, 1, @Position - 1)
				SELECT @PlantCodeString = SUBSTRING(@PlantCodeString, @Position + 1, LEN(@PlantCodeString) - @Position)
				
				SELECT @PlantId = Id FROM Depots WHERE [Code]= @Item and IsPlant=1

				IF(@PlantId <> 0)
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM UserDepotMappings WHERE UserId = @CustomerId and DepotId = @PlantId)
					BEGIN
						INSERT INTO UserDepotMappings(UserId,DepotId,IsSAPData,CreatedBy,CreatedDate) VALUES (@CustomerId,@PlantId,0, @CreatedBy, getdate());
					END
				END
			END
			ELSE
				BEGIN
					SELECT @Item = @PlantCodeString
					SELECT @Loop = 0
				END
			END					
		--UserDepotMapping Ends

		--CustomerShipToPartyMapping begins		

			--Make sure we enter the loop, even if there's only one item
			IF(right(@ShipToPartyCode,1) <> ',' and Len(@ShipToPartyCode)>0)
			BEGIN
				Set @ShipToPartyCode = @ShipToPartyCode + ','
			END 
		
			SET @Loop = CASE WHEN LEN(@ShipToPartyCode) > 0 THEN 1 ELSE 0 END
			WHILE (SELECT @Loop) = 1
			BEGIN
				SELECT @Position = CHARINDEX(',', @ShipToPartyCode, 1)
		
				IF(@Position > 0)
				BEGIN
				SELECT @Item = SUBSTRING(@ShipToPartyCode, 1, @Position - 1)
				SELECT @ShipToPartyCode = SUBSTRING(@ShipToPartyCode, @Position + 1, LEN(@ShipToPartyCode) - @Position)
				
				SELECT @ShipToPartyId = Users.Id From Users,UserRoles r Where [Code]= @Item and  (r.RoleId = @ShipToPartyRoleId) and [DivisionId] = @DivisionID and [StateId]=@StateId

				IF(@ShipToPartyId <> 0)
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM CustomerShipToPartyMappings Where CustomerId = @CustomerId and ShipToPartyId=@ShipToPartyId)
					BEGIN
						INSERT INTO CustomerShipToPartyMappings(CustomerId,ShipToPartyId,CreatedBy,CreatedDate) VALUES (@CustomerId,@ShipToPartyId, @CreatedBy, getdate());	
					END
					--ELSE
					--BEGIN
					--	SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@OrgReportingToUserCode as OrgReportingToUserCode,@SalesReportingToUserCode as SalesReportingToUserCode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@CustomerCode as CustomerCode,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@IsActive as IsActive,  'Failed, Customer code already mapped' as 'Message'
					--END
				END
			END
			ELSE
				BEGIN
					SELECT @Item = @ShipToPartyCode
					SELECT @Loop = 0
				END
			END			
			--CustomerShipToPartyMapping Ends

		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Success' as 'Message' FROM [Users] WHERE [Code]= @Code and [MobileNumber]=@MobileNumber and [DivisionId]=@DivisionId
	END
	ELSE
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Failed In Customer' as 'Message'    
END
ELSE
BEGIN
--Update Users
	Declare @OldSaudaBookingTypeId BigInt = 0
	
	SELECT @CustomerId = u.Id,@OldSaudaBookingTypeId = u.SaudaBookingTypeId FROM [Users] u JOIN UserRoles r on u.id=r.UserId 
	WHERE u.[Code]= @Code and r.RoleId = @RoleId 

	IF NOT EXISTS(SELECT 1 FROM [Users] u,UserRoles r WHERE u.[Code]!= @Code and u.Id!=@CustomerId and u.[MobileNumber]=@MobileNumber and r.RoleId = @RoleId and u.id=r.UserId)
	BEGIN	
		IF (@CustomerId>0)
		BEGIN
		
		UPDATE [dbo].[Users]
		  SET [Email] =@Email,[Name]=@Name,[MobileNumber]=@MobileNumber,[Code]= @Code,[IsActive]=@Isactive,[Pincode] =@Pincode,[ZoneId] = @ZoneId
		  ,[DistrictId] = @DistrictId,[CityId]=@CityId,[StateId] =@StateId,[TerritoryId]= @TerritoryId
		  ,[GSTN] =@GSTN,[SaudaValidityPeriod] = @SaudaValidityPeriod,[Address1] =@Address
		  ,[TransportModeId] =@TransportModeId,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate(),[Password]=@EncryptedPassword,[CustomerGroupFiveId] = @CustomerGroupFiveId

		WHERE Id=@CustomerId

	--UserDivisionMapping begins
	IF NOT EXISTS(SELECT 1 FROM UserDivisionMappings WHERE UserId = @CustomerId And SalesOrganizationId = @SalesOrganizationId and DistributionChannelId = @DistributionChannelId and DivisionId = @DivisionId)
		BEGIN
			INSERT INTO [dbo].[UserDivisionMappings]([UserId],[SalesOrganizationId],[DistributionChannelId],[DivisionId],[SaudaLimit],[CreatedBy],[CreatedDate])
				VALUES (@CustomerId,@SalesOrganizationId,@DistributionChannelId,@DivisionId,@SaudaLimit,@CreatedBy,getdate());
		END
		ELSE
		BEGIN
			UPDATE [dbo].[UserDivisionMappings]
			SET SaudaLimit = @SaudaLimit WHERE UserId = @CustomerId And SalesOrganizationId = @SalesOrganizationId and DistributionChannelId = @DistributionChannelId and DivisionId = @DivisionId
		END
	--UserDivisionMapping Ends

		DECLARE @GetSaudaBookingTypeId bigint, @GetDivisionId bigint
		SELECT @GetSaudaBookingTypeId=[SaudaBookingTypeId] FROM [dbo].[Users] WHERE Id=@CustomerId

		 DELETE From CustomerTruckCapacityMappings where UserId = @CustomerId
	--PlantTruckCapacities

	Create Table #TempPlantTruckCapacitiesUpdate(
			TruckCapacity decimal(18,2)
	)
	INSERT INTO  #TempPlantTruckCapacitiesUpdate(TruckCapacity)
	SELECT * FROM STRING_SPLIT(@PlantTruckCapacity,',')


DECLARE @COUNTERUPDATE BIGINT = 0;
DECLARE @MAXUPDATE BIGINT = (SELECT COUNT(*) FROM #TempPlantTruckCapacitiesUpdate)
DECLARE @VALUEUPDATE decimal(18,2);

WHILE @COUNTERUPDATE < @MAXUPDATE
BEGIN

SET @VALUEUPDATE = (SELECT TruckCapacity FROM
      (SELECT (ROW_NUMBER() OVER (ORDER BY (SELECT NULL))) [index] , TruckCapacity from #TempPlantTruckCapacitiesUpdate) R 
       ORDER BY R.[index] OFFSET @COUNTERUPDATE 
       ROWS FETCH NEXT 1 ROWS ONLY);

       INSERT INTO [dbo].[CustomerTruckCapacityMappings]([UserId],[CreatedBy],[CreatedDate],[StorageTypeId],[TruckCapacity])
		SELECT @CustomerId,@CreatedBy,getdate(),1,@VALUEUPDATE


SET @COUNTERUPDATE = @COUNTERUPDATE + 1

END

Drop table #TempPlantTruckCapacitiesUpdate

   
			--DepotTruckCapacities

	Create Table #TempDepotTruckCapacitiesUpdate(
			TruckCapacity decimal(18,2)
	)
	INSERT INTO  #TempDepotTruckCapacitiesUpdate(TruckCapacity)
	SELECT * FROM STRING_SPLIT(@DepotTruckCapacity,',')


DECLARE @StartUpdate BIGINT = 0;
DECLARE @EndUpdate BIGINT = (SELECT COUNT(*) FROM #TempDepotTruckCapacitiesUpdate)
DECLARE @TruckCapacityUpdate decimal(18,2);

WHILE @StartUpdate < @EndUpdate
BEGIN

SET @TruckCapacityUpdate = (SELECT TruckCapacity FROM
      (SELECT (ROW_NUMBER() OVER (ORDER BY (SELECT NULL))) [index] , TruckCapacity from #TempDepotTruckCapacitiesUpdate) R 
       ORDER BY R.[index] OFFSET @StartUpdate 
       ROWS FETCH NEXT 1 ROWS ONLY);

       INSERT INTO [dbo].[CustomerTruckCapacityMappings]([UserId],[CreatedBy],[CreatedDate],[StorageTypeId],[TruckCapacity])
		SELECT @CustomerId,@CreatedBy,getdate(),2,@TruckCapacityUpdate


SET @StartUpdate = @StartUpdate + 1

END

Drop table #TempDepotTruckCapacitiesUpdate

		IF @GetSaudaBookingTypeId is null or @GetSaudaBookingTypeId=0
		BEGIN
			UPDATE [dbo].[Users] SET [SaudaBookingTypeId] = @SaudaBookingTypeId WHERE Id=@CustomerId
		END

		If @OldSaudaBookingTypeId <> @SaudaBookingTypeId
		Begin
			Delete From UserCustomerMappings Where CustomerId = @CustomerId
			UPDATE [dbo].[Users] SET [SaudaBookingTypeId] = @SaudaBookingTypeId WHERE Id=@CustomerId
		End

		--IncoTerms begins
		IF  EXISTS(SELECT * FROM IncoTerms JOIN STRING_SPLIT(@IncoTerms, ',') ON value = [Name] )
		BEGIN			
			DELETE FROM UserIncoTerms WHERE UserId = @CustomerId 
		
			INSERT INTO [dbo].[UserIncoTerms]([UserId],[CreatedBy],[CreatedDate],[IncoTermsId])
				SELECT @CustomerId,@CreatedBy,getdate(),Id FROM IncoTerms 
					JOIN STRING_SPLIT(@IncoTerms, ',') 
					ON value = [Name];  
		END
		--IncoTerms ends

		--UserDepotMapping begins		

		DELETE FROM UserDepotMappings WHERE UserId = @CustomerId

		SET @DepotCodeString=@DepotCode
		-- Depot Code - Make sure we enter the loop, even if there's only one item
		IF(right(@DepotCodeString,1) <> ',' and Len(@DepotCodeString)>0)
		BEGIN
			Set @DepotCodeString = @DepotCodeString + ','
		END 	
		SET @Loop = CASE WHEN LEN(@DepotCodeString) > 0 THEN 1 ELSE 0 END
		WHILE (SELECT @Loop) = 1
		BEGIN
			SELECT @Position = CHARINDEX(',', @DepotCodeString, 1)
	
			IF(@Position > 0)
			BEGIN
				SELECT @Item = SUBSTRING(@DepotCodeString, 1, @Position - 1)
				SELECT @DepotCodeString = SUBSTRING(@DepotCodeString, @Position + 1, LEN(@DepotCodeString) - @Position)
			
				SELECT @DepotId = Id FROM Depots WHERE [Code]= @Item and IsPlant=0

				IF(@DepotId <> 0)
				BEGIN				
					IF NOT EXISTS(SELECT 1 FROM UserDepotMappings WHERE UserId = @CustomerId and DepotId = @DepotId)
					BEGIN
						INSERT INTO UserDepotMappings(UserId,DepotId,IsSAPData,CreatedBy,CreatedDate) VALUES (@CustomerId,@DepotId,0, @CreatedBy, getdate());
					END
				END
			END
			ELSE
				BEGIN
					SELECT @Item = @DepotCodeString
					SELECT @Loop = 0
				END
		END	
				
		-- Plant Code - Make sure we enter the loop, even if there's only one item
		SET @PlantCodeString=@PlantCode				
		IF(right(@PlantCodeString,1) <> ',' and Len(@PlantCodeString)>0)
			BEGIN
				Set @PlantCodeString = @PlantCodeString + ','
			END 		
		SET @Loop = CASE WHEN LEN(@PlantCodeString) > 0 THEN 1 ELSE 0 END
		WHILE (SELECT @Loop) = 1
			BEGIN
				SELECT @Position = CHARINDEX(',', @PlantCodeString, 1)
		
				IF(@Position > 0)
				BEGIN
					SELECT @Item = SUBSTRING(@PlantCodeString, 1, @Position - 1)
					SELECT @PlantCodeString = SUBSTRING(@PlantCodeString, @Position + 1, LEN(@PlantCodeString) - @Position)
				
					SELECT @PlantId = Id FROM Depots WHERE [Code]= @Item and IsPlant=1

					IF(@PlantId <> 0)
					BEGIN
						IF NOT EXISTS(SELECT 1 FROM UserDepotMappings WHERE UserId = @CustomerId and DepotId = @PlantId)
						BEGIN
							INSERT INTO UserDepotMappings(UserId,DepotId,IsSAPData,CreatedBy,CreatedDate) VALUES (@CustomerId,@PlantId,0, @CreatedBy, getdate());
						END
						--ELSE
						--	SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password',  'Failed, Depot code already mapped' as 'Message'
					END
				END
				ELSE
					BEGIN
						SELECT @Item = @PlantCodeString
						SELECT @Loop = 0
					END
				END		
						
		--UserDepotMapping Ends

		--UserCustomerMapping begins
		IF @BrokerId is not null and @BrokerId!=0
		BEGIN
			Delete ucm from UserCustomerMappings ucm inner join UserRoles ur on ucm.UserId = ur.UserId where ucm.CustomerId = @CustomerId and ur.RoleId=6
			IF NOT EXISTS(SELECT 1 FROM UserCustomerMappings WHERE UserId = @BrokerId and CustomerId = @CustomerId)
			BEGIN			
				INSERT INTO [dbo].[UserCustomerMappings]([UserId],[CustomerId],[CreatedBy],[CreatedDate]) VALUES (@BrokerId,@CustomerId,@CreatedBy,getdate());
			END
		END
	
		IF @BDOId is not null AND @BDOId!=0
		BEGIN
			Delete ucm from UserCustomerMappings ucm inner join UserRoles ur on ucm.UserId = ur.UserId where ucm.CustomerId = @CustomerId and ur.RoleId=7
			IF NOT EXISTS(SELECT 1 FROM UserCustomerMappings WHERE UserId = @BDOId and CustomerId =@CustomerId )
			BEGIN
				INSERT INTO UserCustomerMappings(UserId,CustomerId,CreatedBy,CreatedDate) VALUES (@BDOId,@CustomerId, @CreatedBy, getdate());
			END		
		END	
		--UserCustomerMapping Ends

		--CustomerShipToPartyMapping begins		

			--Removing exsisting ship to parties

			DELETE FROM CustomerShipToPartyMappings WHERE CustomerId = @CustomerId;

			--Adding modified ship to parties
				--Make sure we enter the loop, even if there's only one item
				IF(right(@ShipToPartyCode,1) <> ',' and Len(@ShipToPartyCode)>0)
				BEGIN
					Set @ShipToPartyCode = @ShipToPartyCode + ','
				END 
		
				SET @Loop = CASE WHEN LEN(@ShipToPartyCode) > 0 THEN 1 ELSE 0 END
				WHILE (SELECT @Loop) = 1
				BEGIN
					SELECT @Position = CHARINDEX(',', @ShipToPartyCode, 1)
		
					IF(@Position > 0)
					BEGIN
					SELECT @Item = SUBSTRING(@ShipToPartyCode, 1, @Position - 1)
					SELECT @ShipToPartyCode = SUBSTRING(@ShipToPartyCode, @Position + 1, LEN(@ShipToPartyCode) - @Position)
				
					SELECT @ShipToPartyId = Users.Id From Users,UserRoles r Where [Code]= @Item and  (r.RoleId = @ShipToPartyRoleId) and [StateId]=@StateId

					IF(@ShipToPartyId <> 0)
					BEGIN
						IF NOT EXISTS(SELECT 1 FROM CustomerShipToPartyMappings Where CustomerId = @CustomerId and ShipToPartyId=@ShipToPartyId)
						BEGIN
							INSERT INTO CustomerShipToPartyMappings(CustomerId,ShipToPartyId,CreatedBy,CreatedDate) VALUES (@CustomerId,@ShipToPartyId, @CreatedBy, getdate());	
						END
						--ELSE
						--BEGIN
						--	SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@OrgReportingToUserCode as OrgReportingToUserCode,@SalesReportingToUserCode as SalesReportingToUserCode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@CustomerCode as CustomerCode,@SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@IsActive as IsActive,  'Failed, Customer code already mapped' as 'Message'
						--END
					END
				END
				ELSE
					BEGIN
						SELECT @Item = @ShipToPartyCode
						SELECT @Loop = 0
					END
				END			
				--CustomerShipToPartyMapping Ends

		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Record Updated' as 'Message'
	END
		ELSE
		BEGIN
			SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'User not found' as 'Message'
		END
	END
	ELSE
		SELECT @SalesOrganizationCode as SalesOrganizationCode, @DistributionChannelCode as DistributionChannelCode, @DivisionCode as DivisionCode,@TransportMode as TransportMode,@DepotCode as DepotCode,@PlantCode as PlantCode,@UserCode as UserCode,@BrokerCode as BrokerCode,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@PlantTruckCapacity as PlantTruckCapacity,@DepotTruckCapacity as DepotTruckCapacity,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as 'Address', @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName, @IsActive as IsActive,@ShipToPartyCodeString as ShipToPartyCode,@Password as 'Password', @CustomerGroupFiveName as CustomerGroupFiveName, 'Mobile number already exists' as 'Message'    
	
END
COMMIT
GO

/*********************************************************************************************************************************************************************/

--Modified on - 07/07/2022
--Modification - SaudaLimit to UserDivisionMapping table

ALTER Procedure [dbo].[SP_UpdateDealerSaudaLimit](
	@DealerCode nvarchar(100),
	@SaudaBookingType nvarchar(100),
	@SalesOrganizationCode char(100),
	@DistributionChannelCode char(100),
	@DivisionCode varchar(100),
	@ModifiedBy bigint,
	@SaudaLimit decimal(18,2)
	)
	as
DECLARE @DivisionId bigint, @SaudaBookingTypeId bigint
DECLARE @SalesOrganizationId bigint
DECLARE @DistributionChannelId bigint

set @DealerCode = ltrim(rtrim(@DealerCode))
set @SaudaBookingType = ltrim(rtrim(@SaudaBookingType))
set @DivisionCode = ltrim(rtrim(@DivisionCode))
set @SalesOrganizationCode = ltrim(rtrim(@SalesOrganizationCode))
set @DistributionChannelCode = ltrim(rtrim(@DistributionChannelCode))


Set NOCOUNT OFF

BEGIN TRANSACTION



--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where [SAPCode]= @SalesOrganizationCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Failed, SalesOrganization Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @SalesOrganizationId = ID From SalesOrganizations Where [SAPCode] = @SalesOrganizationCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Failed In SalesOrganization' as 'Message'      
RETURN 
END 
--SalesOrganization Ends

--DistributionChannel begins

IF NOT EXISTS(Select 1 FROM DistributionChannels Where [SAPCode]= @DistributionChannelCode and SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Failed, DistributionChannel Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DistributionChannelId = ID From DistributionChannels Where [SAPCode] = @DistributionChannelCode and SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Failed In DistributionChannel' as 'Message'      
RETURN 
END 
--DistributionChannel Ends

--Division begins
IF NOT EXISTS(SELECT 1 FROM [Divisions] Where [Code]= @DivisionCode and DistributionChannelId = @DistributionChannelId and SalesOrganizationId = @SalesOrganizationId)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit,  'Failed, Division Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @DivisionId = Id From [Divisions] Where [Code]= @DivisionCode  and DistributionChannelId = @DistributionChannelId and SalesOrganizationId = @SalesOrganizationId

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit,  'Failed In Division' as 'Message'      
	RETURN 
END 
--Division Ends

--SaudaBookingTypes begins
IF EXISTS(SELECT 1 FROM [SaudaBookingTypes] Where [Name]= @SaudaBookingType)
BEGIN
	SELECT @SaudaBookingTypeId = Id From [SaudaBookingTypes] Where [Name]= @SaudaBookingType	
END
ELSE
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit,  'SaudaBookingType not exists' as 'Message'     
	
IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit,  'Failed In SaudaBookingType' as 'Message'      
	RETURN 
END 
--SaudaBookingTypes Ends


IF EXISTS(SELECT 1 FROM [Users] u join [UserRoles] r on u.Id = r.UserId join UserDivisionMappings udm on udm.UserId = u.Id Where u.Code= @DealerCode and udm.SalesOrganizationId = @SalesOrganizationId and udm.DistributionChannelId = @DistributionChannelId and udm.DivisionId = @DivisionId and u.SaudaBookingTypeId = @SaudaBookingTypeId)
BEGIN
		UPDATE udm
			 SET udm.[SaudaLimit] = @SaudaLimit
			 ,udm.[ModifiedBy] = @ModifiedBy,udm.[ModifiedDate] = getdate()
			 FROM [Users] u join [UserRoles] r on u.Id = r.UserId join UserDivisionMappings udm on udm.UserId = u.Id Where u.Code= @DealerCode and udm.SalesOrganizationId = @SalesOrganizationId and udm.DistributionChannelId = @DistributionChannelId and udm.DivisionId = @DivisionId and u.SaudaBookingTypeId = @SaudaBookingTypeId

			 SELECT  @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Record Updated' as 'Message' FROM [Users] u join [UserRoles] r on u.Id = r.UserId join UserDivisionMappings udm on udm.UserId = u.Id Where u.Code= @DealerCode and udm.SalesOrganizationId = @SalesOrganizationId and udm.DistributionChannelId = @DistributionChannelId and udm.DivisionId = @DivisionId and u.SaudaBookingTypeId = @SaudaBookingTypeId
END
ELSE
BEGIN
	     SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@DealerCode as DealerCode,@SaudaBookingType as SaudaBookingType,@DivisionCode as DivisionCode,@SaudaLimit as SaudaLimit, 'Record not exists for particular details' as 'Message' 
END
	
COMMIT
GO

/*********************************************************************************************************************************************************************/
