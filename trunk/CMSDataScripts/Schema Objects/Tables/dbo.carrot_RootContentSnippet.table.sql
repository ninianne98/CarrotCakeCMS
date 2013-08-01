CREATE TABLE [dbo].[carrot_RootContentSnippet] (
    [Root_ContentSnippetID] UNIQUEIDENTIFIER NOT NULL,
    [SiteID]                UNIQUEIDENTIFIER NOT NULL,
    [ContentSnippetName]    NVARCHAR (256)   NOT NULL,
    [ContentSnippetSlug]    NVARCHAR (128)   NOT NULL,
    [CreateUserId]          UNIQUEIDENTIFIER NOT NULL,
    [CreateDate]            DATETIME         NOT NULL,
    [GoLiveDate]            DATETIME         NOT NULL,
    [RetireDate]            DATETIME         NOT NULL,
    [ContentSnippetActive]  BIT              NOT NULL,
    [Heartbeat_UserId]      UNIQUEIDENTIFIER NULL,
    [EditHeartbeat]         DATETIME         NULL
);

