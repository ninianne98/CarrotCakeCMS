-- 2012-12-15
-- added new columns to carrot_RootContent

-- USE [CarrotwareCMS]
GO

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_RootContent' and column_name = 'GoLiveDate') BEGIN

	ALTER TABLE [dbo].[carrot_RootContent] ADD [GoLiveDate] [datetime] NULL

END
IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_RootContent' and column_name = 'GoLiveDateLocal') BEGIN

	ALTER TABLE [dbo].[carrot_RootContent] ADD [GoLiveDateLocal] [datetime] NULL

END


GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [GoLiveDate] [datetime] NULL

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [GoLiveDateLocal] [datetime] NULL

GO

if not exists(select * from [sys].[all_objects] 
		where [name] = 'DF_carrot_RootContent_GoLiveDate') begin

	ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_GoLiveDate]  DEFAULT (GetUTCDate()) FOR [GoLiveDate]

END
if not exists(select * from [sys].[all_objects] 
		where [name] = 'DF_carrot_RootContent_GoLiveDateLocal') begin

	ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_GoLiveDateLocal]  DEFAULT (GetUTCDate()) FOR [GoLiveDateLocal]

END

GO

declare @Date1800 as DateTime

set @Date1800 = cast('1890-12-31' as datetime)

UPDATE [dbo].[carrot_RootContent]
SET [GoLiveDate] = DATEADD(mi, -5, CreateDate)
WHERE isnull([GoLiveDate], @Date1800) = @Date1800

UPDATE [dbo].[carrot_RootContent]
SET [GoLiveDateLocal] = DATEADD(mi, -5, CreateDate)
WHERE isnull([GoLiveDateLocal], @Date1800) = @Date1800

GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [GoLiveDate] [datetime] NOT NULL

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [GoLiveDateLocal] [datetime] NOT NULL

GO

--===================================

GO

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_RootContent' and column_name = 'RetireDate') BEGIN

	ALTER TABLE [dbo].[carrot_RootContent] ADD [RetireDate] [datetime] NULL

END

GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [RetireDate] [datetime] NULL

GO

if not exists(select * from [sys].[all_objects] 
		where [name] = 'DF_carrot_RootContent_RetireDate') begin

	ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_RetireDate]  DEFAULT (GetUTCDate()) FOR [RetireDate]

END


GO


declare @Date1800 as DateTime

set @Date1800 = cast('1890-12-31' as datetime)

UPDATE [dbo].[carrot_RootContent]
SET RetireDate = DATEADD(year, 200, CreateDate)
WHERE isnull(RetireDate, @Date1800) = @Date1800


GO

ALTER TABLE [dbo].[carrot_RootContent] 
	ALTER COLUMN  [RetireDate] [datetime] NOT NULL


GO
--===================================
-- Convert the timestamps from whatever was localtime to approximatly UTC

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_Sites' and column_name = 'TimeZone') BEGIN

	ALTER TABLE [dbo].[carrot_Sites] ADD [TimeZone] [nvarchar](128) NULL

END

GO

DECLARE @TimeDiff int

SET @TimeDiff = DATEDIFF(mi, GETDATE(), GETUTCDATE())


IF EXISTS( select * from [dbo].[carrot_Sites]
			where ISNULL(TimeZone, 'notime') = 'notime') BEGIN

	UPDATE [dbo].carrot_ContentComment
	SET CreateDate = DATEADD(mi, @TimeDiff, CreateDate) 

	UPDATE [dbo].carrot_SerialCache
	SET EditDate = DATEADD(mi, @TimeDiff, EditDate) 
	
	UPDATE [dbo].carrot_RootContent
	SET CreateDate = DATEADD(mi, @TimeDiff, CreateDate),
		GoLiveDate = DATEADD(mi, @TimeDiff, GoLiveDate),
		RetireDate = DATEADD(mi, @TimeDiff, RetireDate)

	UPDATE [dbo].carrot_Content
	SET EditDate = DATEADD(mi, @TimeDiff, EditDate) 
	
	UPDATE [dbo].carrot_WidgetData
	SET EditDate = DATEADD(mi, @TimeDiff, EditDate) 	

END

update [dbo].[carrot_Sites]
set TimeZone = 'UTC'
where ISNULL(TimeZone, 'notime') = 'notime' OR TimeZone = ''


--=============== BEGIN UPDATE Content View===================


GO

ALTER VIEW [dbo].[vw_carrot_Content]
AS 

