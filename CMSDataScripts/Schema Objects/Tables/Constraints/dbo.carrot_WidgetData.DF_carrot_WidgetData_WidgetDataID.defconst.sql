ALTER TABLE [dbo].[carrot_WidgetData]
    ADD CONSTRAINT [DF_carrot_WidgetData_WidgetDataID] DEFAULT (newid()) FOR [WidgetDataID];

