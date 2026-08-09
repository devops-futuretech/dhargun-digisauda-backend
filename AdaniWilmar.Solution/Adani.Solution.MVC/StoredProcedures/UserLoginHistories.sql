CREATE OR ALTER PROCEDURE [dbo].[UserLoginHistoryExport]
    @FromDate DATE,
    @ToDate DATE,
    @LoginUserId INT
AS
BEGIN
    CREATE TABLE #TempLoginHistory1 (
        DistributorName NVARCHAR(100),
        LoginDate DATE,
        LoginTime TIME,
		LoginUserId INT);

    INSERT INTO #TempLoginHistory1 (DistributorName, LoginDate, LoginTime, LoginUserId)
    SELECT 
        u.Name AS DistributorName,
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
        AND CAST(ul.LoginDate AS DATE) <= @ToDate;

    CREATE TABLE #TempLoginHistory2 (
        DistributorName NVARCHAR(100),
        LoginDate DATE,
        LoginTime TIME,
		LoginUserId INT);

    INSERT INTO #TempLoginHistory2 (DistributorName, LoginDate, LoginTime, LoginUserId)
    SELECT 
        u.Name AS DistributorName,
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
        AND CAST(ul.LoginDate AS DATE) <= @ToDate;

    SELECT 
        DistributorName as Name,
		FORMAT(LoginDate, 'dd-MM-yyyy') AS LoginDate,
		--STRING_AGG(LoginTime, CHAR(10)) AS InitialLoginTime,
		--MIN(LoginTime) AS InitialLoginTime,
		CONVERT(varchar(15), MIN(LoginTime), 100) AS InitialLoginTime,
		COUNT(LoginUserId) AS LoginCount
		 
        
    FROM 
        #TempLoginHistory1
    GROUP BY 
        DistributorName, FORMAT(LoginDate, 'dd-MM-yyyy')
    ORDER BY 
        DistributorName, FORMAT(LoginDate, 'dd-MM-yyyy');

    SELECT 
        DistributorName as Name,
        --LoginDate,
		FORMAT(LoginDate, 'dd-MM-yyyy') AS LoginDate,
		CONVERT(varchar(15), MIN(LoginTime), 100) AS LoginTime
    FROM 
        #TempLoginHistory2
    GROUP BY 
        DistributorName, FORMAT(LoginDate, 'dd-MM-yyyy')
    ORDER BY 
        DistributorName, FORMAT(LoginDate, 'dd-MM-yyyy');

    DROP TABLE #TempLoginHistory1;
    DROP TABLE #TempLoginHistory2;
END

