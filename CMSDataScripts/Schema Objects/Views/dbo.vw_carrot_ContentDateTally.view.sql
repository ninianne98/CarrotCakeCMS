

CREATE VIEW [dbo].[vw_carrot_ContentDateTally]
AS 


SELECT COUNT(Root_ContentID) AS ContentCount, SiteID, DateMonth, DateSlug, IsLatestVersion, ContentTypeID, ContentTypeValue
FROM   (SELECT Root_ContentID, SiteID, IsLatestVersion, ContentTypeID, ContentTypeValue, 
			CONVERT(datetime, CONVERT(varchar(25), CreateDate - DAY(CreateDate) + 1, 112)) AS DateMonth, 
			CONVERT(varchar(25), CreateDate - DAY(CreateDate) + 1, 112) AS DateSlug
			FROM vw_carrot_Content) AS X
GROUP BY SiteID, DateMonth, DateSlug, IsLatestVersion, ContentTypeID, ContentTypeValue

  


