CREATE PROCEDURE [dbo].[BiddingWindowCautionNotificationForCounterBidDetails]
	@BiddingWindowId int
AS
BEGIN 
 
   
   IF EXISTS(Select 1 FROM BiddingWindows Where StatusId = 2 and Id = @BiddingWindowId)
   BEGIN

   /*Taken both dealer and StateTrader ids of the bidding window with status - inprogress and Conterbidstatus - pending */
	Create TABLE #BiddingWindowUserIds (UserId bigint,CreatedById bigint) 
	INSERT INTO #BiddingWindowUserIds (UserId,CreatedById) 
	select sbch.DealerId,sbch.CreatedBy
	from  SaudaBiddingCartHeaders sbch With(NoLock)  
	Left Join SaudaBiddingCarts  sbc on sbch.Id = sbc.SaudaBiddingCartHeaderId
	where sbch.BiddingWindowId = @BiddingWindowId and sbc.CounterBidStatusId = 1

	/*Inserted the StateTrader ids to the user id column - to take both dealer and StateTrader details from user table.*/
	INSERT INTO #BiddingWindowUserIds (UserId) 
	select CreatedById
	from #BiddingWindowUserIds


	/*Taken details of StateTrader and dealer ids*/
	select Distinct u.Id,u.MobileNumber,u.Email,u.PushTokenKey,u.RegistrationTypeId
	from #BiddingWindowUserIds bwu 
	Left Join Users  u on bwu.UserId = u.Id

 END
  
  DROP TABLE #BiddingWindowUserIds
	
END