-- 2011-07-20
-- added ability to serialize to the database


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblSerialCache_SerialCacheID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblSerialCache] DROP CONSTRAINT [DF_tblSerialCache_SerialCacheID]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblSerialCache_EditDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblSerialCache] DROP CONSTRAINT [DF_tblSerialCache_EditDate]
END


GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblSerialCache]') AND type in (N'U'))
BEGIN

	CREATE TABLE [dbo].[tblSerialCache](
		[SerialCacheID] [uniqueidentifier] NOT NULL,
		[SiteID] [uniqueidentifier] NOT NULL,
		[ItemID] [uniqueidentifier] NOT NULL,
		[EditUserId] [uniqueidentifier] NOT NULL,
		[KeyType] [varchar](256) NULL,
		[SerializedData] [varchar](max) NULL,
		[EditDate] [datetime] NOT NULL,
	 CONSTRAINT [tblSerialCache_PK] PRIMARY KEY CLUSTERED 
	(
		[SerialCacheID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

END


GO


ALTER TABLE [dbo].[tblSerialCache] ADD  CONSTRAINT [DF_tblSerialCache_SerialCacheID]  DEFAULT (newid()) FOR [SerialCacheID]

ALTER TABLE [dbo].[tblSerialCache] ADD  CONSTRAINT [DF_tblSerialCache_EditDate]  DEFAULT (getdate()) FOR [EditDate]


GO


IF not exists(select * from dbo.[aspnet_Roles] where RoleName = 'CarrotCMS Administrators' ) BEGIN	

	update dbo.[aspnet_Roles]
	set RoleName = 'CarrotCMS Administrators'
	where RoleName = 'Administrators'

	update dbo.[aspnet_Roles]
	set RoleName = 'CarrotCMS Editors'
	where RoleName = 'Editors'

	update dbo.[aspnet_Roles]
	set RoleName = 'CarrotCMS Users'
	where RoleName = 'Users'

	update dbo.[aspnet_Roles]
	set LoweredRoleName = LOWER(RoleName)

END	

