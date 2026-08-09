ALTER PROCEDURE [dbo].[SetUserCustomerMapping] (
	@UserCode CHAR(100),
	@CustomerCode VARCHAR(100),
	@CreatedBy BIGINT,
	@IsDeleteOldMapping bit,
	@IsUnassign bit
)
AS
DECLARE @CustomerID INT
	,@BdoRoleId BIGINT
	,@BdoId BIGINT

SET @UserCode = ltrim(rtrim(@UserCode))
SET @CustomerCode = ltrim(rtrim(@CustomerCode))

SELECT @BdoRoleId = Id
FROM Roles
WHERE [Name] = 'State Trader'

SET NOCOUNT OFF

BEGIN TRANSACTION

--Distributor begins
IF NOT EXISTS (
		SELECT 1
		FROM Users u
		JOIN UserDivisionMappings ud ON u.Id = ud.UserId
		WHERE u.[Code] = @CustomerCode
		)
BEGIN
	ROLLBACK

	SELECT @CustomerCode AS CustomerCode
		,@UserCode AS UserCode
		,'Distributor Not Exist' AS 'Message'

	RETURN
END
ELSE
	SELECT @CustomerID = u.ID
	FROM Users u
	JOIN UserDivisionMappings ud ON u.Id = ud.UserId
	WHERE u.[Code] = @CustomerCode

IF @@ERROR <> 0
BEGIN
	ROLLBACK

	SELECT @CustomerCode AS CustomerCode
		,@UserCode AS UserCode
		,'Failed In Distributor' AS 'Message'

	RETURN
END

--Distributor Ends
DECLARE @IsDivisionMapExists BIGINT
	,@ReportCode VARCHAR(max)

SELECT @IsDivisionMapExists = Count(*)
FROM UserDivisionMappings
WHERE UserId = @CustomerID

SELECT @ReportCode = @UserCode

DECLARE @ErrorMsg VARCHAR(max) = ''

--UserCustomerMapping begins- UserCode
IF @IsDivisionMapExists > 0
BEGIN
	CREATE TABLE #UserDivisions (
		SalesOrganizationId BIGINT
		,DistributionChannelId BIGINT
		,DivisionId BIGINT
		)

	INSERT INTO #UserDivisions
	SELECT ud.SalesOrganizationId
		,ud.DistributionChannelId
		,ud.DivisionId
	FROM UserDivisionMappings ud
	WHERE UserId = @CustomerID

	DECLARE @ItemUserCodeUpdate VARCHAR(max)
	DECLARE @positionUserCodeForUpdate INT
	DECLARE @LoopUserCodeForUpdate BIT

	--Make sure we enter the loop, even if there's only one item
	IF (right(@ReportCode, 1) <> ',' AND Len(@ReportCode) > 0
			)
	BEGIN
		SET @ReportCode = @ReportCode + ','
	END

	SET @LoopUserCodeForUpdate = CASE 
			WHEN LEN(@ReportCode) > 0
				THEN 1
			ELSE 0
			END

	IF (
			SELECT @LoopUserCodeForUpdate
			) = 1 and @IsDeleteOldMapping=1
	BEGIN
		DELETE ucm
