CREATE VIEW [dbo].[vw_carrot_CategoryURL]
AS 

SELECT  s.SiteID, cc.ContentCategoryID, cc.CategoryText, cc.IsPublic, ISNULL(cc2.TheCount, 0) AS UseCount, 
		'/'+s.Blog_FolderPath +'/'+ s.Blog_CategoryPath +'/'+ cc.CategorySlug + '.aspx' as CategoryUrl
FROM [dbo].carrot_Sites AS s 
INNER JOIN [dbo].carrot_ContentCategory AS cc ON s.SiteID = cc.SiteID
LEFT JOIN
      (SELECT ContentCategoryID, COUNT(Root_ContentID) AS TheCount
        FROM dbo.carrot_CategoryContentMapping
        GROUP BY ContentCategoryID) AS cc2 ON cc.ContentCategoryID = cc2.ContentCategoryID

