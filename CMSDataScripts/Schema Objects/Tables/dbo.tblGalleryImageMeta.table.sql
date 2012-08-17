CREATE TABLE [dbo].[tblGalleryImageMeta](
	[GalleryImageMetaID] [uniqueidentifier] NOT NULL,
	[GalleryImage] [varchar](512) NULL,
	[ImageMetaData] [varchar](max) NULL,
	[SiteID] [uniqueidentifier] NULL,
 CONSTRAINT [tblGalleryImageMeta_PK] PRIMARY KEY CLUSTERED 
(
	[GalleryImageMetaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


