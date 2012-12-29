ALTER TABLE [dbo].[carrot_TrackbackQueue]
    ADD CONSTRAINT [FK_carrot_TrackbackQueue_Root_ContentID] FOREIGN KEY ([Root_ContentID]) REFERENCES [dbo].[carrot_RootContent] ([Root_ContentID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

