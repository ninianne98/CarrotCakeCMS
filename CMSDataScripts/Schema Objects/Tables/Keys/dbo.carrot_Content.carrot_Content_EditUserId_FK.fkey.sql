ALTER TABLE [dbo].[carrot_Content]
    ADD CONSTRAINT [carrot_Content_EditUserId_FK] FOREIGN KEY ([EditUserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
