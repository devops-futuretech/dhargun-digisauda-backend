GO
/****** Object:  StoredProcedure [dbo].[UserLoginHistoryExport]    Script Date: 6/18/2024 1:22:00 AM ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[UserLoginHistoryExport]
    @FromDate DATE,
    @ToDate DATE,
    @LoginUserId INT
AS
BEGIN

	CREATE TABLE #TempTableDivisions (
    DistributorId INT,
    Divisions NVARCHAR(MAX));

	INSERT INTO #TempTableDivisions (DistributorId, Divisions)
SELECT 
    u.Id AS DistributionId,
    STUFF((SELECT ',' + CONCAT(so.Code, '/', dc.Code, '/', d.Code)
        FROM UserDivisionMappings ud_inner
        JOIN SalesOrganizations so ON so.Id = ud_inner.SalesOrganizationId
        JOIN DistributionChannels dc ON dc.Id = ud_inner.DistributionChannelId
        JOIN Divisions d ON d.Id = ud_inner.DivisionId
        WHERE ud_inner.UserId = u.Id
        FOR XML PATH('')), 1, 1, '') AS [Divisions]
FROM 
    Users u
JOIN 
    UserRoles ur ON u.Id = ur.UserId
WHERE 
    ur.RoleId = 5 --AND u.Id = 8469
GROUP BY 
    u.Id

	CREATE TABLE #TempTableDivisions2 (
    DistributorId INT,
    Divisions NVARCHAR(MAX));

	INSERT INTO #TempTableDivisions2 (DistributorId, Divisions)
SELECT 
    u.Id AS DistributionId,
    STUFF((SELECT ',' + CONCAT(so.Code, '/', dc.Code, '/', d.Code)
        FROM UserDivisionMappings ud_inner
        JOIN SalesOrganizations so ON so.Id = ud_inner.SalesOrganizationId
        JOIN DistributionChannels dc ON dc.Id = ud_inner.DistributionChannelId
        JOIN Divisions d ON d.Id = ud_inner.DivisionId
        WHERE ud_inner.UserId = u.Id
        FOR XML PATH('')), 1, 1, '') AS [Divisions]
FROM 
    Users u
JOIN 
    UserRoles ur ON u.Id = ur.UserId
WHERE 
    ur.RoleId IN (7, 9, 12)  --AND u.Id = 8469
GROUP BY 
    u.Id

	CREATE TABLE #TempLoginHistory1 (
        DistributorName NVARCHAR(100),
		Code INT,
		DistributorId INT,
        LoginDate DATE,
        LoginTime TIME,
		LoginUserId INT,
		Name NVARCHAR(100),
		Divisions NVARCHAR(100),
		ZoneName NVARCHAR(100),
		StateName NVARCHAR(100));
	
	INSERT INTO #TempLoginHistory1 (DistributorName,DistributorId, LoginDate, LoginTime, LoginUserId,Code,Name,ZoneName,StateName)
    SELECT 
        u.Name AS DistributorName,
		 u.Id AS DistributorId,
        CAST(ul.LoginDate AS DATE) AS LoginDate,
        CAST(ul.LoginDate AS TIME) AS LoginTime,
        ul.LoginUserId AS LoginUserId,
		u.Code as [Distributor code],
		--us.Name as [State trader],
		STUFF((SELECT ',' + us.Name
            FROM UserCustomerMappings uc_inner
            JOIN Users us ON uc_inner.UserId = us.Id
			JOIN UserRoles ur_inner ON us.Id = ur_inner.UserId 
            WHERE uc_inner.CustomerId = u.Id
			and ur_inner.RoleId = 7
            FOR XML PATH('')), 1, 1, '') AS [State trader],
		s.StateName,
		z.Name [Zone Name]
    FROM 
        UserLoginHistories ul
    JOIN 
        Users u ON ul.LoginUserId = u.Id
    JOIN 
        UserRoles ur ON u.Id = ur.UserId
	Join 
		UserCustomerMappings uc ON u.Id = uc.CustomerId
	Join	
		Users us ON uc.UserId = us.Id
	Join 
		States s ON u.StateId = s.Id
	Join 
		Zones z ON u.ZoneId = z.Id 
    WHERE 
        ur.RoleId = 5
        AND CAST(ul.LoginDate AS DATE) >= @FromDate
        AND CAST(ul.LoginDate AS DATE) <= @ToDate
	GROUP BY 
		u.Name,u.Id, u.Code, s.StateName, z.Name, CAST(ul.LoginDate AS DATE), CAST(ul.LoginDate AS TIME), ul.LoginUserId
		Order by 
		LoginDate

	CREATE TABLE #TempLoginHistory2 (
    DistributorCode NVARCHAR(100),
    DistributorName NVARCHAR(100),
    LoginDate DATE,
    LoginTime TIME,
    LoginUserId INT
);

    INSERT INTO #TempLoginHistory2 (DistributorName, DistributorCode, LoginDate, LoginTime, LoginUserId)
	SELECT 
	    u.Name AS DistributorName,
	    u.Code AS [Distributor Code],
	    CAST(ul.LoginDate AS DATE) AS LoginDate,
	    CAST(ul.LoginDate AS TIME) AS LoginTime,
	    ul.LoginUserId AS LoginUserId
	FROM 
	    UserLoginHistories ul
	JOIN 
	    Users u ON ul.LoginUserId = u.Id
	JOIN 
	    UserRoles ur ON u.Id = ur.UserId
	WHERE 
	    ur.RoleId = 5
        AND CAST(ul.LoginDate AS DATE) >= @FromDate
        AND CAST(ul.LoginDate AS DATE) <= @ToDate
		Order by
		LoginDate

	CREATE TABLE #TempLoginHistory3 (
    DistributorName NVARCHAR(100),
    LoginDate DATE,
    LoginTime TIME,
    LoginUserId INT,
    Zone NVARCHAR(100),
    State NVARCHAR(100)
);

    INSERT INTO #TempLoginHistory3 (DistributorName, LoginDate, LoginTime, LoginUserId, Zone, State)
SELECT 
    u.Name AS DistributorName,
    CAST(ul.LoginDate AS DATE) AS LoginDate,
    CAST(ul.LoginDate AS TIME) AS LoginTime,
    ul.LoginUserId AS LoginUserId,
    s.StateName AS State,
    z.Name AS Zone
FROM 
    UserLoginHistories ul
JOIN 
    Users u ON ul.LoginUserId = u.Id
JOIN 
    UserRoles ur ON u.Id = ur.UserId
JOIN 
    States s ON u.StateId = s.Id
JOIN 
    Zones z ON u.ZoneId = z.Id 
WHERE 
    ur.RoleId IN (7, 9, 12)
	AND CAST(ul.LoginDate AS DATE) >= @FromDate
    AND CAST(ul.LoginDate AS DATE) <= @ToDate
	Order by
	LoginDate

	SELECT 
        DistributorName as [Distibutor Name],
		Code As [Distibutor Code],
		FORMAT(LoginDate, 'dd-MM-yyyy') AS LoginDate,
		CONVERT(varchar(15), MIN(LoginTime), 100) AS [Initial Login Time],
		COUNT(LoginUserId) AS [Login Count],
		Name as [State trader],
		tt.Divisions,
		ZoneName As [Zone],
		StateName As [State]
    FROM 
        #TempLoginHistory1 as lh
	JOIN 
    #TempTableDivisions tt ON tt.DistributorId = lh.LoginUserId
    GROUP BY 
        Code, FORMAT(LoginDate, 'dd-MM-yyyy'),Name,ZoneName,StateName,tt.Divisions,DistributorName
    ORDER BY FORMAT(LoginDate, 'dd-MM-yyyy');

	WITH NumberedLogins AS (
    SELECT 
        DistributorName AS [Distributor Name],
        DistributorCode AS [Distributor Code],
        FORMAT(LoginDate, 'dd-MM-yyyy') AS LoginDate,
        LoginTime,
        ROW_NUMBER() OVER (PARTITION BY DistributorName, FORMAT(LoginDate, 'dd-MM-yyyy') ORDER BY LoginTime) AS rn
    FROM 
        #TempLoginHistory2
)

SELECT 
    CASE WHEN rn > 1 THEN '' ELSE [Distributor Name] END AS [Distributor Name],
    CASE WHEN rn > 1 THEN '' ELSE [Distributor Code] END AS [Distributor Code],
    CASE WHEN rn > 1 THEN '' ELSE LoginDate END AS LoginDate,
    --LoginTime
    CONVERT(varchar(15), LoginTime, 100) AS [LoginTime]
FROM 
    NumberedLogins;

	WITH NumberedLogins AS (
    SELECT 
        DistributorName AS [Sales person Name],
        FORMAT(LoginDate, 'dd-MM-yyyy') AS LoginDate,
        LoginTime,
        ROW_NUMBER() OVER (PARTITION BY DistributorName, FORMAT(LoginDate, 'dd-MM-yyyy') ORDER BY LoginTime) AS rn,
        tv.Divisions,
        State,
        Zone
    FROM 
        #TempLoginHistory3 AS h
    JOIN 
        #TempTableDivisions2 tv ON tv.DistributorId = h.LoginUserId
    GROUP BY 
        h.DistributorName, FORMAT(LoginDate, 'dd-MM-yyyy'), h.Zone, h.State, tv.Divisions, h.LoginTime
)
SELECT 
    CASE WHEN rn > 1 THEN '' ELSE [Sales person Name] END AS [Sales person Name],
    CASE WHEN rn > 1 THEN '' ELSE LoginDate END AS LoginDate,
    CONVERT(varchar(15), LoginTime, 100) AS [LoginTime],
    Divisions,
	Zone AS Zones,
    State AS States
    
FROM 
    NumberedLogins AS ul;

	DROP TABLE #TempTableDivisions;
	DROP TABLE #TempTableDivisions2;
    DROP TABLE #TempLoginHistory1;
    DROP TABLE #TempLoginHistory2;
	DROP TABLE #TempLoginHistory3;
END

