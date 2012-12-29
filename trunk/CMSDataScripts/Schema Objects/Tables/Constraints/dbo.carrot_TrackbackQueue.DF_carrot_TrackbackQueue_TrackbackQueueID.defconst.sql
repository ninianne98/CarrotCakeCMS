ALTER TABLE [dbo].[carrot_TrackbackQueue]
    ADD CONSTRAINT [DF_carrot_TrackbackQueue_TrackbackQueueID] DEFAULT (newid()) FOR [TrackbackQueueID];

