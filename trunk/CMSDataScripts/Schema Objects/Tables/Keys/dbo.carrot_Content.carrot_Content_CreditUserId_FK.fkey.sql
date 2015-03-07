ALTER TABLE [dbo].[carrot_Content]
    ADD CONSTRAINT [carrot_Content_CreditUserId_FK] FOREIGN KEY ([CreditUserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

