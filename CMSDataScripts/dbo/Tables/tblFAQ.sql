CREATE TABLE [dbo].[tblFAQ] (
    [FaqID]     UNIQUEIDENTIFIER CONSTRAINT [DF_tblFAQ_FaqID] DEFAULT (newid()) NOT NULL,
    [Question]  VARCHAR (MAX)    NULL,
    [Answer]    VARCHAR (MAX)    NULL,
    [IsActive]  BIT              NULL,
    [SortOrder] INT              NULL,
    [dtStamp]   DATETIME         NULL,
    [SiteID]    UNIQUEIDENTIFIER NULL,
    CONSTRAINT [tblFAQ_PK_UC1] PRIMARY KEY CLUSTERED ([FaqID] ASC)
);

