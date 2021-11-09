-- 2012-11-19
-- addition of blogging tables

-- USE [CarrotwareCMS]

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [IsLatestVersion] [bit] NULL

GO

UPDATE [dbo].[carrot_Content] 
SET [IsLatestVersion] = 0
WHERE ISNULL([IsLatestVersion], 0) = 0

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [IsLatestVersion] [bit] NOT NULL

GO

UPDATE [dbo].[carrot_Content]
SET [NavOrder] = 5
WHERE ISNULL([NavOrder], -1) = -1

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [NavOrder] [int] NOT NULL

GO

ALTER TABLE [dbo].[carrot_WidgetData] 
	ALTER COLUMN  [IsLatestVersion] [bit] NULL

GO

UPDATE [dbo].[carrot_WidgetData] 
SET [IsLatestVersion] = 0
WHERE ISNULL([IsLatestVersion], 0) = 0

GO

ALTER TABLE [dbo].[carrot_WidgetData] 
	ALTER COLUMN  [IsLatestVersion] [bit] NOT NULL

GO

IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_ContentType' and column_name = 'ContentTypeValue') BEGIN 

	CREATE TABLE [dbo].[carrot_ContentType](
		[ContentTypeID] [uniqueidentifier] NOT NULL,
		[ContentTypeValue] [nvarchar](256) NULL,
	 CONSTRAINT [carrot_ContentType_PK] PRIMARY KEY CLUSTERED 
	(
		[ContentTypeID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END


GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_ContentType_ContentTypeID]')  ) BEGIN

	ALTER TABLE [dbo].[carrot_ContentType] DROP CONSTRAINT [DF_carrot_ContentType_ContentTypeID]

END


ALTER TABLE [dbo].[carrot_ContentType] ADD  CONSTRAINT [DF_carrot_ContentType_ContentTypeID]  DEFAULT (newid()) FOR [ContentTypeID]


GO


IF ((select count(*) from [dbo].[carrot_ContentType] where [ContentTypeValue] = N'ContentEntry') < 1) BEGIN

	insert into [dbo].[carrot_ContentType]([ContentTypeValue])
	values('BlogEntry')

	insert into [dbo].[carrot_ContentType]([ContentTypeValue])
	values('ContentEntry')

END


GO


IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_RootContent' and column_name = 'ContentTypeID') BEGIN

	ALTER TABLE [dbo].[carrot_RootContent] ADD [ContentTypeID] [uniqueidentifier] NULL

END

GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [ContentTypeID] [uniqueidentifier] NULL

GO



DECLARE @ContentTypeID uniqueidentifier

SET @ContentTypeID = (select top 1 [ContentTypeID] from [dbo].[carrot_ContentType] where [ContentTypeValue] = N'ContentEntry' )


UPDATE [dbo].[carrot_RootContent]
SET [ContentTypeID] = @ContentTypeID
WHERE [ContentTypeID] is null


UPDATE carrot_Content
SET NavOrder = 10
FROM [dbo].carrot_RootContent 
	INNER JOIN [dbo].carrot_Content ON carrot_RootContent.Root_ContentID = carrot_Content.Root_ContentID
WHERE carrot_RootContent.ContentTypeID <> @ContentTypeID



GO


ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [ContentTypeID] [uniqueidentifier] NOT NULL

GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[carrot_ContentType_carrot_RootContent_FK]') ) BEGIN
	
	ALTER TABLE [dbo].[carrot_RootContent] DROP CONSTRAINT [carrot_ContentType_carrot_RootContent_FK]
	
END

GO


ALTER TABLE [dbo].[carrot_RootContent]  WITH CHECK ADD  CONSTRAINT [carrot_ContentType_carrot_RootContent_FK] FOREIGN KEY([ContentTypeID])
	REFERENCES [dbo].[carrot_ContentType] ([ContentTypeID])

GO


ALTER TABLE [dbo].[carrot_RootContent] CHECK CONSTRAINT [carrot_ContentType_carrot_RootContent_FK]

GO




--============== CREATE Category and Tag Tables ========================


IF EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_Sites' and column_name = 'SiteFolder') BEGIN

	ALTER TABLE carrot_Sites DROP COLUMN SiteFolder

END


IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_Sites' and column_name = 'SiteTagline') BEGIN

	ALTER TABLE [dbo].[carrot_Sites] ADD [SiteTagline] [nvarchar](1024) NULL
	
	ALTER TABLE [dbo].[carrot_Sites] ADD [SiteTitlebarPattern] [nvarchar](1024) NULL

END


IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_Sites' and column_name = 'Blog_Root_ContentID') BEGIN

	ALTER TABLE [dbo].[carrot_Sites] ADD [Blog_Root_ContentID] [uniqueidentifier] NULL
	
	ALTER TABLE [dbo].[carrot_Sites] ADD [Blog_FolderPath] [nvarchar](64) NULL
	
	ALTER TABLE [dbo].[carrot_Sites] ADD [Blog_CategoryPath] [nvarchar](64) NULL
	
	ALTER TABLE [dbo].[carrot_Sites] ADD [Blog_TagPath] [nvarchar](64) NULL
	
	ALTER TABLE [dbo].[carrot_Sites] ADD [Blog_DatePattern] [nvarchar](32) NULL

END


IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_RootContent' and column_name = 'PageSlug') BEGIN
	
	ALTER TABLE [dbo].[carrot_RootContent] ADD [PageSlug] [nvarchar](256) NULL
	
END	


IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_RootContent' and column_name = 'PageThumbnail') BEGIN
	
	ALTER TABLE [dbo].[carrot_RootContent] ADD [PageThumbnail] [nvarchar](128) NULL
	
END	


GO

update [dbo].[carrot_Widget]
set [ControlPath] = 'CLASS:Carrotware.CMS.UI.Controls.ContentRichText, Carrotware.CMS.UI.Controls'
where [ControlPath] = '~/Manage/ucGenericContent.ascx'
  
update [dbo].[carrot_Widget]
set [ControlPath] = 'CLASS:Carrotware.CMS.UI.Controls.ContentPlainText, Carrotware.CMS.UI.Controls'
where [ControlPath] = '~/Manage/ucTextContent.ascx'  


update [dbo].[carrot_Sites]
set [Blog_FolderPath] = 'archive',
	[Blog_CategoryPath] = 'category',
	[Blog_TagPath] = 'tag',
	[Blog_DatePattern] = 'yyyy/MM/dd'
where isnull([Blog_FolderPath], '') = ''


update [dbo].[carrot_Sites]
set [Blog_DatePattern] = 'yyyy/MM/dd'
where isnull([Blog_DatePattern], '') = ''


--SELECT s.SiteID, rc.[FileName]
--FROM [dbo].carrot_Sites AS s 
--INNER JOIN [dbo].carrot_RootContent AS rc ON s.SiteID = rc.SiteID
--where rc.[FileName] like rtrim(ltrim(s.Blog_FolderPath)) + '%'


update [dbo].[carrot_Sites]
set [Blog_FolderPath] = ltrim(rtrim([Blog_FolderPath]))


update s
set Blog_FolderPath = 'archive1'
FROM [dbo].carrot_Sites AS s 
INNER JOIN [dbo].carrot_RootContent AS rc ON s.SiteID = rc.SiteID
where rc.[FileName] like '/'+s.Blog_FolderPath+'/' + '%'

update s
set Blog_FolderPath = 'archive2'
FROM [dbo].carrot_Sites AS s 
INNER JOIN [dbo].carrot_RootContent AS rc ON s.SiteID = rc.SiteID
where rc.[FileName] like '/'+s.Blog_FolderPath+'/' + '%'

update s
set Blog_FolderPath = 'archive3'
FROM [dbo].carrot_Sites AS s 
INNER JOIN [dbo].carrot_RootContent AS rc ON s.SiteID = rc.SiteID
where rc.[FileName] like '/'+s.Blog_FolderPath+'/' + '%'


update [dbo].[carrot_Sites]
set [Blog_FolderPath] = REPLACE([Blog_FolderPath], '/', ''),
	[Blog_CategoryPath] = REPLACE([Blog_CategoryPath], '/', ''),
	[Blog_TagPath] = REPLACE([Blog_TagPath], '/', '')


--=================== define tags


GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_ContentTag_ContentTagID]') ) BEGIN

	ALTER TABLE [dbo].[carrot_ContentTag] DROP CONSTRAINT [DF_carrot_ContentTag_ContentTagID]

END	

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_carrot_ContentTag_SiteID]')  ) BEGIN

	ALTER TABLE [dbo].[carrot_ContentTag] DROP CONSTRAINT [FK_carrot_ContentTag_SiteID]

