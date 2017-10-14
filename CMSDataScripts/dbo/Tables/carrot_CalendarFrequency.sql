CREATE TABLE [dbo].[carrot_CalendarFrequency] (
    [CalendarFrequencyID] UNIQUEIDENTIFIER CONSTRAINT [DF_carrot_CalendarFrequency_CalendarFrequencyID] DEFAULT (newid()) NOT NULL,
    [FrequencySortOrder]  INT              NOT NULL,
    [FrequencyValue]      VARCHAR (64)     NOT NULL,
    [FrequencyName]       VARCHAR (128)    NOT NULL,
    CONSTRAINT [PK_carrot_CalendarFrequency] PRIMARY KEY CLUSTERED ([CalendarFrequencyID] ASC)
);