select rc.Root_ContentID, rc.SiteID, rc.Heartbeat_UserId, rc.EditHeartbeat, rc.[FileName], rc.PageActive, 
		rc.CreateDate, c.ContentID, c.Parent_ContentID, c.IsLatestVersion, c.TitleBar, c.NavMenuText, c.PageHead, 
		c.PageText, c.LeftPageText, c.RightPageText, c.NavOrder, c.EditUserId, c.EditDate, c.TemplateFile, c.MetaKeyword, c.MetaDescription,
		ct.ContentTypeID, ct.ContentTypeValue, rc.PageSlug, rc.PageThumbnail, s.TimeZone,
		rc.RetireDate, rc.GoLiveDate, rc.GoLiveDateLocal,
		cast(case when rc.RetireDate < GetUTCDate() then 1 else 0 end as bit) as IsRetired,
		cast(case when rc.GoLiveDate > GetUTCDate() then 1 else 0 end as bit) as IsUnReleased
from [dbo].carrot_RootContent AS rc 
inner join [dbo].carrot_Sites AS s ON rc.SiteID = s.SiteID 
inner join [dbo].carrot_Content AS c ON rc.Root_ContentID = c.Root_ContentID 
inner join [dbo].carrot_ContentType AS ct ON rc.ContentTypeID = ct.ContentTypeID


GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_carrot_ContentDateTally]'))
DROP VIEW [dbo].[vw_carrot_ContentDateTally]
GO

--=============== END UPDATE Content View===================

GO

--============================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[carrot_BlogMonthlyTallies]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[carrot_BlogMonthlyTallies]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[carrot_BlogMonthlyTallies]
    @SiteID uniqueidentifier,
    @ActiveOnly bit,    
    @TakeTop int = 10

/*

exec [carrot_BlogMonthlyTallies] '3BD253EA-AC65-4eb6-A4E7-BB097C2255A0', 10

exec [carrot_BlogMonthlyTallies] '3BD253EA-AC65-4eb6-A4E7-BB097C2255A0', 16

exec [carrot_BlogMonthlyTallies] '3BD253EA-AC65-4eb6-A4E7-BB097C2255A0', 5

*/

AS BEGIN

SET NOCOUNT ON

	DECLARE @UTCDate Date
	SET @UTCDate = GETUTCDATE()
	
    DECLARE @ContentTypeID uniqueidentifier
    SELECT  @ContentTypeID = (select top 1 ct.ContentTypeID from dbo.carrot_ContentType ct where ct.ContentTypeValue = 'BlogEntry')

	DECLARE @tblTallies TABLE(
		RowID int identity(1,1),
		SiteID uniqueidentifier,
		ContentCount int,
		DateMonth date,
		DateSlug nvarchar(64)
	)
	
	insert into @tblTallies(SiteID, ContentCount, DateMonth, DateSlug)
		SELECT SiteID, COUNT(Root_ContentID) AS ContentCount, DateMonth, DateSlug
		FROM   (SELECT Root_ContentID, SiteID, ContentTypeID, 
					CONVERT(datetime, CONVERT(nvarchar(25), GoLiveDateLocal, 112)) AS DateMonth, 
					DATENAME(MONTH, GoLiveDateLocal) + ' ' + CAST(YEAR(GoLiveDateLocal) as nvarchar(50)) AS DateSlug
			FROM (SELECT Root_ContentID, SiteID, ContentTypeID, (GoLiveDateLocal - DAY(GoLiveDateLocal) + 1) as GoLiveDateLocal
				FROM [dbo].[carrot_RootContent]
				WHERE SiteID = @SiteID
					AND (PageActive = 1 OR @ActiveOnly = 0)
					AND (GoLiveDate < @UTCDate OR @ActiveOnly = 0)
					AND (RetireDate > @UTCDate OR @ActiveOnly = 0)
					AND ContentTypeID = @ContentTypeID ) AS Y) AS Z

		GROUP BY SiteID, DateMonth, DateSlug
		ORDER BY DateMonth DESC

	SELECT * FROM @tblTallies WHERE RowID <= @TakeTop ORDER BY RowID

    RETURN(0)

END


GO

