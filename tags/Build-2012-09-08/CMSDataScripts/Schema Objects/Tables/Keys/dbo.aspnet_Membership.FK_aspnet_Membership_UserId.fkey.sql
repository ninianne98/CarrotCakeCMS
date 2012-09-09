ALTER TABLE [dbo].[aspnet_Membership]
    ADD CONSTRAINT [FK_aspnet_Membership_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

