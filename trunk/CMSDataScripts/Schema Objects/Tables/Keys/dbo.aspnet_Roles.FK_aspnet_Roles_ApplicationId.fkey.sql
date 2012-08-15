ALTER TABLE [dbo].[aspnet_Roles]
    ADD CONSTRAINT [FK_aspnet_Roles_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

