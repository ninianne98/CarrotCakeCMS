CREATE TABLE [dbo].[carrot_ContentTag] (
    [ContentTagID] UNIQUEIDENTIFIER NOT NULL,
    [SiteID]       UNIQUEIDENTIFIER NOT NULL,
    [TagText]      NVARCHAR (256)   NOT NULL,
    [TagSlug]      NVARCHAR (256)   NOT NULL,
    [IsPublic]     BIT              NOT NULL
);



