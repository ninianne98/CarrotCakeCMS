ALTER TABLE [dbo].[carrot_RootContentSnippet]
    ADD CONSTRAINT [DF_carrot_RootContentSnippet_Root_ContentSnippetID] DEFAULT (newid()) FOR [Root_ContentSnippetID];

