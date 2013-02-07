ALTER TABLE [dbo].[carrot_UserSiteMapping]
    ADD CONSTRAINT [DF_carrot_UserSiteMapping_UserSiteMappingID] DEFAULT (newid()) FOR [UserSiteMappingID];
