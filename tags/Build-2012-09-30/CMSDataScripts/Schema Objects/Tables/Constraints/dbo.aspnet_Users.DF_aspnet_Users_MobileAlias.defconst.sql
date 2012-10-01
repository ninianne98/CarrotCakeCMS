ALTER TABLE [dbo].[aspnet_Users]
    ADD CONSTRAINT [DF_aspnet_Users_MobileAlias] DEFAULT (NULL) FOR [MobileAlias];

