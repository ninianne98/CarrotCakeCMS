ALTER TABLE [dbo].[aspnet_UsersInRoles]
    ADD CONSTRAINT [FK_aspnet_UsersInRoles_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

