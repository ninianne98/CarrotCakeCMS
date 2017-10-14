CREATE TABLE [dbo].[carrot_ThemeTemplate] (
    [ThemeTemplateID]       UNIQUEIDENTIFIER CONSTRAINT [DF_carrot_ThemeTemplate_TemplateID] DEFAULT (newid()) NOT NULL,
    [SiteID]                UNIQUEIDENTIFIER NOT NULL,
    [ArchiveFileName]       NVARCHAR (128)   NOT NULL,
    [ArchiveCaption]        NVARCHAR (128)   NOT NULL,
    [ArchiveBlurb]          NVARCHAR (512)   NULL,
    [ArchiveThumb]          NVARCHAR (128)   NULL,
    [DateAdded]             DATETIME         CONSTRAINT [DF_carrot_ThemeTemplate_DateAdded] DEFAULT (getdate()) NOT NULL,
    [IsResponsive]          BIT              CONSTRAINT [DF_carrot_ThemeTemplate_IsResponsive] DEFAULT ((0)) NOT NULL,
    [IsActive]              BIT              CONSTRAINT [DF_carrot_ThemeTemplate_IsActive] DEFAULT ((0)) NOT NULL,
    [UsesHtml5]             BIT              CONSTRAINT [DF_carrot_ThemeTemplate_UsesHtml5] DEFAULT ((0)) NOT NULL,
    [TemplateSourceUrl]     NVARCHAR (128)   NULL,
    [TemplateSourceCaption] NVARCHAR (128)   NULL,
    CONSTRAINT [PK_carrot_ThemeTemplate] PRIMARY KEY CLUSTERED ([ThemeTemplateID] ASC)
);

