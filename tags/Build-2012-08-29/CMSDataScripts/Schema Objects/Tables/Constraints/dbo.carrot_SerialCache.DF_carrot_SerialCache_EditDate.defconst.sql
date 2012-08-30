ALTER TABLE [dbo].[carrot_SerialCache]
    ADD CONSTRAINT [DF_carrot_SerialCache_EditDate] DEFAULT (getdate()) FOR [EditDate];

