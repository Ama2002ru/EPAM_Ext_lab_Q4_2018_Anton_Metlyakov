CREATE PROCEDURE [dbo].[P_DELETEQUESTION]
@QuizID int , 
                                        @QuestionID int , 
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM dbo.M_QUIZES WHERE QUIZ_ID  = @QuizID)
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz '+ CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES table';
	ELSE
	BEGIN
		BEGIN TRY
			IF (EXISTS(SELECT * FROM dbo.[M_QUESTIONS] Q WHERE Q.QUESTION_ID = @QuestionID AND Q.QUIZ_ID = @QuizID))
			BEGIN
				DELETE FROM dbo.[M_QUESTIONS] WHERE QUIZ_ID = @QuizID AND QUESTION_ID = @QuestionID
			END
			ELSE
			BEGIN
				SELECT @ERROR = -1, @ERRORTEXT = N'No Question '+ CAST(@QuestionID as nvarchar(10)) + N' in M_QUESTIONS table';
			END
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END
