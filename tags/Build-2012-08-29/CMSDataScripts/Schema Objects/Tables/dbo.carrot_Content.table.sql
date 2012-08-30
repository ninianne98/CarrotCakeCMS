CREATE TABLE [dbo].[carrot_Content] (
    [ContentID]        UNIQUEIDENTIFIER NOT NULL,
    [Root_ContentID]   UNIQUEIDENTIFIER NOT NULL,
    [Parent_ContentID] UNIQUEIDENTIFIER NULL,
    [IsLatestVersion]  BIT              NULL,
    [TitleBar]         VARCHAR (256)    NULL,
    [NavMenuText]      VARCHAR (256)    NULL,
    [PageHead]         VARCHAR (256)    NULL,
    [PageText]         VARCHAR (MAX)    NULL,
    [LeftPageText]     VARCHAR (MAX)    NULL,
    [RightPageText]    VARCHAR (MAX)    NULL,
    [NavOrder]         INT              NULL,
    [EditUserId]       UNIQUEIDENTIFIER NULL,
    [EditDate]         DATETIME         NOT NULL,
    [TemplateFile]     NVARCHAR (256)   NULL,
    [MetaKeyword]      VARCHAR (1000)   NULL,
    [MetaDescription]  VARCHAR (2000)   NULL
);

