GO
CREATE ROLE [aspnet_Membership_BasicAccess] AUTHORIZATION [dbo]

GO
CREATE ROLE [aspnet_Membership_FullAccess] AUTHORIZATION [dbo]

GO
CREATE ROLE [aspnet_Membership_ReportingAccess] AUTHORIZATION [dbo]

GO
CREATE ROLE [aspnet_Roles_BasicAccess] AUTHORIZATION [dbo]

GO
CREATE ROLE [aspnet_Roles_FullAccess] AUTHORIZATION [dbo]

GO
CREATE ROLE [aspnet_Roles_ReportingAccess] AUTHORIZATION [dbo]

GO
CREATE SCHEMA [aspnet_Membership_BasicAccess] AUTHORIZATION [aspnet_Membership_BasicAccess]

GO
CREATE SCHEMA [aspnet_Membership_FullAccess] AUTHORIZATION [aspnet_Membership_FullAccess]

GO
CREATE SCHEMA [aspnet_Membership_ReportingAccess] AUTHORIZATION [aspnet_Membership_ReportingAccess]

GO
CREATE SCHEMA [aspnet_Roles_BasicAccess] AUTHORIZATION [aspnet_Roles_BasicAccess]

GO
CREATE SCHEMA [aspnet_Roles_FullAccess] AUTHORIZATION [aspnet_Roles_FullAccess]

