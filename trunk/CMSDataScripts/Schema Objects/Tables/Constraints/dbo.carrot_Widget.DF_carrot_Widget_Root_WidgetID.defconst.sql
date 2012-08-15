ALTER TABLE [dbo].[carrot_Widget]
    ADD CONSTRAINT [DF_carrot_Widget_Root_WidgetID] DEFAULT (newid()) FOR [Root_WidgetID];

