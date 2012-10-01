ALTER TABLE [dbo].[tblCalendar]
    ADD CONSTRAINT [DF_tblCalendar_CalendarID] DEFAULT (newid()) FOR [CalendarID];

