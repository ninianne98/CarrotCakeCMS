CREATE TABLE [dbo].[carrot_WidgetData] (
    [WidgetDataID]      UNIQUEIDENTIFIER NOT NULL,
    [Root_WidgetID]     UNIQUEIDENTIFIER NOT NULL,
    [IsLatestVersion]   BIT              NOT NULL,
    [EditDate]          DATETIME         NOT NULL,
    [ControlProperties] NVARCHAR (MAX)   NULL
);
