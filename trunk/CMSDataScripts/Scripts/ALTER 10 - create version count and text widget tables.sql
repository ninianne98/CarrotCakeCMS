--====================
GO

ALTER VIEW [dbo].[vw_carrot_Content]
AS 

SELECT rc.Root_ContentID, rc.SiteID, rc.Heartbeat_UserId, rc.EditHeartbeat, rc.[FileName], rc.PageActive, rc.ShowInSiteNav, rc.ShowInSiteMap, rc.BlockIndex,
		rc.CreateUserId, rc.CreateDate, c.ContentID, c.Parent_ContentID, c.IsLatestVersion, c.TitleBar, c.NavMenuText, c.PageHead, 
		c.PageText, c.LeftPageText, c.RightPageText, c.NavOrder, c.EditUserId, c.EditDate, c.TemplateFile, c.MetaKeyword, c.MetaDescription,
		cvh.VersionCount, ct.ContentTypeID, ct.ContentTypeValue, rc.PageSlug, rc.PageThumbnail, s.TimeZone,
		rc.RetireDate, rc.GoLiveDate, rc.GoLiveDateLocal,
		cast(case when rc.RetireDate < GetUTCDate() then 1 else 0 end as bit) as IsRetired,
		cast(case when rc.GoLiveDate > GetUTCDate() then 1 else 0 end as bit) as IsUnReleased
FROM [dbo].carrot_RootContent AS rc 
INNER JOIN [dbo].carrot_Sites AS s ON rc.SiteID = s.SiteID 
INNER JOIN [dbo].carrot_Content AS c ON rc.Root_ContentID = c.Root_ContentID 
INNER JOIN [dbo].carrot_ContentType AS ct ON rc.ContentTypeID = ct.ContentTypeID
INNER JOIN (SELECT COUNT(*) VersionCount, Root_ContentID 
			FROM [dbo].carrot_Content
			GROUP BY Root_ContentID 
			) cvh on rc.Root_ContentID = cvh.Root_ContentID


GO

--===================

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_carrot_TextWidget_SiteID]') )
ALTER TABLE [dbo].[carrot_TextWidget] DROP CONSTRAINT [FK_carrot_TextWidget_SiteID]