GO
CREATE SCHEMA [aspnet_Roles_ReportingAccess] AUTHORIZATION [aspnet_Roles_ReportingAccess]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_Sites](
	[SiteID] [uniqueidentifier] NOT NULL,
	[MetaKeyword] [nvarchar](1024) NULL,
	[MetaDescription] [nvarchar](1024) NULL,
	[SiteName] [nvarchar](256) NULL,
	[MainURL] [nvarchar](128) NULL,
	[BlockIndex] [bit] NOT NULL,
	[SiteTagline] [nvarchar](1024) NULL,
	[SiteTitlebarPattern] [nvarchar](1024) NULL,
	[Blog_Root_ContentID] [uniqueidentifier] NULL,
	[Blog_FolderPath] [nvarchar](64) NULL,
	[Blog_CategoryPath] [nvarchar](64) NULL,
	[Blog_TagPath] [nvarchar](64) NULL,
	[Blog_DatePattern] [nvarchar](32) NULL,
	[TimeZone] [nvarchar](128) NULL,
	[SendTrackbacks] [bit] NOT NULL,
	[AcceptTrackbacks] [bit] NOT NULL,
	[Blog_DatePath] [nvarchar](64) NULL,
	[Blog_EditorPath] [nvarchar](64) NULL,
 CONSTRAINT [carrot_Sites_PK] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_SerialCache](
	[SerialCacheID] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
	[ItemID] [uniqueidentifier] NOT NULL,
	[EditUserId] [uniqueidentifier] NOT NULL,
	[KeyType] [nvarchar](256) NULL,
	[SerializedData] [nvarchar](max) NULL,
	[EditDate] [datetime] NOT NULL,
 CONSTRAINT [carrot_SerialCache_PK] PRIMARY KEY CLUSTERED 
(
	[SerialCacheID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_DataInfo](
	[DataInfoID] [uniqueidentifier] NOT NULL,
	[DataKey] [nvarchar](256) NOT NULL,
	[DataValue] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_carrot_DataInfo] PRIMARY KEY NONCLUSTERED 
(
	[DataInfoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_ContentType](
	[ContentTypeID] [uniqueidentifier] NOT NULL,
	[ContentTypeValue] [nvarchar](256) NOT NULL,
 CONSTRAINT [carrot_ContentType_PK] PRIMARY KEY CLUSTERED 
(
	[ContentTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[aspnet_Applications](
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](256) NULL,
 CONSTRAINT [PK_aspnet_Applications] PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UQ_aspnet_Applications_ApplicationName] UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UQ_aspnet_Applications_LoweredApplicationName] UNIQUE NONCLUSTERED 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE CLUSTERED INDEX [aspnet_Applications_Index] ON [dbo].[aspnet_Applications] 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Setup_RestorePermissions]
    @name   sysname
AS
BEGIN
    DECLARE @object sysname
    DECLARE @protectType char(10)
    DECLARE @action varchar(60)
    DECLARE @grantee sysname
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT Object, ProtectType, [Action], Grantee FROM #aspnet_Permissions where Object = @name

    OPEN c1

    FETCH c1 INTO @object, @protectType, @action, @grantee
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = @protectType + ' ' + @action + ' on ' + @object + ' TO [' + @grantee + ']'
        EXEC (@cmd)
        FETCH c1 INTO @object, @protectType, @action, @grantee
    END

    CLOSE c1
    DEALLOCATE c1
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Setup_RemoveAllRoleMembers]
    @name   sysname
AS
BEGIN
    CREATE TABLE #aspnet_RoleMembers
    (
        Group_name      sysname,
        Group_id        smallint,
        Users_in_group  sysname,
        User_id         smallint
    )

    INSERT INTO #aspnet_RoleMembers
    EXEC sp_helpuser @name

    DECLARE @user_id smallint
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT User_id FROM #aspnet_RoleMembers

    OPEN c1

    FETCH c1 INTO @user_id
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = 'EXEC sp_droprolemember ' + '''' + @name + ''', ''' + USER_NAME(@user_id) + ''''
        EXEC (@cmd)
        FETCH c1 INTO @user_id
    END

    CLOSE c1
    DEALLOCATE c1
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[aspnet_SchemaVersions](
	[Feature] [nvarchar](128) NOT NULL,
	[CompatibleSchemaVersion] [nvarchar](128) NOT NULL,
	[IsCurrentVersion] [bit] NOT NULL,
 CONSTRAINT [PK_aspnet_SchemaVersions] PRIMARY KEY CLUSTERED 
(
	[Feature] ASC,
	[CompatibleSchemaVersion] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[aspnet_Users](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[LoweredUserName] [nvarchar](256) NOT NULL,
	[MobileAlias] [nvarchar](16) NULL,
	[IsAnonymous] [bit] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
 CONSTRAINT [PK_aspnet_Users] PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE UNIQUE CLUSTERED INDEX [aspnet_Users_Index] ON [dbo].[aspnet_Users] 
(
	[ApplicationId] ASC,
	[LoweredUserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX [aspnet_Users_Index2] ON [dbo].[aspnet_Users] 
(
	[ApplicationId] ASC,
	[LastActivityDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_UnRegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    DELETE FROM dbo.aspnet_SchemaVersions
        WHERE   Feature = LOWER(@Feature) AND @CompatibleSchemaVersion = CompatibleSchemaVersion
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_CheckSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    IF (EXISTS( SELECT  *
                FROM    dbo.aspnet_SchemaVersions
                WHERE   Feature = LOWER( @Feature ) AND
                        CompatibleSchemaVersion = @CompatibleSchemaVersion ))
        RETURN 0

    RETURN 1
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Applications_CreateApplication]
    @ApplicationName      nvarchar(256),
    @ApplicationId        uniqueidentifier OUTPUT
AS
BEGIN
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName

    IF(@ApplicationId IS NULL)
    BEGIN
        DECLARE @TranStarted   bit
        SET @TranStarted = 0

        IF( @@TRANCOUNT = 0 )
        BEGIN
	        BEGIN TRANSACTION
	        SET @TranStarted = 1
        END
        ELSE
    	    SET @TranStarted = 0

        SELECT  @ApplicationId = ApplicationId
        FROM dbo.aspnet_Applications WITH (UPDLOCK, HOLDLOCK)
        WHERE LOWER(@ApplicationName) = LoweredApplicationName

        IF(@ApplicationId IS NULL)
        BEGIN
            SELECT  @ApplicationId = NEWID()
            INSERT  dbo.aspnet_Applications (ApplicationId, ApplicationName, LoweredApplicationName)
            VALUES  (@ApplicationId, @ApplicationName, LOWER(@ApplicationName))
        END


        IF( @TranStarted = 1 )
        BEGIN
            IF(@@ERROR = 0)
            BEGIN
	        SET @TranStarted = 0
	        COMMIT TRANSACTION
            END
            ELSE
            BEGIN
                SET @TranStarted = 0
                ROLLBACK TRANSACTION
            END
        END
    END
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[aspnet_Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
 CONSTRAINT [PK_aspnet_Roles] PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE UNIQUE CLUSTERED INDEX [aspnet_Roles_index1] ON [dbo].[aspnet_Roles] 
(
	[ApplicationId] ASC,
	[LoweredRoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_RegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128),
    @IsCurrentVersion          bit,
    @RemoveIncompatibleSchema  bit
AS
BEGIN
    IF( @RemoveIncompatibleSchema = 1 )
    BEGIN
        DELETE FROM dbo.aspnet_SchemaVersions WHERE Feature = LOWER( @Feature )
    END
    ELSE
    BEGIN
        IF( @IsCurrentVersion = 1 )
        BEGIN
            UPDATE dbo.aspnet_SchemaVersions
            SET IsCurrentVersion = 0
            WHERE Feature = LOWER( @Feature )
        END
    END

    INSERT  dbo.aspnet_SchemaVersions( Feature, CompatibleSchemaVersion, IsCurrentVersion )
    VALUES( LOWER( @Feature ), @CompatibleSchemaVersion, @IsCurrentVersion )
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_ContentCategory](
	[ContentCategoryID] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
	[CategoryText] [nvarchar](256) NOT NULL,
	[CategorySlug] [nvarchar](256) NOT NULL,
	[IsPublic] [bit] NOT NULL,
 CONSTRAINT [PK_carrot_ContentCategory] PRIMARY KEY NONCLUSTERED 
(
	[ContentCategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_ContentTag](
	[ContentTagID] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
	[TagText] [nvarchar](256) NOT NULL,
	[TagSlug] [nvarchar](256) NOT NULL,
	[IsPublic] [bit] NOT NULL,
 CONSTRAINT [PK_carrot_ContentTag] PRIMARY KEY NONCLUSTERED 
(
	[ContentTagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_RootContentSnippet](
	[Root_ContentSnippetID] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
	[ContentSnippetName] [nvarchar](256) NOT NULL,
	[ContentSnippetSlug] [nvarchar](128) NOT NULL,
	[CreateUserId] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[GoLiveDate] [datetime] NOT NULL,
	[RetireDate] [datetime] NOT NULL,
	[ContentSnippetActive] [bit] NOT NULL,
	[Heartbeat_UserId] [uniqueidentifier] NULL,
	[EditHeartbeat] [datetime] NULL,
 CONSTRAINT [PK_carrot_RootContentSnippet] PRIMARY KEY CLUSTERED 
(
	[Root_ContentSnippetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_TextWidget](
	[TextWidgetID] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
	[TextWidgetAssembly] [nvarchar](256) NOT NULL,
	[ProcessBody] [bit] NOT NULL,
	[ProcessPlainText] [bit] NOT NULL,
	[ProcessHTMLText] [bit] NOT NULL,
	[ProcessComment] [bit] NOT NULL,
	[ProcessSnippet] [bit] NOT NULL,
 CONSTRAINT [PK_carrot_TextWidget] PRIMARY KEY NONCLUSTERED 
(
	[TextWidgetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE VIEW [dbo].[vw_aspnet_Applications]
  AS SELECT [dbo].[aspnet_Applications].[ApplicationName], [dbo].[aspnet_Applications].[LoweredApplicationName], [dbo].[aspnet_Applications].[ApplicationId], [dbo].[aspnet_Applications].[Description]
  FROM [dbo].[aspnet_Applications]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Roles_CreateRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS(SELECT RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId))
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    INSERT INTO dbo.aspnet_Roles
                (ApplicationId, RoleName, LoweredRoleName)
         VALUES (@ApplicationId, @RoleName, LOWER(@RoleName))

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Users_CreateUser]
    @ApplicationId    uniqueidentifier,
    @UserName         nvarchar(256),
    @IsUserAnonymous  bit,
    @LastActivityDate DATETIME,
    @UserId           uniqueidentifier OUTPUT
AS
BEGIN
    IF( @UserId IS NULL )
        SELECT @UserId = NEWID()
    ELSE
    BEGIN
        IF( EXISTS( SELECT UserId FROM dbo.aspnet_Users
                    WHERE @UserId = UserId ) )
            RETURN -1
    END

    INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
    VALUES (@ApplicationId, @UserId, @UserName, LOWER(@UserName), @IsUserAnonymous, @LastActivityDate)

    RETURN 0
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_UserSiteMapping](
	[UserSiteMappingID] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [carrot_UserSiteMapping_PK] PRIMARY KEY CLUSTERED 
(
	[UserSiteMappingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_UserData](
	[UserId] [uniqueidentifier] NOT NULL,
	[UserNickName] [nvarchar](64) NULL,
	[FirstName] [nvarchar](64) NULL,
	[LastName] [nvarchar](64) NULL,
	[UserBio] [nvarchar](max) NULL,
 CONSTRAINT [PK_carrot_UserData] PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE VIEW [dbo].[vw_aspnet_Users]
  AS SELECT [dbo].[aspnet_Users].[ApplicationId], [dbo].[aspnet_Users].[UserId], [dbo].[aspnet_Users].[UserName], [dbo].[aspnet_Users].[LoweredUserName], [dbo].[aspnet_Users].[MobileAlias], [dbo].[aspnet_Users].[IsAnonymous], [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Users]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE VIEW [dbo].[vw_aspnet_Roles]
  AS SELECT [dbo].[aspnet_Roles].[ApplicationId], [dbo].[aspnet_Roles].[RoleId], [dbo].[aspnet_Roles].[RoleName], [dbo].[aspnet_Roles].[LoweredRoleName], [dbo].[aspnet_Roles].[Description]
  FROM [dbo].[aspnet_Roles]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_RootContent](
	[Root_ContentID] [uniqueidentifier] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
	[Heartbeat_UserId] [uniqueidentifier] NULL,
	[EditHeartbeat] [datetime] NULL,
	[FileName] [nvarchar](256) NOT NULL,
	[PageActive] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ContentTypeID] [uniqueidentifier] NOT NULL,
	[PageSlug] [nvarchar](256) NULL,
	[PageThumbnail] [nvarchar](128) NULL,
	[GoLiveDate] [datetime] NOT NULL,
	[GoLiveDateLocal] [datetime] NOT NULL,
	[RetireDate] [datetime] NOT NULL,
	[ShowInSiteNav] [bit] NOT NULL,
	[CreateUserId] [uniqueidentifier] NOT NULL,
	[ShowInSiteMap] [bit] NOT NULL,
	[BlockIndex] [bit] NOT NULL,
 CONSTRAINT [carrot_RootContent_PK] PRIMARY KEY CLUSTERED 
(
	[Root_ContentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX [IDX_carrot_RootContent_ContentTypeID] ON [dbo].[carrot_RootContent] 
(
	[ContentTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX [IDX_carrot_RootContent_CreateUserId] ON [dbo].[carrot_RootContent] 
(
	[CreateUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX [IDX_carrot_RootContent_SiteID] ON [dbo].[carrot_RootContent] 
(
	[SiteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_ContentSnippet](
	[ContentSnippetID] [uniqueidentifier] NOT NULL,
	[Root_ContentSnippetID] [uniqueidentifier] NOT NULL,
	[IsLatestVersion] [bit] NOT NULL,
	[EditUserId] [uniqueidentifier] NOT NULL,
	[EditDate] [datetime] NOT NULL,
	[ContentBody] [nvarchar](max) NULL,
 CONSTRAINT [PK_carrot_ContentSnippet] PRIMARY KEY CLUSTERED 
(
	[ContentSnippetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[aspnet_Membership](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[LoweredEmail] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] NULL,
 CONSTRAINT [PK_aspnet_Membership] PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
CREATE CLUSTERED INDEX [aspnet_Membership_index] ON [dbo].[aspnet_Membership] 
(
	[ApplicationId] ASC,
	[LoweredEmail] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Roles_RoleExists]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(0)
    IF (EXISTS (SELECT RoleName FROM dbo.aspnet_Roles WHERE LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId ))
        RETURN(1)
    ELSE
        RETURN(0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Roles_GetAllRoles] (
    @ApplicationName           nvarchar(256))
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN
    SELECT RoleName
    FROM   dbo.aspnet_Roles WHERE ApplicationId = @ApplicationId
    ORDER BY RoleName
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[aspnet_UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_aspnet_UsersInRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX [aspnet_UsersInRoles_index] ON [dbo].[aspnet_UsersInRoles] 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000)
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)


	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames  table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles  table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers  table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num	  int
	DECLARE @Pos	  int
	DECLARE @NextPos  int
	DECLARE @Name	  nvarchar(256)
	DECLARE @CountAll int
	DECLARE @CountU	  int
	DECLARE @CountR	  int


	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId
	SELECT @CountR = @@ROWCOUNT

	IF (@CountR <> @Num)
	BEGIN
		SELECT TOP 1 N'', Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END


	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1


	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	SELECT @CountU = @@ROWCOUNT
	IF (@CountU <> @Num)
	BEGIN
		SELECT TOP 1 Name, N''
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT au.LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE u.UserId = au.UserId)

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(1)
	END

	SELECT  @CountAll = COUNT(*)
	FROM	dbo.aspnet_UsersInRoles ur, @tbUsers u, @tbRoles r
	WHERE   ur.UserId = u.UserId AND ur.RoleId = r.RoleId

	IF (@CountAll <> @CountU * @CountR)
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 @tbUsers tu, @tbRoles tr, dbo.aspnet_Users u, dbo.aspnet_Roles r
		WHERE		 u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND
					 tu.UserId NOT IN (SELECT ur.UserId FROM dbo.aspnet_UsersInRoles ur WHERE ur.RoleId = tr.RoleId) AND
					 tr.RoleId NOT IN (SELECT ur.RoleId FROM dbo.aspnet_UsersInRoles ur WHERE ur.UserId = tu.UserId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	DELETE FROM dbo.aspnet_UsersInRoles
	WHERE UserId IN (SELECT UserId FROM @tbUsers)
	  AND RoleId IN (SELECT RoleId FROM @tbRoles)
	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_IsUserInRole]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(2)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    DECLARE @RoleId uniqueidentifier
    SELECT  @RoleId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(2)

    SELECT  @RoleId = RoleId
    FROM    dbo.aspnet_Roles
    WHERE   LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
        RETURN(3)

    IF (EXISTS( SELECT * FROM dbo.aspnet_UsersInRoles WHERE  UserId = @UserId AND RoleId = @RoleId))
        RETURN(1)
    ELSE
        RETURN(0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetUsersInRoles]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId
    ORDER BY u.UserName
    RETURN(0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetRolesForUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(1)

    SELECT r.RoleName
    FROM   dbo.aspnet_Roles r, dbo.aspnet_UsersInRoles ur
    WHERE  r.RoleId = ur.RoleId AND r.ApplicationId = @ApplicationId AND ur.UserId = @UserId
    ORDER BY r.RoleName
    RETURN (0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_FindUsersInRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256),
    @UserNameToMatch  nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId AND LoweredUserName LIKE LOWER(@UserNameToMatch)
    ORDER BY u.UserName
    RETURN(0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_AddUsersToRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000),
	@CurrentTimeUtc   datetime
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)
	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames	table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles	table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers	table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num		int
	DECLARE @Pos		int
	DECLARE @NextPos	int
	DECLARE @Name		nvarchar(256)

	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		SELECT TOP 1 Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END

	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1

	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		DELETE FROM @tbNames
		WHERE LOWER(Name) IN (SELECT LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE au.UserId = u.UserId)

		INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
		  SELECT @AppId, NEWID(), Name, LOWER(Name), 0, @CurrentTimeUtc
		  FROM   @tbNames

		INSERT INTO @tbUsers
		  SELECT  UserId
		  FROM	dbo.aspnet_Users au, @tbNames t
		  WHERE   LOWER(t.Name) = au.LoweredUserName AND au.ApplicationId = @AppId
	END

	IF (EXISTS (SELECT * FROM dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr WHERE tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId))
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr, aspnet_Users u, aspnet_Roles r
		WHERE		u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	INSERT INTO dbo.aspnet_UsersInRoles (UserId, RoleId)
	SELECT UserId, RoleId
	FROM @tbUsers, @tbRoles

	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Users_DeleteUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @TablesToDeleteFrom int,
    @NumTablesDeletedFrom int OUTPUT
AS
BEGIN
    DECLARE @UserId               uniqueidentifier
    SELECT  @UserId               = NULL
    SELECT  @NumTablesDeletedFrom = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    DECLARE @ErrorCode   int
    DECLARE @RowCount    int

    SET @ErrorCode = 0
    SET @RowCount  = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   u.LoweredUserName       = LOWER(@UserName)
        AND u.ApplicationId         = a.ApplicationId
        AND LOWER(@ApplicationName) = a.LoweredApplicationName

    IF (@UserId IS NULL)
    BEGIN
        GOTO Cleanup
    END

    -- Delete from Membership table if (@TablesToDeleteFrom & 1) is set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        DELETE FROM dbo.aspnet_Membership WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
               @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_UsersInRoles table if (@TablesToDeleteFrom & 2) is set
    IF ((@TablesToDeleteFrom & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_UsersInRoles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_UsersInRoles WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Profile table if (@TablesToDeleteFrom & 4) is set
    IF ((@TablesToDeleteFrom & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_Profile WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_PersonalizationPerUser table if (@TablesToDeleteFrom & 8) is set
    IF ((@TablesToDeleteFrom & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Users table if (@TablesToDeleteFrom & 1,2,4 & 8) are all set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (@TablesToDeleteFrom & 2) <> 0 AND
        (@TablesToDeleteFrom & 4) <> 0 AND
        (@TablesToDeleteFrom & 8) <> 0 AND
        (EXISTS (SELECT UserId FROM dbo.aspnet_Users WHERE @UserId = UserId)))
    BEGIN
        DELETE FROM dbo.aspnet_Users WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:
    SET @NumTablesDeletedFrom = 0

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
	    ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Roles_DeleteRole]
    @ApplicationName            nvarchar(256),
    @RoleName                   nvarchar(256),
    @DeleteOnlyIfRoleIsEmpty    bit
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    DECLARE @RoleId   uniqueidentifier
    SELECT  @RoleId = NULL
    SELECT  @RoleId = RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
    BEGIN
        SELECT @ErrorCode = 1
        GOTO Cleanup
    END
    IF (@DeleteOnlyIfRoleIsEmpty <> 0)
    BEGIN
        IF (EXISTS (SELECT RoleId FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId))
        BEGIN
            SELECT @ErrorCode = 2
            GOTO Cleanup
        END
    END


    DELETE FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DELETE FROM dbo.aspnet_Roles WHERE @RoleId = RoleId  AND ApplicationId = @ApplicationId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUserInfo]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @IsPasswordCorrect              bit,
    @UpdateLastLoginActivityDate    bit,
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @LastLoginDate                  datetime,
    @LastActivityDate               datetime
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @IsApproved                             bit
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @IsApproved = m.IsApproved,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        GOTO Cleanup
    END

    IF( @IsPasswordCorrect = 0 )
    BEGIN
        IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAttemptWindowStart ) )
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = 1
        END
        ELSE
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1
        END

        BEGIN
            IF( @FailedPasswordAttemptCount >= @MaxInvalidPasswordAttempts )
            BEGIN
                SET @IsLockedOut = 1
                SET @LastLockoutDate = @CurrentTimeUtc
            END
        END
    END
    ELSE
    BEGIN
        IF( @FailedPasswordAttemptCount > 0 OR @FailedPasswordAnswerAttemptCount > 0 )
        BEGIN
            SET @FailedPasswordAttemptCount = 0
            SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @FailedPasswordAnswerAttemptCount = 0
            SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )
        END
    END

    IF( @UpdateLastLoginActivityDate = 1 )
    BEGIN
        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @LastActivityDate
        WHERE   @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END

        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @LastLoginDate
        WHERE   UserId = @UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END


    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
        FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
        FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
        FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
        FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
    WHERE @UserId = UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUser]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @Email                nvarchar(256),
    @Comment              ntext,
    @IsApproved           bit,
    @LastLoginDate        datetime,
    @LastActivityDate     datetime,
    @UniqueEmail          int,
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId, @ApplicationId = a.ApplicationId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership WITH (UPDLOCK, HOLDLOCK)
                    WHERE ApplicationId = @ApplicationId  AND @UserId <> UserId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            RETURN(7)
        END
    END

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    UPDATE dbo.aspnet_Users WITH (ROWLOCK)
    SET
         LastActivityDate = @LastActivityDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    UPDATE dbo.aspnet_Membership WITH (ROWLOCK)
    SET
         Email            = @Email,
         LoweredEmail     = LOWER(@Email),
         Comment          = @Comment,
         IsApproved       = @IsApproved,
         LastLoginDate    = @LastLoginDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN -1
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UnlockUser]
    @ApplicationName                         nvarchar(256),
    @UserName                                nvarchar(256)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
        RETURN 1

    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = 0,
        FailedPasswordAttemptCount = 0,
        FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        FailedPasswordAnswerAttemptCount = 0,
        FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        LastLockoutDate = CONVERT( datetime, '17540101', 112 )
    WHERE @UserId = UserId

    RETURN 0
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_SetPassword]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @NewPassword      nvarchar(128),
    @PasswordSalt     nvarchar(128),
    @CurrentTimeUtc   datetime,
    @PasswordFormat   int = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    UPDATE dbo.aspnet_Membership
    SET Password = @NewPassword, PasswordFormat = @PasswordFormat, PasswordSalt = @PasswordSalt,
        LastPasswordChangedDate = @CurrentTimeUtc
    WHERE @UserId = UserId
    RETURN(0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_ResetPassword]
    @ApplicationName             nvarchar(256),
    @UserName                    nvarchar(256),
    @NewPassword                 nvarchar(128),
    @MaxInvalidPasswordAttempts  int,
    @PasswordAttemptWindow       int,
    @PasswordSalt                nvarchar(128),
    @CurrentTimeUtc              datetime,
    @PasswordFormat              int = 0,
    @PasswordAnswer              nvarchar(128) = NULL
AS
BEGIN
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @UserId                                 uniqueidentifier
    SET     @UserId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    SELECT @IsLockedOut = IsLockedOut,
           @LastLockoutDate = LastLockoutDate,
           @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
           @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
           @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
           @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM dbo.aspnet_Membership WITH ( UPDLOCK )
    WHERE @UserId = UserId

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Membership
    SET    Password = @NewPassword,
           LastPasswordChangedDate = @CurrentTimeUtc,
           PasswordFormat = @PasswordFormat,
           PasswordSalt = @PasswordSalt
    WHERE  @UserId = UserId AND
           ( ( @PasswordAnswer IS NULL ) OR ( LOWER( PasswordAnswer ) = LOWER( @PasswordAnswer ) ) )

    IF ( @@ROWCOUNT = 0 )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
    ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

    IF( NOT ( @PasswordAnswer IS NULL ) )
    BEGIN
        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByUserId]
    @UserId               uniqueidentifier,
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    IF ( @UpdateLastActivity = 1 )
    BEGIN
        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        FROM     dbo.aspnet_Users
        WHERE    @UserId = UserId

        IF ( @@ROWCOUNT = 0 ) -- User ID not found
            RETURN -1
    END

    SELECT  m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate, m.LastLoginDate, u.LastActivityDate,
            m.LastPasswordChangedDate, u.UserName, m.IsLockedOut,
            m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId

    IF ( @@ROWCOUNT = 0 ) -- User ID not found
       RETURN -1

    RETURN 0
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByName]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier

    IF (@UpdateLastActivity = 1)
    BEGIN
        -- select user ID from aspnet_users table
        SELECT TOP 1 @UserId = u.UserId
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1

        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        WHERE    @UserId = UserId

        SELECT m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut, m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  @UserId = u.UserId AND u.UserId = m.UserId 
    END
    ELSE
    BEGIN
        SELECT TOP 1 m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1
    END

    RETURN 0
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByEmail]
    @ApplicationName  nvarchar(256),
    @Email            nvarchar(256)
AS
BEGIN
    IF( @Email IS NULL )
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                m.LoweredEmail IS NULL
    ELSE
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                LOWER(@Email) = m.LoweredEmail

    IF (@@rowcount = 0)
        RETURN(1)
    RETURN(0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetPasswordWithFormat]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @UpdateLastLoginActivityDate    bit,
    @CurrentTimeUtc                 datetime
AS
BEGIN
    DECLARE @IsLockedOut                        bit
    DECLARE @UserId                             uniqueidentifier
    DECLARE @Password                           nvarchar(128)
    DECLARE @PasswordSalt                       nvarchar(128)
    DECLARE @PasswordFormat                     int
    DECLARE @FailedPasswordAttemptCount         int
    DECLARE @FailedPasswordAnswerAttemptCount   int
    DECLARE @IsApproved                         bit
    DECLARE @LastActivityDate                   datetime
    DECLARE @LastLoginDate                      datetime

    SELECT  @UserId          = NULL

    SELECT  @UserId = u.UserId, @IsLockedOut = m.IsLockedOut, @Password=Password, @PasswordFormat=PasswordFormat,
            @PasswordSalt=PasswordSalt, @FailedPasswordAttemptCount=FailedPasswordAttemptCount,
		    @FailedPasswordAnswerAttemptCount=FailedPasswordAnswerAttemptCount, @IsApproved=IsApproved,
            @LastActivityDate = LastActivityDate, @LastLoginDate = LastLoginDate
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF (@UserId IS NULL)
        RETURN 1

    IF (@IsLockedOut = 1)
        RETURN 99

    SELECT   @Password, @PasswordFormat, @PasswordSalt, @FailedPasswordAttemptCount,
             @FailedPasswordAnswerAttemptCount, @IsApproved, @LastLoginDate, @LastActivityDate

    IF (@UpdateLastLoginActivityDate = 1 AND @IsApproved = 1)
    BEGIN
        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @CurrentTimeUtc
        WHERE   UserId = @UserId

        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @CurrentTimeUtc
        WHERE   @UserId = UserId
    END


    RETURN 0
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetPassword]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @PasswordAnswer                 nvarchar(128) = NULL
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @PasswordFormat                         int
    DECLARE @Password                               nvarchar(128)
    DECLARE @passAns                                nvarchar(128)
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @Password = m.Password,
            @passAns = m.PasswordAnswer,
            @PasswordFormat = m.PasswordFormat,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    IF ( NOT( @PasswordAnswer IS NULL ) )
    BEGIN
        IF( ( @passAns IS NULL ) OR ( LOWER( @passAns ) <> LOWER( @PasswordAnswer ) ) )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
        ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    IF( @ErrorCode = 0 )
        SELECT @Password, @PasswordFormat

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetNumberOfUsersOnline]
    @ApplicationName            nvarchar(256),
    @MinutesSinceLastInActive   int,
    @CurrentTimeUtc             datetime
AS
BEGIN
    DECLARE @DateActive datetime
    SELECT  @DateActive = DATEADD(minute,  -(@MinutesSinceLastInActive), @CurrentTimeUtc)

    DECLARE @NumOnline int
    SELECT  @NumOnline = COUNT(*)
    FROM    dbo.aspnet_Users u(NOLOCK),
            dbo.aspnet_Applications a(NOLOCK),
            dbo.aspnet_Membership m(NOLOCK)
    WHERE   u.ApplicationId = a.ApplicationId                  AND
            LastActivityDate > @DateActive                     AND
            a.LoweredApplicationName = LOWER(@ApplicationName) AND
            u.UserId = m.UserId
    RETURN(@NumOnline)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetAllUsers]
    @ApplicationName       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0


    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
    SELECT u.UserId
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u
    WHERE  u.ApplicationId = @ApplicationId AND u.UserId = m.UserId
    ORDER BY u.UserName

    SELECT @TotalRecords = @@ROWCOUNT

    SELECT u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName
    RETURN @TotalRecords
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByName]
    @ApplicationName       nvarchar(256),
    @UserNameToMatch       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT u.UserId
        FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND u.LoweredUserName LIKE LOWER(@UserNameToMatch)
        ORDER BY u.UserName


    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByEmail]
    @ApplicationName       nvarchar(256),
    @EmailToMatch          nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    IF( @EmailToMatch IS NULL )
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.Email IS NULL
            ORDER BY m.LoweredEmail
    ELSE
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.LoweredEmail LIKE LOWER(@EmailToMatch)
            ORDER BY m.LoweredEmail

    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY m.LoweredEmail

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_CreateUser]
    @ApplicationName                        nvarchar(256),
    @UserName                               nvarchar(256),
    @Password                               nvarchar(128),
    @PasswordSalt                           nvarchar(128),
    @Email                                  nvarchar(256),
    @PasswordQuestion                       nvarchar(256),
    @PasswordAnswer                         nvarchar(128),
    @IsApproved                             bit,
    @CurrentTimeUtc                         datetime,
    @CreateDate                             datetime = NULL,
    @UniqueEmail                            int      = 0,
    @PasswordFormat                         int      = 0,
    @UserId                                 uniqueidentifier OUTPUT
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @NewUserId uniqueidentifier
    SELECT @NewUserId = NULL

    DECLARE @IsLockedOut bit
    SET @IsLockedOut = 0

    DECLARE @LastLockoutDate  datetime
    SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAttemptCount int
    SET @FailedPasswordAttemptCount = 0

    DECLARE @FailedPasswordAttemptWindowStart  datetime
    SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAnswerAttemptCount int
    SET @FailedPasswordAnswerAttemptCount = 0

    DECLARE @FailedPasswordAnswerAttemptWindowStart  datetime
    SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @NewUserCreated bit
    DECLARE @ReturnValue   int
    SET @ReturnValue = 0

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    SET @CreateDate = @CurrentTimeUtc

    SELECT  @NewUserId = UserId FROM dbo.aspnet_Users WHERE LOWER(@UserName) = LoweredUserName AND @ApplicationId = ApplicationId
    IF ( @NewUserId IS NULL )
    BEGIN
        SET @NewUserId = @UserId
        EXEC @ReturnValue = dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CreateDate, @NewUserId OUTPUT
        SET @NewUserCreated = 1
    END
    ELSE
    BEGIN
        SET @NewUserCreated = 0
        IF( @NewUserId <> @UserId AND @UserId IS NOT NULL )
        BEGIN
            SET @ErrorCode = 6
            GOTO Cleanup
        END
    END

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @ReturnValue = -1 )
    BEGIN
        SET @ErrorCode = 10
        GOTO Cleanup
    END

    IF ( EXISTS ( SELECT UserId
                  FROM   dbo.aspnet_Membership
                  WHERE  @NewUserId = UserId ) )
    BEGIN
        SET @ErrorCode = 6
        GOTO Cleanup
    END

    SET @UserId = @NewUserId

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership m WITH ( UPDLOCK, HOLDLOCK )
                    WHERE ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            SET @ErrorCode = 7
            GOTO Cleanup
        END
    END

    IF (@NewUserCreated = 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate = @CreateDate
        WHERE  @UserId = UserId
        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    INSERT INTO dbo.aspnet_Membership
                ( ApplicationId,
                  UserId,
                  Password,
                  PasswordSalt,
                  Email,
                  LoweredEmail,
                  PasswordQuestion,
                  PasswordAnswer,
                  PasswordFormat,
                  IsApproved,
                  IsLockedOut,
                  CreateDate,
                  LastLoginDate,
                  LastPasswordChangedDate,
                  LastLockoutDate,
                  FailedPasswordAttemptCount,
                  FailedPasswordAttemptWindowStart,
                  FailedPasswordAnswerAttemptCount,
                  FailedPasswordAnswerAttemptWindowStart )
         VALUES ( @ApplicationId,
                  @UserId,
                  @Password,
                  @PasswordSalt,
                  @Email,
                  LOWER(@Email),
                  @PasswordQuestion,
                  @PasswordAnswer,
                  @PasswordFormat,
                  @IsApproved,
                  @IsLockedOut,
                  @CreateDate,
                  @CreateDate,
                  @CreateDate,
                  @LastLockoutDate,
                  @FailedPasswordAttemptCount,
                  @FailedPasswordAttemptWindowStart,
                  @FailedPasswordAnswerAttemptCount,
                  @FailedPasswordAnswerAttemptWindowStart )

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]
    @ApplicationName       nvarchar(256),
    @UserName              nvarchar(256),
    @NewPasswordQuestion   nvarchar(256),
    @NewPasswordAnswer     nvarchar(128)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Membership m, dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId
    IF (@UserId IS NULL)
    BEGIN
        RETURN(1)
    END

    UPDATE dbo.aspnet_Membership
    SET    PasswordQuestion = @NewPasswordQuestion, PasswordAnswer = @NewPasswordAnswer
    WHERE  UserId=@UserId
    RETURN(0)
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE PROCEDURE [dbo].[aspnet_AnyDataInTables]
    @TablesToCheck int
AS
BEGIN
    -- Check Membership table if (@TablesToCheck & 1) is set
    IF ((@TablesToCheck & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Membership))
        BEGIN
            SELECT N'aspnet_Membership'
            RETURN
        END
    END

    -- Check aspnet_Roles table if (@TablesToCheck & 2) is set
    IF ((@TablesToCheck & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Roles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 RoleId FROM dbo.aspnet_Roles))
        BEGIN
            SELECT N'aspnet_Roles'
            RETURN
        END
    END

    -- Check aspnet_Profile table if (@TablesToCheck & 4) is set
    IF ((@TablesToCheck & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Profile))
        BEGIN
            SELECT N'aspnet_Profile'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 8) is set
    IF ((@TablesToCheck & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_PersonalizationPerUser))
        BEGIN
            SELECT N'aspnet_PersonalizationPerUser'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 16) is set
    IF ((@TablesToCheck & 16) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'aspnet_WebEvent_LogEvent') AND (type = 'P'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 * FROM dbo.aspnet_WebEvent_Events))
        BEGIN
            SELECT N'aspnet_WebEvent_Events'
            RETURN
        END
    END

    -- Check aspnet_Users table if (@TablesToCheck & 1,2,4 & 8) are all set
    IF ((@TablesToCheck & 1) <> 0 AND
        (@TablesToCheck & 2) <> 0 AND
        (@TablesToCheck & 4) <> 0 AND
        (@TablesToCheck & 8) <> 0 AND
        (@TablesToCheck & 32) <> 0 AND
        (@TablesToCheck & 128) <> 0 AND
        (@TablesToCheck & 256) <> 0 AND
        (@TablesToCheck & 512) <> 0 AND
        (@TablesToCheck & 1024) <> 0)
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Users))
        BEGIN
            SELECT N'aspnet_Users'
            RETURN
        END
        IF (EXISTS(SELECT TOP 1 ApplicationId FROM dbo.aspnet_Applications))
        BEGIN
            SELECT N'aspnet_Applications'
            RETURN
        END
    END
END

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_ContentComment](
	[ContentCommentID] [uniqueidentifier] NOT NULL,
	[Root_ContentID] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CommenterIP] [nvarchar](32) NOT NULL,
	[CommenterName] [nvarchar](256) NOT NULL,
	[CommenterEmail] [nvarchar](256) NOT NULL,
	[CommenterURL] [nvarchar](256) NOT NULL,
	[PostComment] [nvarchar](max) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsSpam] [bit] NOT NULL,
 CONSTRAINT [PK_carrot_ContentComment] PRIMARY KEY NONCLUSTERED 
(
	[ContentCommentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_Content](
	[ContentID] [uniqueidentifier] NOT NULL,
	[Root_ContentID] [uniqueidentifier] NOT NULL,
	[Parent_ContentID] [uniqueidentifier] NULL,
	[IsLatestVersion] [bit] NOT NULL,
	[TitleBar] [nvarchar](256) NULL,
	[NavMenuText] [nvarchar](256) NULL,
	[PageHead] [nvarchar](256) NULL,
	[PageText] [nvarchar](max) NULL,
	[LeftPageText] [nvarchar](max) NULL,
	[RightPageText] [nvarchar](max) NULL,
	[NavOrder] [int] NOT NULL,
	[EditUserId] [uniqueidentifier] NULL,
	[EditDate] [datetime] NOT NULL,
	[TemplateFile] [nvarchar](256) NULL,
	[MetaKeyword] [nvarchar](1024) NULL,
	[MetaDescription] [nvarchar](1024) NULL,
	[CreditUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_carrot_Content] PRIMARY KEY CLUSTERED 
(
	[ContentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX [IDX_carrot_Content_EditUserId] ON [dbo].[carrot_Content] 
(
	[EditUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX [IDX_carrot_Content_Root_ContentID] ON [dbo].[carrot_Content] 
(
	[Root_ContentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_CategoryContentMapping](
	[CategoryContentMappingID] [uniqueidentifier] NOT NULL,
	[ContentCategoryID] [uniqueidentifier] NOT NULL,
	[Root_ContentID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_carrot_CategoryContentMapping] PRIMARY KEY NONCLUSTERED 
(
	[CategoryContentMappingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

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

	DECLARE @UTCDateTime Datetime
	SET @UTCDateTime = GetUTCDate()
	
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
					CONVERT(datetime, CONVERT(nvarchar(100), GoLiveDateLocal, 112)) AS DateMonth, 
					DATENAME(MONTH, GoLiveDateLocal) + ' ' + CAST(YEAR(GoLiveDateLocal) as nvarchar(100)) AS DateSlug
			FROM (SELECT Root_ContentID, SiteID, ContentTypeID, (GoLiveDateLocal - DAY(GoLiveDateLocal) + 1) as GoLiveDateLocal
				FROM [dbo].[carrot_RootContent]
				WHERE SiteID = @SiteID
					AND (PageActive = 1 OR @ActiveOnly = 0)
					AND (GoLiveDate < @UTCDateTime OR @ActiveOnly = 0)
					AND (RetireDate > @UTCDateTime OR @ActiveOnly = 0)
					AND ContentTypeID = @ContentTypeID ) AS Y) AS Z

		GROUP BY SiteID, DateMonth, DateSlug
		ORDER BY DateMonth DESC

	SELECT * FROM @tblTallies WHERE RowID <= @TakeTop ORDER BY RowID

    RETURN(0)

END

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
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE VIEW [dbo].[vw_aspnet_MembershipUsers]
  AS SELECT [dbo].[aspnet_Membership].[UserId],
            [dbo].[aspnet_Membership].[PasswordFormat],
            [dbo].[aspnet_Membership].[MobilePIN],
            [dbo].[aspnet_Membership].[Email],
            [dbo].[aspnet_Membership].[LoweredEmail],
            [dbo].[aspnet_Membership].[PasswordQuestion],
            [dbo].[aspnet_Membership].[PasswordAnswer],
            [dbo].[aspnet_Membership].[IsApproved],
            [dbo].[aspnet_Membership].[IsLockedOut],
            [dbo].[aspnet_Membership].[CreateDate],
            [dbo].[aspnet_Membership].[LastLoginDate],
            [dbo].[aspnet_Membership].[LastPasswordChangedDate],
            [dbo].[aspnet_Membership].[LastLockoutDate],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptWindowStart],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptWindowStart],
            [dbo].[aspnet_Membership].[Comment],
            [dbo].[aspnet_Users].[ApplicationId],
            [dbo].[aspnet_Users].[UserName],
            [dbo].[aspnet_Users].[MobileAlias],
            [dbo].[aspnet_Users].[IsAnonymous],
            [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Membership] INNER JOIN [dbo].[aspnet_Users]
      ON [dbo].[aspnet_Membership].[UserId] = [dbo].[aspnet_Users].[UserId]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_ContentSnippet]
AS 

SELECT csr.Root_ContentSnippetID, csr.SiteID, csr.ContentSnippetName, csr.ContentSnippetSlug, csr.CreateUserId, csr.CreateDate, 
	csr.ContentSnippetActive, cs.ContentSnippetID, cs.IsLatestVersion, cs.EditUserId, cs.EditDate, cs.ContentBody, 
	csr.Heartbeat_UserId, csr.EditHeartbeat, csr.GoLiveDate, csr.RetireDate,
	cast(case when csr.RetireDate < GetUTCDate() then 1 else 0 end as bit) as IsRetired,
	cast(case when csr.GoLiveDate > GetUTCDate() then 1 else 0 end as bit) as IsUnReleased,
	csvh.VersionCount
FROM carrot_RootContentSnippet AS csr 
	INNER JOIN carrot_ContentSnippet AS cs ON csr.Root_ContentSnippetID = cs.Root_ContentSnippetID
	INNER JOIN (SELECT COUNT(*) VersionCount, Root_ContentSnippetID 
				FROM [dbo].carrot_ContentSnippet
				GROUP BY Root_ContentSnippetID 
				) csvh on csr.Root_ContentSnippetID = csvh.Root_ContentSnippetID

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER OFF

GO
CREATE VIEW [dbo].[vw_aspnet_UsersInRoles]
  AS SELECT [dbo].[aspnet_UsersInRoles].[UserId], [dbo].[aspnet_UsersInRoles].[RoleId]
  FROM [dbo].[aspnet_UsersInRoles]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE PROCEDURE [dbo].[carrot_UpdateGoLiveLocal]
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

		declare @blogType as uniqueidentifier
		set @blogType = (select top 1 ContentTypeID from [dbo].[carrot_ContentType] (nolock) where ContentTypeValue = 'BlogEntry')

		DECLARE @tblContent TABLE
		(
		  GoLiveDate datetime,
		  GoLiveDateLocal datetime
		)

		DECLARE @tblBlogs TABLE
		(
		  GoLiveDate datetime,
		  GoLiveDateLocal datetime,
		  PostPrefix nvarchar(256)  
		)

		INSERT INTO @tblContent(GoLiveDate, GoLiveDateLocal)
		SELECT
			ref.value ('GoLiveDate[1]', 'datetime') as GoLiveDate,
			ref.value ('GoLiveDateLocal[1]', 'datetime') as GoLiveDateLocal
		FROM @xmlDocument.nodes ('//ContentLocalTime') T(ref);

		INSERT INTO @tblBlogs(GoLiveDate, GoLiveDateLocal, PostPrefix)
		SELECT
			ref.value ('GoLiveDate[1]', 'datetime') as GoLiveDate,
			ref.value ('GoLiveDateLocal[1]', 'datetime') as GoLiveDateLocal,
			ref.value ('PostPrefix[1]', 'nvarchar(256)') as PostPrefix
		FROM @xmlDocument.nodes ('//BlogPostPageUrl') T(ref);

		update @tblBlogs
			set PostPrefix = cast(DATEPART(YEAR, GoLiveDateLocal) as varchar(32)) + '/' + cast(DATEPART(MONTH, GoLiveDateLocal) as varchar(32)) + '/' + cast(DATEPART(DAY, GoLiveDateLocal) as varchar(32)) + '/'
		where PostPrefix is null or len(PostPrefix) < 3


		UPDATE rc
			SET GoLiveDateLocal = c.GoLiveDateLocal
		FROM [dbo].[carrot_RootContent] rc
			JOIN @tblContent c on rc.GoLiveDate = c.GoLiveDate
		WHERE rc.SiteID = @SiteID

		UPDATE rc
			SET [FileName] = replace(b.PostPrefix + '/' + rc.PageSlug, '//',  '/')
		FROM [dbo].[carrot_RootContent] rc
			JOIN @tblBlogs b on rc.GoLiveDate = b.GoLiveDate
		WHERE rc.SiteID = @SiteID 
				and rc.ContentTypeID = @blogType


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
CREATE TABLE [dbo].[carrot_TrackbackQueue](
	[TrackbackQueueID] [uniqueidentifier] NOT NULL,
	[Root_ContentID] [uniqueidentifier] NOT NULL,
	[TrackBackURL] [nvarchar](256) NOT NULL,
	[TrackBackResponse] [nvarchar](2048) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[TrackedBack] [bit] NOT NULL,
 CONSTRAINT [PK_carrot_TrackbackQueue] PRIMARY KEY NONCLUSTERED 
(
	[TrackbackQueueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_TagContentMapping](
	[TagContentMappingID] [uniqueidentifier] NOT NULL,
	[ContentTagID] [uniqueidentifier] NOT NULL,
	[Root_ContentID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_carrot_TagContentMapping] PRIMARY KEY NONCLUSTERED 
(
	[TagContentMappingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_Widget](
	[Root_WidgetID] [uniqueidentifier] NOT NULL,
	[Root_ContentID] [uniqueidentifier] NOT NULL,
	[WidgetOrder] [int] NOT NULL,
	[PlaceholderName] [nvarchar](256) NOT NULL,
	[ControlPath] [nvarchar](512) NOT NULL,
	[WidgetActive] [bit] NOT NULL,
	[GoLiveDate] [datetime] NOT NULL,
	[RetireDate] [datetime] NOT NULL,
 CONSTRAINT [PK_carrot_Widget] PRIMARY KEY CLUSTERED 
(
	[Root_WidgetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[carrot_WidgetData](
	[WidgetDataID] [uniqueidentifier] NOT NULL,
	[Root_WidgetID] [uniqueidentifier] NOT NULL,
	[IsLatestVersion] [bit] NOT NULL,
	[EditDate] [datetime] NOT NULL,
	[ControlProperties] [nvarchar](max) NULL,
 CONSTRAINT [PK_carrot_WidgetData] PRIMARY KEY CLUSTERED 
(
	[WidgetDataID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_Content]
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
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_CategoryCounted]
AS 

SELECT cc.ContentCategoryID, cc.SiteID, cc.CategoryText, cc.CategorySlug, cc.IsPublic, ISNULL(cc2.TheCount, 0) AS UseCount
FROM dbo.carrot_ContentCategory AS cc 
LEFT JOIN
      (SELECT ContentCategoryID, COUNT(Root_ContentID) AS TheCount
        FROM dbo.carrot_CategoryContentMapping
        GROUP BY ContentCategoryID) AS cc2 ON cc.ContentCategoryID = cc2.ContentCategoryID

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_TagCounted]
AS 

SELECT cc.ContentTagID, cc.SiteID, cc.TagText, cc.TagSlug, cc.IsPublic, ISNULL(cc2.TheCount, 0) AS UseCount
FROM dbo.carrot_ContentTag AS cc 
LEFT JOIN
      (SELECT ContentTagID, COUNT(Root_ContentID) AS TheCount
        FROM dbo.carrot_TagContentMapping
        GROUP BY ContentTagID) AS cc2 ON cc.ContentTagID = cc2.ContentTagID

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_UserData]
AS 


SELECT m.UserId, ud.UserNickName, ud.FirstName, ud.LastName, m.LoweredEmail, m.IsApproved, m.IsLockedOut, 
	m.CreateDate, m.LastLoginDate, m.UserName, m.LastActivityDate, ud.UserBio
FROM [dbo].vw_aspnet_MembershipUsers AS m 
LEFT JOIN [dbo].carrot_UserData AS ud ON m.UserId = ud.UserId

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_TrackbackQueue]
AS 

SELECT tb.TrackbackQueueID, tb.TrackBackURL, tb.TrackBackResponse, tb.CreateDate, tb.ModifiedDate, tb.TrackedBack, c.Root_ContentID, c.PageActive, c.SiteID
FROM [dbo].carrot_TrackbackQueue AS tb
INNER JOIN [dbo].carrot_RootContent AS c ON tb.Root_ContentID = c.Root_ContentID

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
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

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_Widget]
AS 


SELECT w.Root_WidgetID, w.Root_ContentID, w.WidgetOrder, w.PlaceholderName, w.ControlPath, w.GoLiveDate, w.RetireDate, 
	cast(case when w.RetireDate < GetUTCDate() then 1 else 0 end as bit) as IsRetired,
	cast(case when w.GoLiveDate > GetUTCDate() then 1 else 0 end as bit) as IsUnReleased,
	w.WidgetActive, wd.WidgetDataID, wd.IsLatestVersion, wd.EditDate, wd.ControlProperties, cr.SiteID
FROM [dbo].carrot_Widget AS w 
INNER JOIN [dbo].carrot_WidgetData AS wd ON w.Root_WidgetID = wd.Root_WidgetID 
INNER JOIN [dbo].carrot_RootContent AS cr ON w.Root_ContentID = cr.Root_ContentID

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_TagURL]
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
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_EditorURL]
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
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_ContentChild]
AS 

SELECT DISTINCT cc.SiteID, cc.Root_ContentID, cc.[FileName], 
          cc.RetireDate, cc.GoLiveDate, 
          cc.IsRetired, cc.IsUnReleased, 
          cp.Root_ContentID as Parent_ContentID, cp.[FileName] AS ParentFileName,
          cp.RetireDate AS ParentRetireDate, cp.GoLiveDate AS ParentGoLiveDate, 
          cp.IsRetired as IsParentRetired, cp.IsUnReleased as IsParentUnReleased
FROM dbo.vw_carrot_Content AS cc 
	INNER JOIN dbo.vw_carrot_Content AS cp ON cc.Parent_ContentID = cp.Root_ContentID
WHERE cp.IsLatestVersion = 1 AND cc.IsLatestVersion = 1
	AND cc.SiteID = cp.SiteID

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_Comment]
AS 

SELECT cc.ContentCommentID, cc.CreateDate, cc.CommenterIP, cc.CommenterName, cc.CommenterEmail, cc.CommenterURL, cc.PostComment, cc.IsApproved, cc.IsSpam, 
	c.Root_ContentID, c.SiteID, c.[FileName], c.PageHead, c.TitleBar, c.NavMenuText, c.ContentTypeID, c.IsRetired, c.IsUnReleased, c.RetireDate, c.GoLiveDate
FROM [dbo].carrot_ContentComment AS cc 
	INNER JOIN [dbo].vw_carrot_Content AS c on cc.Root_ContentID = c.Root_ContentID
WHERE c.IsLatestVersion = 1

GO
SET ANSI_NULLS ON

GO
SET QUOTED_IDENTIFIER ON

GO
CREATE VIEW [dbo].[vw_carrot_CategoryURL]
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
ALTER TABLE [dbo].[aspnet_Applications] ADD  CONSTRAINT [DF_aspnet_Applications_ApplicationId]  DEFAULT (newid()) FOR [ApplicationId]

GO
ALTER TABLE [dbo].[aspnet_Membership] ADD  CONSTRAINT [DF_aspnet_Membership_PasswordFormat]  DEFAULT ((0)) FOR [PasswordFormat]

GO
ALTER TABLE [dbo].[aspnet_Roles] ADD  CONSTRAINT [DF_aspnet_Roles_RoleId]  DEFAULT (newid()) FOR [RoleId]

GO
ALTER TABLE [dbo].[aspnet_Users] ADD  CONSTRAINT [DF_aspnet_Users_UserId]  DEFAULT (newid()) FOR [UserId]

GO
ALTER TABLE [dbo].[aspnet_Users] ADD  CONSTRAINT [DF_aspnet_Users_MobileAlias]  DEFAULT (NULL) FOR [MobileAlias]

GO
ALTER TABLE [dbo].[aspnet_Users] ADD  CONSTRAINT [DF_aspnet_Users_IsAnonymous]  DEFAULT ((0)) FOR [IsAnonymous]

GO
ALTER TABLE [dbo].[carrot_CategoryContentMapping] ADD  CONSTRAINT [DF_carrot_CategoryContentMapping_CategoryContentMappingID]  DEFAULT (newid()) FOR [CategoryContentMappingID]

GO
ALTER TABLE [dbo].[carrot_Content] ADD  CONSTRAINT [DF_carrot_Content_ContentID]  DEFAULT (newid()) FOR [ContentID]

GO
ALTER TABLE [dbo].[carrot_Content] ADD  CONSTRAINT [DF_carrot_Content_EditDate]  DEFAULT (getdate()) FOR [EditDate]

GO
ALTER TABLE [dbo].[carrot_ContentCategory] ADD  CONSTRAINT [DF_carrot_ContentCategory_ContentCategoryID]  DEFAULT (newid()) FOR [ContentCategoryID]

GO
ALTER TABLE [dbo].[carrot_ContentComment] ADD  CONSTRAINT [DF_carrot_ContentComment_ContentCommentID]  DEFAULT (newid()) FOR [ContentCommentID]

GO
ALTER TABLE [dbo].[carrot_ContentComment] ADD  CONSTRAINT [DF_carrot_ContentComment_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]

GO
ALTER TABLE [dbo].[carrot_ContentSnippet] ADD  CONSTRAINT [DF_carrot_ContentSnippet_ContentSnippetID]  DEFAULT (newid()) FOR [ContentSnippetID]

GO
ALTER TABLE [dbo].[carrot_ContentTag] ADD  CONSTRAINT [DF_carrot_ContentTag_ContentTagID]  DEFAULT (newid()) FOR [ContentTagID]

GO
ALTER TABLE [dbo].[carrot_ContentType] ADD  CONSTRAINT [DF_carrot_ContentType_ContentTypeID]  DEFAULT (newid()) FOR [ContentTypeID]

GO
ALTER TABLE [dbo].[carrot_DataInfo] ADD  CONSTRAINT [DF_carrot_DataInfo_DataInfoID]  DEFAULT (newid()) FOR [DataInfoID]

GO
ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_Root_ContentID]  DEFAULT (newid()) FOR [Root_ContentID]

GO
ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]

GO
ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_GoLiveDate]  DEFAULT (getutcdate()) FOR [GoLiveDate]

GO
ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_GoLiveDateLocal]  DEFAULT (getutcdate()) FOR [GoLiveDateLocal]

GO
ALTER TABLE [dbo].[carrot_RootContent] ADD  CONSTRAINT [DF_carrot_RootContent_RetireDate]  DEFAULT (getutcdate()) FOR [RetireDate]

GO
ALTER TABLE [dbo].[carrot_RootContentSnippet] ADD  CONSTRAINT [DF_carrot_RootContentSnippet_Root_ContentSnippetID]  DEFAULT (newid()) FOR [Root_ContentSnippetID]

GO
ALTER TABLE [dbo].[carrot_SerialCache] ADD  CONSTRAINT [DF_carrot_SerialCache_SerialCacheID]  DEFAULT (newid()) FOR [SerialCacheID]

GO
ALTER TABLE [dbo].[carrot_SerialCache] ADD  CONSTRAINT [DF_carrot_SerialCache_EditDate]  DEFAULT (getdate()) FOR [EditDate]

GO
ALTER TABLE [dbo].[carrot_Sites] ADD  CONSTRAINT [DF_carrot_Sites_SiteID]  DEFAULT (newid()) FOR [SiteID]

GO
ALTER TABLE [dbo].[carrot_TagContentMapping] ADD  CONSTRAINT [DF_carrot_TagContentMapping_TagContentMappingID]  DEFAULT (newid()) FOR [TagContentMappingID]

GO
ALTER TABLE [dbo].[carrot_TextWidget] ADD  CONSTRAINT [DF_carrot_TextWidget_TextWidgetID]  DEFAULT (newid()) FOR [TextWidgetID]

GO
ALTER TABLE [dbo].[carrot_TrackbackQueue] ADD  CONSTRAINT [DF_carrot_TrackbackQueue_TrackbackQueueID]  DEFAULT (newid()) FOR [TrackbackQueueID]

GO
ALTER TABLE [dbo].[carrot_TrackbackQueue] ADD  CONSTRAINT [DF_carrot_TrackbackQueue_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]

GO
ALTER TABLE [dbo].[carrot_UserSiteMapping] ADD  CONSTRAINT [DF_carrot_UserSiteMapping_UserSiteMappingID]  DEFAULT (newid()) FOR [UserSiteMappingID]

GO
ALTER TABLE [dbo].[carrot_Widget] ADD  CONSTRAINT [DF_carrot_Widget_Root_WidgetID]  DEFAULT (newid()) FOR [Root_WidgetID]

GO
ALTER TABLE [dbo].[carrot_WidgetData] ADD  CONSTRAINT [DF_carrot_WidgetData_WidgetDataID]  DEFAULT (newid()) FOR [WidgetDataID]

GO
ALTER TABLE [dbo].[carrot_WidgetData] ADD  CONSTRAINT [DF_carrot_WidgetData_EditDate]  DEFAULT (getdate()) FOR [EditDate]

GO
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_Membership_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])

GO
ALTER TABLE [dbo].[aspnet_Membership] CHECK CONSTRAINT [FK_aspnet_Membership_ApplicationId]

GO
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_Membership_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])

GO
ALTER TABLE [dbo].[aspnet_Membership] CHECK CONSTRAINT [FK_aspnet_Membership_UserId]

GO
ALTER TABLE [dbo].[aspnet_Roles]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_Roles_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])

GO
ALTER TABLE [dbo].[aspnet_Roles] CHECK CONSTRAINT [FK_aspnet_Roles_ApplicationId]

GO
ALTER TABLE [dbo].[aspnet_Users]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_Users_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])

