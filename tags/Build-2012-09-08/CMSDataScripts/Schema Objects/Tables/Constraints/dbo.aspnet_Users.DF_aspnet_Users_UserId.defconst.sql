ALTER TABLE [dbo].[aspnet_Users]
    ADD CONSTRAINT [DF_aspnet_Users_UserId] DEFAULT (newid()) FOR [UserId];

