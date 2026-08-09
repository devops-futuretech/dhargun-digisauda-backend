CREATE Procedure [dbo].[SetCMSUser](
	@Code nvarchar(100),
	@Name nvarchar(100),
	@MobileNumber nvarchar(100),
	@Email nvarchar(100),
	@ZoneName nvarchar(100),
	@StateName nvarchar(100),
	@TerritoryName nvarchar(100),
	@DistrictName nvarchar(100),
	@CityName nvarchar(100),
	@Pincode nvarchar(100),
	@Address nvarchar(max),
	@Designation nvarchar(100),
	@RoleName nvarchar(100),
	@IsActive bit,
	@CreatedBy bigint,	
	@Headquarters nvarchar(500),
	@Password nvarchar(100),
	@CMSReportingToUserCode nvarchar(100),
	@EncryptedPassword nvarchar(max)
	)
	as
DECLARE @ZoneId bigint, @StateId bigint, @TerritoryId bigint, @DistrictId bigint,@CityId bigint,@RoleId bigint,@UserId bigint,@HeadquartersId bigint,@CMSReportingToId bigint


set @Code = ltrim(rtrim(@Code))
set @Name = ltrim(rtrim(@Name))
set @MobileNumber = ltrim(rtrim(@MobileNumber))
set @Email = ltrim(rtrim(@Email))
set @ZoneName = ltrim(rtrim(@ZoneName))
set @StateName = ltrim(rtrim(@StateName))
set @TerritoryName = ltrim(rtrim(@TerritoryName))
set @DistrictName = ltrim(rtrim(@DistrictName))
set @CityName = ltrim(rtrim(@CityName))
set @Pincode = ltrim(rtrim(@Pincode))
set @Address = ltrim(rtrim(@Address))
set @Designation = ltrim(rtrim(@Designation))
set @RoleName = ltrim(rtrim(@RoleName))
set @Headquarters = ltrim(rtrim(@Headquarters))


Set NOCOUNT OFF

BEGIN TRANSACTION

--Zone begins
IF NOT EXISTS(SELECT 1 FROM [Zones] Where [Name]= @ZoneName)
BEGIN
	ROLLBACK
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed, Zone Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @ZoneId = Id From [Zones] Where [Name] = @ZoneName

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  	 
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed In Zone' as 'Message'    
	RETURN 
END 
--Zone Ends

--State begins
IF NOT EXISTS(Select 1 FROM States Where [StateName]= @StateName)
BEGIN
	ROLLBACK
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive, 'State Not Exist' as 'Message'      
	RETURN
END
ELSE
    Select @StateId = Id From States Where StateName = @StateName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive, 'Failed In State' as 'Message'      
RETURN 
END 


IF NOT EXISTS(SELECT 1 FROM States s,ZoneStateMappings zs,Zones z WHERE z.Id = zs.ZoneId and s.Id = zs.StateId and s.Id = @StateId and z.Id = @ZoneId and  s.StateName= @StateName)
BEGIN
	ROLLBACK
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive, 'State not mapped with Zone' as 'Message'     
	RETURN 
END
ELSE
    SELECT @StateId = s.Id FROM States s,ZoneStateMappings zs,Zones z WHERE z.Id = zs.ZoneId and s.Id = zs.StateId and s.Id = @StateId and z.Id = @ZoneId and  s.StateName= @StateName

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive, 'Failed In State not mapped with Zone' as 'Message'      
	RETURN 
END 
--State Ends

--District Begins
IF NOT EXISTS(SELECT 1 FROM States s, Districts d WHERE d.StateId = s.Id and d.StateId = @StateID and d.DistrictName = @DistrictName)
BEGIN
	ROLLBACK
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed, District Not Exist' as 'Message' 
	RETURN
END
ELSE
	BEGIN
	  SELECT @DistrictID = d.id FROM States s, Districts d WHERE d.StateId = s.Id and d.StateId = @StateID and d.DistrictName = @DistrictName
	END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed In District' as 'Message' 
	RETURN 
END 
--District Ends

--Territory Begins
IF NOT EXISTS(SELECT 1 FROM States s, Territories t WHERE t.StateId = s.Id and t.StateId = @StateID and t.Name = @TerritoryName)
BEGIN
	ROLLBACK
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed, Territory Not Exist' as 'Message' 
	RETURN
END
ELSE
	BEGIN
		SELECT @TerritoryID = d.id FROM States s, Territories d WHERE d.StateId = s.Id and d.StateId = @StateID and d.Name = @TerritoryName
	END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed In Territory' as 'Message' 
	RETURN 
END 
--Territory Ends

--Cities begins
IF NOT EXISTS(SELECT 1 FROM Districts,Territories, Cities, States Where [CityName]= @CityName and Cities.DistrictId = @DistrictID and Cities.TerritoryId = @TerritoryID and  Cities.DistrictId = Districts.Id and Cities.TerritoryId = Territories.Id and Districts.StateId = @StateID and Districts.StateId = States.Id)
BEGIN
	SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive, 'Failed, City Not Exist' as 'Message' 
