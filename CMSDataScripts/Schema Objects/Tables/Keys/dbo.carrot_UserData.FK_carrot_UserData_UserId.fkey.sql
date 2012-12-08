ALTER TABLE [dbo].[carrot_UserData]
    ADD CONSTRAINT [FK_carrot_UserData_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

