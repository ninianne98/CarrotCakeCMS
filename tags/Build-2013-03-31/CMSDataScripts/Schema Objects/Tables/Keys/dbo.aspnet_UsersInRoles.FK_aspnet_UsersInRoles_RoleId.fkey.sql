ALTER TABLE [dbo].[aspnet_UsersInRoles]
    ADD CONSTRAINT [FK_aspnet_UsersInRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[aspnet_Roles] ([RoleId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
