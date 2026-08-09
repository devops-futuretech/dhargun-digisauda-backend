GO

/****** Object:  UserDefinedTableType [dbo].[UDTT_OpenCreditMaster]    Script Date: 12/19/2022 4:51:37 PM ******/
CREATE TYPE [dbo].[UDTT_OpenCreditMaster] AS TABLE(
	[UserId] [bigint] NOT NULL,
	[SalesOrgId] [bigint] NOT NULL,
	[DistChnlId] [bigint] NOT NULL,
	[DivisionId] [bigint] NOT NULL,
	[CreditLimit] [decimal](18, 3) NOT NULL,
	[CreditExposure] [decimal](18, 3) NOT NULL,
	[OpenOrders] [decimal](18, 3) NOT NULL,
	[DeliveryValue] [decimal](18, 3) NOT NULL,
	[BillingDocumentValue] [decimal](18, 3) NOT NULL,
	[AvailableCreditLimit] [decimal](18, 3) NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedBy] [bigint] NULL
)
GO


