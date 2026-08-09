IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SetUserCustomerSalesTarget')
    BEGIN
        DROP  Procedure SetUserCustomerSalesTarget
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SetUserCustomerSaudaTarget](
	@AssignedFromUserCode nvarchar(100),
	@AssignedToUserCode nvarchar(100),
	@Quarter nvarchar(100),
	@Month nvarchar(100),
	@FinancialYear nvarchar(100),
	@Year nvarchar(100),
	@VerticalCode nvarchar(100),
	@OilTypeName nvarchar(100),
	@Target nvarchar(100),
	@CreatedBy bigint
    )
AS
DECLARE @AssignedFromId bigint, @AssignedToId bigint, @TargetId bigint, @MonthId bigint,@FinancialYearId bigint,@VerticalId bigint, @OilTypeId bigint

SET @AssignedFromUserCode = ltrim(rtrim(@AssignedFromUserCode))
SET @AssignedToUserCode = ltrim(rtrim(@AssignedToUserCode))
SET @Quarter = ltrim(rtrim(@Quarter))
SET @Month = ltrim(rtrim(@Month))
SET @FinancialYear = ltrim(rtrim(@FinancialYear))

Set NOCOUNT OFF

BEGIN TRANSACTION

--Vertical begins
IF NOT EXISTS(Select 1 FROM Verticals Where [Code]= @VerticalCode)
BEGIN
		ROLLBACK
		SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed, Vertical Not Exist' as 'Message'      
		RETURN
END
ELSE
		Select @VerticalId = Id From Verticals Where [Code] = @VerticalCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed In Vertical' as 'Message'      
RETURN 
END 
--Vertical Ends

--OilType begins
IF NOT EXISTS(Select 1 FROM [OilTypes] Where [Name]= @OilTypeName and [VerticalId]=@VerticalId)
BEGIN
		ROLLBACK
		SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed, OilType Not Exist' as 'Message'      
		RETURN
END
ELSE
		Select @OilTypeId = Id From [OilTypes] Where [Name] = @OilTypeName and [VerticalId]=@VerticalId

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed In OilType' as 'Message'      
RETURN 
END 
--OilType Ends


--AssignedFrom begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Code]= @AssignedFromUserCode and [VerticalId]=@VerticalId)
BEGIN
	ROLLBACK
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed, AssignedFrom User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedFromId = Id From [Users] Where [Code]= @AssignedFromUserCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed In AssignedFrom' as 'Message'      
RETURN 
END 
--AssignedFrom Ends


--AssignedTo begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Code]= @AssignedToUserCode and [VerticalId]=@VerticalId)
BEGIN
	ROLLBACK
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed, AssignedTo User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedToId = Id From [Users] Where [Code]= @AssignedToUserCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed In AssignedTo' as 'Message'      
RETURN 
END 
--AssignedTo Ends

--Month begins
IF NOT EXISTS(Select 1 FROM [Months] Where [Name]= @Month)
BEGIN
	ROLLBACK
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed, Month Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @MonthId = Id From [Months] Where [Name] = @Month

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed In Month' as 'Message'      
RETURN 
END 
--Month Ends

--FinancialYear begins
IF NOT EXISTS(Select 1 FROM [dbo].[FinancialYears] Where [Year]= @FinancialYear)
BEGIN
	ROLLBACK
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed, FinancialYear Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @FinancialYearId = Id From [FinancialYears] Where [Year]= @FinancialYear

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Failed In FinancialYear' as 'Message'      
RETURN 
END 
--FinancialYear Ends


IF NOT EXISTS(Select 1 FROM [UserCustomerSaudaTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId)
BEGIN
	INSERT INTO [dbo].[UserCustomerSaudaTargets]([AssignedFromId],[AssignedToId],[Quarter],[MonthId],[FinancialYearId],[Year],[VerticalId],[OilTypeId],[Target],[CreatedBy],[CreatedDate])
           VALUES (@AssignedFromId,@AssignedToId,@Quarter,@MonthId,@FinancialYearId,@Year,@VerticalId,@OilTypeId,@Target,@CreatedBy,getdate());

	SELECT @TargetId = Id From [UserCustomerSaudaTargets]  Where [AssignedFromId]= @AssignedFromId and [AssignedToId]= @AssignedToId
	
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Success' as 'Message' FROM [UserCustomerSaudaTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
END
ELSE
BEGIN
	UPDATE [dbo].[UserCustomerSaudaTargets]
	   SET [Quarter] = @Quarter,[MonthId] = @MonthId,[FinancialYearId]= @FinancialYearId,[Year]= @Year,[VerticalId] = @VerticalId,[OilTypeId] = @OilTypeId,[Target] = @Target,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate()
	   WHERE [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId	
	
	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@VerticalCode as VerticalCode,@OilTypeName as OilType,@Target as 'Target', 'Record Updated' as 'Message' FROM [UserCustomerSaudaTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
END
COMMIT
GO


