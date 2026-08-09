IF EXISTS (SELECT * FROM sys.objects WHERE [name] = N'LiftingRequests_update' AND [type] = 'TR')
    BEGIN
        DROP  TRIGGER LiftingRequests_update
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[LiftingRequests_update]
       ON [dbo].[LiftingRequests]
AFTER UPDATE
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
	   
 
    SELECT @LiftingRequestId = I.Id
    FROM INSERTED I

	Select @BDOId = UserId From UserCustomerMappings Where CustomerId = @DealerId

	Select @status = Name From Status Where Id = @statusId
	Select @Notification = @status

	Select @statusId = StatusId,@ModifiedById = CreatedBy,@DealerId = UserId From LiftingRequests Where Id = @LiftingRequestId
 
	IF UPDATE(StatusId)
	BEGIN
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Indent',@LiftingRequestId,@Notification,isnull(@DealerId,1),getdate(),2,@statusId)
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Indent',@LiftingRequestId,@Notification,isnull(@BDOId,1),getdate(),2,@statusId)
	END
END



