ALTER TABLE [dbo].[carrot_ContentType]
    ADD CONSTRAINT [DF_carrot_ContentType_ContentTypeID] DEFAULT (newid()) FOR [ContentTypeID];

