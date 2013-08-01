
CREATE VIEW [dbo].[vw_carrot_EditorURL]
AS 

select  d.SiteID, d.UserId, d.UserName, d.LoweredEmail, cc2.EditDate, ISNULL(cc2.TheCount, 0) as UseCount,
		'/'+d.Blog_FolderPath +'/'+ d.Blog_EditorPath +'/'+ d.UserName + '.aspx' as UserUrl
from (
	select s.SiteID, s.Blog_FolderPath, s.Blog_EditorPath, m.UserId, m.UserName, m.LoweredEmail
		from [dbo].vw_aspnet_MembershipUsers m, [dbo].carrot_Sites s
	) as d
left join (
		select v_cc.EditUserId, v_cc.SiteID, MAX(v_cc.EditDate) as EditDate, COUNT(ContentID) as TheCount
		from dbo.vw_carrot_Content v_cc
		where v_cc.IsLatestVersion = 1
		group by v_cc.EditUserId, v_cc.SiteID
		) as cc2 on d.UserId = cc2.EditUserId
				and d.SiteID = cc2.SiteID