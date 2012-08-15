ALTER TABLE [dbo].[tblFAQ]
    ADD CONSTRAINT [DF_tblFAQ_FaqID] DEFAULT (newid()) FOR [FaqID];

