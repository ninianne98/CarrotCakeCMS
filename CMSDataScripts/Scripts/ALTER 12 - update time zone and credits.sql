GO

SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF
SET QUOTED_IDENTIFIER ON

GO
ALTER PROCEDURE [dbo].[carrot_UpdateGoLiveLocal]
    @SiteID uniqueidentifier,
    @xmlDocument xml = '<rows />'


AS BEGIN

SET NOCOUNT ON

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF ( @@TRANCOUNT = 0 ) BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END ELSE BEGIN
        SET @TranStarted = 0
	END

		DECLARE @tblContent TABLE
		(
		  Root_ContentID uniqueidentifier,
		  GoLiveDateLocal datetime
		)

		DECLARE @tblBlogs TABLE
		(
		  Root_ContentID uniqueidentifier,
		  GoLiveDateLocal datetime,
		  [FileName] nvarchar(256)  
		)

		INSERT INTO @tblContent(Root_ContentID, GoLiveDateLocal)
		SELECT
			ref.value ('Root_ContentID[1]', 'uniqueidentifier') as Root_ContentID,
			ref.value ('GoLiveDateLocal[1]', 'datetime') as GoLiveDateLocal
		FROM @xmlDocument.nodes ('//ContentLocalTime') T(ref);

		INSERT INTO @tblBlogs(Root_ContentID, GoLiveDateLocal, [FileName])
		SELECT
			ref.value ('Root_ContentID[1]', 'uniqueidentifier') as Root_ContentID,
			ref.value ('GoLiveDateLocal[1]', 'datetime') as GoLiveDateLocal,	
			ref.value ('FileName[1]', 'nvarchar(256)') as [FileName]
		FROM @xmlDocument.nodes ('//BlogPostPageUrl') T(ref);

		UPDATE rc
			SET GoLiveDateLocal = c.GoLiveDateLocal
		FROM [dbo].[carrot_RootContent] rc
			INNER JOIN @tblContent c on rc.Root_ContentID = c.Root_ContentID
		WHERE SiteID = @SiteID

		UPDATE rc
			SET [FileName] = b.[FileName]
		FROM [dbo].[carrot_RootContent] rc
			INNER JOIN @tblBlogs b on rc.Root_ContentID = b.Root_ContentID
		WHERE SiteID = @SiteID


	IF ( @@ERROR <> 0 ) BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF ( @TranStarted = 1 ) BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF ( @TranStarted = 1 ) BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END


GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_Content' and column_name = 'CreditUserId') BEGIN

	ALTER TABLE [dbo].[carrot_Content] ADD [CreditUserId] [uniqueidentifier] NULL

END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[carrot_Content_CreditUserId_FK]') AND parent_object_id = OBJECT_ID(N'[dbo].[carrot_Content]'))
	ALTER TABLE [dbo].[carrot_Content] DROP CONSTRAINT [carrot_Content_CreditUserId_FK]
GO