GO
ALTER TABLE [dbo].[aspnet_Users] CHECK CONSTRAINT [FK_aspnet_Users_ApplicationId]

GO
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_UsersInRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])

GO
ALTER TABLE [dbo].[aspnet_UsersInRoles] CHECK CONSTRAINT [FK_aspnet_UsersInRoles_RoleId]

GO
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_UsersInRoles_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])

GO
ALTER TABLE [dbo].[aspnet_UsersInRoles] CHECK CONSTRAINT [FK_aspnet_UsersInRoles_UserId]

GO
ALTER TABLE [dbo].[carrot_CategoryContentMapping]  WITH CHECK ADD  CONSTRAINT [FK_carrot_CategoryContentMapping_ContentCategoryID] FOREIGN KEY([ContentCategoryID])
REFERENCES [dbo].[carrot_ContentCategory] ([ContentCategoryID])

GO
ALTER TABLE [dbo].[carrot_CategoryContentMapping] CHECK CONSTRAINT [FK_carrot_CategoryContentMapping_ContentCategoryID]

GO
ALTER TABLE [dbo].[carrot_CategoryContentMapping]  WITH CHECK ADD  CONSTRAINT [FK_carrot_CategoryContentMapping_Root_ContentID] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])

GO
ALTER TABLE [dbo].[carrot_CategoryContentMapping] CHECK CONSTRAINT [FK_carrot_CategoryContentMapping_Root_ContentID]

