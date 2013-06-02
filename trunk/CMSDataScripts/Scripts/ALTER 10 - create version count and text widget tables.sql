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
	 CONSTRAINT [PK_carrot_TextWidget] PRIMARY KEY NONCLUSTERED 
	(
		[TextWidgetID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

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


