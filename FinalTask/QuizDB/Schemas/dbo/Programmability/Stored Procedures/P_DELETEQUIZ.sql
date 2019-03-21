CREATE PROCEDURE [DBO].[P_DELETEQUIZ]
										@QuizID int , 
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM DBO.M_QUIZES WHERE QUIZ_ID  = @QuizID)
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz '+ CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES table';
	ELSE
	BEGIN
		BEGIN TRY
			DELETE FROM DBO.[M_QUIZES] WHERE QUIZ_ID = @QuizID
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END