ALTER TABLE [dbo].[carrot_ContentComment]
    ADD CONSTRAINT [FK_carrot_ContentComment_Root_ContentID] FOREIGN KEY ([Root_ContentID]) REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

