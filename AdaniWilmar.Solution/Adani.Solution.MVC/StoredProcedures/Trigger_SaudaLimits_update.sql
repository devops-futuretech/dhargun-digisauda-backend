IF EXISTS (SELECT * FROM sys.objects WHERE [name] = N'SaudaLimits_update' AND [type] = 'TR')
    BEGIN
        DROP  TRIGGER SaudaLimits_update
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[SaudaLimits_update]
       ON [dbo].[SaudaLimits]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    Declare @Id BigInt
	Declare @statusId BigInt
	Declare @status Nvarchar(1000)
	Declare @ModifiedById BigInt
    Declare @Notification VARCHAR(Max)
	   
 
    SELECT @Id = I.Id,@statusId = I.StatusId,@ModifiedById = I.CreatedBy
    FROM INSERTED I

	Select @status = Name From Status Where Id = @statusId
	Select @Notification = @status
 
	IF UPDATE(StatusId)
	BEGIN
			Insert Into Notifications (Request,ReferenceId,Notification,CreatedBy,CreatedDate,RequestId,StatusId) Values('Sauda Limit',@Id,@Notification,@ModifiedById,getdate(),5,@statusId)
	END
END