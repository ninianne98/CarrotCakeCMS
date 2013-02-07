ALTER TABLE [dbo].[carrot_CategoryContentMapping]
    ADD CONSTRAINT [DF_carrot_CategoryContentMapping_CategoryContentMappingID] DEFAULT (newid()) FOR [CategoryContentMappingID];
