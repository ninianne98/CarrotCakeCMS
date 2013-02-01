CREATE VIEW [dbo].[vw_carrot_TagURL]
AS 

SELECT  s.SiteID, cc.ContentTagID, cc.TagText, cc.IsPublic, ISNULL(cc2.TheCount, 0) AS UseCount,
		'/'+s.Blog_FolderPath +'/'+ s.Blog_TagPath +'/'+ cc.TagSlug + '.aspx' as TagUrl
FROM [dbo].carrot_Sites AS s 
INNER JOIN [dbo].carrot_ContentTag AS cc ON s.SiteID = cc.SiteID
LEFT JOIN
      (SELECT ContentTagID, COUNT(Root_ContentID) AS TheCount
        FROM dbo.carrot_TagContentMapping
        GROUP BY ContentTagID) AS cc2 ON cc.ContentTagID = cc2.ContentTagID

