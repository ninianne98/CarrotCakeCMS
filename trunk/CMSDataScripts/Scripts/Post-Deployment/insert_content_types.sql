-- =============================================
-- Script Template
-- insert_content_types.sql
-- =============================================


GO

IF ((select count(*) from [dbo].[carrot_ContentType] where [ContentTypeValue] = N'ContentEntry') < 1) BEGIN

	insert into [dbo].[carrot_ContentType]([ContentTypeValue])
	values('BlogEntry')

	insert into [dbo].[carrot_ContentType]([ContentTypeValue])
	values('ContentEntry')

END

GO

