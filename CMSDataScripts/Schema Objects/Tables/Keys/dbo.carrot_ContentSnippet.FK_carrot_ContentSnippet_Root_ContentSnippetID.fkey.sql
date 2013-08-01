ALTER TABLE [dbo].[carrot_ContentSnippet]
    ADD CONSTRAINT [FK_carrot_ContentSnippet_Root_ContentSnippetID] FOREIGN KEY ([Root_ContentSnippetID]) REFERENCES [dbo].[carrot_RootContentSnippet] ([Root_ContentSnippetID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