END

GO

IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_ContentTag' and column_name = 'SiteID') BEGIN 

		CREATE TABLE [dbo].[carrot_ContentTag](
			[ContentTagID] [uniqueidentifier] NOT NULL,
			[SiteID] [uniqueidentifier] NOT NULL,	
			[TagText] [nvarchar](256) NOT NULL,
			[TagSlug] [nvarchar](256) NOT NULL,
		 CONSTRAINT [PK_carrot_ContentTag] PRIMARY KEY NONCLUSTERED 
		(
			[ContentTagID] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]


END

GO

ALTER TABLE [dbo].[carrot_ContentTag]  WITH CHECK ADD  CONSTRAINT [FK_carrot_ContentTag_SiteID] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])

GO

ALTER TABLE [dbo].[carrot_ContentTag] CHECK CONSTRAINT [FK_carrot_ContentTag_SiteID]

GO

ALTER TABLE [dbo].[carrot_ContentTag] ADD  CONSTRAINT [DF_carrot_ContentTag_ContentTagID]  DEFAULT (newid()) FOR [ContentTagID]

GO


--================== define keywords


GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_ContentCategory_ContentCategoryID]')  ) BEGIN

	ALTER TABLE [dbo].[carrot_ContentCategory] DROP CONSTRAINT [DF_carrot_ContentCategory_ContentCategoryID]

