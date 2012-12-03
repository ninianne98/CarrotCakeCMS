ALTER TABLE [dbo].[carrot_WidgetData]
    ADD CONSTRAINT [DF_carrot_WidgetData_EditDate] DEFAULT (getdate()) FOR [EditDate];

