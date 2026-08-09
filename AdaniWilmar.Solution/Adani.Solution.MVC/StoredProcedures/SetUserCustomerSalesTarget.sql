USE [AdaniTryed]
GO

/****** Object:  StoredProcedure [dbo].[SetUserCustomerSalesTarget]    Script Date: 10-06-2022 12:01:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SetUserCustomerSalesTarget](
	@AssignedFromUserCode nvarchar(100),
	@AssignedToUserCode nvarchar(100),
	@Quarter nvarchar(100),
	@Month nvarchar(100),
	@FinancialYear nvarchar(100),
	@Year nvarchar(100),
	@DivisionCode nvarchar(100),
	@SalesOrganizationCode char(100),
	@DistributionChannelCode char(100),
	@OilTypeName nvarchar(100),
	@Target decimal(18,4),
	@CreatedBy bigint
    )
AS
DECLARE @AssignedFromId bigint, @AssignedToId bigint, @TargetId bigint, @MonthId bigint,@FinancialYearId bigint,@DivisionId bigint, @OilTypeId bigint,@UserCustomerSalesTargetId bigint
DECLARE @SalesOrganizationId bigint
DECLARE @DistributionChannelId bigint
SET @AssignedFromUserCode = ltrim(rtrim(@AssignedFromUserCode))
SET @AssignedToUserCode = ltrim(rtrim(@AssignedToUserCode))
SET @Quarter = ltrim(rtrim(@Quarter))
SET @Month = ltrim(rtrim(@Month))
SET @FinancialYear = ltrim(rtrim(@FinancialYear))

Set NOCOUNT OFF

BEGIN TRANSACTION

--Division begins
IF NOT EXISTS(Select 1 FROM Divisions Where [Code]= @DivisionCode)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed, Division Not Exist' as 'Message'      
	RETURN
END
ELSE
		Select @DivisionId = Id From Divisions Where [Code] = @DivisionCode

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In Division' as 'Message'      
	RETURN 
END 
--Division Ends


--SalesOrganization begins

IF NOT EXISTS(Select 1 FROM SalesOrganizations Where [SAPCode]= @SalesOrganizationCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed, SalesOrganization Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @SalesOrganizationId = ID From SalesOrganizations Where [SAPCode] = @SalesOrganizationCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In SalesOrganization' as 'Message'      
RETURN 
END 
--SalesOrganization Ends

--DistributionChannel begins

IF NOT EXISTS(Select 1 FROM DistributionChannels Where [SAPCode]= @DistributionChannelCode)
BEGIN
	ROLLBACK
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed, DistributionChannel Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @DistributionChannelId = ID From DistributionChannels Where [SAPCode] = @DistributionChannelCode

IF @@ERROR <> 0 
BEGIN     
ROLLBACK  
	SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In DistributionChannel' as 'Message'      
RETURN 
END 
--DistributionChannel Ends


--OilType begins
IF NOT EXISTS(Select 1 FROM [OilTypes] Where [Name]= @OilTypeName and [DivisionId]=@DivisionId)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed, OilType Not Exist' as 'Message'      
	RETURN
END
ELSE
		Select @OilTypeId = Id From [OilTypes] Where [Name] = @OilTypeName and [DivisionId]=@DivisionId

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In OilType' as 'Message'      
	RETURN 
END 
--OilType Ends


--AssignedFrom begins
IF NOT EXISTS(Select 1 FROM [Users] Where [Code]= @AssignedFromUserCode and [DivisionId]=@DivisionId and [IsActive] =  1)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed, AssignedFrom User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedFromId = Id From [Users] Where [Code]= @AssignedFromUserCode and [DivisionId]=@DivisionId and [IsActive] =  1

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In AssignedFrom' as 'Message'      
	RETURN 
END 
--AssignedFrom Ends


--AssignedTo begins
IF NOT EXISTS(Select 1 FROM [Users] as u join UserRoles as ur on u.Id = ur.UserId Where u.Code = @AssignedToUserCode and u.DivisionId =@DivisionId and u.IsActive =  1 and ur.RoleId in (5,7,9,12)) -- Assigned to can be dealer ,bdo,zonal head, national head
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed, AssignedTo User Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @AssignedToId = u.Id FROM [Users] as u join UserRoles as ur on u.Id = ur.UserId Where u.Code = @AssignedToUserCode and u.DivisionId =@DivisionId and u.IsActive =  1 and ur.RoleId in (5,7,9,12) -- Assigned to can be dealer ,bdo,zonal head, national head

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In AssignedTo' as 'Message'      
	RETURN 
END 
--AssignedTo Ends

--Month begins
IF NOT EXISTS(Select 1 FROM [Months] Where [Name]= @Month)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed, Month Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @MonthId = Id From [Months] Where [Name] = @Month

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In Month' as 'Message'      
	RETURN 
END 
--Month Ends

--FinancialYear begins
IF NOT EXISTS(Select 1 FROM [dbo].[FinancialYears] Where [Year]= @FinancialYear)
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed, FinancialYear Not Exist' as 'Message'      
	RETURN
END
ELSE
	Select @FinancialYearId = Id From [FinancialYears] Where [Year]= @FinancialYear

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In FinancialYear' as 'Message'      
	RETURN 
END 
--FinancialYear Ends

--Target begins
IF @Target is null OR @Target<=0
BEGIN
	ROLLBACK
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed, Target is invalid' as 'Message'      
	RETURN
END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  	 
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In Target' as 'Message'    
	RETURN 
END 
--Target Ends


IF NOT EXISTS(Select 1 FROM [UserCustomerSalesTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId and [DivisionId]=@DivisionId and [OilTypeId]=@OilTypeId and [MonthId]=@MonthId and [FinancialYearId]=@FinancialYearId and [Year]=@Year)
BEGIN
	DECLARE @NHRoleTypeId bigint,@AdminRoleTypeId bigint,@BFAdminRoleTypeId bigint,@AssignedFromRoleTypeId bigint,@HoSalesAdminRoleTypeId bigint
	 SELECT @NHRoleTypeId=Id FROM RoleTypes WHERE [Name]='National Head'  
	 SELECT @AdminRoleTypeId=Id FROM RoleTypes WHERE [Name]='Admin'  
	 SELECT @BFAdminRoleTypeId=Id FROM RoleTypes WHERE [Name]='Business Finance Admin' 
	 SELECT @HoSalesAdminRoleTypeId=Id FROM RoleTypes WHERE [Name]='HO Sales Admin'  

	SELECT @AssignedFromRoleTypeId=rt.Id FROM Roles r 
	JOIN UserRoles ur on r.Id=ur.RoleId 
	JOIN RoleTypes rt on rt.Id = r.RoleTypeId
	WHERE ur.UserId=@AssignedFromId 

	IF @AssignedFromRoleTypeId=@NHRoleTypeId OR @AssignedFromRoleTypeId=@AdminRoleTypeId OR @AssignedFromRoleTypeId=@BFAdminRoleTypeId OR  @AssignedFromRoleTypeId=@HoSalesAdminRoleTypeId
	BEGIN
		INSERT INTO [dbo].[UserCustomerSalesTargets]([AssignedFromId],[AssignedToId],[Quarter],[MonthId],[FinancialYearId],[Year],[DivisionId],[OilTypeId],[Target],[CreatedBy],[CreatedDate])
           VALUES (@AssignedFromId,@AssignedToId,@Quarter,@MonthId,@FinancialYearId,@Year,@DivisionId,@OilTypeId,@Target,@CreatedBy,getdate());

		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Success' as 'Message' 
	END
	ELSE
		BEGIN
			IF EXISTS (SELECT Top 1 1 FROM [UserCustomerSalesTargets] WHERE [AssignedToId]=@AssignedToId and [FinancialYearId]=@FinancialYearId and [DivisionId]=@DivisionId and OilTypeId = @OilTypeId
			GROUP BY [AssignedToId],[FinancialYearId],[OilTypeId])
			BEGIN
				INSERT INTO [dbo].[UserCustomerSalesTargets]([AssignedFromId],[AssignedToId],[Quarter],[MonthId],[FinancialYearId],[Year],[DivisionId],[OilTypeId],[Target],[CreatedBy],[CreatedDate])
				  VALUES (@AssignedFromId,@AssignedToId,@Quarter,@MonthId,@FinancialYearId,@Year,@DivisionId,@OilTypeId,@Target,@CreatedBy,getdate());

				SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Success' as 'Message' 
			END
			ELSE
			BEGIN     
				ROLLBACK  
					SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Oiltype is not assigned' as 'Message' 
				RETURN 
			END 
		END
END
ELSE

	SELECT @UserCustomerSalesTargetId = Id FROM [UserCustomerSalesTargets] 
		Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId and [DivisionId]=@DivisionId and [OilTypeId]=@OilTypeId and [MonthId]=@MonthId and [FinancialYearId]=@FinancialYearId and [Year]=@Year
		
		IF (@UserCustomerSalesTargetId>0)
		BEGIN
			UPDATE [dbo].[UserCustomerSalesTargets]
			 SET [Target] =@Target,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate()
			Where Id=@UserCustomerSalesTargetId
		END
		ELSE
		BEGIN
			SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed in UserCustomerSalesTarget' as 'Message'  FROM [UserCustomerSalesTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId and [DivisionId]=@DivisionId and [OilTypeId]=@OilTypeId and [MonthId]=@MonthId and [FinancialYearId]=@FinancialYearId and [Year]=@Year
		END
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Record Updated' as 'Message'  FROM [UserCustomerSalesTargets] Where [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId and [DivisionId]=@DivisionId and [OilTypeId]=@OilTypeId and [MonthId]=@MonthId and [FinancialYearId]=@FinancialYearId and [Year]=@Year
	
--BEGIN
--	UPDATE [dbo].[UserCustomerSalesTargets]
--	   SET [Quarter] = @Quarter,[MonthId] = @MonthId,[FinancialYearId]= @FinancialYearId,[Year]= @Year,[DivisionId] = @DivisionId,[OilTypeId] = @OilTypeId,[Target] = @Target,[ModifiedBy] = @CreatedBy,[ModifiedDate] = getdate()
--	   WHERE [AssignedFromId]= @AssignedFromId and [AssignedToId]=@AssignedToId and [DivisionId]=@DivisionId and [OilTypeId]=@OilTypeId and [MonthId]=@MonthId and [FinancialYearId]=@FinancialYearId	
	
--	SELECT @AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Record Updated' as 'Message' 
--END

IF @@ERROR <> 0 
BEGIN     
	ROLLBACK  
		SELECT @SalesOrganizationCode as SalesOrganizationCode,@DistributionChannelCode as DistributionChannelCode,@AssignedFromUserCode as AssignedFromUserCode,@AssignedToUserCode as AssignedToUserCode,@Month as 'Month',@FinancialYear as FinancialYear,@Quarter as 'Quarter',@DivisionCode as DivisionCode,@OilTypeName as OilTypeName,@Year as 'Year',@Target as 'Target', 'Failed In FinancialYear' as 'Message'      
	RETURN 
END 

COMMIT
GO

