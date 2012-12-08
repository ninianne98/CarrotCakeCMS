ALTER TABLE [dbo].[carrot_CategoryContentMapping]
    ADD CONSTRAINT [FK_carrot_CategoryContentMapping_Root_ContentID] FOREIGN KEY ([Root_ContentID]) REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

