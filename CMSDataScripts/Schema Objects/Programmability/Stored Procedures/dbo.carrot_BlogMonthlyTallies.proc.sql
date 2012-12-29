CREATE PROCEDURE [dbo].[carrot_BlogMonthlyTallies]
    @SiteID uniqueidentifier,
    @ActiveOnly bit,    
    @TakeTop int = 10

/*

exec [carrot_BlogMonthlyTallies] '3BD253EA-AC65-4eb6-A4E7-BB097C2255A0', 10

exec [carrot_BlogMonthlyTallies] '3BD253EA-AC65-4eb6-A4E7-BB097C2255A0', 16

exec [carrot_BlogMonthlyTallies] '3BD253EA-AC65-4eb6-A4E7-BB097C2255A0', 5

*/

AS BEGIN

SET NOCOUNT ON

	DECLARE @UTCDate Date
	SET @UTCDate = GETUTCDATE()
	
    DECLARE @ContentTypeID uniqueidentifier
    SELECT  @ContentTypeID = (select top 1 ct.ContentTypeID from dbo.carrot_ContentType ct where ct.ContentTypeValue = 'BlogEntry')

	DECLARE @tblTallies TABLE(
		RowID int identity(1,1),
		SiteID uniqueidentifier,
		ContentCount int,
		DateMonth date,
		DateSlug nvarchar(64)
	)
	
	insert into @tblTallies(SiteID, ContentCount, DateMonth, DateSlug)
		SELECT SiteID, COUNT(Root_ContentID) AS ContentCount, DateMonth, DateSlug
		FROM   (SELECT Root_ContentID, SiteID, ContentTypeID, 
					CONVERT(datetime, CONVERT(nvarchar(25), GoLiveDateLocal, 112)) AS DateMonth, 
					DATENAME(MONTH, GoLiveDateLocal) + ' ' + CAST(YEAR(GoLiveDateLocal) as nvarchar(50)) AS DateSlug
			FROM (SELECT Root_ContentID, SiteID, ContentTypeID, (GoLiveDateLocal - DAY(GoLiveDateLocal) + 1) as GoLiveDateLocal
				FROM [dbo].[carrot_RootContent]
				WHERE SiteID = @SiteID
					AND (PageActive = 1 OR @ActiveOnly = 0)
					AND (GoLiveDate < @UTCDate OR @ActiveOnly = 0)
					AND (RetireDate > @UTCDate OR @ActiveOnly = 0)
					AND ContentTypeID = @ContentTypeID ) AS Y) AS Z

		GROUP BY SiteID, DateMonth, DateSlug
		ORDER BY DateMonth DESC

	SELECT * FROM @tblTallies WHERE RowID <= @TakeTop ORDER BY RowID

    RETURN(0)

END

