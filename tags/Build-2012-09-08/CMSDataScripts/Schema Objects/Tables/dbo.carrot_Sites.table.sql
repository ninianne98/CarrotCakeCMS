CREATE TABLE [dbo].[carrot_Sites] (
    [SiteID]          UNIQUEIDENTIFIER NOT NULL,
    [MetaKeyword]     VARCHAR (1000)   NULL,
    [MetaDescription] VARCHAR (2000)   NULL,
    [SiteName]        VARCHAR (256)    NULL,
    [MainURL]         VARCHAR (256)    NULL,
    [BlockIndex]      BIT              NOT NULL,
    [SiteFolder]      VARCHAR (256)    NULL
);

