CREATE TABLE [dbo].[carrot_CalendarEventProfile] (
    [CalendarEventProfileID]  UNIQUEIDENTIFIER CONSTRAINT [DF_carrot_CalendarEvent_CalendarEventProfileID] DEFAULT (newid()) NOT NULL,
    [CalendarFrequencyID]     UNIQUEIDENTIFIER NOT NULL,
    [CalendarEventCategoryID] UNIQUEIDENTIFIER NOT NULL,
    [EventStartDate]          DATETIME         NOT NULL,
    [EventStartTime]          TIME (7)         NULL,
    [EventEndDate]            DATETIME         NOT NULL,
    [EventEndTime]            TIME (7)         NULL,
    [EventTitle]              VARCHAR (256)    NULL,
    [EventDetail]             VARCHAR (MAX)    NULL,
    [EventRepeatPattern]      INT              NULL,
    [IsAllDayEvent]           BIT              NOT NULL,
    [IsPublic]                BIT              NOT NULL,
    [IsCancelled]             BIT              NOT NULL,
    [IsCancelledPublic]       BIT              NOT NULL,
    [SiteID]                  UNIQUEIDENTIFIER NOT NULL,
    [IsHoliday]               BIT              NOT NULL,
    [IsAnnualHoliday]         BIT              NOT NULL,
    [RecursEvery]             INT              NOT NULL,
    CONSTRAINT [PK_carrot_CalendarEventProfile] PRIMARY KEY CLUSTERED ([CalendarEventProfileID] ASC),
    CONSTRAINT [FK_carrot_CalendarEventProfile_carrot_CalendarEventCategory] FOREIGN KEY ([CalendarEventCategoryID]) REFERENCES [dbo].[carrot_CalendarEventCategory] ([CalendarEventCategoryID]),
    CONSTRAINT [FK_carrot_CalendarEventProfile_carrot_CalendarFrequency] FOREIGN KEY ([CalendarFrequencyID]) REFERENCES [dbo].[carrot_CalendarFrequency] ([CalendarFrequencyID])
);

