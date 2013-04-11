ALTER TABLE [dbo].[carrot_RootContent]
    ADD CONSTRAINT [DF_carrot_RootContent_Root_ContentID] DEFAULT (newid()) FOR [Root_ContentID];