END	

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_carrot_ContentCategory_SiteID]')  ) BEGIN

	ALTER TABLE [dbo].[carrot_ContentCategory] DROP CONSTRAINT [FK_carrot_ContentCategory_SiteID]

END

GO

IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_ContentCategory' and column_name = 'SiteID') BEGIN 

	CREATE TABLE [dbo].[carrot_ContentCategory](
		[ContentCategoryID] [uniqueidentifier] NOT NULL,
		[SiteID] [uniqueidentifier] NOT NULL,	
		[CategoryText] [nvarchar](256) NOT NULL,
		[CategorySlug] [nvarchar](256) NOT NULL,
	 CONSTRAINT [PK_carrot_ContentCategory] PRIMARY KEY NONCLUSTERED 
	(
		[ContentCategoryID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

GO

ALTER TABLE [dbo].[carrot_ContentCategory]  WITH CHECK ADD  CONSTRAINT [FK_carrot_ContentCategory_SiteID] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])

GO

ALTER TABLE [dbo].[carrot_ContentCategory] CHECK CONSTRAINT [FK_carrot_ContentCategory_SiteID]

GO

ALTER TABLE [dbo].[carrot_ContentCategory] ADD  CONSTRAINT [DF_carrot_ContentCategory_ContentCategoryID]  DEFAULT (newid()) FOR [ContentCategoryID]

GO


--===================



IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_CategoryContentMapping_CategoryContentMappingID]')  ) BEGIN
	ALTER TABLE [dbo].[carrot_CategoryContentMapping] DROP CONSTRAINT [DF_carrot_CategoryContentMapping_CategoryContentMappingID]
END	

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_carrot_CategoryContentMapping_Root_ContentID]')) BEGIN
	ALTER TABLE [dbo].[carrot_CategoryContentMapping] DROP CONSTRAINT [FK_carrot_CategoryContentMapping_Root_ContentID]
END

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_carrot_CategoryContentMapping_ContentCategoryID]')) BEGIN
	ALTER TABLE [dbo].[carrot_CategoryContentMapping] DROP CONSTRAINT [FK_carrot_CategoryContentMapping_ContentCategoryID]
