IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SetUserOilTypeTarget')
    BEGIN
        DROP  Procedure SetUserOilTypeTarget
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SetUserOilTypeTarget](
	@AssignedFrom nvarchar(100),
	@AssignedTo nvarchar(100),
	@Quarter nvarchar(100),
	@Month nvarchar(100),
	@FinancialYear nvarchar(100),
	@Year nvarchar(100),
	@OilTypeName  nvarchar(100),
	@Target nvarchar(100),
	@CreatedBy bigint
    )
AS
DECLARE @AssignedFromId bigint, @AssignedToId bigint, @TargetId bigint, @MonthId bigint,@OilTypeId bigint,@FinancialYearId bigint

SET @AssignedFrom = ltrim(rtrim(@AssignedFrom))
SET @AssignedTo = ltrim(rtrim(@AssignedTo))
SET @Quarter = ltrim(rtrim(@Quarter))
SET @Month = ltrim(rtrim(@Month))
SET @Year = ltrim(rtrim(@Year))
SET @OilTypeName = ltrim(rtrim(@OilTypeName))
SET @FinancialYear = ltrim(rtrim(@FinancialYear))

Set NOCOUNT OFF

BEGIN TRANSACTION

--AssignedFrom begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Name]= @AssignedFrom)
BEGIN
	ROLLBACK
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed, AssignedFrom User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedFromId = Id From [Users] Where [Name]= @AssignedFrom

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed In AssignedFrom' as 'Message'      
RETURN 
END 
--AssignedFrom Ends


--AssignedTo begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Name]= @AssignedTo)
BEGIN
	ROLLBACK
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed, AssignedTo User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedToId = Id From [Users] Where [Name]= @AssignedTo

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed In AssignedTo' as 'Message'      
RETURN 
END 
--AssignedTo Ends

--Month begins
IF NOT EXISTS(Select 1 FROM [Months] Where [Name]= @Month)
BEGIN
	ROLLBACK
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed, Month Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @MonthId = Id From [Months] Where [Name] = @Month

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed In Month' as 'Message'      
RETURN 
END 
--Month Ends

--FinancialYear begins
IF NOT EXISTS(Select 1 FROM [dbo].[FinancialYears] Where [Year]= @FinancialYear)
BEGIN
	ROLLBACK
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed, FinancialYear Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @FinancialYearId = Id From [FinancialYears] Where [Year]= @FinancialYear

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed In FinancialYear' as 'Message'      
RETURN 
END 
--FinancialYear Ends

--OilType begins
IF NOT EXISTS(Select 1 FROM [OilTypes] Where [Name]= @OilTypeName)
BEGIN
		ROLLBACK
		SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed, OilType Not Exist' as 'Message'      
		RETURN
END
ELSE
		Select @OilTypeId = Id From [OilTypes] Where [Name] = @OilTypeName

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Failed In OilType' as 'Message'      
RETURN 
END 
--OilType Ends

IF NOT EXISTS(Select 1 FROM [UserOilTypeTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId)
BEGIN
	INSERT INTO [dbo].[UserOilTypeTargets]([AssignedFromId],[AssignedToId],[Quarter],[MonthId],[Year],[FinancialYearId],[OilTypeId],[Target],[CreatedBy],[CreatedDate])
           VALUES (@AssignedFromId,@AssignedToId,@Quarter,@MonthId,@Year,@FinancialYearId,@OilTypeId,@Target,@CreatedBy,getdate());

	SELECT @TargetId = Id From [UserOilTypeTargets]  Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
	
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Success' as 'Message' FROM [UserOilTypeTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
END
ELSE
BEGIN
	UPDATE [dbo].[UserOilTypeTargets]
	   SET [Quarter] = @Quarter,[MonthId] = @MonthId,[Year]= @Year,[FinancialYearId]= @FinancialYearId,[OilTypeId] = @OilTypeId,[Target] = @Target,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate()
	   WHERE [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId	
	
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@OilTypeName as OilTypeName,@Target as 'Target', 'Record Updated' as 'Message' FROM [UserOilTypeTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
END
COMMIT
GO


