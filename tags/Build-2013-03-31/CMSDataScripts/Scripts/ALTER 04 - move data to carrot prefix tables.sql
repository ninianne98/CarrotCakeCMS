-- USE [CarrotwareCMS]
GO
-- 2012-08-12 
-- altered table storage to use the carrot_ prefix so that the CMS tables are distinct from user's tables

/****** Object:  Table [dbo].[carrot_Sites]    Script Date: 08/12/2012 18:43:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_Sites](
	[SiteID] [uniqueidentifier] NOT NULL,
	[MetaKeyword] [varchar](1000) NULL,
	[MetaDescription] [varchar](2000) NULL,
	[SiteName] [varchar](256) NULL,
	[MainURL] [varchar](256) NULL,
	[BlockIndex] [bit] NOT NULL,
	[SiteFolder] [varchar](256) NULL,
 CONSTRAINT [carrot_Sites_PK] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[carrot_SerialCache]    Script Date: 08/12/2012 18:43:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_SerialCache](
	[SerialCacheID] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
	[ItemID] [uniqueidentifier] NOT NULL,
	[EditUserId] [uniqueidentifier] NOT NULL,
	[KeyType] [varchar](256) NULL,
	[SerializedData] [varchar](max) NULL,
	[EditDate] [datetime] NOT NULL,
 CONSTRAINT [carrot_SerialCache_PK] PRIMARY KEY CLUSTERED 
(
	[SerialCacheID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[carrot_RootContent]    Script Date: 08/12/2012 18:43:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_RootContent](
	[Root_ContentID] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
	[Heartbeat_UserId] [uniqueidentifier] NULL,
	[EditHeartbeat] [datetime] NULL,
	[FileName] [varchar](256) NOT NULL,
	[PageActive] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [carrot_RootContent_PK] PRIMARY KEY CLUSTERED 
(
	[Root_ContentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[carrot_Content]    Script Date: 08/12/2012 18:43:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_Content](
	[ContentID] [uniqueidentifier] NOT NULL,
	[Root_ContentID] [uniqueidentifier] NOT NULL,
	[Parent_ContentID] [uniqueidentifier] NULL,
	[IsLatestVersion] [bit] NULL,
	[TitleBar] [varchar](256) NULL,
	[NavMenuText] [varchar](256) NULL,
	[PageHead] [varchar](256) NULL,
	[PageText] [varchar](max) NULL,
	[LeftPageText] [varchar](max) NULL,
	[RightPageText] [varchar](max) NULL,
	[NavOrder] [int] NULL,
	[EditUserId] [uniqueidentifier] NULL,
	[EditDate] [datetime] NOT NULL,
	[TemplateFile] [nvarchar](256) NULL,
	[MetaKeyword] [varchar](1000) NULL,
	[MetaDescription] [varchar](2000) NULL,
 CONSTRAINT [PK_carrot_Content] PRIMARY KEY CLUSTERED 
(
	[ContentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[carrot_Widget]    Script Date: 08/12/2012 18:43:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_Widget](
	[Root_WidgetID] [uniqueidentifier] NOT NULL,
	[Root_ContentID] [uniqueidentifier] NOT NULL,
	[WidgetOrder] [int] NOT NULL,
	[PlaceholderName] [varchar](256) NOT NULL,
	[ControlPath] [varchar](512) NOT NULL,
	[WidgetActive] [bit] NOT NULL,
 CONSTRAINT [PK_carrot_Widget] PRIMARY KEY CLUSTERED 
(
	[Root_WidgetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[carrot_UserSiteMapping]    Script Date: 08/12/2012 18:43:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[carrot_UserSiteMapping](
	[UserSiteMappingID] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [carrot_UserSiteMapping_PK] PRIMARY KEY CLUSTERED 
(
	[UserSiteMappingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[carrot_WidgetData]    Script Date: 08/12/2012 18:43:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_WidgetData](
	[WidgetDataID] [uniqueidentifier] NOT NULL,
	[Root_WidgetID] [uniqueidentifier] NOT NULL,
	[IsLatestVersion] [bit] NULL,
	[EditDate] [datetime] NOT NULL,
	[ControlProperties] [varchar](max) NULL,
 CONSTRAINT [PK_carrot_WidgetData] PRIMARY KEY CLUSTERED 
(
	[WidgetDataID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_carrot_Content_ContentID]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_Content] ADD  CONSTRAINT [DF_carrot_Content_ContentID]  DEFAULT (newid()) FOR [ContentID]
GO
/****** Object:  Default [DF_carrot_Content_EditDate]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_Content] ADD  CONSTRAINT [DF_carrot_Content_EditDate]  DEFAULT (getdate()) FOR [EditDate]
GO
/****** Object:  Default [DF_carrot_RootContent_Root_ContentID]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_Root_ContentID]  DEFAULT (newid()) FOR [Root_ContentID]
GO
/****** Object:  Default [DF_carrot_RootContent_CreateDate]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_carrot_SerialCache_SerialCacheID]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_SerialCache] ADD  CONSTRAINT [DF_carrot_SerialCache_SerialCacheID]  DEFAULT (newid()) FOR [SerialCacheID]
GO
/****** Object:  Default [DF_carrot_SerialCache_EditDate]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_SerialCache] ADD  CONSTRAINT [DF_carrot_SerialCache_EditDate]  DEFAULT (getdate()) FOR [EditDate]
GO
/****** Object:  Default [DF_carrot_Sites_SiteID]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_Sites] ADD  CONSTRAINT [DF_carrot_Sites_SiteID]  DEFAULT (newid()) FOR [SiteID]
GO
/****** Object:  Default [DF_carrot_UserSiteMapping_UserSiteMappingID]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_UserSiteMapping] ADD  CONSTRAINT [DF_carrot_UserSiteMapping_UserSiteMappingID]  DEFAULT (newid()) FOR [UserSiteMappingID]
GO
/****** Object:  Default [DF_carrot_Widget_Root_WidgetID]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_Widget] ADD  CONSTRAINT [DF_carrot_Widget_Root_WidgetID]  DEFAULT (newid()) FOR [Root_WidgetID]
GO
/****** Object:  Default [DF_carrot_WidgetData_WidgetDataID]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_WidgetData] ADD  CONSTRAINT [DF_carrot_WidgetData_WidgetDataID]  DEFAULT (newid()) FOR [WidgetDataID]
GO
/****** Object:  Default [DF_carrot_WidgetData_EditDate]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_WidgetData] ADD  CONSTRAINT [DF_carrot_WidgetData_EditDate]  DEFAULT (getdate()) FOR [EditDate]
GO
/****** Object:  ForeignKey [carrot_Content_EditUserId_FK]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_Content]  WITH CHECK ADD  CONSTRAINT [carrot_Content_EditUserId_FK] FOREIGN KEY([EditUserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[carrot_Content] CHECK CONSTRAINT [carrot_Content_EditUserId_FK]
GO
/****** Object:  ForeignKey [carrot_RootContent_carrot_Content_FK]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_Content]  WITH CHECK ADD  CONSTRAINT [carrot_RootContent_carrot_Content_FK] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])
GO
ALTER TABLE [dbo].[carrot_Content] CHECK CONSTRAINT [carrot_RootContent_carrot_Content_FK]
GO
/****** Object:  ForeignKey [carrot_Sites_carrot_RootContent_FK]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_RootContent]  WITH CHECK ADD  CONSTRAINT [carrot_Sites_carrot_RootContent_FK] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])
GO
ALTER TABLE [dbo].[carrot_RootContent] CHECK CONSTRAINT [carrot_Sites_carrot_RootContent_FK]
GO
/****** Object:  ForeignKey [aspnet_Users_carrot_UserSiteMapping_FK]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_UserSiteMapping]  WITH CHECK ADD  CONSTRAINT [aspnet_Users_carrot_UserSiteMapping_FK] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[carrot_UserSiteMapping] CHECK CONSTRAINT [aspnet_Users_carrot_UserSiteMapping_FK]
GO
/****** Object:  ForeignKey [carrot_Sites_carrot_UserSiteMapping_FK]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_UserSiteMapping]  WITH CHECK ADD  CONSTRAINT [carrot_Sites_carrot_UserSiteMapping_FK] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])
GO
ALTER TABLE [dbo].[carrot_UserSiteMapping] CHECK CONSTRAINT [carrot_Sites_carrot_UserSiteMapping_FK]
GO
/****** Object:  ForeignKey [carrot_RootContent_carrot_Widget_FK]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_Widget]  WITH CHECK ADD  CONSTRAINT [carrot_RootContent_carrot_Widget_FK] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])
GO
ALTER TABLE [dbo].[carrot_Widget] CHECK CONSTRAINT [carrot_RootContent_carrot_Widget_FK]
GO
/****** Object:  ForeignKey [carrot_WidgetData_Root_WidgetID_FK]    Script Date: 08/12/2012 18:43:47 ******/
ALTER TABLE [dbo].[carrot_WidgetData]  WITH CHECK ADD  CONSTRAINT [carrot_WidgetData_Root_WidgetID_FK] FOREIGN KEY([Root_WidgetID])
REFERENCES [dbo].[carrot_Widget] ([Root_WidgetID])
GO
ALTER TABLE [dbo].[carrot_WidgetData] CHECK CONSTRAINT [carrot_WidgetData_Root_WidgetID_FK]
GO


