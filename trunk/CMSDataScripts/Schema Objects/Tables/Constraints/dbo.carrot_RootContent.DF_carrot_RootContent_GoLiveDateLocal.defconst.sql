ALTER TABLE [dbo].[carrot_RootContent]
    ADD CONSTRAINT [DF_carrot_RootContent_GoLiveDateLocal] DEFAULT (getutcdate()) FOR [GoLiveDateLocal];

