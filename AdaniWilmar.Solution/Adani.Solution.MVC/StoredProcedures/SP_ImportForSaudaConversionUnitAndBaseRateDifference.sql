CREATE Procedure [dbo].[SetSaudaConversionUnitAndBaseRateDifference](
	@OilType nvarchar(100),
	@PlantOrDepot nvarchar(100),
	@State nvarchar(100),
	@FromPackGroup nvarchar(100),
	@FromSku nvarchar(100),
	@FromUnit decimal(18,3),
	@ToPackGroup nvarchar(100),
	@ToSku nvarchar(100),
	@ToUnit decimal(18,3),
	@ValidFrom datetime,
	@ValidTo datetime,
	@IsActive bit,
	@CreatedBy bigint,
	@BasicRate decimal(18,2),
	@FromSkuCode nvarchar(100),
	@ToSkuCode nvarchar(100)
	)
	as
DECLARE @OilTypeId bigint, @StateId bigint, @PlantOrDepotId bigint, @FromPackGroupId bigint,@FromSkuId bigint,@ToPackGroupId bigint, @ToSkuId bigint, @SaudaConversionUnitAndBaseRateDifferenceId bigint,@InsertedSaudaConversionUnitAndBaseRateDifferenceId bigint


set @FromUnit = ltrim(rtrim(@FromUnit))
set @ToUnit = ltrim(rtrim(@ToUnit))
set @IsActive = ltrim(rtrim(@IsActive))
set @CreatedBy = ltrim(rtrim(@CreatedBy))
set @BasicRate = ltrim(rtrim(@BasicRate))
set @FromSkuCode = ltrim(rtrim(@FromSkuCode))
set @ToSkuCode = ltrim(rtrim(@ToSkuCode))

Set NOCOUNT OFF

BEGIN TRANSACTION

--OilType begins
IF NOT EXISTS(SELECT 1 FROM [OilTypes] Where [Name]= @OilType)
BEGIN
	ROLLBACK
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed, OilType Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @OilTypeId = Id From [OilTypes] Where Name = @OilType

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  	 
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed In OilType' as 'Message'      
	RETURN 
END 
--OilType Ends

--State begins
IF NOT EXISTS(Select 1 FROM States Where [StateName]= @State)
BEGIN
	ROLLBACK
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed,State not exists' as 'Message'      
	RETURN
END
ELSE
    Select @StateId = Id From States Where StateName = @State

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed In State' as 'Message'      
RETURN 
END 
--State Ends

--PlantOrDepot Begins
IF NOT EXISTS(SELECT 1 FROM Depots Where [Name]= @PlantOrDepot)
BEGIN
	ROLLBACK
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed,Plant Or Depot not exists' as 'Message'  
	RETURN
END
ELSE
	BEGIN
	  SELECT @PlantOrDepotId = Id FROM Depots Where Name = @PlantOrDepot
	END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed In PlantOrDepot' as 'Message'  
	RETURN 
END 
--PlantOrDepot Ends

--FromPackGroup Begins
IF NOT EXISTS(SELECT 1 FROM PackGroups WHERE [Name] = @FromPackGroup)
BEGIN
	ROLLBACK
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed,FromPackGroup not exists' as 'Message'  
	RETURN
END
ELSE
	BEGIN
		SELECT @FromPackGroupId = Id FROM PackGroups WHERE Name = @FromPackGroup
	END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed In FromPackGroup' as 'Message'  
	RETURN 
END 
--FromPackGroup Ends

--FromSku begins
IF NOT EXISTS(Select 1 FROM Skus, OilTypes Where Skus.OilTypeId = OilTypes.Id and Skus.OilTypeId = @OilTypeId and  [SkuCode]= @FromSkuCode)
BEGIN
	SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed,FromSku not exists' as 'Message'  
END
ELSE
	BEGIN
		SELECT @FromSkuId = Skus.Id FROM Skus, OilTypes Where Skus.OilTypeId = OilTypes.Id and Skus.OilTypeId = @OilTypeId and  [SkuCode]= @FromSkuCode
	END
IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		 SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed In FromSku' as 'Message'  
	RETURN 
END 
--FromSku Ends

--ToPackGroup Begins
IF NOT EXISTS(SELECT 1 FROM PackGroups WHERE [Name] = @ToPackGroup)
BEGIN
	ROLLBACK
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed,ToPackGroup not exists' as 'Message'  
	RETURN
END
ELSE
	BEGIN
		SELECT @ToPackGroupId = Id FROM PackGroups WHERE Name = @ToPackGroup
	END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed In ToPackGroup' as 'Message'  
	RETURN 
END 
--ToPackGroup Ends

--ToSku begins
IF NOT EXISTS(Select 1 FROM Skus, OilTypes Where Skus.OilTypeId = OilTypes.Id and Skus.OilTypeId = @OilTypeId and  [SkuCode]= @ToSkuCode)
BEGIN
	SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed,ToSku not exists' as 'Message'  
END
ELSE
	BEGIN
		SELECT @ToSkuId = Skus.Id FROM Skus, OilTypes Where Skus.OilTypeId = OilTypes.Id and Skus.OilTypeId = @OilTypeId and  [SkuCode]= @ToSkuCode
	END
IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		 SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed In ToSku' as 'Message'  
	RETURN 
END 
--ToSku Ends

--ValidFrom begins
IF @ValidFrom < DATEADD(day, DATEDIFF(day,0,GETDATE()),0)
BEGIN
	ROLLBACK
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'From date is invalid' as 'Message'  
	RETURN
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'From date is invalid' as 'Message'      
	RETURN 
