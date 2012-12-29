ALTER TABLE [dbo].[carrot_TrackbackQueue]
    ADD CONSTRAINT [DF_carrot_TrackbackQueue_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

