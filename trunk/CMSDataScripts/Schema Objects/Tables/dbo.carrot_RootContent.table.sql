CREATE TABLE [dbo].[carrot_RootContent] (
    [Root_ContentID]   UNIQUEIDENTIFIER NOT NULL,
    [SiteID]           UNIQUEIDENTIFIER NOT NULL,
    [Heartbeat_UserId] UNIQUEIDENTIFIER NULL,
    [EditHeartbeat]    DATETIME         NULL,
    [FileName]         NVARCHAR (256)   NOT NULL,
    [PageActive]       BIT              NOT NULL,
    [CreateDate]       DATETIME         NOT NULL,
    [ContentTypeID]    UNIQUEIDENTIFIER NOT NULL,
    [PageSlug]         NVARCHAR (256)   NULL
);

