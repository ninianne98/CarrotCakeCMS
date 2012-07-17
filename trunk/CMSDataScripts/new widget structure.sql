-- USE [CarrotwareCMS]
GO

/****** Object:  Table [dbo].[tblWidget]    Script Date: 07/16/2012 19:55:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblWidget] (
	[Root_WidgetID] [uniqueidentifier] NOT NULL,
	[Root_ContentID] [uniqueidentifier] NOT NULL,
	[WidgetOrder] [int] NOT NULL,	
	[PlaceholderName] [varchar](256) NOT NULL,
	[ControlPath] [varchar](512) NOT NULL,
	[WidgetActive] [bit] NOT NULL,	

 CONSTRAINT [PK_tblWidget] PRIMARY KEY CLUSTERED 
(
	[Root_WidgetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblWidget]  WITH CHECK ADD  CONSTRAINT [tblRootContent_tblWidget_FK] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[tblRootContent] ([Root_ContentID])
GO

ALTER TABLE [dbo].[tblWidget] CHECK CONSTRAINT [tblRootContent_tblWidget_FK]
GO

ALTER TABLE [dbo].[tblWidget] ADD  CONSTRAINT [DF_tblWidget_Root_WidgetID]  DEFAULT (newid()) FOR [Root_WidgetID]
GO



/****** Object:  Table [dbo].[tblWidgetData]   Script Date: 07/16/2012 19:55:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblWidgetData](
	[WidgetDataID] [uniqueidentifier] NOT NULL,
	[Root_WidgetID] [uniqueidentifier] NOT NULL,
	[IsLatestVersion] [bit] NULL,
	[EditDate] [datetime] NOT NULL,	
	[ControlProperties] [varchar](max) NULL,
 CONSTRAINT [PK_tblWidgetData] PRIMARY KEY CLUSTERED 
(
	[WidgetDataID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblWidgetData]  WITH CHECK ADD  CONSTRAINT [tblWidgetData_Root_WidgetID_FK] FOREIGN KEY([Root_WidgetID])
REFERENCES [dbo].[tblWidget] ([Root_WidgetID])
GO

ALTER TABLE [dbo].[tblWidgetData] CHECK CONSTRAINT [tblWidgetData_Root_WidgetID_FK]
GO

ALTER TABLE [dbo].[tblWidgetData] ADD  CONSTRAINT [DF_tblWidgetData_WidgetDataID]  DEFAULT (newid()) FOR [WidgetDataID]
GO

ALTER TABLE [dbo].[tblWidgetData] ADD  CONSTRAINT [DF_tblWidgetData_EditDate]  DEFAULT (getdate()) FOR [EditDate]
GO



IF (select COUNT(*) from  [dbo].[tblWidget]) < 1 BEGIN
		INSERT INTO [dbo].[tblWidget]
				   ([Root_WidgetID]
				   ,[Root_ContentID]
				   ,[WidgetOrder]
				   ,[PlaceholderName]
				   ,[ControlPath]
				   ,[WidgetActive])
			SELECT [PageWidgetID]
				  ,[Root_ContentID]
				  ,[WidgetOrder]      
				  ,[PlaceholderName]
				  ,[ControlPath]
				  ,1
			  FROM [dbo].[tblPageWidgets]


		INSERT INTO [dbo].[tblWidgetData]
				   ([WidgetDataID]
				   ,[Root_WidgetID]
				   ,[IsLatestVersion]
				   ,[EditDate]
				   ,[ControlProperties])
			SELECT  NEWID() 
				  ,[PageWidgetID]
				  , 1
				  ,GETDATE()
				  ,[ControlProperties]
			  FROM [dbo].[tblPageWidgets]

END  
  
GO





