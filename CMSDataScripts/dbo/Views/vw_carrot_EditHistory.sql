

CREATE VIEW [dbo].[vw_carrot_EditHistory]
AS 

SELECT  rc.SiteID, c.ContentID, c.Root_ContentID, c.IsLatestVersion, c.TitleBar, c.NavMenuText, c.PageHead, c.EditUserId, c.CreditUserId, c.EditDate, rc.CreateDate,
	rc.[FileName], ct.ContentTypeID, ct.ContentTypeValue, rc.PageActive, rc.GoLiveDate, rc.RetireDate, u.UserName as EditUserName, m.Email as EditEmail, 
	m.IsLockedOut, m.CreateDate as UserCreateDate, m.LastLoginDate, m.LastPasswordChangedDate, m.LastLockoutDate, 
	rc.CreateUserId, u2.UserName as CreateUserName, m2.Email as CreateEmail
FROM [dbo].carrot_RootContent AS rc
	INNER JOIN [dbo].carrot_Content AS c ON rc.Root_ContentID = c.Root_ContentID 
	INNER JOIN [dbo].carrot_ContentType AS ct ON rc.ContentTypeID = ct.ContentTypeID
	INNER JOIN [dbo].aspnet_Users AS u ON c.EditUserId = u.UserId 
	INNER JOIN [dbo].aspnet_Membership AS m ON u.UserId = m.UserId
	INNER JOIN [dbo].aspnet_Users AS u2 ON rc.CreateUserId = u2.UserId 
	INNER JOIN [dbo].aspnet_Membership AS m2 ON u2.UserId = m2.UserId