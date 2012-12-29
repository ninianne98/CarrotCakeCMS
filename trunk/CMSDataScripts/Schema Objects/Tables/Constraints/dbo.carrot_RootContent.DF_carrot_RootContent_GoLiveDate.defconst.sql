ALTER TABLE [dbo].[carrot_RootContent]
    ADD CONSTRAINT [DF_carrot_RootContent_GoLiveDate] DEFAULT (getutcdate()) FOR [GoLiveDate];

