ALTER TABLE [dbo].[carrot_ContentCategory]
    ADD CONSTRAINT [FK_carrot_ContentCategory_SiteID] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[carrot_Sites] ([SiteID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

