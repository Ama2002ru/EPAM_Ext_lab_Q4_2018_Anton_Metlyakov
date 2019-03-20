CREATE PROCEDURE [dbo].[P_SAVEVARIANT]
										@QuizID int , 
                                        @QuestionID int , 
                                        @VariantID int,
                                        @text nvarchar(1000),
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM dbo.M_QUIZES WHERE QUIZ_ID  = @QuizID) OR NOT EXISTS(SELECT * FROM dbo.M_QUESTIONS WHERE QUESTION_ID  = @QuestionID)
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz/Questiuon '+ CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES/M_QUESTIONS table';
	ELSE
	BEGIN
		BEGIN TRY
			IF (EXISTS(SELECT * FROM dbo.[M_VARIANTS] V WHERE V.QUESTION_ID = @QuestionID )
				AND @VariantID >0)
			BEGIN
			/* update record */
				UPDATE dbo.[M_VARIANTS]
				SET 
					[TEXT] = @text
				WHERE 
					QUESTION_ID = @QuestionID AND VARIANT_ID = @VariantID
			END
			ELSE
			BEGIN
			/* new record */
				DECLARE @NEWID NUMERIC;
				EXECUTE dbo.P_GETNEXTPK @TABLE_NAME = 'M_VARIANTS', @ID = @NEWID OUT
				INSERT INTO dbo.[M_VARIANTS]
				([QUESTION_ID],[VARIANT_ID], [TEXT])
				VALUES (@QuestionID, @NEWID, @text)
			END
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END

