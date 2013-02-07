ALTER TABLE [dbo].[carrot_ContentTag]
    ADD CONSTRAINT [FK_carrot_ContentTag_SiteID] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[carrot_Sites] ([SiteID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
