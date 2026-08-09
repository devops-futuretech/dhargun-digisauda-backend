CREATE Procedure [dbo].[SetSku](
	@SkuName nvarchar(150),
	@SkuCode nvarchar(150),
	@OilTypeName nvarchar(100),
	@VerticalCode nvarchar(100),
	@PackType nvarchar(100),
	@PackSizeQuantity decimal(18,2),
	@PackSize nvarchar(100),
	@PackGroup nvarchar(100),
	@ProcessCost decimal(18,2),
	@SubCategory char(100),
	@IsActive bit,
	@UOM1_No decimal(18,2),
	@Uom2_CaseToNumberConversion decimal(18,2),
	@Uom3_MetricTonToNumberConversion decimal(18,2),
	@SapStatusId int,
	@CreatedBy bigint,
	@MaterialTypeName  nvarchar(150)
    )

	as
DECLARE @VerticalId bigint, @OilTypeId bigint, @PackTypeId bigint, @PackGroupId bigint, @UomId bigint, @SubCategoryId bigint,@SkuId bigint,
@Conv1_UomId bigint,@Conv2_UomId bigint, @Conv3_UomId bigint,@Rel_UomId bigint,@MaterialTypeId bigint

set @VerticalCode = ltrim(rtrim(@VerticalCode))
set @OilTypeName = ltrim(rtrim(@OilTypeName))
set @PackType = ltrim(rtrim(@PackType))
set @PackGroup = ltrim(rtrim(@PackGroup))
set @PackSize = ltrim(rtrim(@PackSize))
set @SubCategory = ltrim(rtrim(@SubCategory))
set @MaterialTypeName = ltrim(rtrim(@MaterialTypeName))
set @SkuName = ltrim(rtrim(@SkuName))
set @SkuCode = ltrim(rtrim(@SkuCode))

Set NOCOUNT OFF
BEGIN TRANSACTION

--Vertical begins
IF NOT EXISTS(SELECT 1 FROM Verticals WHERE [Code]= @VerticalCode)
BEGIN
	ROLLBACK
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed, Vertical Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @VerticalId = Id From Verticals WHERE [Code] = @VerticalCode

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed In Vertical' as 'Message'      
	RETURN 
END 
--Vertical Ends

--OilType begins
IF NOT EXISTS(SELECT 1 FROM [OilTypes] WHERE [Name]= @OilTypeName and VerticalId=@VerticalId)
BEGIN
	ROLLBACK
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed, OilType Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @OilTypeId = Id From [OilTypes] WHERE [Name] = @OilTypeName

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed In OilType' as 'Message'      
	RETURN 
END 
--OilType Ends

--PackType begins
IF NOT EXISTS(SELECT 1 FROM [PackTypes] WHERE [Name]= @PackType)
BEGIN
	ROLLBACK
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed, PackType Not Exist' as 'Message' 
	RETURN
END
ELSE
	SELECT @PackTypeId = Id From [PackTypes] WHERE [Name] = @PackType

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed In PackType' as 'Message'      
	RETURN 
END 
--PackType Ends

--PackGroup begins
IF NOT EXISTS(SELECT 1 FROM [PackGroups] WHERE [Name]= @PackGroup)
BEGIN
	ROLLBACK
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed, PackGroup Not Exist' as 'Message'      
	RETURN
END
ELSE
		SELECT @PackGroupId = Id From [PackGroups] WHERE [Name] = @PackGroup

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed In PackGroup' as 'Message'      
	RETURN 
END 
--PackGroup Ends

--PackSize begins
IF NOT EXISTS(SELECT 1 FROM [Uoms] WHERE [Name]= @PackSize and IsQuantityType=1)
BEGIN
	ROLLBACK
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed, PackSize Not Exist' as 'Message'      
	RETURN
END
ELSE
	SELECT @UomId = Id From [Uoms] WHERE [Name] = @PackSize and IsQuantityType=1

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed In PackSize' as 'Message'      
	RETURN 
END 
--PackSize Ends

--SubCategory begins
--IF NOT EXISTS(SELECT 1 FROM [SubCategories] WHERE [Name]= @SubCategory)
--BEGIN
--		ROLLBACK
--		SELECT @PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as
-- VerticalName,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@IsActive as IsActive, 'Failed, SubCategory Not Exist' as 'Message'      
--		RETURN
--END
--ELSE
--		SELECT @SubCategoryId = Id From [SubCategories] WHERE [Name] = @SubCategory

--IF @@ERROR <> 0 
--BEGIN     
--ROLLBACK  
--	SELECT @PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as 
--VerticalName,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@IsActive as IsActive, 'Failed In SubCategory' as 'Message'      
--RETURN 
--END 
--SubCategory Ends

