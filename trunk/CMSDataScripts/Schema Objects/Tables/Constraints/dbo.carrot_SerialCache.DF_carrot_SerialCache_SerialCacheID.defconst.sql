ALTER TABLE [dbo].[carrot_SerialCache]
    ADD CONSTRAINT [DF_carrot_SerialCache_SerialCacheID] DEFAULT (newid()) FOR [SerialCacheID];
