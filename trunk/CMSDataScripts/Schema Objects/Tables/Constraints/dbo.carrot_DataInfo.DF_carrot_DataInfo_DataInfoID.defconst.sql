ALTER TABLE [dbo].[carrot_DataInfo]
    ADD CONSTRAINT [DF_carrot_DataInfo_DataInfoID] DEFAULT (newid()) FOR [DataInfoID];

