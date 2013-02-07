ALTER TABLE [dbo].[carrot_RootContent]
    ADD CONSTRAINT [DF_carrot_RootContent_CreateDate] DEFAULT (getdate()) FOR [CreateDate];
