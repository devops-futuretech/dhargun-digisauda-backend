GO

/****** Object:  UserDefinedTableType [dbo].[UDTT_CustomerLedgers]    Script Date: 12/19/2022 4:51:55 PM ******/
CREATE TYPE [dbo].[UDTT_CustomerLedgers] AS TABLE(
	[Reference] [nvarchar](max) NULL,
	[PostingDate] [nvarchar](max) NULL,
	[DueDate] [nvarchar](max) NULL,
	[DocumentType] [nvarchar](max) NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[UserCode] [nvarchar](max) NULL,
	[CompanyCode] [nvarchar](max) NULL,
	[Currency] [nvarchar](max) NULL,
	[Credit] [decimal](18, 2) NOT NULL,
	[Debit] [decimal](18, 2) NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedBy] [bigint] NOT NULL
)
GO