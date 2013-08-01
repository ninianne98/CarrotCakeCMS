CREATE TABLE [dbo].[carrot_ContentSnippet] (
    [ContentSnippetID]      UNIQUEIDENTIFIER NOT NULL,
    [Root_ContentSnippetID] UNIQUEIDENTIFIER NOT NULL,
    [IsLatestVersion]       BIT              NOT NULL,
    [EditUserId]            UNIQUEIDENTIFIER NOT NULL,
    [EditDate]              DATETIME         NOT NULL,
    [ContentBody]           NVARCHAR (MAX)   NULL
);

