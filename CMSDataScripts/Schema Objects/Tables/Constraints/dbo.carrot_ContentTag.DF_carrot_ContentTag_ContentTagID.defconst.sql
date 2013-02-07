ALTER TABLE [dbo].[carrot_ContentTag]
    ADD CONSTRAINT [DF_carrot_ContentTag_ContentTagID] DEFAULT (newid()) FOR [ContentTagID];
