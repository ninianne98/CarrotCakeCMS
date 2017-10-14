CREATE TABLE [dbo].[tblCalendar] (
    [CalendarID]  UNIQUEIDENTIFIER CONSTRAINT [DF_tblCalendar_CalendarID] DEFAULT (newid()) NOT NULL,
    [EventDate]   DATETIME         NULL,
    [EventTitle]  VARCHAR (255)    NULL,
    [EventDetail] VARCHAR (MAX)    NULL,
    [IsActive]    BIT              NULL,
    [SiteID]      UNIQUEIDENTIFIER NULL,
    CONSTRAINT [tblCalendar_PK_UC1] PRIMARY KEY CLUSTERED ([CalendarID] ASC)
);

