CREATE TABLE [dbo].[carrot_TextWidget] (
    [TextWidgetID]       UNIQUEIDENTIFIER NOT NULL,
    [SiteID]             UNIQUEIDENTIFIER NOT NULL,
    [TextWidgetAssembly] NVARCHAR (256)   NOT NULL,
    [ProcessBody]        BIT              NOT NULL,
    [ProcessPlainText]   BIT              NOT NULL,
    [ProcessHTMLText]    BIT              NOT NULL,
    [ProcessComment]     BIT              NOT NULL
);



