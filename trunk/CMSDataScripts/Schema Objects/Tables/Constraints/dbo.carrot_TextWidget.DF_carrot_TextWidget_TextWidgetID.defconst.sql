ALTER TABLE [dbo].[carrot_TextWidget]
    ADD CONSTRAINT [DF_carrot_TextWidget_TextWidgetID] DEFAULT (newid()) FOR [TextWidgetID];