END 
--ValidFrom Ends

--ValidTo begins
IF @ValidTo < DATEADD(day, DATEDIFF(day,0,GETDATE()),0)
BEGIN
	ROLLBACK
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'To date is invalid' as 'Message'  
	RETURN
END
IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'To date is invalid' as 'Message'  
	RETURN 
END 

IF @ValidTo < @ValidFrom
BEGIN
	ROLLBACK
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'To date is invalid' as 'Message'  
	RETURN
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'To date is invalid' as 'Message'      
	RETURN 
END 
--ValidTo Ends

IF EXISTS (SELECT 1 FROM SaudaConversionUnitAndDifferenceRates WHERE SourceId = @PlantOrDepotId and StateId = @StateId and FromPackGroupId = @FromPackGroupId and FromSkuId = @FromSkuId
and (((DATEADD(dd, DATEDIFF(dd, 0, FromDate),0) <=  DATEADD(dd, DATEDIFF(dd, 0, @ValidFrom),0)) and (DATEADD(dd, DATEDIFF(dd, 0, @ValidFrom),0) <= DATEADD(dd, DATEDIFF(dd, 0, ToDate),0) )) 
or ((DATEADD(dd, DATEDIFF(dd, 0, FromDate),0) <=  DATEADD(dd, DATEDIFF(dd, 0, @ValidTo),0)) and (DATEADD(dd, DATEDIFF(dd, 0, @ValidTo),0) <= DATEADD(dd, DATEDIFF(dd, 0, ToDate),0))))
)
BEGIN
	Select @SaudaConversionUnitAndBaseRateDifferenceId = Id FROM SaudaConversionUnitAndDifferenceRates WHERE SourceId = @PlantOrDepotId and StateId = @StateId and FromPackGroupId = @FromPackGroupId and FromSkuId = @FromSkuId
	and (((DATEADD(dd, DATEDIFF(dd, 0, FromDate),0) <=  DATEADD(dd, DATEDIFF(dd, 0, @ValidFrom),0)) and (DATEADD(dd, DATEDIFF(dd, 0, @ValidFrom),0) <= DATEADD(dd, DATEDIFF(dd, 0, ToDate),0) )) 
	or ((DATEADD(dd, DATEDIFF(dd, 0, FromDate),0) <=  DATEADD(dd, DATEDIFF(dd, 0, @ValidTo),0)) and (DATEADD(dd, DATEDIFF(dd, 0, @ValidTo),0) <= DATEADD(dd, DATEDIFF(dd, 0, ToDate),0))))

	UPDATE SaudaConversionUnitAndDifferenceRateDetails SET IsActive=0,ModifiedBy = @CreatedBy,ModifiedDate = GETDATE()  WHERE SaudaConversionUnitAndDifferenceRateId = @SaudaConversionUnitAndBaseRateDifferenceId

	--Insert in fromsku table 
	Insert into SaudaConversionUnitAndDifferenceRates(FromPackGroupId,FromSkuId,FromUnit,FromDate,ToDate,CreatedBy,CreatedDate,SourceId,StateId) values(@FromPackGroupId,@FromSkuId,@FromUnit,@ValidFrom,@ValidTo,@CreatedBy,GETDATE(),@PlantOrDepotId,@StateId)
	SELECT @InsertedSaudaConversionUnitAndBaseRateDifferenceId = SCOPE_IDENTITY()
	--Insert in tosku table
	 Insert into SaudaConversionUnitAndDifferenceRateDetails(SaudaConversionUnitAndDifferenceRateId,ToPackGroupId,ToSkuId,ToUnit,BasicRate,CreatedBy,CreatedDate,IsActive) values(@InsertedSaudaConversionUnitAndBaseRateDifferenceId,@ToPackGroupId,@ToSkuId,@ToUnit,@BasicRate,@CreatedBy,GETDATE(),@IsActive)

	SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Success' as 'Message'  
END
ELSE
BEGIN
	--Insert in fromsku table 
	Insert into SaudaConversionUnitAndDifferenceRates(FromPackGroupId,FromSkuId,FromUnit,FromDate,ToDate,CreatedBy,CreatedDate,SourceId,StateId) values(@FromPackGroupId,@FromSkuId,@FromUnit,@ValidFrom,@ValidTo,@CreatedBy,GETDATE(),@PlantOrDepotId,@StateId)
	SELECT @InsertedSaudaConversionUnitAndBaseRateDifferenceId = SCOPE_IDENTITY()
	--Insert in tosku table
	 Insert into SaudaConversionUnitAndDifferenceRateDetails(SaudaConversionUnitAndDifferenceRateId,ToPackGroupId,ToSkuId,ToUnit,BasicRate,CreatedBy,CreatedDate,IsActive) values(@InsertedSaudaConversionUnitAndBaseRateDifferenceId,@ToPackGroupId,@ToSkuId,@ToUnit,@BasicRate,@CreatedBy,GETDATE(),@IsActive)

	SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Success' as 'Message'  
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @OilType as OilType,@PlantOrDepot as PlantOrDepot ,@State as State,@FromPackGroup as FromPackGroup,@FromSku as FromSku,@FromUnit as FromUnit,@ToPackGroup as ToPackGroup,@ToSku as ToSku,@ToUnit as ToUnit,@ValidFrom as ValidFrom,@ValidTo as ValidTo,@BasicRate as BasicRate,@FromSkuCode as FromSkuCode,@ToSkuCode as ToSkuCode,@IsActive as IsActive, 'Failed in SaudaConversion unit and base rate difference insert' as 'Message'  
	RETURN 
END 
COMMIT