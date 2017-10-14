CREATE VIEW [dbo].[vw_carrot_CalendarEventProfile]
AS 


SELECT ces.SiteID, ces.CalendarEventProfileID, ces.CalendarFrequencyID, ces.CalendarEventCategoryID, ces.EventStartDate, ces.EventStartTime, ces.EventEndDate, 
      ces.EventEndTime, ces.EventTitle, ces.EventRepeatPattern, ces.EventDetail, ces.IsCancelled, ces.IsCancelledPublic, ces.IsHoliday, ces.IsAnnualHoliday, ces.RecursEvery,
      ces.IsAllDayEvent, ces.IsPublic, cef.FrequencyValue, cef.FrequencyName, cef.FrequencySortOrder, cec.CategoryFGColor, cec.CategoryBGColor, cec.CategoryName
FROM dbo.carrot_CalendarEventProfile AS ces 
INNER JOIN dbo.carrot_CalendarFrequency AS cef ON ces.CalendarFrequencyID = cef.CalendarFrequencyID 
INNER JOIN dbo.carrot_CalendarEventCategory AS cec ON ces.CalendarEventCategoryID = cec.CalendarEventCategoryID 
		AND ces.SiteID = cec.SiteID