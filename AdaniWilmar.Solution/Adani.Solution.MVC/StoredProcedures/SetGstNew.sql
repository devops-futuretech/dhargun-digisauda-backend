ALTER PROCEDURE [dbo].[SetGstNew]

	@SourceState nvarchar(100),
	@DestinationState nvarchar(100),
	@PlantName nvarchar(100),	
	@OilTypeName nvarchar(100),
	@CGST decimal(18,2),
	@SGST decimal(18,2),
	@IGST decimal(18,2),
	@ValidFrom datetime2,
	@ValidTo datetime2,
	@CreatedBy bigint,
	@ParentId bigint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

	DECLARE @SourceStateId INT,@DestinationStateId INT,@OilTypeId INT = 0,@DepotId BIGINT = 0,@ParentIdReturn BIGINT = 0

	set @SourceState = ltrim(rtrim(@SourceState))
	set @DestinationState = ltrim(rtrim(@DestinationState))
	
	BEGIN TRANSACTION

	--ValidFrom begins
	IF @ValidFrom < DATEADD(day, DATEDIFF(day,0,GETDATE()),0) 
	BEGIN
		ROLLBACK
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName, @CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'From date is invalid' as 'Message'
		RETURN
	END
	IF @@ERROR <> 0 
	BEGIN     
		ROLLBACK  
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName, @CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'From date is invalid' as 'Message'      
		RETURN 
	END 
	--ValidFrom end

	--ValidTo begins
	IF @ValidTo < DATEADD(day, DATEDIFF(day,0,GETDATE()),0) 
	BEGIN
		ROLLBACK
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName, @CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'To date is invalid' as 'Message'
		RETURN
	END

	IF @@ERROR <> 0 
	BEGIN     
		ROLLBACK  
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName, @CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo,'To date is invalid' as 'Message'      
		RETURN 
	END 

	IF @ValidTo < @ValidFrom
	BEGIN
		ROLLBACK
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName, @CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo,'To date is invalid' as 'Message'
		RETURN
	END

	IF @@ERROR <> 0 
	BEGIN     
		ROLLBACK  
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName, @CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'To date is invalid' as 'Message'      
		RETURN 
	END 
	--ValidTo Ends

	--Source State begins
	IF NOT EXISTS(SELECT 1 FROM States WHERE [StateName]= @SourceState)
	BEGIN
		ROLLBACK
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Source State Not Exist' as 'Message'     
		RETURN 
	END
	ELSE
		SELECT @SourceStateId = Id From States Where StateName = @SourceState

	IF @@ERROR <> 0 
	BEGIN     
		ROLLBACK  
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Failed In Source State' as 'Message'      
		RETURN 
	END 
	--Source State Ends

	--Destination State begins
	IF NOT EXISTS(SELECT 1 FROM States WHERE [StateName]= @DestinationState)
	BEGIN
		ROLLBACK
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Destination State Not Exist' as 'Message'     
		RETURN 
	END
	ELSE
			SELECT @DestinationStateId = Id From States Where StateName = @DestinationState

	IF @@ERROR <> 0 
	BEGIN     
		ROLLBACK  
			SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Failed In Source Destination' as 'Message'      
		RETURN 
	END 
	--Destination State Ends

	--Depot begins
	IF(@PlantName != '' OR @PlantName IS NOT NULL)
	BEGIN    
		IF NOT EXISTS(SELECT 1 FROM Depots WHERE [Name]= @PlantName)
		BEGIN
			ROLLBACK
				SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Plant Not Exist' as 'Message'     
			RETURN
		END
		ELSE
		   SELECT @DepotId = Id from Depots WHERE [Name]= @PlantName

		IF @@ERROR <> 0
		BEGIN
			ROLLBACK
				SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName, @CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Failed In Plant' as 'Message'      
			RETURN
		END
	END
	--Depot Ends

	--OilType begins
	IF(@OilTypeName != '' OR @OilTypeName IS NOT NULL)
	BEGIN    
		IF NOT EXISTS(SELECT 1 FROM OilTypes o,Verticals v  Where o.Name = @OilTypeName)
		BEGIN
			ROLLBACK
				SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'OilType Not Exist' as 'Message'     
			RETURN 
		END
		ELSE
		 SELECT @OilTypeId = o.Id FROM OilTypes o,Verticals v  Where o.Name = @OilTypeName

		IF @@ERROR <> 0 
		BEGIN     
			ROLLBACK  
				SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Failed In OilType' as 'Message'      
			RETURN 
		END 
	END
	--OilType Ends

	--IGST=0.00
	--ValidFrom begins
		IF @CGST < 0 or @CGST > 100
		BEGIN		
			ROLLBACK
				SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Invaild CGST Percentage' as 'Message'
			RETURN		

			IF @@ERROR <> 0 
			BEGIN     
				ROLLBACK  
					SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Invaild CGST Percentage' as 'Message'      
				RETURN 
			END 
		END

		IF @SGST < 0 or @SGST > 100
		BEGIN
			ROLLBACK
					SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Invaild SGST Percentage' as 'Message'
			RETURN			

			IF @@ERROR <> 0 
			BEGIN     
				ROLLBACK  
					SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Invaild SGST Percentage' as 'Message'      
				RETURN 
			END
		END

		----CGST,SGST = 0.00
		IF @IGST < 0 or @IGST > 100
		BEGIN
			ROLLBACK
				SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Invaild IGST Percentage' as 'Message'
			RETURN			

			IF @@ERROR <> 0 
			BEGIN     
				ROLLBACK  
					SELECT @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Invaild IGST Percentage' as 'Message'      
				RETURN 
			END
		END
	--CGST,SGST,IGST Ends
		
	
		IF NOT EXISTS(SELECT 1 FROM Gsts WHERE SourceStateId = @SourceStateId and DestinationStateId = @DestinationStateId 
		and (DATEADD(dd, DATEDIFF(dd, 0, @ValidFrom),0) <=  DATEADD(dd, DATEDIFF(dd, 0, ValidTo),0)) 
		and (DATEADD(dd, DATEDIFF(dd, 0, ValidFrom),0) <=  DATEADD(dd, DATEDIFF(dd, 0, @ValidTo),0)))
		BEGIN
			IF(@SourceState = @DestinationState)
				BEGIN
					INSERT INTO Gsts(DepotId,OilTypeId,SourceStateId,DestinationStateId,CGST,SGST,IGST,IsActive,ValidFrom,ValidTo,CreatedBy,CreatedDate,ParentId) VALUES (@DepotId,@OilTypeId,@SourceStateId,@DestinationStateId,@CGST,@SGST,0,1,@ValidFrom,@ValidTo,@CreatedBy, getdate(),@ParentId);
					
				    SET @ParentIdReturn = SCOPE_IDENTITY();
					
					SELECT  @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Success' as 'Message', @ParentIdReturn as ParentId
				END	
			ELSE
				BEGIN
					INSERT INTO Gsts(DepotId,OilTypeId,SourceStateId,DestinationStateId,CGST,SGST,IGST,IsActive,ValidFrom,ValidTo,CreatedBy,CreatedDate,ParentId) VALUES (@DepotId,@OilTypeId,@SourceStateId,@DestinationStateId,0,0,@IGST,1,@ValidFrom,@ValidTo,@CreatedBy, getdate(),@ParentId);
					
					SET @ParentIdReturn = SCOPE_IDENTITY();
					
					SELECT  @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Success' as 'Message', @ParentIdReturn as ParentId
				END
		END
		ELSE
		BEGIN
			UPDATE Gsts SET IsActive = 0 WHERE SourceStateId = @SourceStateId and DestinationStateId = @DestinationStateId
			and (DATEADD(dd, DATEDIFF(dd, 0, @ValidFrom),0) <=  DATEADD(dd, DATEDIFF(dd, 0, ValidTo),0))
			and (DATEADD(dd, DATEDIFF(dd, 0, ValidFrom),0) <=  DATEADD(dd, DATEDIFF(dd, 0, @ValidTo),0))
		
			IF(@SourceState = @DestinationState)
				BEGIN
					INSERT INTO Gsts(DepotId,OilTypeId,SourceStateId,DestinationStateId,CGST,SGST,IGST,IsActive,ValidFrom,ValidTo,CreatedBy,CreatedDate,ParentId) VALUES (@DepotId,@OilTypeId,@SourceStateId,@DestinationStateId,@CGST,@SGST,0,1,@ValidFrom,@ValidTo,@CreatedBy, getdate(),@ParentId);
					
					SET @ParentIdReturn = SCOPE_IDENTITY();
					
					SELECT  @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Success' as 'Message', @ParentIdReturn as ParentId 
				END	
			ELSE
				BEGIN		
					INSERT INTO Gsts(DepotId,OilTypeId,SourceStateId,DestinationStateId,CGST,SGST,IGST,IsActive,ValidFrom,ValidTo,CreatedBy,CreatedDate,ParentId) VALUES (@DepotId,@OilTypeId,@SourceStateId,@DestinationStateId,0,0,@IGST,1,@ValidFrom,@ValidTo,@CreatedBy, getdate(),@ParentId);
					
					SET @ParentIdReturn = SCOPE_IDENTITY();
					
					SELECT  @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Success' as 'Message', @ParentIdReturn as ParentId
				END
		END
END

IF @@ERROR <> 0
BEGIN
	ROLLBACK
		SELECT  @SourceState as SourceState,@DestinationState as DestinationState,@PlantName as PlantName,@OilTypeName as OilTypeName,@CGST as CGST, @SGST as SGST,@IGST as IGST,@ValidFrom as ValidFrom,@ValidTo as ValidTo, 'Failed In GST' as 'Message' 
	RETURN
END

COMMIT