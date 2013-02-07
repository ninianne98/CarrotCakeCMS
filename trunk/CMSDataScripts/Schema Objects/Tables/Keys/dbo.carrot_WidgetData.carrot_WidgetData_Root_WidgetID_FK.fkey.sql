ALTER TABLE [dbo].[carrot_WidgetData]
    ADD CONSTRAINT [carrot_WidgetData_Root_WidgetID_FK] FOREIGN KEY ([Root_WidgetID]) REFERENCES [dbo].[carrot_Widget] ([Root_WidgetID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
