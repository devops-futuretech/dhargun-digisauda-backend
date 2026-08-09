GO
/****** Object:  StoredProcedure [dbo].[SP_OverDueNotification]    Script Date: 05-10-2022 1.46.53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_OverDueNotification](
@DateRemainder BIGINT
)
AS
BEGIN

SELECT UserId,u.Name UserName,u.Email,u.MobileNumber ,SUM(Balance) DueAmount, FORMAT (o.DueDate ,'dd-MM-yy') DueDate 
FROM OverduePayments o
JOIN Users u ON u.Id = o.UserId 
WHERE FORMAT (DueDate,'dd-MM-yy')  = FORMAT (GETDATE() - @DateRemainder ,'dd-MM-yy') GROUP BY UserId,u.Name,u.Email,u.MobileNumber,o.DueDate  

END



