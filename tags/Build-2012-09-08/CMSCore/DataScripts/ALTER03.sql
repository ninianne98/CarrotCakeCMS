-- USE [CarrotwareCMS]
GO

-- 2012-07-22
-- added new column to tblRootContent

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'tblRootContent' and column_name = 'CreateDate') BEGIN

	ALTER TABLE [dbo].[tblRootContent] ADD [CreateDate] [datetime] NULL

END

GO

ALTER TABLE [dbo].[tblRootContent] 
	ALTER COLUMN  [CreateDate] [datetime] NULL

GO

if not exists(select * from [sys].[all_objects] 
		where [name] = 'DF_tblRootContent_CreateDate') begin

	ALTER TABLE [dbo].[tblRootContent] ADD  CONSTRAINT [DF_tblRootContent_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]

END

GO

UPDATE RC
SET CreateDate = c.CreatedDate
FROM [dbo].[tblRootContent] as rc
JOIN (SELECT [Root_ContentID], min([EditDate]) as CreatedDate
		FROM [dbo].[tblContent]
		GROUP BY [Root_ContentID]) c on rc.Root_ContentID = c.Root_ContentID
WHERE rc.CreateDate is null

GO

ALTER TABLE [dbo].[tblRootContent] 
	ALTER COLUMN  [CreateDate] [datetime] NOT NULL

