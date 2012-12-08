-- =============================================
-- Script Template
-- create_groups_and_roles.sql
-- =============================================

GO

INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'common', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'membership', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'role manager', N'1', 1)

GO


declare @AppID uniqueidentifier
declare @NewUserID uniqueidentifier
declare @DefUsername nvarchar(200)
declare @DefEmail nvarchar(200)


set @DefUsername = N'carrotadmin'  -- change to the username you want to use to start
set @DefEmail = N'user@example.com' -- change to the email address you want to link to the username

-- default password for the user is carrot123
--=========================================================

declare @GrpAdminID uniqueidentifier
declare @GrpEditID uniqueidentifier
declare @GrpUserID uniqueidentifier

set @AppID = NEWID()

set @NewUserID = NEWID()
set @DefUsername = LOWER(@DefUsername)
set @DefEmail = LOWER(@DefEmail)



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


IF (select count([UserId]) from [dbo].[aspnet_Users] where [UserName] = @DefUsername) < 1 BEGIN

	INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) 
		VALUES (@AppID, @NewUserID, @DefUsername, @DefUsername, NULL, 0, GetDate())

	SELECT 'user "' + @DefUsername + '" created' as msg	

	INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (@NewUserID, @GrpAdminID)
	INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (@NewUserID, @GrpEditID)
	INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (@NewUserID, @GrpUserID)

	INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) 
		VALUES (@AppID, @NewUserID, N'3zWCYeLdZe183VNsgMyVhoLKIvw=', 1, N'Xh6pEVmNItc0+hqRjtNJSA==', NULL, @DefEmail, @DefEmail, NULL, NULL, 1, 0, GetDate(), GetDate(), GetDate(), N'1754-01-01', 0, N'1754-01-01', 0, N'1754-01-01', NULL)
	
	SELECT 'user "' + @DefUsername + '" added to groups' as msg	

END ELSE BEGIN

	SELECT 'user "' + @DefUsername + '" not added, already exists' as msg

END


GO

