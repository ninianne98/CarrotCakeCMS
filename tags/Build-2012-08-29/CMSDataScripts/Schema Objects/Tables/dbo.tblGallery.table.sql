CREATE TABLE [dbo].[tblGallery](
	[GalleryID] [uniqueidentifier] NOT NULL,
	[GalleryTitle] [varchar](255) NULL,
	[SiteID] [uniqueidentifier] NULL,
 CONSTRAINT [tblGallery_PK] PRIMARY KEY CLUSTERED 
(
	[GalleryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