END


GO


IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_CategoryContentMapping' and column_name = 'Root_ContentID') BEGIN 

	CREATE TABLE [dbo].[carrot_CategoryContentMapping](
		[CategoryContentMappingID] [uniqueidentifier] NOT NULL,
		[ContentCategoryID] [uniqueidentifier] NOT NULL,
		[Root_ContentID] [uniqueidentifier] NOT NULL,
		
	 CONSTRAINT [PK_carrot_CategoryContentMapping] PRIMARY KEY NONCLUSTERED 
	(
		[CategoryContentMappingID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

ALTER TABLE [dbo].[carrot_CategoryContentMapping] ADD  CONSTRAINT [DF_carrot_CategoryContentMapping_CategoryContentMappingID]  DEFAULT (newid()) FOR [CategoryContentMappingID]

GO

ALTER TABLE [dbo].[carrot_CategoryContentMapping]  WITH CHECK ADD  CONSTRAINT [FK_carrot_CategoryContentMapping_Root_ContentID] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])

GO

ALTER TABLE [dbo].[carrot_CategoryContentMapping] CHECK CONSTRAINT [FK_carrot_CategoryContentMapping_Root_ContentID]

GO

ALTER TABLE [dbo].[carrot_CategoryContentMapping]  WITH CHECK ADD  CONSTRAINT [FK_carrot_CategoryContentMapping_ContentCategoryID] FOREIGN KEY([ContentCategoryID])
REFERENCES [dbo].[carrot_ContentCategory] ([ContentCategoryID])

GO

ALTER TABLE [dbo].[carrot_CategoryContentMapping] CHECK CONSTRAINT [FK_carrot_CategoryContentMapping_ContentCategoryID]

GO


--======


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_TagContentMapping_TagContentMappingID]')  ) BEGIN
	ALTER TABLE [dbo].[carrot_TagContentMapping] DROP CONSTRAINT [DF_carrot_TagContentMapping_TagContentMappingID]
END	

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_carrot_TagContentMapping_Root_ContentID]')) BEGIN
	ALTER TABLE [dbo].[carrot_TagContentMapping] DROP CONSTRAINT [FK_carrot_TagContentMapping_Root_ContentID]
END

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_carrot_TagContentMapping_ContentTagID]')) BEGIN
	ALTER TABLE [dbo].[carrot_TagContentMapping] DROP CONSTRAINT [FK_carrot_TagContentMapping_ContentTagID]
END


GO


IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_TagContentMapping' and column_name = 'Root_ContentID') BEGIN 

	CREATE TABLE [dbo].[carrot_TagContentMapping](
		[TagContentMappingID] [uniqueidentifier] NOT NULL,
		[ContentTagID] [uniqueidentifier] NOT NULL,
		[Root_ContentID] [uniqueidentifier] NOT NULL,
		
	 CONSTRAINT [PK_carrot_TagContentMapping] PRIMARY KEY NONCLUSTERED 
	(
		[TagContentMappingID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

ALTER TABLE [dbo].[carrot_TagContentMapping] ADD  CONSTRAINT [DF_carrot_TagContentMapping_TagContentMappingID]  DEFAULT (newid()) FOR [TagContentMappingID]

GO

ALTER TABLE [dbo].[carrot_TagContentMapping]  WITH CHECK ADD  CONSTRAINT [FK_carrot_TagContentMapping_Root_ContentID] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])

GO

ALTER TABLE [dbo].[carrot_TagContentMapping] CHECK CONSTRAINT [FK_carrot_TagContentMapping_Root_ContentID]

GO

ALTER TABLE [dbo].[carrot_TagContentMapping]  WITH CHECK ADD  CONSTRAINT [FK_carrot_TagContentMapping_ContentTagID] FOREIGN KEY([ContentTagID])
REFERENCES [dbo].[carrot_ContentTag] ([ContentTagID])

GO

ALTER TABLE [dbo].[carrot_TagContentMapping] CHECK CONSTRAINT [FK_carrot_TagContentMapping_ContentTagID]

GO



--=============== BLOG COMMENTS


GO


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_ContentComment_ContentCommentID]') ) BEGIN

	ALTER TABLE [dbo].[carrot_ContentComment] DROP CONSTRAINT [DF_carrot_ContentComment_ContentCommentID]

