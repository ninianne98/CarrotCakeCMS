

CREATE VIEW [dbo].[vw_carrot_Content]
AS 


SELECT rc.Root_ContentID, rc.SiteID, rc.Heartbeat_UserId, rc.EditHeartbeat, rc.[FileName], rc.PageActive, 
		rc.CreateDate, c.ContentID, c.Parent_ContentID, c.IsLatestVersion, c.TitleBar, c.NavMenuText, c.PageHead, 
		c.PageText, c.LeftPageText, c.RightPageText, c.NavOrder, c.EditUserId, c.EditDate, c.TemplateFile, c.MetaKeyword, c.MetaDescription,
		ct.ContentTypeID, ct.ContentTypeValue, rc.PageSlug
FROM dbo.carrot_Content AS c 
INNER JOIN dbo.carrot_RootContent AS rc ON c.Root_ContentID = rc.Root_ContentID
INNER JOIN dbo.carrot_ContentType AS ct ON rc.ContentTypeID = ct.ContentTypeID
  