ALTER TABLE [dbo].[carrot_Content]  WITH CHECK ADD  CONSTRAINT [carrot_Content_CreditUserId_FK] FOREIGN KEY([CreditUserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO

ALTER TABLE [dbo].[carrot_Content] CHECK CONSTRAINT [carrot_Content_CreditUserId_FK]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[vw_carrot_Content]
AS 

SELECT rc.Root_ContentID, rc.SiteID, rc.Heartbeat_UserId, rc.EditHeartbeat, rc.[FileName], rc.PageActive, rc.ShowInSiteNav, rc.ShowInSiteMap, rc.BlockIndex,
		rc.CreateUserId, rc.CreateDate, c.ContentID, c.Parent_ContentID, c.IsLatestVersion, c.TitleBar, c.NavMenuText, c.PageHead, 
		c.PageText, c.LeftPageText, c.RightPageText, c.NavOrder, c.EditUserId, c.CreditUserId, c.EditDate, c.TemplateFile, c.MetaKeyword, c.MetaDescription,
		cvh.VersionCount, ct.ContentTypeID, ct.ContentTypeValue, rc.PageSlug, rc.PageThumbnail, s.TimeZone,
		rc.RetireDate, rc.GoLiveDate, rc.GoLiveDateLocal,
		cast(case when rc.RetireDate <= GetUTCDate() then 1 else 0 end as bit) as IsRetired,
		cast(case when rc.GoLiveDate >= GetUTCDate() then 1 else 0 end as bit) as IsUnReleased
FROM [dbo].carrot_RootContent AS rc 
	INNER JOIN [dbo].carrot_Sites AS s ON rc.SiteID = s.SiteID 
	INNER JOIN [dbo].carrot_Content AS c ON rc.Root_ContentID = c.Root_ContentID 
	INNER JOIN [dbo].carrot_ContentType AS ct ON rc.ContentTypeID = ct.ContentTypeID
	INNER JOIN (SELECT COUNT(*) VersionCount, Root_ContentID 
				FROM [dbo].carrot_Content
				GROUP BY Root_ContentID 
				) cvh on rc.Root_ContentID = cvh.Root_ContentID



GO


ALTER VIEW [dbo].[vw_carrot_EditHistory]
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


GO


ALTER VIEW [dbo].[vw_carrot_EditorURL]
AS 

select  d.SiteID, d.UserId, d.UserName, d.LoweredEmail, cc2.EditDate, 
		ISNULL(cc2.TheCount, 0) as UseCount, ISNULL(cc3.TheCount, 0) as PublicUseCount,
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
			union
			select v_cc.CreditUserId, v_cc.SiteID, MAX(v_cc.EditDate) as EditDate, COUNT(ContentID) as TheCount
			from dbo.vw_carrot_Content v_cc
			where v_cc.IsLatestVersion = 1
				and v_cc.CreditUserId is not null
			group by v_cc.CreditUserId, v_cc.SiteID		
		
			) as cc2 on d.UserId = cc2.EditUserId
					and d.SiteID = cc2.SiteID
	left join (
			select v_cc.EditUserId, v_cc.SiteID, MAX(v_cc.EditDate) as EditDate, COUNT(ContentID) as TheCount
			from dbo.vw_carrot_Content v_cc
			where v_cc.IsLatestVersion = 1
				and v_cc.PageActive = 1 and v_cc.RetireDate >= GETUTCDATE() and v_cc.GoLiveDate <= GETUTCDATE() 
			group by v_cc.EditUserId, v_cc.SiteID
			union
			select v_cc.CreditUserId, v_cc.SiteID, MAX(v_cc.EditDate) as EditDate, COUNT(ContentID) as TheCount
			from dbo.vw_carrot_Content v_cc
			where v_cc.IsLatestVersion = 1 and v_cc.CreditUserId is not null
				and v_cc.PageActive = 1 and v_cc.RetireDate >= GETUTCDATE() and v_cc.GoLiveDate <= GETUTCDATE() 
			group by v_cc.CreditUserId, v_cc.SiteID	
			) as cc3 on d.UserId = cc3.EditUserId
					and d.SiteID = cc3.SiteID


GO


ALTER VIEW [dbo].[vw_carrot_CategoryURL]
AS 

select  s.SiteID, cc.ContentCategoryID, cc.CategoryText, cc.IsPublic, cc2.EditDate, 
		ISNULL(cc2.TheCount, 0) as UseCount, ISNULL(cc3.TheCount, 0) as PublicUseCount,
		'/' + s.Blog_FolderPath + '/' + s.Blog_CategoryPath + '/' + cc.CategorySlug + '.aspx' as CategoryUrl
from [dbo].carrot_Sites as s 
	inner join [dbo].carrot_ContentCategory as cc on s.SiteID = cc.SiteID
	left join (select m.ContentCategoryID, MAX(v_cc.EditDate) as EditDate, COUNT(m.Root_ContentID) as TheCount
				 from [dbo].vw_carrot_Content v_cc
					join [dbo].carrot_CategoryContentMapping m on v_cc.Root_ContentID = m.Root_ContentID
				 where v_cc.IsLatestVersion = 1
				 group by m.ContentCategoryID) as cc2 on cc.ContentCategoryID = cc2.ContentCategoryID

	left join (select m.ContentCategoryID, COUNT(m.Root_ContentID) as TheCount
				 from [dbo].vw_carrot_Content v_cc
					join [dbo].carrot_CategoryContentMapping m on v_cc.Root_ContentID = m.Root_ContentID
				 where v_cc.IsLatestVersion = 1
						and v_cc.PageActive = 1 and v_cc.RetireDate >= GETUTCDATE() and v_cc.GoLiveDate <= GETUTCDATE() 
				 group by m.ContentCategoryID) as cc3 on cc.ContentCategoryID = cc3.ContentCategoryID


GO


ALTER VIEW [dbo].[vw_carrot_TagURL]
AS 

select  s.SiteID, cc.ContentTagID, cc.TagText, cc.IsPublic, cc2.EditDate, 
		ISNULL(cc2.TheCount, 0) as UseCount, ISNULL(cc3.TheCount, 0) as PublicUseCount,
		'/' + s.Blog_FolderPath + '/' + s.Blog_TagPath + '/' + cc.TagSlug + '.aspx' as TagUrl
from [dbo].carrot_Sites as s 
	inner join [dbo].carrot_ContentTag as cc on s.SiteID = cc.SiteID
	left join (select m.ContentTagID, MAX(v_cc.EditDate) as EditDate, COUNT(m.Root_ContentID) as TheCount
				 from [dbo].vw_carrot_Content v_cc
					join [dbo].carrot_TagContentMapping m on v_cc.Root_ContentID = m.Root_ContentID
				 where v_cc.IsLatestVersion = 1
				 group by m.ContentTagID) as cc2 on cc.ContentTagID = cc2.ContentTagID

	left join (select m.ContentTagID, COUNT(m.Root_ContentID) as TheCount
				 from [dbo].vw_carrot_Content v_cc
					join [dbo].carrot_TagContentMapping m on v_cc.Root_ContentID = m.Root_ContentID
				 where v_cc.IsLatestVersion = 1
						and v_cc.PageActive = 1 and v_cc.RetireDate >= GETUTCDATE() and v_cc.GoLiveDate <= GETUTCDATE() 
				 group by m.ContentTagID) as cc3 on cc.ContentTagID = cc3.ContentTagID



GO


