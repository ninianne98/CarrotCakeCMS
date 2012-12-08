CREATE TABLE [dbo].[carrot_Sites] (
    [SiteID]              UNIQUEIDENTIFIER NOT NULL,
    [SiteName]            VARCHAR (256)    NULL,
    [SiteTagline]         VARCHAR (1024)   NULL,
    [SiteTitlebarPattern] VARCHAR (1024)   NULL,
    [MainURL]             VARCHAR (256)    NULL,
    [BlockIndex]          BIT              NOT NULL,
    [MetaKeyword]         VARCHAR (1000)   NULL,
    [MetaDescription]     VARCHAR (2000)   NULL,
    [Blog_Root_ContentID] UNIQUEIDENTIFIER NULL,
    [Blog_FolderPath]     VARCHAR (256)    NULL,
    [Blog_CategoryPath]   VARCHAR (256)    NULL,
    [Blog_TagPath]        VARCHAR (256)    NULL,
    [Blog_DatePattern]    VARCHAR (64)     NULL
);

