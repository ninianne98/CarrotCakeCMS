ALTER TABLE [dbo].[carrot_UserSiteMapping]
    ADD CONSTRAINT [carrot_Sites_carrot_UserSiteMapping_FK] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[carrot_Sites] ([SiteID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