END	

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_ContentComment_CreateDate]') ) BEGIN

	ALTER TABLE [dbo].[carrot_ContentComment] DROP CONSTRAINT [DF_carrot_ContentComment_CreateDate]

END	

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_carrot_ContentComment_Root_ContentID]')  ) BEGIN

	ALTER TABLE [dbo].[carrot_ContentComment] DROP CONSTRAINT [FK_carrot_ContentComment_Root_ContentID]

END

GO

IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_ContentComment' and column_name = 'Root_ContentID') BEGIN 

		CREATE TABLE [dbo].[carrot_ContentComment](
			[ContentCommentID] [uniqueidentifier] NOT NULL,
			[Root_ContentID] [uniqueidentifier] NOT NULL,
			[CreateDate] [datetime] NOT NULL,	
			[CommenterIP] [nvarchar](32) NOT NULL,
			[CommenterName] [nvarchar](256) NOT NULL,
			[CommenterEmail] [nvarchar](256) NOT NULL,
			[CommenterURL] [nvarchar](256) NOT NULL,
			[PostComment] [nvarchar](max) NOT NULL,
			[IsApproved] [bit] NOT NULL,
			[IsSpam] [bit] NOT NULL,
		 CONSTRAINT [PK_carrot_ContentComment] PRIMARY KEY NONCLUSTERED 
		(
			[ContentCommentID] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]

END


GO


ALTER TABLE [dbo].[carrot_ContentComment] 
	ALTER COLUMN  [CommenterName] [nvarchar](256) NOT NULL

ALTER TABLE [dbo].[carrot_ContentComment] 
	ALTER COLUMN  [CommenterEmail] [nvarchar](256) NOT NULL

ALTER TABLE [dbo].[carrot_ContentComment] 
	ALTER COLUMN  [CommenterURL] [nvarchar](256) NOT NULL


GO


IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_ContentComment' and column_name = 'CommenterURL') BEGIN

	ALTER TABLE [dbo].[carrot_ContentComment] ADD [CommenterURL] [nvarchar](128) NULL

END

GO

ALTER TABLE [dbo].[carrot_ContentComment] 
	ALTER COLUMN  [CommenterURL] [nvarchar](256)  NULL

GO

UPDATE [dbo].[carrot_ContentComment] 
SET [CommenterURL] = ''
WHERE ISNULL([CommenterURL], '') = ''

GO

ALTER TABLE [dbo].[carrot_ContentComment] 
	ALTER COLUMN  [CommenterURL] [nvarchar](256) NOT NULL

GO

ALTER TABLE [dbo].[carrot_ContentComment] 
	ALTER COLUMN  [PostComment] [nvarchar](max) NULL

GO

ALTER TABLE [dbo].[carrot_ContentComment]  WITH CHECK ADD  CONSTRAINT [FK_carrot_ContentComment_Root_ContentID] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])

GO

ALTER TABLE [dbo].[carrot_ContentComment] CHECK CONSTRAINT [FK_carrot_ContentComment_Root_ContentID]

GO

ALTER TABLE [dbo].[carrot_ContentComment] ADD  CONSTRAINT [DF_carrot_ContentComment_ContentCommentID]  DEFAULT (newid()) FOR [ContentCommentID]

GO

ALTER TABLE [dbo].[carrot_ContentComment] ADD  CONSTRAINT [DF_carrot_ContentComment_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]

GO


--=============== BEGIN UPDATE Category View===================

GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_carrot_CategoryCounted]'))
	DROP VIEW [dbo].[vw_carrot_CategoryCounted]

GO


CREATE VIEW [dbo].[vw_carrot_CategoryCounted]
AS 


