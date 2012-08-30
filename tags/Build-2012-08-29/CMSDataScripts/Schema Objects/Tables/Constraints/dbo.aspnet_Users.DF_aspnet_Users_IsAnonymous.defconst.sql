ALTER TABLE [dbo].[aspnet_Users]
    ADD CONSTRAINT [DF_aspnet_Users_IsAnonymous] DEFAULT ((0)) FOR [IsAnonymous];

