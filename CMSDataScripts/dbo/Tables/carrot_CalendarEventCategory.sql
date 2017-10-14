CREATE TABLE [dbo].[carrot_CalendarEventCategory] (
    [CalendarEventCategoryID] UNIQUEIDENTIFIER CONSTRAINT [DF_carrot_CalendarEventCategory_CalendarEventCategoryID] DEFAULT (newid()) NOT NULL,
    [CategoryFGColor]         VARCHAR (32)     NOT NULL,
    [CategoryBGColor]         VARCHAR (32)     NOT NULL,
    [CategoryName]            VARCHAR (128)    NOT NULL,
    [SiteID]                  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_carrot_CalendarEventCategory] PRIMARY KEY CLUSTERED ([CalendarEventCategoryID] ASC)
);