SELECT cc.ContentCategoryID, cc.SiteID, cc.CategoryText, cc.CategorySlug, ISNULL(cc2.TheCount, 0) AS UseCount
FROM dbo.carrot_ContentCategory AS cc 
LEFT JOIN
      (SELECT ContentCategoryID, COUNT(Root_ContentID) AS TheCount
        FROM dbo.carrot_CategoryContentMapping
        GROUP BY ContentCategoryID) AS cc2 ON cc.ContentCategoryID = cc2.ContentCategoryID
  


GO

--=============== END UPDATE Category View===================



--=============== BEGIN UPDATE Tag View===================

GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_carrot_TagCounted]'))
	DROP VIEW [dbo].[vw_carrot_TagCounted]

GO


CREATE VIEW [dbo].[vw_carrot_TagCounted]
AS 


SELECT cc.ContentTagID, cc.SiteID, cc.TagText, cc.TagSlug, ISNULL(cc2.TheCount, 0) AS UseCount
FROM dbo.carrot_ContentTag AS cc 
LEFT JOIN
      (SELECT ContentTagID, COUNT(Root_ContentID) AS TheCount
        FROM dbo.carrot_TagContentMapping
        GROUP BY ContentTagID) AS cc2 ON cc.ContentTagID = cc2.ContentTagID
  


GO

--=============== END UPDATE Tag View===================


--=============== BEGIN UPDATE CategoryURL  View===================

GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_carrot_CategoryURL]'))
	DROP VIEW [dbo].[vw_carrot_CategoryURL]

GO


CREATE VIEW [dbo].[vw_carrot_CategoryURL]
AS 


SELECT  s.SiteID, cc.ContentCategoryID, cc.CategoryText, ISNULL(cc2.TheCount, 0) AS UseCount, 
		'/'+s.Blog_FolderPath +'/'+ s.Blog_CategoryPath +'/'+ cc.CategorySlug + '.aspx' as CategoryUrl
FROM [dbo].carrot_Sites AS s 
INNER JOIN [dbo].carrot_ContentCategory AS cc ON s.SiteID = cc.SiteID
LEFT JOIN
      (SELECT ContentCategoryID, COUNT(Root_ContentID) AS TheCount
        FROM dbo.carrot_CategoryContentMapping
        GROUP BY ContentCategoryID) AS cc2 ON cc.ContentCategoryID = cc2.ContentCategoryID


GO

--=============== END UPDATE CategoryURL View===================



--=============== BEGIN UPDATE TagURL View===================

GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_carrot_TagURL]'))
	DROP VIEW [dbo].[vw_carrot_TagURL]

GO


CREATE VIEW [dbo].[vw_carrot_TagURL]
AS 


SELECT  s.SiteID, cc.ContentTagID, cc.TagText, ISNULL(cc2.TheCount, 0) AS UseCount, 
		'/'+s.Blog_FolderPath +'/'+ s.Blog_TagPath +'/'+ cc.TagSlug + '.aspx' as TagUrl
FROM [dbo].carrot_Sites AS s 
INNER JOIN [dbo].carrot_ContentTag AS cc ON s.SiteID = cc.SiteID
LEFT JOIN
      (SELECT ContentTagID, COUNT(Root_ContentID) AS TheCount
        FROM dbo.carrot_TagContentMapping
        GROUP BY ContentTagID) AS cc2 ON cc.ContentTagID = cc2.ContentTagID
  


GO

--=============== END UPDATE TagURL View===================




--=================== define extended user


GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[FK_carrot_UserData_UserId]')  ) BEGIN

	ALTER TABLE [dbo].[carrot_UserData] DROP CONSTRAINT [FK_carrot_UserData_UserId]

END

GO

IF NOT EXISTS( select * from [INFORMATION_SCHEMA].[COLUMNS] 
		where table_name = 'carrot_UserData' and column_name = 'UserId') BEGIN 

		CREATE TABLE [dbo].[carrot_UserData](
			[UserId] [uniqueidentifier] NOT NULL,
			[UserNickName] [nvarchar](64) NULL,
			[FirstName] [nvarchar](64) NULL,
			[LastName] [nvarchar](64) NULL,
		 CONSTRAINT [PK_carrot_UserData] PRIMARY KEY NONCLUSTERED 
		(
			[UserId] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]


END

GO

ALTER TABLE [dbo].[carrot_UserData]  WITH CHECK ADD  CONSTRAINT [FK_carrot_UserData_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])

