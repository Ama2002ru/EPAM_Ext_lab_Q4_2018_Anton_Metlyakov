CREATE PROCEDURE [dbo].[P_SAVEQUIZ]
                                        @QuizID int, 
                                        @QuizName nvarchar(100),
                                        @AuthorID int, 
                                        @CreatedDate datetime,
                                        @SuccessRate real,
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
BEGIN TRY
		IF (@QuizID = -1) 
		BEGIN
		/* new quiz */
			IF EXISTS(SELECT * FROM dbo.M_QUIZES WHERE QUIZ_NAME = @QuizName)
				SELECT @ERROR = -1,	@ERRORTEXT = N'Dublicate Quiz name '+@QuizName + N' in M_QUIZES table';
			ELSE
			BEGIN
				DECLARE @NEWID NUMERIC;
				EXECUTE dbo.P_GETNEXTPK @TABLE_NAME = 'M_QUIZES', @ID = @NEWID OUT
				INSERT INTO dbo.M_QUIZES 
				([QUIZ_ID],[QUIZ_NAME], [AUTHOR_ID], [CREATED_DATE], [SUCCESS_RATE])
					VALUES (@NEWID,@QuizName,@AuthorID,GETDATE(),@SuccessRate)
			END
		END
		ELSE
		BEGIN
		/* Update existing quiz info */
			IF NOT EXISTS(SELECT * FROM dbo.M_QUIZES WHERE QUIZ_ID = @QuizID)
			/* DB consistency error */
				SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz '+CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES table';
			ELSE
			BEGIN
			/* change quiz name of existing quiz. Is new Quiz name free ?*/
				IF EXISTS(SELECT * FROM dbo.M_QUIZES WHERE [QUIZ_NAME] = @QuizName AND QUIZ_ID != @QuizID )
					SELECT @ERROR = -1,	@ERRORTEXT = N'Can''t change quiz name to '+@QuizName + N' in M_QUIZES table, this quiz name already exists';
				ELSE
					UPDATE dbo.M_QUIZES 
					SET [QUIZ_NAME] = @QuizName,
						[AUTHOR_ID] = @AuthorID,
						[SUCCESS_RATE] = @SuccessRate
					WHERE [QUIZ_ID] = @QuizID
			END
		END
	END TRY
	BEGIN CATCH
		SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
	END CATCH

END