

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_carrot_ContentChild]'))
DROP VIEW [dbo].[vw_carrot_ContentChild]
GO


CREATE VIEW [dbo].[vw_carrot_ContentChild]
AS 

SELECT DISTINCT cc.SiteID, cc.Root_ContentID, cc.[FileName], 
          cc.RetireDate, cc.GoLiveDate, 
          cc.IsRetired, cc.IsUnReleased, 
          cp.Root_ContentID as Parent_ContentID, cp.[FileName] AS ParentFileName,
          cp.RetireDate AS ParentRetireDate, cp.GoLiveDate AS ParentGoLiveDate, 
          cp.IsRetired as IsParentRetired, cp.IsUnReleased as IsParentUnReleased
FROM dbo.vw_carrot_Content AS cc 
	INNER JOIN dbo.vw_carrot_Content AS cp ON cc.Parent_ContentID = cp.Root_ContentID
WHERE cp.IsLatestVersion = 1 AND cc.IsLatestVersion = 1
	AND cc.SiteID = cp.SiteID
GO

--===================================

GO

ALTER TABLE [dbo].[carrot_ContentComment] 
	ALTER COLUMN  [CommenterName] [nvarchar](256) NOT NULL

ALTER TABLE [dbo].[carrot_ContentComment] 
	ALTER COLUMN  [CommenterEmail] [nvarchar](256) NOT NULL

ALTER TABLE [dbo].[carrot_ContentComment] 
	ALTER COLUMN  [CommenterURL] [nvarchar](256) NOT NULL

GO

UPDATE [dbo].[carrot_Content]
SET [NavOrder] = 5
WHERE ISNULL([NavOrder], -1) = -1

GO

ALTER TABLE [dbo].[carrot_Content] 
	ALTER COLUMN  [NavOrder] [int] NOT NULL

GO

--===================================

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[carrot_RootContent_CreateUserId_FK]') AND parent_object_id = OBJECT_ID(N'[dbo].[carrot_RootContent]'))
ALTER TABLE [dbo].[carrot_RootContent] DROP CONSTRAINT [carrot_RootContent_CreateUserId_FK]
GO


IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_RootContent' and column_name = 'ShowInSiteNav') BEGIN

	ALTER TABLE [dbo].[carrot_RootContent] ADD [ShowInSiteNav] [bit] NULL

	ALTER TABLE [dbo].[carrot_RootContent] ADD [CreateUserId] [uniqueidentifier] NULL

END


GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [ShowInSiteNav] [bit] NULL


ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [CreateUserId] [uniqueidentifier] NULL

GO

UPDATE [dbo].[carrot_RootContent] 
SET [ShowInSiteNav] = 1
WHERE ISNULL([ShowInSiteNav], -1) = -1


UPDATE RC
SET [CreateUserId] = c.EditUserId
FROM [dbo].[carrot_RootContent] as RC
JOIN (SELECT [Root_ContentID], min([EditDate]) as [OldestRec]
		FROM [dbo].[carrot_Content]
		GROUP BY [Root_ContentID]) CD on RC.Root_ContentID = CD.Root_ContentID
JOIN [dbo].[carrot_Content] C on RC.Root_ContentID = C.Root_ContentID 
			AND CD.[OldestRec] = C.[EditDate] 
			AND CD.Root_ContentID = C.Root_ContentID 
WHERE RC.[CreateUserId] is null

GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [ShowInSiteNav] [bit] NOT NULL


ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [CreateUserId] [uniqueidentifier] NOT NULL

GO
 

ALTER TABLE [dbo].[carrot_RootContent]  WITH CHECK ADD  CONSTRAINT [carrot_RootContent_CreateUserId_FK] FOREIGN KEY([CreateUserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO

ALTER TABLE [dbo].[carrot_RootContent] CHECK CONSTRAINT [carrot_RootContent_CreateUserId_FK]
GO


--==============================


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[vw_carrot_Content]
AS 

select rc.Root_ContentID, rc.SiteID, rc.Heartbeat_UserId, rc.EditHeartbeat, rc.[FileName], rc.PageActive, rc.ShowInSiteNav,
		rc.CreateUserId, rc.CreateDate, c.ContentID, c.Parent_ContentID, c.IsLatestVersion, c.TitleBar, c.NavMenuText, c.PageHead, 
		c.PageText, c.LeftPageText, c.RightPageText, c.NavOrder, c.EditUserId, c.EditDate, c.TemplateFile, c.MetaKeyword, c.MetaDescription,
		ct.ContentTypeID, ct.ContentTypeValue, rc.PageSlug, rc.PageThumbnail, s.TimeZone,
		rc.RetireDate, rc.GoLiveDate, rc.GoLiveDateLocal,
		cast(case when rc.RetireDate < GetUTCDate() then 1 else 0 end as bit) as IsRetired,
		cast(case when rc.GoLiveDate > GetUTCDate() then 1 else 0 end as bit) as IsUnReleased
from [dbo].carrot_RootContent AS rc 
INNER JOIN [dbo].carrot_Sites AS s ON rc.SiteID = s.SiteID 
INNER JOIN [dbo].carrot_Content AS c ON rc.Root_ContentID = c.Root_ContentID 
INNER JOIN [dbo].carrot_ContentType AS ct ON rc.ContentTypeID = ct.ContentTypeID


GO


--==============================


