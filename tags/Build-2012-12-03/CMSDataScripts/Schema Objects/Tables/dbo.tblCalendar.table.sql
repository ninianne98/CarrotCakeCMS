CREATE TABLE [dbo].[tblCalendar] (
    [CalendarID]  UNIQUEIDENTIFIER NOT NULL,
    [EventDate]   DATETIME         NULL,
    [EventTitle]  VARCHAR (255)    NULL,
    [EventDetail] VARCHAR (MAX)    NULL,
    [IsActive]    BIT              NULL,
    [SiteID]      UNIQUEIDENTIFIER NULL
);

