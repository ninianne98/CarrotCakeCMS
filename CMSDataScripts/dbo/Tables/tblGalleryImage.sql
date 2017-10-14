CREATE TABLE [dbo].[tblGalleryImage] (
    [GalleryImageID] UNIQUEIDENTIFIER CONSTRAINT [DF_tblGalleryImage_GalleryImageID] DEFAULT (newid()) NOT NULL,
    [GalleryImage]   VARCHAR (512)    NULL,
    [ImageOrder]     INT              NULL,
    [GalleryID]      UNIQUEIDENTIFIER NULL,
    CONSTRAINT [tblGalleryImage_PK_UC1] PRIMARY KEY CLUSTERED ([GalleryImageID] ASC),
    CONSTRAINT [tblGallery_tblGalleryImage_FK] FOREIGN KEY ([GalleryID]) REFERENCES [dbo].[tblGallery] ([GalleryID])
);

