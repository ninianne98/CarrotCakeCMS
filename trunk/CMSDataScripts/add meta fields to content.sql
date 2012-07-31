-- 2012-01-25
-- added two new columns to tblContent

if not exists( select * from information_schema.columns 
		where table_name = 'tblContent' and column_name = 'MetaKeyword') begin

	ALTER TABLE tblContent ADD [MetaKeyword] [varchar](1000) NULL
	ALTER TABLE tblContent ADD [MetaDescription] [varchar](2000) NULL
	
end

GO

update [dbo].[tblContent]  
set [MetaKeyword] = ISNULL([MetaKeyword], ''),
	[MetaDescription] = ISNULL([MetaDescription], '')