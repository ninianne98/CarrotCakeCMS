CREATE TABLE [dbo].[carrot_Content] (
    [ContentID]        UNIQUEIDENTIFIER NOT NULL,
    [Root_ContentID]   UNIQUEIDENTIFIER NOT NULL,
    [Parent_ContentID] UNIQUEIDENTIFIER NULL,
    [IsLatestVersion]  BIT              NOT NULL,
    [TitleBar]         NVARCHAR (256)   NULL,
    [NavMenuText]      NVARCHAR (256)   NULL,
    [PageHead]         NVARCHAR (256)   NULL,
    [PageText]         NVARCHAR (MAX)   NULL,
    [LeftPageText]     NVARCHAR (MAX)   NULL,
    [RightPageText]    NVARCHAR (MAX)   NULL,
    [NavOrder]         INT              NULL,
    [EditUserId]       UNIQUEIDENTIFIER NULL,
    [EditDate]         DATETIME         NOT NULL,
    [TemplateFile]     NVARCHAR (256)   NULL,
    [MetaKeyword]      NVARCHAR (1024)  NULL,
    [MetaDescription]  NVARCHAR (1024)  NULL
);

