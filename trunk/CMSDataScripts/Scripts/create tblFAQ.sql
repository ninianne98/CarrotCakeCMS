/****** Object:  Table [dbo].[tblFAQ]    Script Date: 07/16/2012 18:38:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblFAQ](
	[FaqID] [uniqueidentifier] NOT NULL,
	[Question] [varchar](max) NULL,
	[Answer] [varchar](max) NULL,
	[IsActive] [bit] NULL,
	[SortOrder] [int] NULL,
	[dtStamp] [datetime] NULL,
	[SiteID] [uniqueidentifier] NULL,
 CONSTRAINT [tblFAQ_PK] PRIMARY KEY CLUSTERED 
(
	[FaqID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Default [DF_tblFAQ_FaqID]    Script Date: 07/16/2012 18:38:25 ******/
ALTER TABLE [dbo].[tblFAQ] ADD  CONSTRAINT [DF_tblFAQ_FaqID]  DEFAULT (newid()) FOR [FaqID]
GO