IF  EXISTS (SELECT * FROM [dbo].sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_TextWidget_TextWidgetID]') AND type = 'D')
ALTER TABLE [dbo].[carrot_TextWidget] DROP CONSTRAINT [DF_carrot_TextWidget_TextWidgetID]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[carrot_TextWidget]') AND type in (N'U')) BEGIN

	CREATE TABLE [dbo].[carrot_TextWidget](
		[TextWidgetID] [uniqueidentifier] NOT NULL,
		[SiteID] [uniqueidentifier] NOT NULL,
		[TextWidgetAssembly] [nvarchar](256) NOT NULL,
		[ProcessBody] [bit] NOT NULL,
		[ProcessPlainText] [bit] NOT NULL,
		[ProcessHTMLText] [bit] NOT NULL,
		[ProcessComment] [bit] NOT NULL,
		[ProcessSnippet] [bit] NOT NULL,
	 CONSTRAINT [PK_carrot_TextWidget] PRIMARY KEY NONCLUSTERED 
	(
		[TextWidgetID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

GO

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_TextWidget' and column_name = 'ProcessSnippet') BEGIN

	ALTER TABLE [dbo].[carrot_TextWidget] ADD [ProcessSnippet] [bit] NULL

END

GO

ALTER TABLE [dbo].[carrot_TextWidget] 
	ALTER COLUMN  [ProcessSnippet] [bit] NULL

GO

UPDATE [dbo].[carrot_TextWidget]
SET [ProcessSnippet] = 0
WHERE IsNUll([ProcessSnippet], 0) = 0

ALTER TABLE [dbo].[carrot_TextWidget] 
	ALTER COLUMN  [ProcessSnippet] [bit] NOT NULL

GO

--===================

GO

ALTER TABLE [dbo].[carrot_TextWidget]  WITH CHECK ADD  CONSTRAINT [FK_carrot_TextWidget_SiteID] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])
GO

ALTER TABLE [dbo].[carrot_TextWidget] CHECK CONSTRAINT [FK_carrot_TextWidget_SiteID]
GO

ALTER TABLE [dbo].[carrot_TextWidget] ADD  CONSTRAINT [DF_carrot_TextWidget_TextWidgetID]  DEFAULT (newid()) FOR [TextWidgetID]
GO

--==============================

GO

IF  EXISTS (SELECT * FROM [dbo].sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_DataInfo_DataInfoID]') AND type = 'D')
ALTER TABLE [dbo].[carrot_DataInfo] DROP CONSTRAINT [DF_carrot_DataInfo_DataInfoID]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[carrot_DataInfo]') AND type in (N'U')) BEGIN

	CREATE TABLE [dbo].[carrot_DataInfo](
		[DataInfoID] [uniqueidentifier] NOT NULL,
		[DataKey] [nvarchar](256) NOT NULL,
		[DataValue] [nvarchar](256) NOT NULL,
	 CONSTRAINT [PK_carrot_DataInfo] PRIMARY KEY NONCLUSTERED 
	(
		[DataInfoID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

GO

ALTER TABLE [dbo].[carrot_DataInfo] ADD  CONSTRAINT [DF_carrot_DataInfo_DataInfoID]  DEFAULT (newid()) FOR [DataInfoID]

GO

--===================

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_carrot_RootContentSnippet_SiteID]') )
ALTER TABLE [dbo].[carrot_RootContentSnippet] DROP CONSTRAINT [FK_carrot_RootContentSnippet_SiteID]

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_carrot_ContentSnippet_Root_ContentSnippetID]') )
ALTER TABLE [dbo].[carrot_ContentSnippet] DROP CONSTRAINT [FK_carrot_ContentSnippet_Root_ContentSnippetID]

IF  EXISTS (SELECT * FROM [dbo].sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_RootContentSnippet_Root_ContentSnippetID]') AND type = 'D')
ALTER TABLE [dbo].[carrot_RootContentSnippet] DROP CONSTRAINT [DF_carrot_RootContentSnippet_Root_ContentSnippetID]

IF  EXISTS (SELECT * FROM [dbo].sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_ContentSnippet_ContentSnippetID]') AND type = 'D')
ALTER TABLE [dbo].[carrot_ContentSnippet] DROP CONSTRAINT [DF_carrot_ContentSnippet_ContentSnippetID]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[carrot_RootContentSnippet]') AND type in (N'U')) BEGIN

	CREATE TABLE [dbo].[carrot_RootContentSnippet](
		[Root_ContentSnippetID] [uniqueidentifier] NOT NULL,
		[SiteID] [uniqueidentifier] NOT NULL,
		[ContentSnippetName] [nvarchar](256) NOT NULL,
		[ContentSnippetSlug] [nvarchar](128) NOT NULL,
		[Heartbeat_UserId] [uniqueidentifier] NULL,
		[EditHeartbeat] [datetime] NULL,
		[CreateUserId] [uniqueidentifier] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[GoLiveDate] [datetime] NOT NULL,
		[RetireDate] [datetime] NOT NULL,		
		[ContentSnippetActive] [bit] NOT NULL,
	 CONSTRAINT [PK_carrot_RootContentSnippet] PRIMARY KEY CLUSTERED 
	(
		[Root_ContentSnippetID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[carrot_ContentSnippet]') AND type in (N'U')) BEGIN

	CREATE TABLE [dbo].[carrot_ContentSnippet](
		[ContentSnippetID] [uniqueidentifier] NOT NULL,
		[Root_ContentSnippetID] [uniqueidentifier] NOT NULL,
		[IsLatestVersion] [bit] NOT NULL,
		[EditUserId] [uniqueidentifier] NOT NULL,
		[EditDate] [datetime] NOT NULL,
		[ContentBody] [nvarchar](max) NULL,
	 CONSTRAINT [PK_carrot_ContentSnippet] PRIMARY KEY CLUSTERED 
	(
		[ContentSnippetID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

GO

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_RootContentSnippet' and column_name = 'EditHeartbeat') BEGIN

	ALTER TABLE [dbo].[carrot_RootContentSnippet] ADD [Heartbeat_UserId] [uniqueidentifier] NULL
	ALTER TABLE [dbo].[carrot_RootContentSnippet] ADD [EditHeartbeat] [datetime] NULL	

END

GO

ALTER TABLE [dbo].[carrot_RootContentSnippet] ADD  CONSTRAINT [DF_carrot_RootContentSnippet_Root_ContentSnippetID]  DEFAULT (newid()) FOR [Root_ContentSnippetID]
ALTER TABLE [dbo].[carrot_ContentSnippet] ADD  CONSTRAINT [DF_carrot_ContentSnippet_ContentSnippetID]  DEFAULT (newid()) FOR [ContentSnippetID]
GO


ALTER TABLE [dbo].[carrot_RootContentSnippet]  WITH CHECK ADD  CONSTRAINT [FK_carrot_RootContentSnippet_SiteID] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])
GO

ALTER TABLE [dbo].[carrot_RootContentSnippet] CHECK CONSTRAINT [FK_carrot_RootContentSnippet_SiteID]
GO


ALTER TABLE [dbo].[carrot_ContentSnippet]  WITH CHECK ADD  CONSTRAINT [FK_carrot_ContentSnippet_Root_ContentSnippetID] FOREIGN KEY([Root_ContentSnippetID])
REFERENCES [dbo].[carrot_RootContentSnippet] ([Root_ContentSnippetID])
GO

ALTER TABLE [dbo].[carrot_ContentSnippet] CHECK CONSTRAINT [FK_carrot_ContentSnippet_Root_ContentSnippetID]
GO


IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_carrot_ContentSnippet]'))
DROP VIEW [dbo].[vw_carrot_ContentSnippet]

GO

--===================

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_carrot_ContentSnippet]
AS 

SELECT csr.Root_ContentSnippetID, csr.SiteID, csr.ContentSnippetName, csr.ContentSnippetSlug, csr.CreateUserId, csr.CreateDate, 
	csr.ContentSnippetActive, cs.ContentSnippetID, cs.IsLatestVersion, cs.EditUserId, cs.EditDate, cs.ContentBody, 
	csr.Heartbeat_UserId, csr.EditHeartbeat, csr.GoLiveDate, csr.RetireDate,
	cast(case when csr.RetireDate < GetUTCDate() then 1 else 0 end as bit) as IsRetired,
	cast(case when csr.GoLiveDate > GetUTCDate() then 1 else 0 end as bit) as IsUnReleased,
	csvh.VersionCount
FROM carrot_RootContentSnippet AS csr 
	INNER JOIN carrot_ContentSnippet AS cs ON csr.Root_ContentSnippetID = cs.Root_ContentSnippetID
	INNER JOIN (SELECT COUNT(*) VersionCount, Root_ContentSnippetID 
				FROM [dbo].carrot_ContentSnippet
				GROUP BY Root_ContentSnippetID 
				) csvh on csr.Root_ContentSnippetID = csvh.Root_ContentSnippetID


GO

--===============================

GO

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_UserData' and column_name = 'UserBio') BEGIN

	ALTER TABLE [dbo].[carrot_UserData] ADD [UserBio] [nvarchar](max) NULL

END

GO

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_Sites' and column_name = 'Blog_EditorPath') BEGIN

	ALTER TABLE [dbo].[carrot_Sites] ADD [Blog_EditorPath] [nvarchar](64) NULL

END

GO

update [dbo].[carrot_Sites]
set [Blog_EditorPath] = 'author_editor'
where [Blog_EditorPath] is null
		and ([Blog_CategoryPath] like 'author%'
				or [Blog_TagPath] like 'author%'
				or [Blog_DatePath] like 'author%')

update [dbo].[carrot_Sites]
set [Blog_EditorPath] = 'author'
where [Blog_EditorPath] is null

--update [dbo].[carrot_Sites]
--set [Blog_EditorPath] = REPLACE([Blog_EditorPath], '/', '')


GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_carrot_EditorURL]'))
DROP VIEW [dbo].[vw_carrot_EditorURL]

GO

CREATE VIEW [dbo].[vw_carrot_EditorURL]
AS 

SELECT  d.SiteID, d.UserId, d.UserName, d.LoweredEmail, ISNULL(c.TheCount, 0) AS UseCount,
		'/'+d.Blog_FolderPath +'/'+ d.Blog_EditorPath +'/'+ d.UserName + '.aspx' as UserUrl
FROM (
	SELECT s.SiteID, s.Blog_FolderPath, s.Blog_EditorPath, m.UserId, m.UserName, m.LoweredEmail
		FROM [dbo].vw_aspnet_MembershipUsers m, [dbo].carrot_Sites s
	) as d
LEFT JOIN (
		SELECT EditUserId, SiteID, COUNT(ContentID) AS TheCount
		FROM dbo.vw_carrot_Content
		where IsLatestVersion = 1
		GROUP BY EditUserId, SiteID
		) AS c on d.UserId = c.EditUserId
				and d.SiteID = c.SiteID

GO


ALTER VIEW [dbo].[vw_carrot_UserData]
AS 


SELECT m.UserId, ud.UserNickName, ud.FirstName, ud.LastName, m.LoweredEmail, m.IsApproved, m.IsLockedOut, 
	m.CreateDate, m.LastLoginDate, m.UserName, m.LastActivityDate, ud.UserBio
FROM [dbo].vw_aspnet_MembershipUsers AS m 
LEFT JOIN [dbo].carrot_UserData AS ud ON m.UserId = ud.UserId


GO

