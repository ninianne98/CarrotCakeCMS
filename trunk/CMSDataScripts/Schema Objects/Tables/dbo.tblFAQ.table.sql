CREATE TABLE [dbo].[tblFAQ] (
    [FaqID]     UNIQUEIDENTIFIER NOT NULL,
    [Question]  VARCHAR (MAX)    NULL,
    [Answer]    VARCHAR (MAX)    NULL,
    [IsActive]  BIT              NULL,
    [SortOrder] INT              NULL,
    [dtStamp]   DATETIME         NULL,
    [SiteID]    UNIQUEIDENTIFIER NULL
);

