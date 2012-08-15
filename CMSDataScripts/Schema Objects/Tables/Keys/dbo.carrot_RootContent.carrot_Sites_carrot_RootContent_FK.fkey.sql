ALTER TABLE [dbo].[carrot_RootContent]
    ADD CONSTRAINT [carrot_Sites_carrot_RootContent_FK] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[carrot_Sites] ([SiteID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

