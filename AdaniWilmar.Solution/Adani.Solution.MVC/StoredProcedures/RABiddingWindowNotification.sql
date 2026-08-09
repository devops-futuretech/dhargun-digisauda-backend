CREATE PROCEDURE [dbo].[RABiddingWindowNotification](
@BiddingWindowId BIGINT,
@CustomerGroupId BIGINT,
@NotificationActionId BIGINT,
@TemplateName VARCHAR(100)
)
AS
BEGIN

Select n.Id,nd.CustomerGroupId,n.Email as IsEmail,n.SMS as IsSMS,n.InAppNotification as IsInAppNotification,nd.DealerId,
u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey,ucm.UserId as BdoId 
INTO #DealerTemp
From RaNotifications n
Join RaNotificationDetails nd on n.Id = nd.RaNotificationId
Join Users u on u.Id = nd.DealerId
Join UserCustomermappings ucm ON ucm.CustomerId = nd.DealerId
Where nd.IsActive = 1
and nd.CustomerGroupId IN (@CustomerGroupId)
and nd.NotificationActionId = @NotificationActionId
and Convert(varchar,GETDATE(), 111) >= Convert(varchar, n.ValidFrom, 111)
and Convert(varchar,GETDATE(), 111) <= Convert(varchar, n.ValidTo, 111)

Select * From #DealerTemp

Select Distinct u.Id,u.Name,u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey
From Users u Join UserCustomerMappings uc on u.Id = uc.UserId
Join UserRoles ur on u.Id = ur.UserId
Where uc.CustomerId in (Select DealerId From #DealerTemp)
and ur.RoleId = 7 --StateTrader Role
and u.SaudaBookingTypeId = 2 --RA Booking Type

Select Name,PlainTemplate,Template From EmailTemplates
Where Name = @TemplateName + 'Email' or Name = @TemplateName + 'SMS'

Select Name,StartTime,EndTime,SaudaAllocationStartTime,SaudaAllocationEndTime From BiddingWindows Where Id = @BiddingWindowId

Select SaudaAllocationTime From RaSaudaConfigurations Where IsActive = 1

Select [Key],Value From Configurations
Where [Key] = 'FirebaseSenderId' or [Key] = 'PushNotifyServerkey' or [Key] = 'PushNotifyUrl'

DROP TABLE #DealerTemp

END


--EXEC [RABiddingWindowNotification] 400,18,6,'WindowCreation'