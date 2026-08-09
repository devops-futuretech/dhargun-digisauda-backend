IF EXISTS (SELECT * FROM sys.objects WHERE [name] = N'SaudaOrders_update' AND [type] = 'TR')
    BEGIN
        DROP  TRIGGER SaudaOrders_update
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[SaudaOrders_update]
       ON [dbo].[SaudaOrders]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    Declare @saudaOrderId BigInt
	Declare @statusId BigInt
	Declare @status Nvarchar(1000)
	Declare @BiddingWindowId BigInt
	Declare @SaudaId BigInt
	Declare @SaudaBookingTypeId BigInt
	Declare @ModifiedById BigInt
    Declare @Notification VARCHAR(Max)
	   
 
    SELECT @saudaOrderId = I.Id,@SaudaId = I.SaudaId,@statusId = I.StatusId,@BiddingWindowId = I.BiddingwindowId,@ModifiedById = I.CreatedBy
    FROM INSERTED I

	Select @status = Name From Status Where Id = @statusId
	Select @Notification = @status
 
	IF UPDATE(StatusId)
	BEGIN
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Sauda',@saudaOrderId,@Notification,@ModifiedById,getdate(),1,@statusId)
	END
END
