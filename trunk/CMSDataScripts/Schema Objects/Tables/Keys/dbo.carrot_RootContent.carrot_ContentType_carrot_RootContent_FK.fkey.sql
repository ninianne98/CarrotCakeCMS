ALTER TABLE [dbo].[carrot_RootContent]
    ADD CONSTRAINT [carrot_ContentType_carrot_RootContent_FK] FOREIGN KEY ([ContentTypeID]) REFERENCES [dbo].[carrot_ContentType] ([ContentTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

