ALTER TABLE [dbo].[aspnet_Users]
    ADD CONSTRAINT [FK_aspnet_Users_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
