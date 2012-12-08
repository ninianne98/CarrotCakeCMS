CREATE TABLE [dbo].[carrot_ContentCategory] (
    [ContentCategoryID] UNIQUEIDENTIFIER NOT NULL,
    [SiteID]            UNIQUEIDENTIFIER NOT NULL,
    [CategoryText]      NVARCHAR (256)   NOT NULL,
    [CategorySlug]      NVARCHAR (256)   NOT NULL
);

