ALTER PROCEDURE [dbo].[GetGeographyDiscountDetailsForList]
    @DiscountInputDate DATETIME,
    @RoleId INT,
    @ZoneId VARCHAR(MAX)='',
    @StateId VARCHAR(MAX)='',
    @DistrictId VARCHAR(MAX)='',
    @CityId VARCHAR(MAX)='',
    @PageNumber INT = NULL,
    @PageSize INT = NULL,
	@Status NVARCHAR(20) = ''
AS
BEGIN
    SET NOCOUNT ON;
	DECLARE @IsActive BIT;
    SET @IsActive = (CASE WHEN @Status = 'true' THEN 1 ELSE 0 END);

	UPDATE DiscountGeographies SET IsActive = 0 WHERE IsActive = 1 AND ValidTo < GETDATE();

	;WITH ZoneList AS (
		SELECT TRY_CAST(value AS BIGINT) AS ZoneId
		FROM STRING_SPLIT(@ZoneId, ',')
		WHERE TRY_CAST(value AS BIGINT) IS NOT NULL
	),
	StateList AS (
	    SELECT TRY_CAST(value AS BIGINT) AS StateId
	    FROM STRING_SPLIT(@StateId, ',')
	    WHERE TRY_CAST(value AS BIGINT) IS NOT NULL
	),
	 DistrictList AS (
		SELECT TRY_CAST(value AS BIGINT) AS DistrictId
		FROM STRING_SPLIT(@DistrictId, ',')
		WHERE TRY_CAST(value AS BIGINT) IS NOT NULL
	),
	CityList AS (
	    SELECT TRY_CAST(value AS BIGINT) AS CityId
	    FROM STRING_SPLIT(@CityId, ',')
	    WHERE TRY_CAST(value AS BIGINT) IS NOT NULL
	),
    FilteredGeo AS (
        SELECT *
        FROM DiscountGeographies DG
        WHERE CAST(@DiscountInputDate AS DATE) BETWEEN CAST(DG.ValidFrom AS DATE) AND CAST(DG.ValidTo AS DATE)
            AND (
                @RoleId IN (1, 12) --admin,NationalTrader
                AND
				(
					((@ZoneId IS NULL OR @ZoneId = '') OR DG.ZoneId IN(SELECT ZoneId FROM ZoneList)) AND
					((@StateId IS NULL OR @StateId = '') OR DG.StateId IN (SELECT StateId FROM StateList)) AND
					((@DistrictId IS NULL OR @DistrictId = '') OR DG.DistrictId IN (SELECT DistrictId FROM DistrictList)) AND
					((@CityId IS NULL OR @CityId = '') OR DG.CityId IN (SELECT CityId FROM CityList)) AND
					(@Status = '' OR DG.IsActive = @IsActive)
				)
            )
    ),
    ParentRecords AS (
        --SELECT * FROM FilteredGeo WHERE ParentId = 0
        SELECT * FROM DiscountGeographies WHERE Id in (select Distinct ParentId from FilteredGeo)
    ),
    CombinedResult AS (
        SELECT 
            P.Id,
            P.SalesOrganizationId,
            P.DistributionChannelId,
            P.DivisionId,
			P.OilTypeId,
			P.PackGroupId,
            SO.Name AS SalesOrganization,
            DC.Name AS DistributionChannel,
            DV.Name AS Division,
            P.DiscountReason,
            ISNULL((
				SELECT STRING_AGG(SkuName, ',') 
				FROM (
					SELECT DISTINCT CAST(S.SkuName AS NVARCHAR(MAX)) AS SkuName
					FROM DiscountGeographies C
					INNER JOIN Skus S ON S.Id = C.SkuId
					WHERE C.ParentId = P.Id
				) AS DistinctSkus
			), '') AS SkuName,
            SParent.SkuCode,
            P.ValidFrom,
            P.ValidTo,
            P.Id AS ParentId,
            ISNULL((
                SELECT TOP 1 C.ActualDiscount 
                FROM DiscountGeographies C 
                WHERE C.ParentId = P.Id 
                ORDER BY C.Id
            ), 0) AS ActualDiscount,
            (
                SELECT STRING_AGG(CAST(SkuId AS NVARCHAR(MAX)), ',') 
                FROM DiscountGeographies 
                WHERE ParentId = P.Id
            ) AS SkuIdsString,
            (SELECT COUNT(*) FROM ParentRecords) AS TotalRecords,
			p.IsActive as IsActive
        FROM ParentRecords P
        INNER JOIN SalesOrganizations SO ON SO.Id = P.SalesOrganizationId
        INNER JOIN DistributionChannels DC ON DC.Id = P.DistributionChannelId
        INNER JOIN Divisions DV ON DV.Id = P.DivisionId
        INNER JOIN Skus SParent ON SParent.Id = P.SkuId
    )

    SELECT *
    FROM CombinedResult
    ORDER BY Id DESC
    OFFSET 
        CASE WHEN @PageNumber IS NOT NULL THEN ISNULL(@PageNumber - 1, 0) * ISNULL(@PageSize, 10) ELSE 0 END ROWS
    FETCH NEXT 
        CASE WHEN @PageNumber IS NOT NULL THEN ISNULL(@PageSize, 10) ELSE 1000000 END ROWS ONLY;  -- 1 million as high fallback
END