--===========================================
--              COPY DATA, can run more than once if delete tables has not been engaged
--===========================================

GO

if (select COUNT(*) from [dbo].[carrot_Sites]) < 1 BEGIN

	INSERT INTO [dbo].[carrot_Sites]
			   ([SiteID]
			   ,[MetaKeyword]
			   ,[MetaDescription]
			   ,[SiteName]
			   ,[MainURL]
			   ,[BlockIndex]
			   ,[SiteFolder])

	SELECT [SiteID]
		  ,[MetaKeyword]
		  ,[MetaDescription]
		  ,[SiteName]
		  ,[MainURL]
		  ,[BlockIndex]
		  ,[SiteFolder]
	  FROM [dbo].[tblSites]
	  
END


if (select COUNT(*) from [dbo].[carrot_RootContent]) < 1 BEGIN

	INSERT INTO [dbo].[carrot_RootContent]
			   ([Root_ContentID]
			   ,[SiteID]
			   ,[Heartbeat_UserId]
			   ,[EditHeartbeat]
			   ,[FileName]
			   ,[PageActive]
			   ,[CreateDate])

	SELECT [Root_ContentID]
		  ,[SiteID]
		  ,[Heartbeat_UserId]
		  ,[EditHeartbeat]
		  ,[FileName]
		  ,[PageActive]
		  ,[CreateDate]
	  FROM [dbo].[tblRootContent]

