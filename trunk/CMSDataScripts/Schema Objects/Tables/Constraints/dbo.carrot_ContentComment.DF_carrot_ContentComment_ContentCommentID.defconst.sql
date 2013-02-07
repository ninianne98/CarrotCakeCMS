ALTER TABLE [dbo].[carrot_ContentComment]
    ADD CONSTRAINT [DF_carrot_ContentComment_ContentCommentID] DEFAULT (newid()) FOR [ContentCommentID];
