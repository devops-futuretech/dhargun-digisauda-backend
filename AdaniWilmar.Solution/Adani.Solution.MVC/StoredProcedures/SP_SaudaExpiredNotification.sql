CREATE PROCEDURE [dbo].[SaudaExpiredNotification](
@DateRemainder BIGINT
)
AS
BEGIN

WITH cte AS
(
  Select DISTINCT u.Name UserName,u.Email,u.MobileNumber,u.PushTokenKey ,s.SaudaNumber , FORMAT (so.ValidToDate,'dd-MM-yy') ExpiredDate, FORMAT (s.CreatedDate,'dd-MM-yy') CreatedDate
From Saudas s
Join SaudaOrders so on s.Id = so.SaudaId
Join Skus sk on sk.Id = so.SkuId
Join Users u on u.Id = s.UserId 
WHERE s.StatusId != 6 and so.BidQuantityCase >= so.SalesOrderQuantityCase and FORMAT (so.ValidToDate,'dd-MM-yy')  = FORMAT (GETDATE() - @DateRemainder ,'dd-MM-yy') 
)
SELECT DISTINCT UserName, ExpiredDate,CreatedDate, MobileNumber, Email,PushTokenKey,  
     [SaudaNumber]       = (STUFF((SELECT CAST(', ' + SaudaNumber AS VARCHAR(MAX)) 
                            FROM cte   c2  
                            WHERE c2.CreatedDate = c1.CreatedDate
                            FOR XML PATH ('')), 1, 2, ''))
FROM cte c1;



END