END


if (select COUNT(*) from [dbo].[carrot_Content]) < 1 BEGIN

	INSERT INTO [dbo].[carrot_Content]
			   ([ContentID]
			   ,[Root_ContentID]
			   ,[Parent_ContentID]
			   ,[IsLatestVersion]
			   ,[TitleBar]
			   ,[NavMenuText]
			   ,[PageHead]
			   ,[PageText]
			   ,[LeftPageText]
			   ,[RightPageText]
			   ,[NavOrder]
			   ,[EditUserId]
			   ,[EditDate]
			   ,[TemplateFile]
			   ,[MetaKeyword]
			   ,[MetaDescription])

	SELECT [ContentID]
		  ,[Root_ContentID]
		  ,[Parent_ContentID]
		  ,[IsLatestVersion]
		  ,[TitleBar]
		  ,[NavMenuText]
		  ,[PageHead]
		  ,[PageText]
		  ,[LeftPageText]
		  ,[RightPageText]
		  ,[NavOrder]
		  ,[EditUserId]
		  ,[EditDate]
		  ,[TemplateFile]
		  ,[MetaKeyword]
		  ,[MetaDescription]
	  FROM [dbo].[tblContent]

END


if (select COUNT(*) from [dbo].[carrot_Widget]) < 1 BEGIN

	INSERT INTO [dbo].[carrot_Widget]
			   ([Root_WidgetID]
			   ,[Root_ContentID]
			   ,[WidgetOrder]
			   ,[PlaceholderName]
			   ,[ControlPath]
			   ,[WidgetActive])

	SELECT [Root_WidgetID]
		  ,[Root_ContentID]
		  ,[WidgetOrder]
		  ,[PlaceholderName]
		  ,[ControlPath]
		  ,[WidgetActive]
	  FROM [dbo].[tblWidget]

END


if (select COUNT(*) from [dbo].[carrot_WidgetData]) < 1 BEGIN

	INSERT INTO [dbo].[carrot_WidgetData]
			   ([WidgetDataID]
			   ,[Root_WidgetID]
			   ,[IsLatestVersion]
			   ,[EditDate]
			   ,[ControlProperties])
	           
		SELECT [WidgetDataID]
			  ,[Root_WidgetID]
			  ,[IsLatestVersion]
			  ,[EditDate]
			  ,[ControlProperties]
		  FROM [dbo].[tblWidgetData]

END



if (select COUNT(*) from [dbo].[carrot_SerialCache]) < 1 BEGIN

	INSERT INTO [dbo].[carrot_SerialCache]
			   ([SerialCacheID]
			   ,[SiteID]
			   ,[ItemID]
			   ,[EditUserId]
			   ,[KeyType]
			   ,[SerializedData]
			   ,[EditDate])

	SELECT [SerialCacheID]
		  ,[SiteID]
		  ,[ItemID]
		  ,[EditUserId]
		  ,[KeyType]
		  ,[SerializedData]
		  ,[EditDate]
	  FROM [dbo].[tblSerialCache]

END