GO

ALTER TABLE [dbo].[carrot_UserData] CHECK CONSTRAINT [FK_carrot_UserData_UserId]

GO

--=============== BEGIN UPDATE TagURL View===================

GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_carrot_UserData]'))
	DROP VIEW [dbo].[vw_carrot_UserData]

GO


CREATE VIEW [dbo].[vw_carrot_UserData]
AS 


SELECT   m.UserId, ud.UserNickName, ud.FirstName, ud.LastName, m.LoweredEmail, m.IsApproved, m.IsLockedOut, m.CreateDate, m.LastLoginDate, 
                      m.UserName, m.LastActivityDate
FROM      [dbo].vw_aspnet_MembershipUsers    AS m 
LEFT JOIN [dbo].carrot_UserData AS ud ON m.UserId = ud.UserId


GO

--============

ALTER TABLE [dbo].[carrot_WidgetData] 
	ALTER COLUMN  [ControlProperties] [nvarchar](max) NULL

GO

ALTER TABLE [dbo].[carrot_Widget] 
	ALTER COLUMN  [PlaceholderName] [nvarchar](256) NOT NULL

GO

ALTER TABLE [dbo].[carrot_Widget] 
	ALTER COLUMN  [ControlPath] [nvarchar](512) NOT NULL

GO

--============

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [FileName] [nvarchar](256) NOT NULL

GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [PageSlug] [nvarchar](256) NULL

GO

--============

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [TitleBar] [nvarchar](256) NULL

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [NavMenuText] [nvarchar](256) NULL

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [PageHead] [nvarchar](256) NULL

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [PageText] [nvarchar](max) NULL

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [LeftPageText] [nvarchar](max) NULL

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [RightPageText] [nvarchar](max) NULL

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [TemplateFile] [nvarchar](256) NULL

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [MetaKeyword] [nvarchar](1024) NULL

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [MetaDescription] [nvarchar](1024) NULL

GO

--============

ALTER TABLE [dbo].[carrot_SerialCache] 
	ALTER COLUMN  [SerializedData] [nvarchar](max) NULL

GO

ALTER TABLE [dbo].[carrot_SerialCache] 
	ALTER COLUMN  [KeyType] [nvarchar](256) NULL

GO

--============

ALTER TABLE [dbo].[carrot_ContentType] 
	ALTER COLUMN  [ContentTypeValue] [nvarchar](256) NOT NULL

GO

--============

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [MetaKeyword] [nvarchar](1024) NULL

GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [MetaDescription] [nvarchar](1024) NULL

GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [SiteName] [nvarchar](256) NULL

GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [MainURL] [nvarchar](128) NULL

GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [Blog_FolderPath] [nvarchar](64) NULL

GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [Blog_CategoryPath] [nvarchar](64) NULL

GO
ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [Blog_TagPath] [nvarchar](64) NULL

GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [Blog_DatePattern] [nvarchar](32) NULL

GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [SiteTagline] [nvarchar](1024) NULL

GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [SiteTitlebarPattern] [nvarchar](1024) NULL

GO

--====================

GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_carrot_Widget]'))
DROP VIEW [dbo].[vw_carrot_Widget]
GO

CREATE VIEW [dbo].[vw_carrot_Widget]
AS 


SELECT w.Root_WidgetID, w.Root_ContentID, w.WidgetOrder, w.PlaceholderName, w.ControlPath, w.WidgetActive, 
	wd.WidgetDataID, wd.IsLatestVersion, wd.EditDate, wd.ControlProperties, cr.SiteID
FROM [dbo].carrot_Widget AS w 
	INNER JOIN [dbo].carrot_WidgetData AS wd ON w.Root_WidgetID = wd.Root_WidgetID 
	INNER JOIN [dbo].carrot_RootContent AS cr ON w.Root_ContentID = cr.Root_ContentID
  


GO
