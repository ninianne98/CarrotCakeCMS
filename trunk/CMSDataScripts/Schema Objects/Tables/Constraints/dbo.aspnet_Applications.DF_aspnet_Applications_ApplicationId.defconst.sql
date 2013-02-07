ALTER TABLE [dbo].[aspnet_Applications]
    ADD CONSTRAINT [DF_aspnet_Applications_ApplicationId] DEFAULT (newid()) FOR [ApplicationId];
