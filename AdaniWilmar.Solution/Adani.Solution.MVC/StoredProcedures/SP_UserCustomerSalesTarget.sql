IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SetUserCustomerSalesTarget')
    BEGIN
        DROP  Procedure SetUserCustomerSalesTarget
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SetUserCustomerSalesTarget](
	@AssignedFromCode nvarchar(100),
	@AssignedToCode nvarchar(100),
	@Quarter nvarchar(100),
	@Month nvarchar(100),
	@FinancialYear nvarchar(100),
	@Year nvarchar(100),
	@VerticalName nvarchar(100),
	@OilTypeName nvarchar(100),
	@Target nvarchar(100),
	@CreatedBy bigint
    )
AS
DECLARE @AssignedFromId bigint, @AssignedToId bigint, @TargetId bigint, @MonthId bigint,@FinancialYearId bigint,@VerticalId bigint, @OilTypeId bigint

SET @AssignedFromCode = ltrim(rtrim(@AssignedFromCode))
SET @AssignedToCode = ltrim(rtrim(@AssignedToCode))
SET @Quarter = ltrim(rtrim(@Quarter))
SET @Month = ltrim(rtrim(@Month))
SET @FinancialYear = ltrim(rtrim(@FinancialYear))

Set NOCOUNT OFF

BEGIN TRANSACTION

--Vertical begins
IF NOT EXISTS(Select 1 FROM Verticals Where [Name]= @VerticalName)
BEGIN
		ROLLBACK
		SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed, Vertical Not Exist' as 'Message'      
		RETURN
END
ELSE
		Select @VerticalId = Id From Verticals Where [Name] = @VerticalName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed In Vertical' as 'Message'      
RETURN 
END 
--Vertical Ends

--OilType begins
IF NOT EXISTS(Select 1 FROM [OilTypes] Where [Name]= @OilTypeName and [VerticalId]=@VerticalId)
BEGIN
		ROLLBACK
		SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed, OilType Not Exist' as 'Message'      
		RETURN
END
ELSE
		Select @OilTypeId = Id From [OilTypes] Where [Name] = @OilTypeName and [VerticalId]=@VerticalId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed In OilType' as 'Message'      
RETURN 
END 
--OilType Ends


--AssignedFrom begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Code]= @AssignedFromCode and [VerticalId]=@VerticalId)
BEGIN
	ROLLBACK
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed, AssignedFrom User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedFromId = Id From [Users] Where [Code]= @AssignedFromCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed In AssignedFrom' as 'Message'      
RETURN 
END 
--AssignedFrom Ends


--AssignedTo begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Code]= @AssignedToCode and [VerticalId]=@VerticalId)
BEGIN
	ROLLBACK
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed, AssignedTo User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedToId = Id From [Users] Where [Code]= @AssignedToCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed In AssignedTo' as 'Message'      
RETURN 
END 
--AssignedTo Ends

--Month begins
IF NOT EXISTS(Select 1 FROM [Months] Where [Name]= @Month)
BEGIN
	ROLLBACK
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed, Month Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @MonthId = Id From [Months] Where [Name] = @Month

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed In Month' as 'Message'      
RETURN 
END 
--Month Ends

--FinancialYear begins
IF NOT EXISTS(Select 1 FROM [dbo].[FinancialYears] Where [Year]= @FinancialYear)
BEGIN
	ROLLBACK
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed, FinancialYear Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @FinancialYearId = Id From [FinancialYears] Where [Year]= @FinancialYear

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Failed In FinancialYear' as 'Message'      
RETURN 
END 
--FinancialYear Ends


IF NOT EXISTS(Select 1 FROM [UserCustomerSalesTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId)
BEGIN
	INSERT INTO [dbo].[UserCustomerSalesTargets]([AssignedFromId],[AssignedToId],[Quarter],[MonthId],[FinancialYearId],[Year],[VerticalId],[OilTypeId],[Target],[CreatedBy],[CreatedDate])
           VALUES (@AssignedFromId,@AssignedToId,@Quarter,@MonthId,@FinancialYearId,@Year,@VerticalId,@OilTypeId,@Target,@CreatedBy,getdate());

	SELECT @TargetId = Id From [UserCustomerSalesTargets]  Where [AssignedFromId]= @AssignedFromId and [AssignedToId]= @AssignedToId
	
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Success' as 'Message' FROM [UserCustomerSalesTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
END
ELSE
BEGIN
	UPDATE [dbo].[UserCustomerSalesTargets]
	   SET [Quarter] = @Quarter,[MonthId] = @MonthId,[FinancialYearId]= @FinancialYearId,[Year]= @Year,[VerticalId] = @VerticalId,[OilTypeId] = @OilTypeId,[Target] = @Target,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate()
	   WHERE [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId	
	
	SELECT @AssignedFromCode as AssignedFromCode,@AssignedToCode as AssignedToCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalName as VerticalName,@OilTypeName as OilType,@Target as 'Target', 'Record Updated' as 'Message' FROM [UserCustomerSalesTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
END
COMMIT
GO


