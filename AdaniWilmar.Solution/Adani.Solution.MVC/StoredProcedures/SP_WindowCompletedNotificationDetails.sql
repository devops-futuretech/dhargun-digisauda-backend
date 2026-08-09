IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_WindowCompletedNotificationDetails')
    BEGIN
        DROP  Procedure SP_WindowCompletedNotificationDetails
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_WindowCompletedNotificationDetails]

@BiddingWindowId BIGINT,
@NotificationActionId BIGINT

AS
BEGIN

CREATE TABLE #WindowCompletedNotification(IsEmail bit, IsSMS bit, IsInAppNotification bit,DealerId bigint,Email varchar(250),MobileNumber varchar(20),RegistrationTypeId bigint,PushTokenKey varchar(MAX),
BdoId bigint,BdoEmail varchar(250),BdoMobileNumber varchar(20),BdoRegistrationTypeId bigint,BdoPushTokenKey varchar(MAX),IsBooked bit)


INSERT INTO #WindowCompletedNotification(IsEmail,IsSMS,IsInAppNotification,DealerId,Email,MobileNumber,RegistrationTypeId,PushTokenKey,BdoId,BdoEmail,BdoMobileNumber,BdoRegistrationTypeId,BdoPushTokenKey,IsBooked)
SELECT DISTINCT n.Email as IsEmail,n.SMS as IsSMS,n.InAppNotification as IsInAppNotification,
nd.DealerId,u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey,
ubdo.Id as BdoId,ubdo.Email as BdoEmail,ubdo.MobileNumber as BdoMobileNumber,ubdo.RegistrationTypeId as BdoRegistrationTypeId,ubdo.PushTokenKey as BdoPushTokenKey,0 as IsBooked
From RaNotifications n
Join RaNotificationDetails nd on n.Id = nd.RaNotificationId
Left Join Users u on u.Id = nd.DealerId
Left Join UserCustomerMappings ucm on ucm.CustomerId = nd.DealerId
Left Join Users ubdo ON ubdo.Id = ucm.UserId
Where nd.IsActive = 1
and nd.CustomerGroupId IN (Select CustomerGroupId From PriceGenerateDetails Where BiddingWindowId = @BiddingWindowId)
and nd.NotificationActionId = @NotificationActionId
and Convert(varchar,GETDATE(), 111) >= Convert(varchar, n.ValidFrom, 111)
and Convert(varchar,GETDATE(), 111) <= Convert(varchar, n.ValidTo, 111)


--Update sauda booked user IsBookd true
UPDATE #WindowCompletedNotification Set IsBooked = 1
WHERE DealerId IN (

Select s.UserId From Saudas s
Join SaudaOrders so ON s.Id = so.SaudaId
Where so.BiddingWindowId = @BiddingWindowId
And s.UserId IN (Select DealerId From #WindowCompletedNotification)
	
)

------------Main Function------------------

SELECT * FROM #WindowCompletedNotification

DROP TABLE #WindowCompletedNotification

END;