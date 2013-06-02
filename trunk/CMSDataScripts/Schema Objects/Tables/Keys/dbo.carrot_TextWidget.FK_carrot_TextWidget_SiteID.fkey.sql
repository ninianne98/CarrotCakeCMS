ALTER TABLE [dbo].[carrot_TextWidget]
    ADD CONSTRAINT [FK_carrot_TextWidget_SiteID] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[carrot_Sites] ([SiteID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

