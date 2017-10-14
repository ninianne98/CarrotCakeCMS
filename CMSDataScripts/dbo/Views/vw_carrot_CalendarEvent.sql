CREATE VIEW [dbo].[vw_carrot_CalendarEvent]
AS 


SELECT ces.SiteID, ces.CalendarEventProfileID, ces.CalendarFrequencyID, ces.CalendarEventCategoryID, ces.EventStartDate, ces.EventEndDate, 
      ces.EventStartTime, ces.EventEndTime, ces.EventTitle, ces.EventRepeatPattern, ces.EventDetail as EventSeriesDetail, ces.IsCancelledPublic, 
      ces.IsAllDayEvent, ces.IsPublic, ces.IsCancelled AS IsCancelledSeries, ce.IsCancelled AS IsCancelledEvent, ces.IsHoliday, ces.IsAnnualHoliday, ces.RecursEvery,
      ce.CalendarEventID, ce.EventDate, ce.EventDetail, cef.FrequencyValue, cef.FrequencyName, cef.FrequencySortOrder, 
      ce.EventStartTime as EventStartTimeOverride, ce.EventEndTime as EventEndTimeOverride, cec.CategoryFGColor, cec.CategoryBGColor, cec.CategoryName
FROM dbo.carrot_CalendarEventProfile AS ces 
INNER JOIN dbo.carrot_CalendarEvent AS ce ON ces.CalendarEventProfileID = ce.CalendarEventProfileID 
INNER JOIN dbo.carrot_CalendarFrequency AS cef ON ces.CalendarFrequencyID = cef.CalendarFrequencyID 
INNER JOIN dbo.carrot_CalendarEventCategory AS cec ON ces.CalendarEventCategoryID = cec.CalendarEventCategoryID 
		AND ces.SiteID = cec.SiteID