GO
ALTER TABLE [dbo].[carrot_Content]  WITH CHECK ADD  CONSTRAINT [carrot_Content_CreditUserId_FK] FOREIGN KEY([CreditUserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])

GO
ALTER TABLE [dbo].[carrot_Content] CHECK CONSTRAINT [carrot_Content_CreditUserId_FK]

GO
ALTER TABLE [dbo].[carrot_Content]  WITH CHECK ADD  CONSTRAINT [carrot_Content_EditUserId_FK] FOREIGN KEY([EditUserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])

GO
ALTER TABLE [dbo].[carrot_Content] CHECK CONSTRAINT [carrot_Content_EditUserId_FK]

GO
ALTER TABLE [dbo].[carrot_Content]  WITH CHECK ADD  CONSTRAINT [carrot_RootContent_carrot_Content_FK] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])

GO
ALTER TABLE [dbo].[carrot_Content] CHECK CONSTRAINT [carrot_RootContent_carrot_Content_FK]

GO
ALTER TABLE [dbo].[carrot_ContentCategory]  WITH CHECK ADD  CONSTRAINT [FK_carrot_ContentCategory_SiteID] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])

GO
ALTER TABLE [dbo].[carrot_ContentCategory] CHECK CONSTRAINT [FK_carrot_ContentCategory_SiteID]