--==========================================================

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblPageWidgets]') AND type in (N'U'))
	AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblWidget]') AND type in (N'U'))
	AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblWidgetData]') AND type in (N'U')) BEGIN
	ALTER TABLE [dbo].[tblPageWidgets] DROP CONSTRAINT [tblRootContent_tblPageWidgets_FK]
	ALTER TABLE [dbo].[tblPageWidgets] DROP CONSTRAINT [DF_tblPageWidgets_PageWidgetID]
	DROP TABLE [dbo].[tblPageWidgets]
END


GO


--==========================================================
-- DELETE OLD TABLES, set @DeleteTables = 1 to actually delete 
--==========================================================

declare @DeleteTables bit

set @DeleteTables = 1


IF @DeleteTables = 1 BEGIN
	
	SELECT 'delete tables has been set' as msg

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblSerialCache]') AND type in (N'U')) BEGIN
		ALTER TABLE [dbo].[tblSerialCache] DROP CONSTRAINT [DF_tblSerialCache_SerialCacheID]
		ALTER TABLE [dbo].[tblSerialCache] DROP CONSTRAINT [DF_tblSerialCache_EditDate]
		DROP TABLE [dbo].[tblSerialCache]
	END

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblUserSiteMapping]') AND type in (N'U')) BEGIN
		ALTER TABLE [dbo].[tblUserSiteMapping] DROP CONSTRAINT [aspnet_Users_tblUserSiteMapping_FK]
		ALTER TABLE [dbo].[tblUserSiteMapping] DROP CONSTRAINT [tblSites_tblUserSiteMapping_FK]
		ALTER TABLE [dbo].[tblUserSiteMapping] DROP CONSTRAINT [DF_tblUserSiteMapping_UserSiteMappingID]
		DROP TABLE [dbo].[tblUserSiteMapping]
	END

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblWidgetData]') AND type in (N'U')) BEGIN
		ALTER TABLE [dbo].[tblWidgetData] DROP CONSTRAINT [tblWidgetData_Root_WidgetID_FK]
		ALTER TABLE [dbo].[tblWidgetData] DROP CONSTRAINT [DF_tblWidgetData_WidgetDataID]
		ALTER TABLE [dbo].[tblWidgetData] DROP CONSTRAINT [DF_tblWidgetData_EditDate]
		DROP TABLE [dbo].[tblWidgetData]
	END

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblWidget]') AND type in (N'U')) BEGIN
		ALTER TABLE [dbo].[tblWidget] DROP CONSTRAINT [tblRootContent_tblWidget_FK]
		ALTER TABLE [dbo].[tblWidget] DROP CONSTRAINT [DF_tblWidget_Root_WidgetID]
		DROP TABLE [dbo].[tblWidget]
	END

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblContent]') AND type in (N'U')) BEGIN
		ALTER TABLE [dbo].[tblContent] DROP CONSTRAINT [tblContent_EditUserId_FK]
		ALTER TABLE [dbo].[tblContent] DROP CONSTRAINT [tblRootContent_tblContent_FK]
		ALTER TABLE [dbo].[tblContent] DROP CONSTRAINT [DF_tblContent_ContentID]
		ALTER TABLE [dbo].[tblContent] DROP CONSTRAINT [DF_tblContent_EditDate]
		DROP TABLE [dbo].[tblContent]
	END

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblRootContent]') AND type in (N'U')) BEGIN
		ALTER TABLE [dbo].[tblRootContent] DROP CONSTRAINT [tblSites_tblRootContent_FK]
		ALTER TABLE [dbo].[tblRootContent] DROP CONSTRAINT [DF_tblRootContent_Root_ContentID]
		ALTER TABLE [dbo].[tblRootContent] DROP CONSTRAINT [DF_tblRootContent_CreateDate]
		DROP TABLE [dbo].[tblRootContent]
	END

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblSites]') AND type in (N'U')) BEGIN
		ALTER TABLE [dbo].[tblSites] DROP CONSTRAINT [DF_tblSites_SiteID]
		DROP TABLE [dbo].[tblSites]
	END

END

GO

IF not exists(select * from dbo.[aspnet_Roles] where RoleName = 'CarrotCMS Administrators' ) BEGIN	

	update dbo.[aspnet_Roles]
	set RoleName = 'CarrotCMS Administrators'
	where RoleName = 'Administrators'

	update dbo.[aspnet_Roles]
	set RoleName = 'CarrotCMS Editors'
	where RoleName = 'Editors'

	update dbo.[aspnet_Roles]
	set RoleName = 'CarrotCMS Users'
	where RoleName = 'Users'

	update dbo.[aspnet_Roles]
	set LoweredRoleName = LOWER(RoleName)

END	