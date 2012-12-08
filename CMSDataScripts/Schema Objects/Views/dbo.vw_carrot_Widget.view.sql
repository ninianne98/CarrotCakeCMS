

CREATE VIEW [dbo].[vw_carrot_Widget]
AS 


SELECT w.Root_WidgetID, w.Root_ContentID, w.WidgetOrder, w.PlaceholderName, w.ControlPath, w.WidgetActive, 
	wd.WidgetDataID, wd.IsLatestVersion, wd.EditDate, wd.ControlProperties, cr.SiteID
FROM carrot_Widget AS w 
	INNER JOIN carrot_WidgetData AS wd ON w.Root_WidgetID = wd.Root_WidgetID 
	INNER JOIN carrot_RootContent AS cr ON w.Root_ContentID = cr.Root_ContentID
  