----MaterialType begins
--IF EXISTS(SELECT 1 FROM MaterialTypes WHERE [Name]= @MaterialTypeName)
--BEGIN
--	ROLLBACK
--		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed, MaterialType Not Exist' as 'Message'      
--	RETURN
--END
--ELSE
--	SELECT @MaterialTypeId = Id From MaterialTypes WHERE [Name] = @MaterialTypeName

--IF @@ERROR <> 0 
--BEGIN     
--	ROLLBACK  
--		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed In MaterialType' as 'Message'      
--	RETURN 
--END 
----MaterialType Ends

--MaterialType begins
IF EXISTS(SELECT 1 FROM MaterialTypes WHERE [Name]= @MaterialTypeName)
	SELECT @MaterialTypeId = Id From MaterialTypes WHERE [Name] = @MaterialTypeName
ELSE
	SET @MaterialTypeId = null
 
--MaterialType Ends


IF NOT EXISTS(SELECT 1 FROM [Skus] WHERE [SkuCode]= @SkuCode and [OilTypeId]=@OilTypeId and [VerticalId]=@VerticalId)
BEGIN
	INSERT INTO [Skus]([SkuName],[SkuCode],[OilTypeId],[IsActive],[ProcessCost],[Quantity],[VerticalId],[PackTypeId],[PackGroupId],[UomId],[SubCategoryId],[SapStatusId],[LitreConversion],
	[IsSAPData],[IsSAPDataSyncOrNot],[VerticalGroupId],[MaterialTypeId],[CreatedBy],[CreatedDate])
           VALUES (@SkuName,@SkuCode,@OilTypeId,@Isactive,@ProcessCost,@PackSizeQuantity,@VerticalId,@PackTypeId,@PackGroupId,@UomId,@SubCategoryId
		   ,@SapStatusId,0,0,0,0,@MaterialTypeId,@CreatedBy,getdate());

	SELECT @SkuId = Id From [Skus] WHERE [SkuName] = @SkuName

	IF @SkuId>0
	BEGIN
		SELECT @Conv1_UomId = Id From [Uoms] WHERE [Name] = 'NOS'
		SELECT @Conv2_UomId = Id From [Uoms] WHERE [Name] = 'Case'
		SELECT @Conv3_UomId = Id From [Uoms] WHERE [Name] = 'MT'
		SELECT @Rel_UomId = Id From [Uoms] WHERE [Name] = 'NOS'
	
		INSERT INTO [SkuUomMappings]([SkuId],[UomId],[RelationUomId],[ConversionFactor],[CreatedBy],[CreatedDate])
		VALUES (@SkuId,@Conv1_UomId,@Rel_UomId,@UOM1_No,@CreatedBy,getdate());

		INSERT INTO [SkuUomMappings]([SkuId],[UomId],[RelationUomId],[ConversionFactor],[CreatedBy],[CreatedDate])
		VALUES (@SkuId,@Conv2_UomId,@Rel_UomId,@Uom2_CaseToNumberConversion,@CreatedBy,getdate());

		INSERT INTO [SkuUomMappings]([SkuId],[UomId],[RelationUomId],[ConversionFactor],[CreatedBy],[CreatedDate])
		VALUES (@SkuId,@Conv3_UomId,@Rel_UomId,@Uom3_MetricTonToNumberConversion,@CreatedBy,getdate());	

		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Success' as 'Message' 
	END
	ELSE
		SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed In Sku' as 'Message'    
END
ELSE
BEGIN  	
	UPDATE [dbo].[Skus]
	   SET [SkuName] = @SkuName,[SkuCode] = @SkuCode,[OilTypeId] = @OilTypeId,[ProcessCost] = @ProcessCost,[Quantity] =@PackSizeQuantity,[VerticalId] = @VerticalId,[PackTypeId] = @PackTypeId
	   ,[PackGroupId] = @PackGroupId,[UomId] = @UomId,[SubCategoryId] =@SubCategoryId,[IsActive]=@Isactive,[MaterialTypeId]=@MaterialTypeId,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate()
	   WHERE [SkuCode]= @SkuCode and [OilTypeId]=@OilTypeId and [VerticalId]=@VerticalId

	SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Record Updated' as 'Message' 
END 

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @ProcessCost as ProcessCost,@PackSizeQuantity as PackSizeQuantity,@UOM1_No as UOM1_No, @Uom2_CaseToNumberConversion as Uom2_CaseToNumberConversion,@Uom3_MetricTonToNumberConversion as Uom3_MetricTonToNumberConversion, @SkuName as SkuName,@SkuCode as SkuCode,@VerticalCode as VerticalCode,@OilTypeName as OilTypeName,@PackType as PackType,@PackGroup as PackGroup,@PackSize as PackSize,@SubCategory as SubCategory,@MaterialTypeName as MaterialTypeName,@IsActive as IsActive, 'Failed in Sku' as 'Message' 
RETURN 
END 
COMMIT