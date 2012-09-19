-- 2012-09-08
-- add a view to combine root content and content

-- USE [CarrotwareCMS]
GO

/****** Object:  View [dbo].[vw_carrot_Content]    Script Date: 09/08/2012 01:22:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_carrot_Content]'))
DROP VIEW [dbo].[vw_carrot_Content]
GO

/****** Object:  View [dbo].[vw_carrot_Widget]    Script Date: 09/08/2012 01:22:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_carrot_Widget]'))
DROP VIEW [dbo].[vw_carrot_Widget]
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

/****** Object:  View [dbo].[vw_carrot_Widget]    Script Date: 09/08/2012 01:22:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE VIEW [dbo].[vw_carrot_Widget]
AS 


SELECT w.Root_WidgetID, w.Root_ContentID, w.WidgetOrder, w.PlaceholderName, w.ControlPath, w.WidgetActive, 
	wd.WidgetDataID, wd.IsLatestVersion, wd.EditDate, wd.ControlProperties, cr.SiteID
FROM carrot_Widget AS w 
	INNER JOIN carrot_WidgetData AS wd ON w.Root_WidgetID = wd.Root_WidgetID 
	INNER JOIN carrot_RootContent AS cr ON w.Root_ContentID = cr.Root_ContentID
  

GO



