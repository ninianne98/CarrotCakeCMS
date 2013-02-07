ALTER TABLE [dbo].[carrot_CategoryContentMapping]
    ADD CONSTRAINT [FK_carrot_CategoryContentMapping_ContentCategoryID] FOREIGN KEY ([ContentCategoryID]) REFERENCES [dbo].[carrot_ContentCategory] ([ContentCategoryID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
