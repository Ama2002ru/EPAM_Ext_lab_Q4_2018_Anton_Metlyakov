CREATE PROCEDURE [dbo].[P_SAVEANSWER]
										@QuizResultId int,
                                        @QuestionID int , 
                                        @AnswerID int,
                                        @AnswerFlag int,
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF ( NOT EXISTS(SELECT * FROM dbo.M_QUESTIONS WHERE QUESTION_ID = @QuestionID) OR 
	   NOT EXISTS(SELECT * FROM dbo.M_QUIZ_RESULTS WHERE QUIZ_RESULT_ID = @QuizResultId))
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz|Question|Result '+ CAST(@QuizResultId as nvarchar(10)) + N' in M_QUESTIONS|M_QUIZ_RESULTS table';
	ELSE
	BEGIN
		IF (@AnswerID >0) 
		BEGIN
			SELECT @ERROR = -2,	@ERRORTEXT = N' AnswerID '+ CAST(@AnswerID as nvarchar(10)) + N' is positive. No updates allowed';
		END
		ELSE
		BEGIN
			BEGIN TRY
				/* new record */
				DECLARE @NEWID NUMERIC;
				EXECUTE dbo.P_GETNEXTPK @TABLE_NAME = 'M_QUIZ_ANSWERS', @ID = @NEWID OUT
				INSERT INTO dbo.[M_QUIZ_ANSWERS]
					([QUIZ_ANSWERS_ID], [QUIZ_RESULT_ID], [QUIZ_ID], [QUESTION_ID],[ANSWER_FLAG], [TIMESTAMP])
					VALUES (
						@NEWID,
						@QuizResultId,
						(SELECT QUIZ_ID FROM dbo.[M_QUIZ_RESULTS] QR WHERE QR.QUIZ_RESULT_ID = @QuizResultId),
						@QuestionID,
						@AnswerFlag,
						GETDATE())
			END TRY
			BEGIN CATCH
				SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
			END CATCH
		END
	END

END