GO
ALTER TABLE [dbo].[carrot_ContentComment]  WITH CHECK ADD  CONSTRAINT [FK_carrot_ContentComment_Root_ContentID] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])

GO
ALTER TABLE [dbo].[carrot_ContentComment] CHECK CONSTRAINT [FK_carrot_ContentComment_Root_ContentID]

GO
ALTER TABLE [dbo].[carrot_ContentSnippet]  WITH CHECK ADD  CONSTRAINT [FK_carrot_ContentSnippet_Root_ContentSnippetID] FOREIGN KEY([Root_ContentSnippetID])
REFERENCES [dbo].[carrot_RootContentSnippet] ([Root_ContentSnippetID])

GO
ALTER TABLE [dbo].[carrot_ContentSnippet] CHECK CONSTRAINT [FK_carrot_ContentSnippet_Root_ContentSnippetID]

GO
ALTER TABLE [dbo].[carrot_ContentTag]  WITH CHECK ADD  CONSTRAINT [FK_carrot_ContentTag_SiteID] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])

GO
ALTER TABLE [dbo].[carrot_ContentTag] CHECK CONSTRAINT [FK_carrot_ContentTag_SiteID]

GO
ALTER TABLE [dbo].[carrot_RootContent]  WITH CHECK ADD  CONSTRAINT [carrot_ContentType_carrot_RootContent_FK] FOREIGN KEY([ContentTypeID])
REFERENCES [dbo].[carrot_ContentType] ([ContentTypeID])

