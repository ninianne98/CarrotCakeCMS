ALTER TABLE [dbo].[aspnet_Membership]
    ADD CONSTRAINT [DF_aspnet_Membership_PasswordFormat] DEFAULT ((0)) FOR [PasswordFormat];
