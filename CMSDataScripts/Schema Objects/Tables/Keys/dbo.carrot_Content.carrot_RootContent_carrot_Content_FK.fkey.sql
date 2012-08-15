ALTER TABLE [dbo].[carrot_Content]
    ADD CONSTRAINT [carrot_RootContent_carrot_Content_FK] FOREIGN KEY ([Root_ContentID]) REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