FROM UserCustomerMappings ucm
INNER JOIN UserRoles ur ON ucm.UserId = ur.UserId
WHERE ur.RoleId=7 and ucm.CustomerId=@CustomerID; 
		--DELETE
		--FROM UserCustomerMappings ucm
		--WHERE CustomerId = @CustomerID
	END

	WHILE (
			SELECT @LoopUserCodeForUpdate
			) = 1
	BEGIN
		SELECT @positionUserCodeForUpdate = CHARINDEX(',', @ReportCode, 1)

		IF (@positionUserCodeForUpdate > 0)
		BEGIN
			SELECT @ItemUserCodeUpdate = SUBSTRING(@ReportCode, 1, @positionUserCodeForUpdate - 1)

			SELECT @ReportCode = SUBSTRING(@ReportCode, @positionUserCodeForUpdate + 1, LEN(@ReportCode) - @positionUserCodeForUpdate)
			
			SELECT @BdoId = u.Id
			FROM Users u
			JOIN UserRoles r ON u.Id = r.UserId
			WHERE u.[Code] = @ItemUserCodeUpdate
				AND r.RoleId = @BDORoleId


			IF (@BdoId <> 0)
			BEGIN
			  
			  IF(@IsUnassign <> 1)
			   BEGIN
				IF NOT EXISTS (SELECT 1 FROM UserCustomerMappings WHERE UserId = @BdoId AND CustomerId = @CustomerID)
				BEGIN
					
					CREATE TABLE #ReportUserDivisions (
						SalesOrganizationId BIGINT
						,DistributionChannelId BIGINT
						,DivisionId BIGINT
						)

					INSERT INTO #ReportUserDivisions
					SELECT ud.SalesOrganizationId
						,ud.DistributionChannelId
						,ud.DivisionId
					FROM UserDivisionMappings ud
					WHERE UserId = @BDOId

					DECLARE @divCountMatch BIGINT = 0

					SELECT @divCountMatch = Count(*)
					FROM #UserDivisions ud
					JOIN #ReportUserDivisions rd ON ud.SalesOrganizationId = rd.SalesOrganizationId
						AND ud.DistributionChannelId = rd.DistributionChannelId
						AND ud.DivisionId = rd.DivisionId

					IF (@divCountMatch > 0)
					BEGIN
						INSERT INTO UserCustomerMappings (
							UserId
							,CustomerId
							,CreatedBy
							,CreatedDate
							)
						VALUES (
							@BdoId
							,@CustomerID
							,@CreatedBy
							,getdate()
							);
					END
					ELSE
					BEGIN
						SELECT @ErrorMsg = @ErrorMsg + @ItemUserCodeUpdate+','
					END

					DROP TABLE #ReportUserDivisions

						--INSERT INTO UserCustomerMappings(UserId,CustomerId,CreatedBy,CreatedDate) VALUES (@BDOId,@UserId, @CreatedBy, getdate());
				END
				Select @BdoId=0
			   END
			   ELSE
			    BEGIN
				  IF EXISTS (SELECT 1 FROM UserCustomerMappings WHERE UserId = @BdoId AND CustomerId = @CustomerID)
				   BEGIN
				      DELETE FROM UserCustomerMappings WHERE UserId = @BdoId AND CustomerId = @CustomerID
				   END
				  ELSE
				   BEGIN
				   	  SELECT @ErrorMsg = @ErrorMsg + ltrim(rtrim(@ItemUserCodeUpdate))+',';
				   END

				END	
			END
			ELSE
			BEGIN
				SELECT @ErrorMsg = @ErrorMsg + ltrim(rtrim(@ItemUserCodeUpdate))+','
			END
		END
		ELSE
		BEGIN
			SELECT @ItemUserCodeUpdate = @ReportCode
			SELECT @LoopUserCodeForUpdate = 0
		END
	END

	IF @ErrorMsg <> ''
	BEGIN

	 IF(@IsUnassign <> 1)
	  BEGIN
		SELECT @CustomerCode AS CustomerCode, @UserCode AS UserCode,'Failed, ' + @ErrorMsg + ' Combination not mapped matched for this user or User Not Found' AS 'Message'
	  END
	  ELSE
	   BEGIN

	      IF RIGHT(@ErrorMsg, 1) = ','
			BEGIN
				SET @ErrorMsg = LEFT(@ErrorMsg, LEN(@ErrorMsg) - 1)
			END
	       
	   		SELECT @CustomerCode AS CustomerCode, @UserCode AS UserCode,'Failed, Combination not mapped for this '  + @ErrorMsg + ' user' AS 'Message'
	   END

	END
	ELSE
	BEGIN
		SELECT @CustomerCode AS CustomerCode
			,@UserCode AS UserCode
			,'Successfully Updated' AS 'Message'
	END

	DROP TABLE #UserDivisions
END
ELSE
BEGIN
	SELECT @CustomerCode AS CustomerCode
		,@UserCode AS UserCode
		,'User Division mapping Not exists for Current User' AS 'Message'
END

--UserCustomerMapping begins
--IF NOT EXISTS(Select 1 FROM UserCustomerMappings d Where d.UserId = @UserID and d.CustomerId = @CustomerID)
--BEGIN
--		INSERT INTO UserCustomerMappings(UserId,CustomerId,CreatedBy,CreatedDate) VALUES (@UserID,@CustomerID, 1, getdate());
--		SELECT @CustomerCode as CustomerCode,@UserCode as UserCode, 'Success' as 'Message' 
--END
--ELSE
--		SELECT @CustomerCode as CustomerCode,@UserCode as UserCode, 'Record Exists' as 'Message' 
--IF @@ERROR <> 0 
--BEGIN     
--ROLLBACK  
--	SELECT @CustomerCode as CustomerCode,@UserCode as UserCode, 'Failed In UserDepotMappings' as 'Message'     
--RETURN 
--END 
--UserCustomerMapping Ends
COMMIT
