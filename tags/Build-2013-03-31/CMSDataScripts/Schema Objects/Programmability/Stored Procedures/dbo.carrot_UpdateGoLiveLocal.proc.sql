CREATE PROCEDURE [dbo].[carrot_UpdateGoLiveLocal]
    @SiteID uniqueidentifier,
    @UTCOffsetInMinutes int = 0

/*

exec [carrot_UpdateGoLiveLocal] '3BD253EA-AC65-4eb6-A4E7-BB097C2255A0', 360

*/

AS BEGIN

SET NOCOUNT ON

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF ( @@TRANCOUNT = 0 ) BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END ELSE
        SET @TranStarted = 0


	UPDATE [dbo].[carrot_RootContent]
	set GoLiveDateLocal = DATEADD(mi, @UTCOffsetInMinutes, GoLiveDate)
	WHERE SiteID = @SiteID


	IF ( @@ERROR <> 0 ) BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF ( @TranStarted = 1 ) BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF ( @TranStarted = 1 ) BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

