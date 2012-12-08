ALTER TABLE [dbo].[carrot_ContentCategory]
    ADD CONSTRAINT [DF_carrot_ContentCategory_ContentCategoryID] DEFAULT (newid()) FOR [ContentCategoryID];

