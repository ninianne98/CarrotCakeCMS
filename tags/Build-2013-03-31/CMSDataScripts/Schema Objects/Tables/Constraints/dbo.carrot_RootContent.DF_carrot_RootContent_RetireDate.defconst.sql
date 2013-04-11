ALTER TABLE [dbo].[carrot_RootContent]
    ADD CONSTRAINT [DF_carrot_RootContent_RetireDate] DEFAULT (getutcdate()) FOR [RetireDate];
