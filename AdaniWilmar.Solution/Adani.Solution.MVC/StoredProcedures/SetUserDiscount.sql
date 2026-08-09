
/****** Object:  StoredProcedure [dbo].[SetUserDiscount]    Script Date: 17-04-2023 19:18:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[SetUserDiscount] (@LoginUserId bigint,
@SalesOrganization varchar(max),
@DistributionChannel varchar(max),
@Division varchar(max),
@Discount decimal(18, 3),
@DiscountReason varchar(max),
@MaterialCode varchar(max),
@EmployeeCode varchar(max),
@ValidFrom datetime,
@ValidTo datetime,
@StateName varchar(max))
AS
  DECLARE @SalesOrganizationId bigint,
          @DistributionChannelId bigint,
          @DivisionId bigint,
          @RoleId bigint,
          @Item varchar(max),
          @position int,
          @Loop bit,
          @UserId bigint,
          @ReportingToUserId bigint,
          @SkuItem varchar(max),
          @Skuposition int,
          @SkuLoop bit,
          @SkuId bigint,
          @StateLoop bit,
          @StateId bigint,
          @StateItem varchar(max),
          @Stateposition int

  SET NOCOUNT OFF
  BEGIN TRANSACTION

    --Role Begins

    IF NOT EXISTS (SELECT
        1
      FROM UserRoles
      WHERE [UserId] = @LoginUserId)
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed, UserRole Not Exist' AS 'Message'
      RETURN
    END
    ELSE
      SELECT
        @RoleId = RoleId
      FROM UserRoles
      WHERE [UserId] = @LoginUserId

    IF @@ERROR <> 0
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed in UserRoles' AS 'Message'
      RETURN
    END

    --Role Ends

    --SalesOrganization begins

    IF NOT EXISTS (SELECT
        1
      FROM SalesOrganizations
      WHERE [Code] = @SalesOrganization)
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed, SalesOrganization Not Exist' AS 'Message'
      RETURN
    END
    ELSE
      SELECT
        @SalesOrganizationId = ID
      FROM SalesOrganizations
      WHERE [Code] = @SalesOrganization

    IF @@ERROR <> 0
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed In SalesOrganization' AS 'Message'
      RETURN
    END
    --SalesOrganization Ends


    --DistributionChannel begins

    IF NOT EXISTS (SELECT
        1
      FROM DistributionChannels
      WHERE [Code] = @DistributionChannel
      AND SalesOrganizationId = @SalesOrganizationId)
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed, Distribution Channel Not Exist' AS 'Message'
      RETURN
    END
    ELSE
      SELECT
        @DistributionChannelId = ID
      FROM DistributionChannels
      WHERE [Code] = @DistributionChannel
      AND SalesOrganizationId = @SalesOrganizationId

    IF @@ERROR <> 0
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed In Distribution Channel' AS 'Message'
      RETURN
    END
    --DistributionChannel Ends

    --Division begins

    IF NOT EXISTS (SELECT
        1
      FROM Divisions
      WHERE [Code] = @Division
      AND SalesOrganizationId = @SalesOrganizationId
      AND DistributionChannelId = @DistributionChannelId)
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed, Division Not Exist' AS 'Message'
      RETURN
    END
    ELSE
      SELECT
        @DivisionId = ID
      FROM Divisions
      WHERE [Code] = @Division
      AND SalesOrganizationId = @SalesOrganizationId
      AND DistributionChannelId = @DistributionChannelId

    IF @@ERROR <> 0
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed In Division' AS 'Message'
      RETURN
    END
    --Division Ends

    IF @Discount = 0
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed, Discount amount is zero' AS 'Message'
      RETURN
    END


    IF NOT EXISTS (SELECT
        1
      FROM UserDivisionMappings
      WHERE UserId = @LoginUserId
      AND SalesOrganizationId = @SalesOrganizationId
      AND DistributionChannelId = @DistributionChannelId
      AND DivisionId = @DivisionId)
    BEGIN
      ROLLBACK
      SELECT
        @SalesOrganization AS SalesOrganization,
        @DistributionChannel AS DistributionChannel,
        @Division AS Division,
        @Discount AS Discount,
        @DiscountReason AS DiscountReason,
        @MaterialCode AS MaterialCode,
        @EmployeeCode AS EmployeeCode,
        @ValidFrom AS ValidFrom,
        @ValidTo AS ValidTo,
		@StateName as StateName,
        'Failed,  Sales org , Distribution Channel , Division Combination Not Exist for LoginUser' AS 'Message'
      RETURN
    END


    DECLARE @ErrorMsg varchar(max) = ''
    DECLARE @IsFirstRecord bit = 0
    DECLARE @ParentId bigint = 0
    DECLARE @DiscountId bigint
    DECLARE @ActualDiscount decimal(18, 3)
    DECLARE @ExistingValidFrom datetime
    DECLARE @ExistingValdTo datetime
    DECLARE @OilTypeId bigint
    DECLARE @MaterialCodeForLoop varchar(max) = ''

    DECLARE @StateForLoop varchar(max) = ''
    --Customer Starts   
    DECLARE @ErrorMsgSku varchar(max) = ''
    DECLARE @ErrorMsgState varchar(max) = ''
    DECLARE @ErrorMsgUserState varchar(max) = ''
    DECLARE @ErrorMsgUserDivision varchar(max) = ''

    IF (@EmployeeCode IS NOT NULL
      AND @EmployeeCode != '')
    BEGIN

      IF (RIGHT(@EmployeeCode, 1) <> ','
        AND LEN(@EmployeeCode) > 0)
      BEGIN
        SET @EmployeeCode = @EmployeeCode + ','
      END
      --Customer Loop Begins
      SET @Loop =
                   CASE
                     WHEN LEN(@EmployeeCode) > 0 THEN 1
                     ELSE 0
                   END
      WHILE (SELECT
          @Loop)
        = 1
      BEGIN
        SET @ReportingToUserId = 0
        SELECT
          @Position = CHARINDEX(',', @EmployeeCode, 1)

        IF (@Position > 0)
        BEGIN
          SELECT
            @Item = SUBSTRING(@EmployeeCode, 1, @Position - 1)
          SELECT
            @EmployeeCode = SUBSTRING(@EmployeeCode, @Position + 1, LEN(@EmployeeCode) - @Position)

          --Reporting To Check Begins
          IF @RoleId = 12
            OR @RoleId = 9
          BEGIN
            SELECT
              @UserId = u.Id
            FROM Users u
            WHERE u.Code = @Item
            SELECT
              @ReportingToUserId = ReportingToUserId
            FROM UserReportingToMappings AS ur
            WHERE ur.UserId = @UserId
            AND ur.ReportingToUserId = @LoginUserId
          END
          ELSE
          IF @RoleId = 7
          BEGIN
            SELECT
              @UserId = u.Id
            FROM Users u
            WHERE u.Code = @Item
            SELECT
              @ReportingToUserId = UserId
            FROM UserCustomerMappings AS ur
            WHERE ur.CustomerId = @UserId
            AND ur.UserId = @LoginUserId
          END
          --Reporting To Check Ends
          IF (@ReportingToUserId <> 0)
          BEGIN
            --Material Starts 

            IF (@MaterialCode IS NOT NULL
              AND @MaterialCode != '')
            BEGIN

              --Material Loop Starts
              SET @MaterialCodeForLoop = @MaterialCode
              IF (RIGHT(@MaterialCodeForLoop, 1) <> ','
                AND LEN(@MaterialCodeForLoop) > 0)
              BEGIN
                SET @MaterialCodeForLoop = @MaterialCodeForLoop + ','
              END

              SET @SkuLoop =
                              CASE
                                WHEN LEN(@MaterialCodeForLoop) > 0 THEN 1
                                ELSE 0
                              END
              WHILE (SELECT
                  @SkuLoop)
                = 1
              BEGIN
                SET @SkuId = 0
                SELECT
                  @Skuposition = CHARINDEX(',', @MaterialCodeForLoop, 1)

                IF (@Skuposition > 0)
                BEGIN
                  SELECT
                    @SkuItem = SUBSTRING(@MaterialCodeForLoop, 1, @Skuposition - 1)
                  SELECT
                    @MaterialCodeForLoop = SUBSTRING(@MaterialCodeForLoop, @Skuposition + 1, LEN(@MaterialCodeForLoop) - @Skuposition)

                  SELECT
                    @SkuId = s.Id
                  FROM Skus AS s
                  WHERE s.SkuCode = @SkuItem
                  AND s.SalesOrganizationId = @SalesOrganizationId
                  AND s.DistributionChannelId = @DistributionChannelId
                  AND s.DivisionId = @DivisionId
                  AND s.IsActive = 1

                  IF (@SkuId > 0)
                  BEGIN

                    IF((@StateName IS NOT NULL
                      AND @StateName != '' AND @RoleId in (12,9,7))) -- NT , ZT , ST are allowed
					  --OR (@RoleId=7 AND (@StateName IS NULL OR @StateName != '' )))
                    BEGIN
                      --State Loop Starts
                      SET @StateForLoop = @StateName
                      IF (RIGHT(@StateForLoop, 1) <> ','
                        AND LEN(@StateForLoop) > 0)
                      BEGIN
                        SET @StateForLoop = @StateForLoop + ','
                      END

                      SET @StateLoop =
                                        CASE
                                          WHEN LEN(@StateForLoop) > 0 THEN 1
                                          ELSE 0
                                        END
                      WHILE (SELECT
                          @StateLoop)
                        = 1
                      BEGIN
                        SET @StateId = 0
                        SELECT
                          @Stateposition = CHARINDEX(',', @StateForLoop, 1)

                        IF (@Stateposition > 0)
                        BEGIN
                          SELECT
                            @StateItem = SUBSTRING(@StateForLoop, 1, @Stateposition - 1)
                          SELECT
                            @StateForLoop = SUBSTRING(@StateForLoop, @Stateposition + 1, LEN(@StateForLoop) - @Stateposition)

                          SELECT
                            @StateId = s.Id
                          FROM States AS s
                          WHERE Lower(s.StateName) = Lower(@StateItem)
                          AND s.IsActive = 1

                          IF (@StateId > 0)
						   --AND @RoleId <> 7)
         --                   OR (@StateId = 0
         --                   AND @RoleId = 7)
                          BEGIN
                            SELECT
                              @OilTypeId = OilTypeId
                            FROM Skus
                            WHERE Id = @SkuId

                            IF @RoleId = 12
                            BEGIN

                              IF (@IsFirstRecord = 0)
                              BEGIN
                                DECLARE @id int
                                INSERT INTO DiscountUsers (UserId, SkuId, SalesOrganizationId, DistributionChannelId, DivisionId, ActualDiscount, DiscountReason, ParentId, Status, SaudaBookingTypeId, ValidFrom, ValidTo, CreatedBy, CreatedDate, OilTypeId, RequestedDiscount, ApprovedBy, ParentDiscountId, StateId)
                                  VALUES (@UserId, @SkuId, @SalesOrganizationId, @DistributionChannelId, @DivisionId, @Discount, @DiscountReason, @ParentId, 1, 1, @ValidFrom, @ValidTo, @LoginUserId, GETDATE(), @OilTypeId, 0, 0, 0, @StateId)
                                SELECT
                                  @id = SCOPE_IDENTITY()
                                SET @IsFirstRecord = 1
                                SET @ParentId = @id
                              END
                              IF (@IsFirstRecord = 1)
                              BEGIN
                                INSERT INTO DiscountUsers (UserId, SkuId, SalesOrganizationId, DistributionChannelId, DivisionId, ActualDiscount, DiscountReason, ParentId, Status, SaudaBookingTypeId, ValidFrom, ValidTo, CreatedBy, CreatedDate, OilTypeId, RequestedDiscount, ApprovedBy, ParentDiscountId, StateId)
                                  VALUES (@UserId, @SkuId, @SalesOrganizationId, @DistributionChannelId, @DivisionId, @Discount, @DiscountReason, @ParentId, 1, 1, @ValidFrom, @ValidTo, @LoginUserId, GETDATE(), @OilTypeId, 0, 0, 0, @StateId)
                              END
                            END
                            ELSE
                            BEGIN

                              IF @RoleId = 9
                              BEGIN

                                --DECLARE @UserStateId int = 0

                                --SELECT
                                --  @UserStateId = StateId
                                --FROM Users
                                --WHERE Id = @UserId
                                --AND StateId = @StateId

                                --IF (@UserStateId <> 0)
                                --BEGIN
                                  SELECT TOP 1
                                    @DiscountId = Id
                                  FROM DiscountUsers
                                  WHERE UserId = @LoginUserId
                                  AND SkuId = @SkuId
								  AND StateId = @StateId
                                  AND ((ValidFrom >= @ValidFrom
                                  AND ValidFrom <= @ValidTo)
                                  OR (ValidTo >= @ValidFrom
                                  AND ValidTo <= @ValidTo))
                                  ORDER BY Id DESC

                                  IF (@DiscountId <> 0)
                                  BEGIN
                                    SELECT
                                      @ActualDiscount = ActualDiscount,
                                      @ExistingValidFrom = ValidFrom,
                                      @ExistingValdTo = ValidTo
                                    FROM DiscountUsers
                                    WHERE Id = @DiscountId
                                    IF (@Discount > @ActualDiscount)
                                    BEGIN
                                      ROLLBACK
                                      SELECT
                                        @SalesOrganization AS SalesOrganization,
                                        @DistributionChannel AS DistributionChannel,
                                        @Division AS Division,
                                        @Discount AS Discount,
                                        @DiscountReason AS DiscountReason,
                                        @MaterialCode AS MaterialCode,
                                        @EmployeeCode AS EmployeeCode,
                                        @ValidFrom AS ValidFrom,
                                        @ValidTo AS ValidTo,
										@StateName as StateName,
                                        'Failed, Discount amount should be less than or equal to amount assigned to you' AS 'Message'
                                      RETURN
                                    END
                                    IF (NOT (@ValidFrom >= @ExistingValidFrom
                                      AND @ValidFrom <= @ExistingValdTo
                                      AND @ValidTo <= @ExistingValdTo
                                      AND @ValidTo >= @ExistingValidFrom))
                                    BEGIN
                                      ROLLBACK
                                      SELECT
                                        @SalesOrganization AS SalesOrganization,
                                        @DistributionChannel AS DistributionChannel,
                                        @Division AS Division,
                                        @Discount AS Discount,
                                        @DiscountReason AS DiscountReason,
                                        @MaterialCode AS MaterialCode,
                                        @EmployeeCode AS EmployeeCode,
                                        @ValidFrom AS ValidFrom,
                                        @ValidTo AS ValidTo,
										@StateName as StateName,
                                        'Failed, Valid From and Valid To date range should be less than or equal to date range assigned to you' AS 'Message'
                                      RETURN
                                    END
                                    DECLARE @DivCount bigint = 0

                                    SELECT
                                      @DivCount = COUNT(*)
                                    FROM UserDivisionMappings ud
                                    JOIN DiscountUsers d
                                      ON ud.SalesOrganizationId = d.SalesOrganizationId
                                      AND ud.DistributionChannelId = d.DistributionChannelId
                                      AND ud.DivisionId = d.DivisionId
                                    WHERE d.Id = @DiscountId
                                    AND ud.UserId = @UserId

                                    IF @DivCount <> 0
                                    BEGIN
                                      IF (@IsFirstRecord = 0)
                                      BEGIN
                                        DECLARE @primarykeyId int
                                        INSERT INTO DiscountUsers (UserId, SkuId, SalesOrganizationId, DistributionChannelId, DivisionId, ActualDiscount, DiscountReason, ParentId, Status, SaudaBookingTypeId, ValidFrom, ValidTo, CreatedBy, CreatedDate, ParentDiscountId, OilTypeId, RequestedDiscount, ApprovedBy, StateId)
                                          VALUES (@UserId, @SkuId, @SalesOrganizationId, @DistributionChannelId, @DivisionId, @Discount, @DiscountReason, @ParentId, 1, 1, @ValidFrom, @ValidTo, @LoginUserId, GETDATE(), @DiscountId, @OilTypeId, 0, 0, @StateId)
                                        SELECT
                                          @primarykeyId = SCOPE_IDENTITY()
                                        SET @IsFirstRecord = 1
                                        SET @ParentId = @primarykeyId
                                      END
                                      IF (@IsFirstRecord = 1)
                                      BEGIN
                                        INSERT INTO DiscountUsers (UserId, SkuId, SalesOrganizationId, DistributionChannelId, DivisionId, ActualDiscount, DiscountReason, ParentId, Status, SaudaBookingTypeId, ValidFrom, ValidTo, CreatedBy, CreatedDate, ParentDiscountId, OilTypeId, RequestedDiscount, ApprovedBy, StateId)
                                          VALUES (@UserId, @SkuId, @SalesOrganizationId, @DistributionChannelId, @DivisionId, @Discount, @DiscountReason, @ParentId, 1, 1, @ValidFrom, @ValidTo, @LoginUserId, GETDATE(), @DiscountId, @OilTypeId, 0, 0, @StateId)
                                      END
                                    END
                                    ELSE
                                    BEGIN
                                      SET @ErrorMsgUserDivision = @ErrorMsgUserDivision + @Item
                                    END

                                  END
                                  ELSE
                                  BEGIN
                                    ROLLBACK
                                    SELECT
                                      @SalesOrganization AS SalesOrganization,
                                      @DistributionChannel AS DistributionChannel,
                                      @Division AS Division,
                                      @Discount AS Discount,
                                      @DiscountReason AS DiscountReason,
                                      @MaterialCode AS MaterialCode,
                                      @EmployeeCode AS EmployeeCode,
                                      @ValidFrom AS ValidFrom,
                                      @ValidTo AS ValidTo,
									  @StateName as StateName,
                                      'Failed, Discount not assigned to you' AS 'Message'
                                    RETURN
                                  END
                                --END
                                --ELSE
                                --BEGIN
                                --  --Error Message
                                --  SELECT
                                --    @ErrorMsgUserState = @ErrorMsgUserState + @Item + '-' + @StateItem
                                --END


                              END
                              ELSE
                              BEGIN
                                SELECT TOP 1
                                  @DiscountId = Id
                                FROM DiscountUsers
                                WHERE UserId = @LoginUserId
                                AND SkuId = @SkuId
								AND StateId = @StateId
                                AND ((ValidFrom >= @ValidFrom
                                AND ValidFrom <= @ValidTo)
                                OR (ValidTo >= @ValidFrom
                                AND ValidTo <= @ValidTo))
                                ORDER BY Id DESC

                                IF (@DiscountId <> 0)
                                BEGIN
                                  SELECT
                                    @ActualDiscount = ActualDiscount,
                                    @ExistingValidFrom = ValidFrom,
                                    @ExistingValdTo = ValidTo
                                  FROM DiscountUsers
                                  WHERE Id = @DiscountId
                                  IF (@Discount > @ActualDiscount)
                                  BEGIN
                                    ROLLBACK
                                    SELECT
                                      @SalesOrganization AS SalesOrganization,
                                      @DistributionChannel AS DistributionChannel,
                                      @Division AS Division,
                                      @Discount AS Discount,
                                      @DiscountReason AS DiscountReason,
                                      @MaterialCode AS MaterialCode,
                                      @EmployeeCode AS EmployeeCode,
                                      @ValidFrom AS ValidFrom,
                                      @ValidTo AS ValidTo,
									  @StateName as StateName,
                                      'Failed, Discount amount should be less than or equal to amount assigned to you' AS 'Message'
                                    RETURN
                                  END
                                  IF (NOT (@ValidFrom >= @ExistingValidFrom
                                    AND @ValidFrom <= @ExistingValdTo
                                    AND @ValidTo <= @ExistingValdTo
                                    AND @ValidTo >= @ExistingValidFrom))
                                  BEGIN
                                    ROLLBACK
                                    SELECT
                                      @SalesOrganization AS SalesOrganization,
                                      @DistributionChannel AS DistributionChannel,
                                      @Division AS Division,
                                      @Discount AS Discount,
                                      @DiscountReason AS DiscountReason,
                                      @MaterialCode AS MaterialCode,
                                      @EmployeeCode AS EmployeeCode,
                                      @ValidFrom AS ValidFrom,
                                      @ValidTo AS ValidTo,
									  @StateName as StateName,
                                      'Failed, Valid From and Valid To date range should be less than or equal to date range assigned to you' AS 'Message'
                                    RETURN
                                  END

                                  IF (@IsFirstRecord = 0)
                                  BEGIN
                                    DECLARE @primarykeyIdName int
                                    INSERT INTO DiscountUsers (UserId, SkuId, SalesOrganizationId, DistributionChannelId, DivisionId, ActualDiscount, DiscountReason, ParentId, Status, SaudaBookingTypeId, ValidFrom, ValidTo, CreatedBy, CreatedDate, ParentDiscountId, OilTypeId, RequestedDiscount, ApprovedBy, StateId)
                                      VALUES (@UserId, @SkuId, @SalesOrganizationId, @DistributionChannelId, @DivisionId, @Discount, @DiscountReason, @ParentId, 1, 1, @ValidFrom, @ValidTo, @LoginUserId, GETDATE(), @DiscountId, @OilTypeId, 0, 0, @StateId)
                                    SELECT
                                      @primarykeyIdName = SCOPE_IDENTITY()
                                    SET @IsFirstRecord = 1
                                    SET @ParentId = @primarykeyIdName
                                  END
                                  IF (@IsFirstRecord = 1)
                                  BEGIN
                                    INSERT INTO DiscountUsers (UserId, SkuId, SalesOrganizationId, DistributionChannelId, DivisionId, ActualDiscount, DiscountReason, ParentId, Status, SaudaBookingTypeId, ValidFrom, ValidTo, CreatedBy, CreatedDate, ParentDiscountId, OilTypeId, RequestedDiscount, ApprovedBy, StateId)
                                      VALUES (@UserId, @SkuId, @SalesOrganizationId, @DistributionChannelId, @DivisionId, @Discount, @DiscountReason, @ParentId, 1, 1, @ValidFrom, @ValidTo, @LoginUserId, GETDATE(), @DiscountId, @OilTypeId, 0, 0, @StateId)
                                  END

                                END
                                ELSE
                                BEGIN
                                  ROLLBACK
                                  SELECT
                                    @SalesOrganization AS SalesOrganization,
                                    @DistributionChannel AS DistributionChannel,
                                    @Division AS Division,
                                    @Discount AS Discount,
                                    @DiscountReason AS DiscountReason,
                                    @MaterialCode AS MaterialCode,
                                    @EmployeeCode AS EmployeeCode,
                                    @ValidFrom AS ValidFrom,
                                    @ValidTo AS ValidTo,
									@StateName as StateName,
                                    'Failed, Discount not assigned to you' AS 'Message'
                                  RETURN
                                END



                              END


                            END
                          END
                          ELSE
                          BEGIN
                            SELECT
                              @ErrorMsgState = @ErrorMsgState + @StateItem
                          END
                        END
                        ELSE
                        BEGIN
                          SELECT
                            @StateItem = @StateForLoop
                          SELECT
                            @StateLoop = 0
                        END

                      --State Loop Ends

                      END
                    END

                  END
                  ELSE
                  BEGIN
                    SELECT
                      @ErrorMsgSku = @ErrorMsgSku + @SkuItem
                  END

                END
                ELSE
                BEGIN
                  SELECT
                    @SkuItem = @MaterialCodeForLoop
                  SELECT
                    @SkuLoop = 0
                END
              END
              --Material Loop Ends
             
            END

          --Material Ends					
          END
          ELSE
          BEGIN
            SELECT
              @ErrorMsg = @ErrorMsg + @Item
          END

        END
        ELSE
        BEGIN
          SELECT
            @Item = @EmployeeCode
          SELECT
            @Loop = 0
        END
      END
      --Customer Loop Ends
      IF (@ErrorMsg <> '' OR @ErrorMsgSku <> '' OR @ErrorMsgState <> '' OR @ErrorMsgUserDivision <> '' OR @ErrorMsgUserState<>'')
      BEGIN
        ROLLBACK
        SELECT
          @SalesOrganization AS SalesOrganization,
          @DistributionChannel AS DistributionChannel,
          @Division AS Division,
          @Discount AS Discount,
          @DiscountReason AS DiscountReason,
          @MaterialCode AS MaterialCode,
          @EmployeeCode AS EmployeeCode,
          @ValidFrom AS ValidFrom,
          @ValidTo AS ValidTo,
		  @StateName as StateName,
          'Failed, ' 
		  + CASE WHEN  @ErrorMsg <> '' THEN @ErrorMsg+' these users not mapped for this Login User' ELSE '' END 
		  + CASE WHEN  @ErrorMsgSku <> '' THEN @ErrorMsgSku+' these Materials not Exists' ELSE '' END 
		  + CASE WHEN  @ErrorMsgState <> ''  THEN @ErrorMsgState +' these State are not Exists' ELSE '' END 
		  + CASE WHEN  @ErrorMsgUserDivision <> '' THEN @ErrorMsgUserDivision+' these Users Does not have Assigned Discount SalesOrganization,Distribution Channel, Division' ELSE '' END 
		  + CASE WHEN  @ErrorMsgUserState <> '' THEN @ErrorMsgUserState+' these users not mapped to the State' ELSE '' END 
		  + '' AS 'Message'
        RETURN
      END
      ELSE
        SELECT
          @SalesOrganization AS SalesOrganization,
          @DistributionChannel AS DistributionChannel,
          @Division AS Division,
          @Discount AS Discount,
          @DiscountReason AS DiscountReason,
          @MaterialCode AS MaterialCode,
          @EmployeeCode AS EmployeeCode,
          @ValidFrom AS ValidFrom,
          @ValidTo AS ValidTo,
		  @StateName as StateName,
          'Success, Discount added successfully' AS 'Message'
    END
  --Customer Ends



  COMMIT