ALTER TABLE [dbo].[carrot_TagContentMapping]
    ADD CONSTRAINT [DF_carrot_TagContentMapping_TagContentMappingID] DEFAULT (newid()) FOR [TagContentMappingID];
