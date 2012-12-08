ALTER TABLE [dbo].[carrot_TagContentMapping]
    ADD CONSTRAINT [FK_carrot_TagContentMapping_ContentTagID] FOREIGN KEY ([ContentTagID]) REFERENCES [dbo].[carrot_ContentTag] ([ContentTagID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

