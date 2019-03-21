CREATE PROCEDURE [DBO].[P_DELETEVARIANT]
                                        @QuizID int , 
                                        @QuestionID int , 
										@VariantID int,
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM DBO.M_QUIZES WHERE QUIZ_ID  = @QuizID) OR NOT EXISTS(SELECT * FROM DBO.M_QUESTIONS WHERE QUESTION_ID  = @QuestionID)
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz/Questiuon '+ CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES/M_QUESTIONS table';
	ELSE
	BEGIN
		BEGIN TRY
			IF (EXISTS(SELECT * FROM DBO.[M_VARIANTS] V WHERE V.QUESTION_ID = @QuestionID AND V.VARIANT_ID = @VariantID))
			BEGIN
				DELETE FROM DBO.[M_VARIANTS] WHERE QUESTION_ID = @QuestionID AND VARIANT_ID = @VariantID
			END
			ELSE
			BEGIN
				SELECT @ERROR = -1, @ERRORTEXT = N'No Variant '+ CAST(@VariantID as nvarchar(10)) + N' in M_VARIANTS table';
			END
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END
