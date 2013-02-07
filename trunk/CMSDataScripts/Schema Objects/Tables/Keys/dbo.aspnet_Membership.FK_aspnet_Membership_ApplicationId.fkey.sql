ALTER TABLE [dbo].[aspnet_Membership]
    ADD CONSTRAINT [FK_aspnet_Membership_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