GO
ALTER TABLE [dbo].[carrot_RootContent] CHECK CONSTRAINT [carrot_ContentType_carrot_RootContent_FK]

GO
ALTER TABLE [dbo].[carrot_RootContent]  WITH CHECK ADD  CONSTRAINT [carrot_RootContent_CreateUserId_FK] FOREIGN KEY([CreateUserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])

GO
ALTER TABLE [dbo].[carrot_RootContent] CHECK CONSTRAINT [carrot_RootContent_CreateUserId_FK]

GO
ALTER TABLE [dbo].[carrot_RootContent]  WITH CHECK ADD  CONSTRAINT [carrot_Sites_carrot_RootContent_FK] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])

GO
ALTER TABLE [dbo].[carrot_RootContent] CHECK CONSTRAINT [carrot_Sites_carrot_RootContent_FK]

GO
ALTER TABLE [dbo].[carrot_RootContentSnippet]  WITH CHECK ADD  CONSTRAINT [FK_carrot_RootContentSnippet_SiteID] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])

GO
ALTER TABLE [dbo].[carrot_RootContentSnippet] CHECK CONSTRAINT [FK_carrot_RootContentSnippet_SiteID]

GO
ALTER TABLE [dbo].[carrot_TagContentMapping]  WITH CHECK ADD  CONSTRAINT [FK_carrot_TagContentMapping_ContentTagID] FOREIGN KEY([ContentTagID])
REFERENCES [dbo].[carrot_ContentTag] ([ContentTagID])

