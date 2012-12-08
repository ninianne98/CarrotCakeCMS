

CREATE VIEW [dbo].[vw_carrot_UserData]
AS 


SELECT   m.UserId, ud.UserNickName, ud.FirstName, ud.LastName, m.LoweredEmail, m.IsApproved, m.IsLockedOut, m.CreateDate, m.LastLoginDate, 
                      m.UserName, m.LastActivityDate
FROM      vw_aspnet_MembershipUsers    AS m 
LEFT JOIN carrot_UserData AS ud ON m.UserId = ud.UserId


