ALTER TABLE [dbo].[tblGalleryImage] ADD  CONSTRAINT [DF_tblGalleryImage_GalleryImageID]  DEFAULT (newid()) FOR [GalleryImageID]


