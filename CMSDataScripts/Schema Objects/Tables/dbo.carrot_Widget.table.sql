CREATE TABLE [dbo].[carrot_Widget] (
    [Root_WidgetID]   UNIQUEIDENTIFIER NOT NULL,
    [Root_ContentID]  UNIQUEIDENTIFIER NOT NULL,
    [WidgetOrder]     INT              NOT NULL,
    [PlaceholderName] NVARCHAR (256)   NOT NULL,
    [ControlPath]     NVARCHAR (512)   NOT NULL,
    [WidgetActive]    BIT              NOT NULL
);

