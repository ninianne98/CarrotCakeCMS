ALTER TABLE [dbo].[carrot_ContentSnippet]
    ADD CONSTRAINT [DF_carrot_ContentSnippet_ContentSnippetID] DEFAULT (newid()) FOR [ContentSnippetID];