GO
ALTER TABLE [dbo].[carrot_TagContentMapping] CHECK CONSTRAINT [FK_carrot_TagContentMapping_ContentTagID]

GO
ALTER TABLE [dbo].[carrot_TagContentMapping]  WITH CHECK ADD  CONSTRAINT [FK_carrot_TagContentMapping_Root_ContentID] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])

GO
ALTER TABLE [dbo].[carrot_TagContentMapping] CHECK CONSTRAINT [FK_carrot_TagContentMapping_Root_ContentID]

GO
ALTER TABLE [dbo].[carrot_TextWidget]  WITH CHECK ADD  CONSTRAINT [FK_carrot_TextWidget_SiteID] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])

GO
ALTER TABLE [dbo].[carrot_TextWidget] CHECK CONSTRAINT [FK_carrot_TextWidget_SiteID]

GO
ALTER TABLE [dbo].[carrot_TrackbackQueue]  WITH CHECK ADD  CONSTRAINT [FK_carrot_TrackbackQueue_Root_ContentID] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])

GO
ALTER TABLE [dbo].[carrot_TrackbackQueue] CHECK CONSTRAINT [FK_carrot_TrackbackQueue_Root_ContentID]

GO
ALTER TABLE [dbo].[carrot_UserData]  WITH CHECK ADD  CONSTRAINT [FK_carrot_UserData_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])

