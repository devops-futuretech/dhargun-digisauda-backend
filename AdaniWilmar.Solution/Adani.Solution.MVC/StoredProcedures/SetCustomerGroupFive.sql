/****** Object:  StoredProcedure [dbo].[SetCustomerGroupFive]    Script Date: 18-07-2022 11:54:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SetCustomerGroupFive](
	@LoginUserId bigint,
	@CustomerGroupName varchar(100),
    @CustomerGroupCode varchar(100),
	@IsActive bit
    )
	as
set @CustomerGroupName = ltrim(rtrim(@CustomerGroupName))
set @CustomerGroupCode = ltrim(rtrim(@CustomerGroupCode))
set @LoginUserId = ltrim(rtrim(@LoginUserId))
set @IsActive = ltrim(rtrim(@IsActive))

Set NOCOUNT OFF

BEGIN TRANSACTION

IF NOT EXISTS(Select 1 FROM CustomerGroupFives Where [GroupName] = @CustomerGroupName and [GroupCode] = @CustomerGroupCode and [IsActive] = 1)
BEGIN
    INSERT INTO CustomerGroupFives(GroupName,GroupCode,CreatedBy,IsActive,CreatedDate) VALUES (@CustomerGroupName, @CustomerGroupCode, @LoginUserId, @IsActive, getdate());
	SELECT @CustomerGroupName as CustomerGroupName, @CustomerGroupCode as CustomerGroupCode,@IsActive as IsActive, 'Success' as 'Message' 
END
ELSE
	SELECT @CustomerGroupName as CustomerGroupName, @CustomerGroupCode as CustomerGroupCode,@IsActive as IsActive, 'Record Exists' as 'Message'

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @CustomerGroupName as CustomerGroupName, @CustomerGroupCode as CustomerGroupCode,@IsActive as IsActive, 'Failed in CustomerGroupFive' as 'Message'     
RETURN 
END 
COMMIT
