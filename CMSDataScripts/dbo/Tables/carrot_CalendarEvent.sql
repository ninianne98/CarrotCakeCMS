CREATE TABLE [dbo].[carrot_CalendarEvent] (
    [CalendarEventID]        UNIQUEIDENTIFIER CONSTRAINT [DF_carrot_CalendarEvent_CalendarEventID] DEFAULT (newid()) NOT NULL,
    [CalendarEventProfileID] UNIQUEIDENTIFIER NOT NULL,
    [EventDate]              DATETIME         NOT NULL,
    [EventDetail]            VARCHAR (MAX)    NULL,
    [IsCancelled]            BIT              NOT NULL,
    [EventStartTime]         TIME (7)         NULL,
    [EventEndTime]           TIME (7)         NULL,
    CONSTRAINT [PK_carrot_CalendarEvent] PRIMARY KEY CLUSTERED ([CalendarEventID] ASC),
    CONSTRAINT [FK_carrot_CalendarEvent_carrot_CalendarEventProfile] FOREIGN KEY ([CalendarEventProfileID]) REFERENCES [dbo].[carrot_CalendarEventProfile] ([CalendarEventProfileID])
);

