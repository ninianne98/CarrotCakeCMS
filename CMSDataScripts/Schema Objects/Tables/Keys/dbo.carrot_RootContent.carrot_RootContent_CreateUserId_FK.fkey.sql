ALTER TABLE [dbo].[carrot_RootContent]
    ADD CONSTRAINT [carrot_RootContent_CreateUserId_FK] FOREIGN KEY ([CreateUserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

