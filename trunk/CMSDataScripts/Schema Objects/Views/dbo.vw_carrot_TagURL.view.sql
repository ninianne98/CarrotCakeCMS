
CREATE VIEW [dbo].[vw_carrot_TagURL]
AS 

select  s.SiteID, cc.ContentTagID, cc.TagText, cc.IsPublic, cc2.EditDate, ISNULL(cc2.TheCount, 0) as UseCount,
		'/'+s.Blog_FolderPath +'/'+ s.Blog_TagPath +'/'+ cc.TagSlug + '.aspx' as TagUrl
from [dbo].carrot_Sites as s 
inner join [dbo].carrot_ContentTag as cc on s.SiteID = cc.SiteID
left join (select m.ContentTagID, MAX(v_cc.EditDate) as EditDate, COUNT(m.Root_ContentID) as TheCount
			 from [dbo].vw_carrot_Content v_cc
			 join [dbo].carrot_TagContentMapping m on v_cc.Root_ContentID = m.Root_ContentID
			 where v_cc.IsLatestVersion = 1
			 group by m.ContentTagID) as cc2 on cc.ContentTagID = cc2.ContentTagID

