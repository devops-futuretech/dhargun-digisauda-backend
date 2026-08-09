CREATE TYPE [dbo].[UDTT_PendingContracts] AS TABLE(
	[UserId] [bigint] NOT NULL,
	[SaudaOrderId] [bigint] NOT NULL,
	[SaudaNumber] [nvarchar](max) NULL,
	[MaterialCode] [nvarchar](max) NULL,
	[CustomerCode] [nvarchar](max) NULL,
	[CustomerName] [nvarchar](max) NULL,
	[ContractValidTo] [nvarchar](max) NULL,
	[ContractValidFrom] [nvarchar](max) NULL,
	[BasicRate] [decimal](18, 3) NOT NULL,
	[PendingQuantityInCase] [decimal](18, 3) NOT NULL,
	[SaudaQuantity] [decimal](18, 3) NOT NULL,
	[SalesOrgId] [bigint] NOT NULL,
	[DistChnlId] [bigint] NOT NULL,
	[DivisionId] [bigint] NOT NULL,
	[TotalValue] [decimal](18, 2) NOT NULL,
	[IsSaudaExtended] [bit] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedBy] [bigint] NULL
)
GO