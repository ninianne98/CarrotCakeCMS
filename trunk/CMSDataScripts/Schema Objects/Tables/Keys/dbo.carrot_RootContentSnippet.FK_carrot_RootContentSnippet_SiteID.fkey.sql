ALTER TABLE [dbo].[carrot_RootContentSnippet]
    ADD CONSTRAINT [FK_carrot_RootContentSnippet_SiteID] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[carrot_Sites] ([SiteID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

