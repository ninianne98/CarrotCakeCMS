ALTER TABLE [dbo].[carrot_Sites]
    ADD CONSTRAINT [DF_carrot_Sites_SiteID] DEFAULT (newid()) FOR [SiteID];

