ALTER TABLE [dbo].[carrot_Content]
    ADD CONSTRAINT [DF_carrot_Content_ContentID] DEFAULT (newid()) FOR [ContentID];

