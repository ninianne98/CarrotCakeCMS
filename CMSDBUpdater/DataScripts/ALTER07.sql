-- 2012-12-15
-- added new columns to carrot_RootContent

-- USE [CarrotwareCMS]
GO

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_RootContent' and column_name = 'GoLiveDate') BEGIN

	ALTER TABLE [dbo].[carrot_RootContent] ADD [GoLiveDate] [datetime] NULL

END

GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [GoLiveDate] [datetime] NULL

GO

if not exists(select * from [sys].[all_objects] 
		where [name] = 'DF_carrot_RootContent_GoLiveDate') begin

	ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_GoLiveDate]  DEFAULT (getdate()) FOR [GoLiveDate]

END

GO

UPDATE [dbo].[carrot_RootContent]
SET GoLiveDate = CreateDate - 1
WHERE GoLiveDate is null

GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [GoLiveDate] [datetime] NOT NULL

GO

--===================================

GO

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_RootContent' and column_name = 'RetireDate') BEGIN

	ALTER TABLE [dbo].[carrot_RootContent] ADD [RetireDate] [datetime] NULL

END

GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [RetireDate] [datetime] NULL

GO

if not exists(select * from [sys].[all_objects] 
		where [name] = 'DF_carrot_RootContent_RetireDate') begin

	ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_RetireDate]  DEFAULT (getdate()) FOR [RetireDate]

END


GO


UPDATE [dbo].[carrot_RootContent]
SET RetireDate = DATEADD(year, 200, CreateDate)
WHERE RetireDate is null


GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [RetireDate] [datetime] NOT NULL


GO

--===================================

GO


ALTER VIEW [dbo].[vw_carrot_Content]
AS 


SELECT rc.Root_ContentID, rc.SiteID, rc.Heartbeat_UserId, rc.EditHeartbeat, rc.[FileName], rc.PageActive, 
		rc.CreateDate, c.ContentID, c.Parent_ContentID, c.IsLatestVersion, c.TitleBar, c.NavMenuText, c.PageHead, 
		c.PageText, c.LeftPageText, c.RightPageText, c.NavOrder, c.EditUserId, c.EditDate, c.TemplateFile, c.MetaKeyword, c.MetaDescription,
		ct.ContentTypeID, ct.ContentTypeValue, rc.PageSlug, rc.RetireDate, rc.GoLiveDate,
		cast(case when rc.RetireDate < GETDATE() then 1 else 0 end as bit) as IsRetired,
		cast(case when rc.GoLiveDate > GETDATE() then 1 else 0 end as bit) as IsUnReleased
FROM dbo.carrot_RootContent AS rc 
INNER JOIN dbo.carrot_Content AS c ON rc.Root_ContentID = c.Root_ContentID 
INNER JOIN dbo.carrot_ContentType AS ct ON rc.ContentTypeID = ct.ContentTypeID



GO

--===================================

GO

