CREATE PROCEDURE [dbo].[GetWindowCautionNotificationDetails]
AS
BEGIN

Select DISTINCT BiddingWindowId,NotificationTypeId,NotificationTime,CustomerGroupId From BiddingWindowNotificationTimings 
Where CAST(CONVERT(CHAR(16), NotificationTime,20) AS DATETIME) = (SELECT CAST(CONVERT(CHAR(16), GETDATE(),20) AS DATETIME)) AND StatusId = 1
--Where StatusId = 1
Update BiddingWindowNotificationTimings Set StatusId = 2 Where CAST(CONVERT(CHAR(16), NotificationTime,20) AS DATETIME) = (SELECT CAST(CONVERT(CHAR(16), GETDATE(),20) AS DATETIME)) AND StatusId = 1

END;