CREATE TABLE [dbo].[tblGallery] (
    [GalleryID]    UNIQUEIDENTIFIER CONSTRAINT [DF_tblGallery_GalleryID] DEFAULT (newid()) NOT NULL,
    [GalleryTitle] VARCHAR (255)    NULL,
    [SiteID]       UNIQUEIDENTIFIER NULL,
    CONSTRAINT [tblGallery_PK_UC1] PRIMARY KEY CLUSTERED ([GalleryID] ASC)
);

