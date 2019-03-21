CREATE PROCEDURE [DBO].[P_SAVEQUESTION]
                                        @QuizID int , 
                                        @QuestionID int , 
                                        @info nvarchar(1000) ,
                                        @text nvarchar(1000),
                                        @CorrectOptionFlag int,
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
			IF (EXISTS(SELECT * FROM DBO.[M_QUESTIONS] Q WHERE Q.QUESTION_ID = @QuestionID AND Q.QUIZ_ID = @QuizID)
				AND @QuestionID >0)
			BEGIN
			/* update record */
				UPDATE DBO.[M_QUESTIONS]
				SET 
					[INFO] = @info, [TEXT] = @text, [CORRECT_OPTION_FLAG] = @CorrectOptionFlag
				WHERE 
					QUESTION_ID = @QuestionID AND QUIZ_ID = @QuizID
			END
			ELSE
			BEGIN
			/* new record */
				DECLARE @NEWID NUMERIC;
				EXECUTE DBO.P_GETNEXTPK @TABLE_NAME = 'M_QUESTIONS', @ID = @NEWID OUT
				INSERT INTO DBO.[M_QUESTIONS]
				([QUIZ_ID], [QUESTION_ID],[INFO], [TEXT],[CORRECT_OPTION_FLAG])
				VALUES (@QuizID, @NEWID,@info, @text,@CorrectOptionFlag)
			END
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END