END
ELSE
	BEGIN
		SELECT @CityId = Id FROM Cities WHERE [CityName] = @CityName
	END
IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive, 'Failed In City' as 'Message'     
	RETURN 
END 
--Cities Ends

--Headquarters begins
IF NOT EXISTS(SELECT 1 FROM [Headquarters] Where [Name]= @Headquarters)
BEGIN
	ROLLBACK
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed, Headquarters Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @HeadquartersId = Id From [Headquarters] Where [Name]= @Headquarters

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed In Headquarters' as 'Message'      
	RETURN 
END 
--Headquarters Ends

--Role starts
	IF NOT EXISTS(SELECT 1 FROM [Roles] Where [Name]= @RoleName)
BEGIN
	ROLLBACK
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed, Role Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @RoleId = Id From [Roles] Where [Name] = @RoleName

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed In Role' as 'Message'      
	RETURN 
END 
--Role Ends

--CMSReportingTo starts
IF NOT EXISTS(SELECT 1 FROM [Users] Where [Code]= @CMSReportingToUserCode)
BEGIN
	ROLLBACK
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed, CMS Reporting to User Not Exist' as 'Message'      
	RETURN
END
ELSE
BEGIN
	SELECT @CMSReportingToId = Id From [Users] Where [Code] = @CMSReportingToUserCode
END
IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive,  'Failed In CMS Reporting to User' as 'Message'      
	RETURN 
END
--CMSReportingTo Ends

IF NOT EXISTS(SELECT 1 FROM [Users] u, [UserRoles] r Where u.Code= @Code and u.MobileNumber=@MobileNumber and r.RoleId = @RoleId and r.UserId = u.Id)
BEGIN
	
		INSERT INTO [Users]([Name],[Code],[Email],[MobileNumber],[Designation],[ZoneId],[StateId],[TerritoryId],[DistrictId],[CityId],[Pincode],[Address],[HeadquartersId]
		,[IsActive],[CreatedBy],[CreatedDate],[Password],[CMSReportingToId],[IsApproved],[IsSAPData],[IsBlacklisted],[SaudaLimit],[IsSelf]
		,[IsBroker],[Loadability],[IsSAPDataSyncOrNot],[SapStatusId],[DepotLoadability])
           VALUES (@Name,@Code,@Email,@MobileNumber,@Designation,@ZoneId,@StateId,@TerritoryId,@DistrictId,@CityId,@Pincode,@Address,@HeadquartersId
		   ,@Isactive,@CreatedBy,getdate(),@EncryptedPassword,@CMSReportingToId,0,0,0,0,0,0,0,0,0,0);

		   Declare @Id BigInt = 0
		SELECT @Id = SCOPE_IDENTITY()	
	
		--UserRoles begins
		IF NOT EXISTS(SELECT 1 FROM UserRoles WHERE UserId = @Id)
		BEGIN
			INSERT INTO [dbo].[UserRoles]([UserId],[RoleId],[IsSAPData],[CreatedBy],[CreatedDate])
				VALUES (@Id,@RoleId,0,@CreatedBy,getdate());
		END
		--UserRoles Ends
END
ELSE
BEGIN  
		SELECT @UserId = u.Id FROM [Users] u JOIN UserRoles r on u.id=r.UserId 
		WHERE [Code]= @Code and [MobileNumber]=@MobileNumber and r.RoleId = @RoleId 
		
		IF (@UserId>0)
		BEGIN
			UPDATE [dbo].[Users]
			 SET [Email] =@Email,[Designation]= @Designation,[IsActive]=@Isactive,[Pincode] =@Pincode,[ZoneId] = @ZoneId
			 ,[DistrictId] = @DistrictId,[CityId]=@CityId,[StateId] =@StateId,[TerritoryId]= @TerritoryId,[Address] =@Address,[HeadquartersId]=@HeadquartersId
			 ,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate(),[Password]=@EncryptedPassword,[CMSReportingToId]=@CMSReportingToId
			Where Id=@UserId
		END
		ELSE
		BEGIN
			SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive, 'Failed in User' as 'Message' FROM [Users] Where [Code]= @Code and [MobileNumber]=@MobileNumber
		END
		SELECT @Code as Code,@Name as 'Name',@MobileNumber as MobileNumber,@Email as Email,@Password as 'Password',@CMSReportingToUserCode as CMSReportingToUserCode,@Pincode as Pincode,@Address as 'Address',@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@Designation as Designation,@RoleName as RoleName,@Headquarters as Headquarters,@IsActive as IsActive, 'Record Updated' as 'Message' FROM [Users] Where [Code]= @Code and [MobileNumber]=@MobileNumber
	
END
COMMIT