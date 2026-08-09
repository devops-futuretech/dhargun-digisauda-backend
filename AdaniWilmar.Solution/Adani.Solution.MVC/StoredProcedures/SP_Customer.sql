/****** Object:  StoredProcedure [dbo].[SetCustomer]    Script Date: 01-11-2018 14:46:43 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SetCustomer')
    BEGIN
        DROP  Procedure SetCustomer
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SetCustomer](
	@Code nvarchar(100),
	@Name nvarchar(100),
	@MobileNumber nvarchar(100),
	@Email nvarchar(100),
	@SaudaValidityPeriod int,
	@SaudaLimit decimal,
	@GSTN nvarchar(100),
	@Loadability nvarchar(100),
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
	@PlantCode nvarchar(100),
	@DepotCode nvarchar(100),
	@FreightZoneName nvarchar(100),
	@FreightRouteName nvarchar(100),
	@IsSelf nvarchar(100),
	@IsBroker nvarchar(100),
	@BrokerCode nvarchar(100),
	@IsActive bit,
	@CreatedBy bigint,
	@RoleId bigint,
	@VerticalCode nvarchar(100)
    )

	as
DECLARE @TransportModeId bigint, @SaudaBookingTypeId bigint, @ZoneId bigint, @StateId bigint, @TerritoryId bigint, @DistrictId bigint,@CityId bigint,
 @PlantId bigint,@DepotId bigint,@FreightZoneId bigint,@FreightRouteId bigint,@BrokerId bigint,@CustomerId bigint,@VerticalId bigint

set @Code = ltrim(rtrim(@Code))
set @Name = ltrim(rtrim(@Name))
set @MobileNumber = ltrim(rtrim(@MobileNumber))
set @Email = ltrim(rtrim(@Email))
set @GSTN = ltrim(rtrim(@GSTN))
set @Loadability = ltrim(rtrim(@Loadability))
set @IncoTerms = ltrim(rtrim(@IncoTerms))
set @TransportMode = ltrim(rtrim(@TransportMode))
set @SaudaBookingType = ltrim(rtrim(@SaudaBookingType))
set @ZoneName = ltrim(rtrim(@ZoneName))
set @StateName = ltrim(rtrim(@StateName))
set @TerritoryName = ltrim(rtrim(@TerritoryName))
set @DistrictName = ltrim(rtrim(@DistrictName))
set @CityName = ltrim(rtrim(@CityName))
set @Pincode = ltrim(rtrim(@Pincode))
set @Address = ltrim(rtrim(@Address))
set @PlantCode = ltrim(rtrim(@PlantCode))
set @DepotCode = ltrim(rtrim(@DepotCode))
set @FreightZoneName = ltrim(rtrim(@FreightZoneName))
set @FreightRouteName = ltrim(rtrim(@FreightRouteName))
set @BrokerCode = ltrim(rtrim(@BrokerCode))
set @VerticalCode = ltrim(rtrim(@VerticalCode))

Set NOCOUNT OFF

BEGIN TRANSACTION

--Zone begins
IF NOT EXISTS(Select 1 FROM [Zones] Where [Name]= @ZoneName)
BEGIN
	ROLLBACK
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod, @Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode, @ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed, Zone Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @ZoneId = Id From [Zones] Where [Name] = @ZoneName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  	 
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed In State' as 'Message'    
RETURN 
END 
--Zone Ends
--State begins
IF NOT EXISTS(Select 1 FROM States Where [StateName]= @StateName)
BEGIN
		ROLLBACK
		SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed, State Not Exist' as 'Message'      
		RETURN
END
ELSE
		Select @StateID = ID From States Where StateName = @StateName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed In State' as 'Message'      
RETURN 
END 
--State Ends

--District Begins
IF NOT EXISTS(SELECT 1 FROM States s, Districts d WHERE d.StateId = s.Id and d.StateId = @StateID and d.DistrictName = @DistrictName)
BEGIN
	ROLLBACK
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed, District Not Exist' as 'Message' 
	RETURN
END
ELSE
	BEGIN
    Select @DistrictID = d.id FROM States s, Districts d WHERE d.StateId = s.Id and d.StateId = @StateID and d.DistrictName = @DistrictName
	END

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed In District' as 'Message' 
RETURN 
END 


--District Ends

--Territory Begins
IF NOT EXISTS(SELECT 1 FROM States s, Territories t WHERE t.StateId = s.Id and t.StateId = @StateID and t.Name = @TerritoryName)
BEGIN
	ROLLBACK
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed, Territory Not Exist' as 'Message' 
	RETURN
END
ELSE
	BEGIN
    Select @TerritoryID = d.id FROM States s, Territories d WHERE d.StateId = s.Id and d.StateId = @StateID and d.Name = @TerritoryName
	END

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed In Territory' as 'Message' 
RETURN 
END 


--Territory Ends

--Cities begins
IF NOT EXISTS(Select 1 FROM Districts,Territories, Cities, States Where [CityName]= @CityName and Cities.DistrictId = @DistrictID and Cities.TerritoryId = @TerritoryID and  Cities.DistrictId = Districts.Id and Cities.TerritoryId = Territories.Id and Districts.StateId = @StateID and Districts.StateId = States.Id)
BEGIN
		INSERT INTO Cities([CityName], TerritoryId, DistrictId, CreatedBy, IsActive, CreatedDate) VALUES (@CityName, @TerritoryID,@DistrictID, 1, @IsActive, getdate());
		SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive, 'Success' as 'Message' 
END
ELSE
		SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive, 'Record Exists' as 'Message' 

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive, 'Failed In Territory' as 'Message'     
RETURN 
END 
--Cities Ends

--FreightZone begins
IF NOT EXISTS(Select 1 FROM [FreightZones] Where [Name]= @FreightZoneName)
BEGIN
	ROLLBACK
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed, FreightZone Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @FreightZoneId = Id From [FreightZones] Where [Name] = @FreightZoneName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed In FreightZone' as 'Message'      
RETURN 
END 
--FreightZone Ends

--FreightRoute begins
IF NOT EXISTS(Select 1 FROM [FreightRoutes] Where [Name]= @FreightRouteName)
BEGIN
	ROLLBACK
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed, FreightRoute Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @FreightRouteId = Id From [FreightRoutes] Where [Name] = @FreightRouteName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed In FreightRoute' as 'Message'      
RETURN 
END 
--FreightRoute Ends

--Depot begins
IF NOT EXISTS(Select 1 FROM [Depots] Where [Code]= @DepotCode)
BEGIN
	ROLLBACK
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed, Depot Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DepotId = Id From [Depots] Where [Code] = @DepotCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed In Depot' as 'Message'      
RETURN 
END 
--Depot Ends

--Plant begins
IF NOT EXISTS(Select 1 FROM [Depots] Where [Code]= @PlantCode and IsPlant=1)
BEGIN
	ROLLBACK
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed, Plant Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @PlantId = Id From [Depots] Where [Code] = @PlantCode and IsPlant=1

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed In Plant' as 'Message'      
RETURN 
END 
--Plant Ends

IF @IsBroker=1
BEGIN
--Broker begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Code] = @BrokerCode)
BEGIN
	ROLLBACK
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive, 'Failed, Broker Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @BrokerId = Id From [Users] Where [Code] = @BrokerCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive, 'Failed In Broker' as 'Message'      
RETURN 
END 
END
--Broker Ends

--Vertical begins
IF NOT EXISTS(Select 1 FROM [Verticals] Where [Code]= @VerticalCode)
BEGIN
	ROLLBACK
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed, Vertical Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @VerticalId = Id From [Verticals] Where [Code]= @VerticalCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, @IsActive as IsActive,  'Failed In Vertical' as 'Message'      
RETURN 
END 
--Vertical Ends


IF NOT EXISTS(Select 1 FROM [Users] u, [UserRoles] r Where u.Code= @Code and u.VerticalId = @VerticalId and r.RoleId = @RoleId and u.MobileNumber=@MobileNumber)
BEGIN
	INSERT INTO [Users]([Name],[Code],[Email],[MobileNumber],[FreightZoneId],[FreightRouteId],[GSTN],[Loadability],[TransportModeId],[SaudaBookingTypeId]
		,[SaudaValidityPeriod],[SaudaLimit],[IsSelf],[IsBroker],[CustomerGroup],[ZoneId],[StateId],[TerritoryId],[DistrictId],[CityId],[Pincode],[Address]
		,[IsActive],[CreatedBy],[CreatedDate],[IsSAPData],[IsSAPDataSyncOrNot],[SapStatusId],[IsApproved],[ApprovedBy],[IsBlacklisted],[VerticalId])
           VALUES (@Name,@Code,@Email,@MobileNumber,@FreightZoneId,@FreightRouteId,@GSTN,@Loadability,@TransportModeId,@SaudaBookingTypeId
		   ,@SaudaValidityPeriod,@SaudaLimit,@IsSelf,@IsBroker,'01',@ZoneId,@StateId,@TerritoryId,@DistrictId,@CityId,@Pincode,@Address
		   ,@Isactive,@CreatedBy,getdate(),0,0,0,0,0,0,@VerticalId);

	SELECT @CustomerId = Id From [Users] Where [Code]= @Code and [MobileNumber]=@MobileNumber

	if @CustomerId>0
	BEGIN
	
	--Insert UserRoles
	INSERT INTO [dbo].[UserRoles]([UserId],[RoleId],[IsSAPData],[CreatedBy],[CreatedDate])
		 VALUES (@CustomerId,@RoleId,0,@CreatedBy,getdate());

	--Insert UserIncoTerms
	INSERT INTO [dbo].[UserIncoTerms]([UserId],[CreatedBy],[CreatedDate],[IncoTermsId])
		SELECT @CustomerId,@CreatedBy,getdate(),Id FROM IncoTerms 
			JOIN STRING_SPLIT(@IncoTerms, ',') 
			ON value = [Name];  
--	SELECT Id,Name
--FROM IncoTerms 
--JOIN STRING_SPLIT(@IncoTerms, ',') 
--    ON value = Name;  

	--INSERT INTO [dbo].[UserIncoTerms]([UserId],[CreatedBy],[CreatedDate],[IncoTermsId])
	--	SELECT @CustomerId,@CreatedBy,getdate(), CAST(value AS INT)
	--	FROM  STRING_SPLIT(@IncoTerms, ',')
			
	--Insert UserDepotMapping - Depot
	INSERT INTO [dbo].[UserDepotMappings]([UserId],[DepotId],[IsSAPData],[CreatedBy],[CreatedDate])
		VALUES (@CustomerId,@DepotId,0,@CreatedBy,getdate());

	--Insert UserDepotMapping - Plant
	INSERT INTO [dbo].[UserDepotMappings]([UserId],[DepotId],[IsSAPData],[CreatedBy],[CreatedDate])
		VALUES (@CustomerId,@PlantId,0,@CreatedBy,getdate());	

	IF @IsBroker=1
	BEGIN  
	--Insert UserCustomerMapping
	INSERT INTO [dbo].[UserCustomerMappings]([UserId],[CustomerId],[CreatedBy],[CreatedDate])
		VALUES (@CustomerId,@BrokerId,@CreatedBy,getdate());
	END

    SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode,MobileNumber, IsActive, 'Success' as 'Message' FROM [Users] Where [Code]= @Code and [MobileNumber]=@MobileNumber
	END
	ELSE
	SELECT @Name as [Name], 'Failed In Customer' as 'Message'    
END
ELSE
BEGIN  
	UPDATE [dbo].[Users]
	  SET [Email] =@Email,[FreightZoneId]= @FreightZoneId,[FreightRouteId]=@FreightRouteId,[IsActive]=@Isactive,[Pincode] =@Pincode,[ZoneId] = @ZoneId
	  ,[DistrictId] =  @DistrictId,[CityId]=@CityId,[StateId] =@StateId,[TerritoryId]= @TerritoryId,[IsSelf]=@IsSelf
	  ,[GSTN] =@GSTN,[SaudaValidityPeriod] = @SaudaValidityPeriod,[SaudaLimit] =@SaudaLimit,[Loadability] = @Loadability,[Address] =@Address
	  ,[SaudaBookingTypeId] = @SaudaBookingTypeId,[TransportModeId] =@TransportModeId,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate()
	 Where [Code]= @Code and [MobileNumber]=@MobileNumber and [VerticalId] = @VerticalId
	

	SELECT @VerticalCode as VerticalCode,@IsBroker as IsBroker,@IsSelf as IsSelf,@SaudaLimit as SaudaLimit,@SaudaValidityPeriod as SaudaValidityPeriod,@Code as Code,@Name as Name,@MobileNumber as MobileNumber,@Email as Email,@GSTN as GSTN,@Loadability as Loadability,@IncoTerms as IncoTerms,@TransportMode as TransportMode,@SaudaBookingType as SaudaBookingType,@Pincode as Pincode,@Address as Address,@BrokerCode as BrokerCode,@ZoneName as ZoneName,@StateName as StateName,@TerritoryName as TerritoryName, @DistrictName as DistrictName, @CityName as CityName,@FreightZoneName as FreightZoneName,@FreightRouteName as FreightRouteName,@DepotCode as DepotCode,@PlantCode as PlantCode, IsActive, 'Record Updated' as 'Message' FROM [Users] Where [Code]= @Code and [MobileNumber]=@MobileNumber

END
COMMIT
GO
