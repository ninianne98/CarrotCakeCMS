ALTER TABLE [dbo].[carrot_Widget]
    ADD CONSTRAINT [carrot_RootContent_carrot_Widget_FK] FOREIGN KEY ([Root_ContentID]) REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