--============================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[carrot_UpdateGoLiveLocal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[carrot_UpdateGoLiveLocal]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[carrot_UpdateGoLiveLocal]
    @SiteID uniqueidentifier,
    @UTCOffsetInMinutes int = 0

/*

exec [carrot_UpdateGoLiveLocal] '3BD253EA-AC65-4eb6-A4E7-BB097C2255A0', 360

*/

AS BEGIN

SET NOCOUNT ON

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF ( @@TRANCOUNT = 0 ) BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END ELSE
        SET @TranStarted = 0


	UPDATE [dbo].[carrot_RootContent]
	set GoLiveDateLocal = DATEADD(mi, @UTCOffsetInMinutes, GoLiveDate)
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

--============================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[carrot_BlogDateFilenameUpdate]') )
DROP PROCEDURE [dbo].[carrot_BlogDateFilenameUpdate]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[carrot_BlogDateFilenameUpdate]
    @SiteID uniqueidentifier
    
/*

exec [carrot_BlogDateFilenameUpdate] '3BD253EA-AC65-4eb6-A4E7-BB097C2255A0' 

*/    
    
AS BEGIN

SET NOCOUNT ON

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0
    
    DECLARE @DatePattern nvarchar(50)
    SELECT  @DatePattern = (select top 1 ct.Blog_DatePattern from dbo.carrot_Sites ct where ct.SiteID = @SiteID)

    DECLARE @ContentTypeID uniqueidentifier
    SELECT  @ContentTypeID = (select top 1 ct.ContentTypeID from dbo.carrot_ContentType ct where ct.ContentTypeValue = 'BlogEntry')

	DECLARE @tblTimeSlugs TABLE(
		Root_ContentID uniqueidentifier,
		GoLiveDateLocal datetime not null,
		TempSlug nvarchar(128) not null,
		URLBase nvarchar(50) null
	)

	insert into @tblTimeSlugs(Root_ContentID, GoLiveDateLocal, TempSlug)
		select rc.Root_ContentID, rc.GoLiveDateLocal,  '/' + LOWER(CAST(rc.Root_ContentID as nvarchar(60)) + '.aspx') as tmpSlug 
		from dbo.[carrot_RootContent] as rc
		where rc.SiteID = @SiteID
			AND rc.ContentTypeID = @ContentTypeID


	IF (ISNULL(@DatePattern, 'yyyy/MM/dd') = 'yyyy/MM/dd' ) BEGIN
		update @tblTimeSlugs
		set URLBase = CONVERT(NVARCHAR(20), GoLiveDateLocal, 111)
	END

	IF (@DatePattern = 'yyyy/M/d' ) BEGIN
		update @tblTimeSlugs
		set URLBase = REPLACE(CONVERT(NVARCHAR(20), GoLiveDateLocal, 111), '/0', '/')
	END

	IF (@DatePattern = 'yyyy/MM' ) BEGIN
		update @tblTimeSlugs
		set URLBase = SUBSTRING(CONVERT(NVARCHAR(20), GoLiveDateLocal, 111), 1, 7)
	END

	IF (@DatePattern = 'yyyy/MMMM' ) BEGIN
		update @tblTimeSlugs
		set URLBase = CAST(YEAR(GoLiveDateLocal) as nvarchar(20)) +'/'+ DATENAME(MONTH, GoLiveDateLocal)
	END

	IF (@DatePattern = 'yyyy' ) BEGIN
		update @tblTimeSlugs
		set URLBase = CAST(YEAR(GoLiveDateLocal) as nvarchar(20))
	END


    IF ( @@TRANCOUNT = 0 ) BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END ELSE
        SET @TranStarted = 0

		--select replace('/'+ s.URLBase +'/' + ISNULL(PageSlug, s.TempSlug), '//','/') as FN
		update rc
		set [FileName] = replace('/'+ s.URLBase +'/' + ISNULL(rc.PageSlug, s.TempSlug) , '//','/')
		from dbo.[carrot_RootContent] rc
			join @tblTimeSlugs s on rc.Root_ContentID = s.Root_ContentID
		where rc.SiteID = @SiteID
			AND rc.ContentTypeID = @ContentTypeID


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

--===================================================


DECLARE @tblSlugs TABLE (
	RowID int identity(1,1),
	SiteID uniqueidentifier
)

insert into @tblSlugs(SiteID)
	select SiteID 
	from [carrot_Sites] 
	where TimeZone = 'UTC'


declare @row int,
		@siteCurrent uniqueidentifier,
		@rowMax int

set @row = 1
set @rowMax = (select MAX(RowID) from @tblSlugs)

while (@row <= @rowMax) begin
	set @siteCurrent = (select top 1 SiteID from @tblSlugs where RowID = @row)
	
	EXEC [dbo].[carrot_UpdateGoLiveLocal] @siteCurrent, 0
	exec [dbo].[carrot_BlogDateFilenameUpdate] @siteCurrent	
	
	print 'updated - ' + cast(@siteCurrent as nvarchar(50))
	
	set @row = @row + 1
end


GO


