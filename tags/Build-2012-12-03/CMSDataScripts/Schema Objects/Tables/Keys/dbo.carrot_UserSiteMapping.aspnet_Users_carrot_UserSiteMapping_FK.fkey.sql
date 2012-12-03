ALTER TABLE [dbo].[carrot_UserSiteMapping]
    ADD CONSTRAINT [aspnet_Users_carrot_UserSiteMapping_FK] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

