ALTER TABLE [dbo].[carrot_TagContentMapping]
    ADD CONSTRAINT [FK_carrot_TagContentMapping_Root_ContentID] FOREIGN KEY ([Root_ContentID]) REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
