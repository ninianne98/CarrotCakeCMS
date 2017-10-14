CREATE TABLE [dbo].[tblGalleryImageMeta] (
    [GalleryImageMetaID] UNIQUEIDENTIFIER CONSTRAINT [DF_tblGalleryImageMeta_GalleryImageMetaID] DEFAULT (newid()) NOT NULL,
    [GalleryImage]       VARCHAR (512)    NULL,
    [ImageTitle]         VARCHAR (256)    NULL,
    [ImageMetaData]      VARCHAR (MAX)    NULL,
    [SiteID]             UNIQUEIDENTIFIER NULL,
    CONSTRAINT [tblGalleryImageMeta_PK] PRIMARY KEY CLUSTERED ([GalleryImageMetaID] ASC)
);

