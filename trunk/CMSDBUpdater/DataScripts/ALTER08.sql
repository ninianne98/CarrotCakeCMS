
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_carrot_Comment]'))
DROP VIEW [dbo].[vw_carrot_Comment]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_carrot_Comment]
AS 

SELECT cc.ContentCommentID, cc.CreateDate, cc.CommenterIP, cc.CommenterName, cc.CommenterEmail, cc.CommenterURL, cc.PostComment, cc.IsApproved, cc.IsSpam, 
	c.Root_ContentID, c.SiteID, c.[FileName], c.PageHead, c.TitleBar, c.NavMenuText, c.ContentTypeID, c.IsRetired, c.IsUnReleased, c.RetireDate, c.GoLiveDate
FROM [dbo].carrot_ContentComment AS cc 
	INNER JOIN [dbo].vw_carrot_Content AS c on cc.Root_ContentID = c.Root_ContentID
WHERE c.IsLatestVersion = 1
  

GO

--==============================================================

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_carrot_TrackbackQueue_Root_ContentID]') )
ALTER TABLE [dbo].[carrot_TrackbackQueue] DROP CONSTRAINT [FK_carrot_TrackbackQueue_Root_ContentID]
GO

IF  EXISTS (SELECT * FROM [dbo].sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_TrackbackQueue_TrackbackQueueID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[carrot_TrackbackQueue] DROP CONSTRAINT [DF_carrot_TrackbackQueue_TrackbackQueueID]
END
GO

IF  EXISTS (SELECT * FROM [dbo].sysobjects WHERE id = OBJECT_ID(N'[DF_carrot_TrackbackQueue_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[carrot_TrackbackQueue] DROP CONSTRAINT [DF_carrot_TrackbackQueue_CreateDate]
END
GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[carrot_TrackbackQueue]') AND type in (N'U')) BEGIN

	CREATE TABLE [dbo].[carrot_TrackbackQueue](
		[TrackbackQueueID] [uniqueidentifier] NOT NULL,
		[Root_ContentID] [uniqueidentifier] NOT NULL,
		[TrackBackURL] [nvarchar](256) NOT NULL,
		[TrackBackResponse] [nvarchar](2048) NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[CreateDate] [datetime] NOT NULL,
		[TrackedBack] [bit] NOT NULL,	
	 CONSTRAINT [PK_carrot_TrackbackQueue] PRIMARY KEY NONCLUSTERED 
	(
		[TrackbackQueueID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

GO

ALTER TABLE [dbo].[carrot_TrackbackQueue] ADD  CONSTRAINT [DF_carrot_TrackbackQueue_TrackbackQueueID]  DEFAULT (newid()) FOR [TrackbackQueueID]
GO

ALTER TABLE [dbo].[carrot_TrackbackQueue]  WITH CHECK ADD  CONSTRAINT [FK_carrot_TrackbackQueue_Root_ContentID] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])
GO

ALTER TABLE [dbo].[carrot_TrackbackQueue] CHECK CONSTRAINT [FK_carrot_TrackbackQueue_Root_ContentID]
GO

ALTER TABLE [dbo].[carrot_TrackbackQueue] ADD  CONSTRAINT [DF_carrot_TrackbackQueue_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO


--==============================================================

GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_carrot_TrackbackQueue]'))
DROP VIEW [dbo].[vw_carrot_TrackbackQueue]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_carrot_TrackbackQueue]
AS 

SELECT tb.TrackbackQueueID, tb.TrackBackURL, tb.TrackBackResponse, tb.CreateDate, tb.ModifiedDate, tb.TrackedBack, c.Root_ContentID, c.PageActive, c.SiteID
FROM [dbo].carrot_TrackbackQueue AS tb
INNER JOIN [dbo].carrot_RootContent AS c ON tb.Root_ContentID = c.Root_ContentID

GO


--==============================


IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_Sites' and column_name = 'SendTrackbacks') BEGIN

	ALTER TABLE [dbo].[carrot_Sites] ADD [SendTrackbacks] [bit] NULL

	ALTER TABLE [dbo].[carrot_Sites] ADD [AcceptTrackbacks] [bit] NULL

END


GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [SendTrackbacks] [bit] NULL


ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [AcceptTrackbacks] [bit] NULL

GO

UPDATE [dbo].[carrot_Sites] 
SET [SendTrackbacks] = 0
WHERE ISNULL([SendTrackbacks], 0) = 0


UPDATE [dbo].[carrot_Sites] 
SET [AcceptTrackbacks] = 0
WHERE ISNULL([AcceptTrackbacks], 0) = 0

GO

ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [SendTrackbacks] [bit] NOT NULL


ALTER TABLE [dbo].[carrot_Sites] 
	ALTER COLUMN  [AcceptTrackbacks] [bit] NOT NULL

GO

GO

ALTER VIEW [dbo].[vw_carrot_Content]
AS 

select rc.Root_ContentID, rc.SiteID, rc.Heartbeat_UserId, rc.EditHeartbeat, rc.[FileName], rc.PageActive, 
		rc.CreateDate, c.ContentID, c.Parent_ContentID, c.IsLatestVersion, c.TitleBar, c.NavMenuText, c.PageHead, 
		c.PageText, c.LeftPageText, c.RightPageText, c.NavOrder, c.EditUserId, c.EditDate, c.TemplateFile, c.MetaKeyword, c.MetaDescription,
		ct.ContentTypeID, ct.ContentTypeValue, rc.PageSlug, rc.PageThumbnail, s.TimeZone,
		rc.RetireDate, rc.GoLiveDate, rc.GoLiveDateLocal,
		cast(case when rc.RetireDate < GetUTCDate() then 1 else 0 end as bit) as IsRetired,
		cast(case when rc.GoLiveDate > GetUTCDate() then 1 else 0 end as bit) as IsUnReleased
from [dbo].carrot_RootContent AS rc 
inner join [dbo].carrot_Sites AS s ON rc.SiteID = s.SiteID 
inner join [dbo].carrot_Content AS c ON rc.Root_ContentID = c.Root_ContentID 
inner join [dbo].carrot_ContentType AS ct ON rc.ContentTypeID = ct.ContentTypeID


GO




