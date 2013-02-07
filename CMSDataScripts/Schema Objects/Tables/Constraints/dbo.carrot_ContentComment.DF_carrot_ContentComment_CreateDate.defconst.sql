ALTER TABLE [dbo].[carrot_ContentComment]
    ADD CONSTRAINT [DF_carrot_ContentComment_CreateDate] DEFAULT (getdate()) FOR [CreateDate];
