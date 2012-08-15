ALTER TABLE [dbo].[aspnet_Roles]
    ADD CONSTRAINT [DF_aspnet_Roles_RoleId] DEFAULT (newid()) FOR [RoleId];

