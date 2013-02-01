CREATE TABLE [dbo].[carrot_ContentComment] (
    [ContentCommentID] UNIQUEIDENTIFIER NOT NULL,
    [Root_ContentID]   UNIQUEIDENTIFIER NOT NULL,
    [CreateDate]       DATETIME         NOT NULL,
    [CommenterIP]      NVARCHAR (32)    NOT NULL,
    [CommenterName]    NVARCHAR (256)   NOT NULL,
    [CommenterEmail]   NVARCHAR (256)   NOT NULL,
    [CommenterURL]     NVARCHAR (256)   NOT NULL,
    [PostComment]      NVARCHAR (MAX)   NULL,
    [IsApproved]       BIT              NOT NULL,
    [IsSpam]           BIT              NOT NULL
);



