ALTER TABLE [dbo].[tblGalleryImage]  WITH CHECK ADD  CONSTRAINT [tblGallery_tblGalleryImage_FK] FOREIGN KEY([GalleryID])
REFERENCES [dbo].[tblGallery] ([GalleryID])


GO
ALTER TABLE [dbo].[tblGalleryImage] CHECK CONSTRAINT [tblGallery_tblGalleryImage_FK]

