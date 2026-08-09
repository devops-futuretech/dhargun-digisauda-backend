CREATE TYPE DiscountGeographyImportStatusType AS TABLE
(
    Id BIGINT,
    SalesOrganization NVARCHAR(255),
    DistributionChannel NVARCHAR(255),
    Division NVARCHAR(255),
    MaterialCode NVARCHAR(255),
    DiscountReason NVARCHAR(255),
    Discount DECIMAL(18, 2),  -- Should be decimal
    ValidFrom DATETIME,       -- Should be datetime
    ValidTo DATETIME,         -- Should be datetime
    LoginUserId BIGINT,
    Zone NVARCHAR(255),
    State NVARCHAR(255),
    District NVARCHAR(255),
    City NVARCHAR(255),
    Message NVARCHAR(1000)
);