-- 2012-09-08
-- add a view to combine root content and content

-- USE [CarrotwareCMS]
GO

/****** Object:  View [dbo].[vw_carrot_Content]    Script Date: 09/08/2012 01:22:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_carrot_Content]'))
DROP VIEW [dbo].[vw_carrot_Content]
GO

-- USE [CarrotwareCMS]
GO

/****** Object:  View [dbo].[vw_carrot_Content]    Script Date: 09/08/2012 01:22:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE VIEW [dbo].[vw_carrot_Content]
AS 


SELECT rc.Root_ContentID, rc.SiteID, rc.Heartbeat_UserId, rc.EditHeartbeat, rc.[FileName], rc.PageActive, 
		rc.CreateDate, c.ContentID, c.Parent_ContentID, c.IsLatestVersion, c.TitleBar, c.NavMenuText, c.PageHead, 
		c.PageText, c.LeftPageText, c.RightPageText, c.NavOrder, c.EditUserId, c.EditDate, c.TemplateFile, c.MetaKeyword, c.MetaDescription
FROM carrot_Content AS c 
INNER JOIN carrot_RootContent AS rc ON c.Root_ContentID = rc.Root_ContentID
  

GO


