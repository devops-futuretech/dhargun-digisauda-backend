Create TRIGGER [dbo].[LiftingRequests_Insert]
       ON [dbo].[LiftingRequests]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
 
    Declare @LiftingRequestId BigInt
	Declare @statusId BigInt
	Declare @status Nvarchar(1000)
	Declare @ModifiedById BigInt
    Declare @Notification VARCHAR(Max)
	Declare @DealerId VARCHAR(Max)
	Declare @BDOId VARCHAR(Max)
	Declare @ZonalHeadId VARCHAR(Max)
	   
 
    SELECT @LiftingRequestId = I.Id,@statusId = I.StatusId,@ModifiedById = I.CreatedBy,@DealerId = I.UserId
    FROM INSERTED I

	Select @BDOId = UserId From UserCustomerMappings Where CustomerId = @DealerId

	Select @ZonalHeadId = OrganizationReportingToId from Users  where Id = @BDOId

	IF(@ModifiedById = @DealerId)
	BEGIN
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Indent',@LiftingRequestId,'pending for Approve',@DealerId,getdate(),2,@statusId)
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Indent',@LiftingRequestId,'pending for Approve',@BDOId,getdate(),2,@statusId)
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Indent',@LiftingRequestId,'pending for Approve',@ZonalHeadId,getdate(),2,@statusId)
	END
	ELSE 
	BEGIN
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Indent',@LiftingRequestId,'Approved',@DealerId,getdate(),2,@statusId)
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Indent',@LiftingRequestId,'Approved',@BDOId,getdate(),2,@statusId)
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Indent',@LiftingRequestId,'Approved',@ZonalHeadId,getdate(),2,@statusId)
	END
END