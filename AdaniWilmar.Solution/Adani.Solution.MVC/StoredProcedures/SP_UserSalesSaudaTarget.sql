IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SetUserSalesSaudaTarget')
    BEGIN
        DROP  Procedure SetUserSalesSaudaTarget
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SetUserSalesSaudaTarget](
	@AssignedFrom nvarchar(100),
	@AssignedTo nvarchar(100),
	@Quarter nvarchar(100),
	@Month nvarchar(100),
	@Year nvarchar(100),
	@SaudaTarget decimal,
	@SalesTarget decimal,
	@CreatedBy bigint
    )
AS
DECLARE @AssignedFromId bigint, @AssignedToId bigint, @TargetId bigint, @MonthId bigint

SET @AssignedFrom = ltrim(rtrim(@AssignedFrom))
SET @AssignedTo = ltrim(rtrim(@AssignedTo))
SET @Quarter = ltrim(rtrim(@Quarter))
SET @Month = ltrim(rtrim(@Month))
SET @Year = ltrim(rtrim(@Year))

Set NOCOUNT OFF

BEGIN TRANSACTION

--AssignedFrom begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Name]= @AssignedFrom)
BEGIN
	ROLLBACK
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@Quarter as 'Quarter',@SalesTarget as 'SalesTarget',@SaudaTarget as 'SaudaTarget', 'Failed, AssignedFrom User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedFromId = Id From [Users] Where [Name]= @AssignedFrom

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@Quarter as 'Quarter',@SalesTarget as 'SalesTarget',@SaudaTarget as 'SaudaTarget', 'Failed In AssignedFrom' as 'Message'      
RETURN 
END 
--AssignedFrom Ends


--AssignedTo begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Name]= @AssignedTo)
BEGIN
	ROLLBACK
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@Quarter as 'Quarter',@SalesTarget as 'SalesTarget',@SaudaTarget as 'SaudaTarget', 'Failed, AssignedTo User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedToId = Id From [Users] Where [Name]= @AssignedTo

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@Quarter as 'Quarter',@SalesTarget as 'SalesTarget',@SaudaTarget as 'SaudaTarget', 'Failed In AssignedTo' as 'Message'      
RETURN 
END 
--AssignedTo Ends

--Month begins
IF NOT EXISTS(Select 1 FROM [Months] Where [Name]= @Month)
BEGIN
	ROLLBACK
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@Quarter as 'Quarter',@SalesTarget as 'SalesTarget',@SaudaTarget as 'SaudaTarget', 'Failed, Month Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @MonthId = Id From [Months] Where [Name] = @Month

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@Quarter as 'Quarter',@SalesTarget as 'SalesTarget',@SaudaTarget as 'SaudaTarget', 'Failed In Month' as 'Message'      
RETURN 
END 
--Month Ends

IF NOT EXISTS(Select 1 FROM [UserSalesSaudaTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId)
BEGIN
	INSERT INTO [dbo].[UserSalesSaudaTargets]([AssignedFromId],[AssignedToId],[Quarter],[Month],[Year],[SaudaTarget],[SalesTarget],[CreatedBy],[CreatedDate])
           VALUES (@AssignedFromId,@AssignedToId,@Quarter,@MonthId,@Year,@SaudaTarget,@SalesTarget,@CreatedBy,getdate());

	SELECT @TargetId = Id From [UserSalesSaudaTargets]  Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
	
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@Quarter as 'Quarter',@SalesTarget as 'SalesTarget',@SaudaTarget as 'SaudaTarget', 'Success' as 'Message' FROM [UserSalesSaudaTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
END
ELSE
BEGIN
	UPDATE [dbo].[UserSalesSaudaTargets]
	   SET [Quarter] = @Quarter,[Month] = @MonthId,[Year] =@Year ,[SaudaTarget] = @SaudaTarget,[SalesTarget]=@SalesTarget,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate()
	   WHERE [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId	
	
	SELECT @AssignedFrom as AssignedFrom,@AssignedTo as AssignedTo,@Month as 'Month',@Quarter as 'Quarter',@SalesTarget as 'SalesTarget',@SaudaTarget as 'SaudaTarget', 'Record Updated' as 'Message' FROM [UserSalesSaudaTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId
END
COMMIT
GO


