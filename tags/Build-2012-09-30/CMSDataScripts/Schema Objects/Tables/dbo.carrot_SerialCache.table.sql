CREATE TABLE [dbo].[carrot_SerialCache] (
    [SerialCacheID]  UNIQUEIDENTIFIER NOT NULL,
    [SiteID]         UNIQUEIDENTIFIER NOT NULL,
    [ItemID]         UNIQUEIDENTIFIER NOT NULL,
    [EditUserId]     UNIQUEIDENTIFIER NOT NULL,
    [KeyType]        VARCHAR (256)    NULL,
    [SerializedData] VARCHAR (MAX)    NULL,
    [EditDate]       DATETIME         NOT NULL
);

