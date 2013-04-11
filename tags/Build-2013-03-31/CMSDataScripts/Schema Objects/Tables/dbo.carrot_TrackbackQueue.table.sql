CREATE TABLE [dbo].[carrot_TrackbackQueue] (
    [TrackbackQueueID]  UNIQUEIDENTIFIER NOT NULL,
    [Root_ContentID]    UNIQUEIDENTIFIER NOT NULL,
    [TrackBackURL]      NVARCHAR (256)   NOT NULL,
    [TrackBackResponse] NVARCHAR (2048)  NULL,
    [ModifiedDate]      DATETIME         NOT NULL,
    [CreateDate]        DATETIME         NOT NULL,
    [TrackedBack]       BIT              NOT NULL
);
