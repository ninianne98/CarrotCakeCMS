CREATE TABLE [dbo].[tblGalleryImage](
	[GalleryImageID] [uniqueidentifier] NOT NULL,
	[GalleryImage] [varchar](512) NULL,
	[ImageOrder]  int NULL,
	[GalleryID] [uniqueidentifier] NULL,
 CONSTRAINT [tblGalleryImage_PK_UC1] PRIMARY KEY CLUSTERED 
(
	[GalleryImageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


