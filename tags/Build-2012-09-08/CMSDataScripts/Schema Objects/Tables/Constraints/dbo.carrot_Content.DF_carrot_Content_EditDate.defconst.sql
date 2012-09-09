ALTER TABLE [dbo].[carrot_Content]
    ADD CONSTRAINT [DF_carrot_Content_EditDate] DEFAULT (getdate()) FOR [EditDate];

