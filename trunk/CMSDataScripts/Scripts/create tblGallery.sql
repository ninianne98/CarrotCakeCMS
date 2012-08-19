--  USE [CarrotwareCMS]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblGallery_GalleryID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblGallery] DROP CONSTRAINT [DF_tblGallery_GalleryID]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[tblGallery_tblGalleryImage_FK]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblGalleryImage]'))
ALTER TABLE [dbo].[tblGalleryImage] DROP CONSTRAINT [tblGallery_tblGalleryImage_FK]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblGalleryImage_GalleryImageID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tblGalleryImage] DROP CONSTRAINT [DF_tblGalleryImage_GalleryImageID]
END

GO

--  USE [CarrotwareCMS]
GO

/****** Object:  Table [dbo].[tblGallery]    Script Date: 11/11/2011 23:00:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblGallery]') AND type in (N'U'))
DROP TABLE [dbo].[tblGallery]
GO

/****** Object:  Table [dbo].[tblGalleryImage]    Script Date: 11/11/2011 23:00:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblGalleryImage]') AND type in (N'U'))
DROP TABLE [dbo].[tblGalleryImage]
GO

--  USE [CarrotwareCMS]
GO

/****** Object:  Table [dbo].[tblGallery]    Script Date: 11/11/2011 23:00:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblGallery](
	[GalleryID] [uniqueidentifier] NOT NULL,
	[GalleryTitle] [varchar](255) NULL,
	[SiteID] [uniqueidentifier] NULL,
 CONSTRAINT [tblGallery_PK] PRIMARY KEY CLUSTERED 
(
	[GalleryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--  USE [CarrotwareCMS]
GO

/****** Object:  Table [dbo].[tblGalleryImage]    Script Date: 11/11/2011 23:00:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblGalleryImage](
	[GalleryImageID] [uniqueidentifier] NOT NULL,
	[GalleryImage] [varchar](512) NULL,
	[ImageOrder]  int NULL,
	[GalleryID] [uniqueidentifier] NULL,
 CONSTRAINT [tblGalleryImage_PK] PRIMARY KEY CLUSTERED 
(
	[GalleryImageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblGallery] ADD  CONSTRAINT [DF_tblGallery_GalleryID]  DEFAULT (newid()) FOR [GalleryID]
GO

ALTER TABLE [dbo].[tblGalleryImage]  WITH CHECK ADD  CONSTRAINT [tblGallery_tblGalleryImage_FK] FOREIGN KEY([GalleryID])
REFERENCES [dbo].[tblGallery] ([GalleryID])
GO

ALTER TABLE [dbo].[tblGalleryImage] CHECK CONSTRAINT [tblGallery_tblGalleryImage_FK]
GO

ALTER TABLE [dbo].[tblGalleryImage] ADD  CONSTRAINT [DF_tblGalleryImage_GalleryImageID]  DEFAULT (newid()) FOR [GalleryImageID]
GO


/****** Object:  Table [dbo].[tblGalleryImageMeta]    Script Date: 08/16/2012 22:13:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblGalleryImageMeta](
	[GalleryImageMetaID] [uniqueidentifier] NOT NULL,
	[GalleryImage] [varchar](512) NULL,
	[ImageTitle] [varchar](256) NULL,
	[ImageMetaData] [varchar](max) NULL,
	[SiteID] [uniqueidentifier] NULL,
 CONSTRAINT [tblGalleryImageMeta_PK] PRIMARY KEY CLUSTERED 
(
	[GalleryImageMetaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblGalleryImageMeta] ADD  CONSTRAINT [DF_tblGalleryImageMeta_GalleryImageMetaID]  DEFAULT (newid()) FOR [GalleryImageMetaID]
GO

