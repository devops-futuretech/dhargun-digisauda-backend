CREATE PROCEDURE [dbo].[BiddingWindowCautionNotification]
	@BiddingWindowId int
	
AS
	BEGIN 
 
     /*Taken dealer ids */
	Create TABLE #BiddingWindowUserIds (UserId bigint) 
	INSERT INTO #BiddingWindowUserIds (UserId) 

	select cgd.CustomerId
	from  BiddingWindowCustomerGroups bwcg With(NoLock)  
	Left Join CustomerGroupDetails cgd on bwcg.CustomerGroupId = cgd.CustomerGroupId
	where bwcg.BiddingWindowId = @BiddingWindowId

	/*Taken StateTrader ids*/
	INSERT INTO #BiddingWindowUserIds (UserId) 
	select ucm.UserId
	from  #BiddingWindowUserIds bwu With(NoLock)
	Left Join UserCustomerMappings ucm on bwu.UserId = ucm.CustomerId

	/*Taken details of StateTrader and dealer ids*/
	select Distinct u.Id,u.MobileNumber,u.Email,u.PushTokenKey,u.RegistrationTypeId
	from #BiddingWindowUserIds bwu 
	Left Join Users  u on bwu.UserId = u.Id

	/*Pushnotification Details*/
	Select [Key],Value From Configurations
	Where [Key] = 'FirebaseSenderId' or [Key] = 'PushNotifyServerkey' or [Key] = 'PushNotifyUrl'

	/*Bidding Window Details */
	Select Name,StartTime,EndTime,SaudaAllocationStartTime,SaudaAllocationEndTime From BiddingWindows Where Id = @BiddingWindowId

	/* Email & Sms templates */
	Select Name,PlainTemplate,Template From EmailTemplates
	Where Name = @TemplateName + 'Email' or Name = @TemplateName + 'SMS'
	
	DROP TABLE #BiddingWindowUserIds
	
END