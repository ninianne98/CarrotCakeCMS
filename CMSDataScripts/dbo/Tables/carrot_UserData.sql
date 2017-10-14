CREATE TABLE [dbo].[carrot_UserData] (
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [UserNickName] NVARCHAR (64)    NULL,
    [FirstName]    NVARCHAR (64)    NULL,
    [LastName]     NVARCHAR (64)    NULL,
    [UserBio]      NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_carrot_UserData] PRIMARY KEY NONCLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_carrot_UserData_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
);

