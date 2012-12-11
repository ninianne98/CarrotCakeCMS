CREATE TABLE [dbo].[carrot_SerialCache] (
    [SerialCacheID]  UNIQUEIDENTIFIER NOT NULL,
    [SiteID]         UNIQUEIDENTIFIER NOT NULL,
    [ItemID]         UNIQUEIDENTIFIER NOT NULL,
    [EditUserId]     UNIQUEIDENTIFIER NOT NULL,
    [KeyType]        NVARCHAR (256)   NULL,
    [SerializedData] NVARCHAR (MAX)   NULL,
    [EditDate]       DATETIME         NOT NULL
);