GO
ALTER TABLE [dbo].[carrot_UserData] CHECK CONSTRAINT [FK_carrot_UserData_UserId]

GO
ALTER TABLE [dbo].[carrot_UserSiteMapping]  WITH CHECK ADD  CONSTRAINT [aspnet_Users_carrot_UserSiteMapping_FK] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])

GO
ALTER TABLE [dbo].[carrot_UserSiteMapping] CHECK CONSTRAINT [aspnet_Users_carrot_UserSiteMapping_FK]

GO
ALTER TABLE [dbo].[carrot_UserSiteMapping]  WITH CHECK ADD  CONSTRAINT [carrot_Sites_carrot_UserSiteMapping_FK] FOREIGN KEY([SiteID])
REFERENCES [dbo].[carrot_Sites] ([SiteID])

GO
ALTER TABLE [dbo].[carrot_UserSiteMapping] CHECK CONSTRAINT [carrot_Sites_carrot_UserSiteMapping_FK]

GO
ALTER TABLE [dbo].[carrot_Widget]  WITH CHECK ADD  CONSTRAINT [carrot_RootContent_carrot_Widget_FK] FOREIGN KEY([Root_ContentID])
REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID])

GO
ALTER TABLE [dbo].[carrot_Widget] CHECK CONSTRAINT [carrot_RootContent_carrot_Widget_FK]

GO
ALTER TABLE [dbo].[carrot_WidgetData]  WITH CHECK ADD  CONSTRAINT [carrot_WidgetData_Root_WidgetID_FK] FOREIGN KEY([Root_WidgetID])
REFERENCES [dbo].[carrot_Widget] ([Root_WidgetID])

GO
ALTER TABLE [dbo].[carrot_WidgetData] CHECK CONSTRAINT [carrot_WidgetData_Root_WidgetID_FK]

GO

--================================================================================

GO
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'common', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'membership', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'role manager', N'1', 1)

GO

declare @AppID uniqueidentifier
declare @GrpAdminID uniqueidentifier
declare @GrpEditID uniqueidentifier
declare @GrpUserID uniqueidentifier

set @AppID = NEWID()

IF ((select count([RoleId]) from [dbo].[aspnet_Roles] where [RoleName] = N'CarrotCMS Administrators') < 1) BEGIN

	INSERT [dbo].[aspnet_Applications] ([ApplicationName], [LoweredApplicationName], [ApplicationId], [Description]) 
		VALUES (N'/', N'/', @AppID, NULL)

	INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description])
		 VALUES (@AppID, NewID(), N'CarrotCMS Administrators', N'carrotcms administrators', NULL)
	INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description])
		 VALUES (@AppID, NewID(), N'CarrotCMS Editors', N'carrotcms editors', NULL)
	INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description])
		 VALUES (@AppID, NewID(), N'CarrotCMS Users', N'carrotcms users', NULL)

	SELECT 'new app "' + CAST( @AppID as varchar(60)) + '" created' as msg	

END ELSE BEGIN

	set @AppID = (select top 1 [ApplicationId] from [dbo].[aspnet_Roles] where [RoleName] = N'CarrotCMS Administrators')
	SELECT 'app "' + CAST( @AppID as varchar(60)) + '" located' as msg

END

set @GrpAdminID = (select top 1 [RoleId] from [dbo].[aspnet_Roles] where [RoleName] = N'CarrotCMS Administrators' AND [ApplicationId] = @AppID)
set @GrpEditID = (select top 1 [RoleId] from [dbo].[aspnet_Roles] where [RoleName] = N'CarrotCMS Editors' AND [ApplicationId] = @AppID)
set @GrpUserID = (select top 1 [RoleId] from [dbo].[aspnet_Roles] where [RoleName] = N'CarrotCMS Users' AND [ApplicationId] = @AppID)


select [RoleId], [RoleName] from [dbo].[aspnet_Roles] where [RoleId] in (@GrpAdminID, @GrpEditID, @GrpUserID)


GO

--=======================================

GO

IF ((select count(*) from [dbo].[carrot_ContentType] where [ContentTypeValue] = N'ContentEntry') < 1) BEGIN

	insert into [dbo].[carrot_ContentType]([ContentTypeValue])
	values('BlogEntry')

	insert into [dbo].[carrot_ContentType]([ContentTypeValue])
	values('ContentEntry')

END

GO

--=======================================

GO

if (not exists(select * from [carrot_DataInfo] where [DataKey] = 'DBSchema')) begin

	INSERT [dbo].[carrot_DataInfo] ([DataInfoID], [DataKey], [DataValue]) VALUES (NewID(), N'DBSchema', N'20200915')

end

GO