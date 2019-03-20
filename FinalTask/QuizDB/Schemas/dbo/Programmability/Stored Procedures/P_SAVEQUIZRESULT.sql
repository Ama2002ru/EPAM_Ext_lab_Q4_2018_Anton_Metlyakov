CREATE PROCEDURE [dbo].[P_SAVEQUIZRESULT]
                                        @QuizResultID int, 
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	/*  test script
	DECLARE @error int, @Errortext nvarchar(1000)
	 execute [dbo].[P_SAVEQUZRESULT] 1, @Error out, @errortext out
 */
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM dbo.M_QUIZ_RESULTS WHERE QUIZ_RESULT_ID = @QuizResultID)
				SELECT @ERROR = -1,	@ERRORTEXT = N'NO QuizResult '+CAST(@QuizResultID as nvarchar(10)) + N' in M_QUIZ_RESULTS table';
	ELSE
	BEGIN
		BEGIN TRY
		;WITH Save_Quiz_Result AS
		(SELECT 
				QR.QUIZ_RESULT_ID as QUIZRESULTID,
				(SELECT SUCCESS_RATE FROM [dbo].[M_QUIZES] QQ WHERE QQ.QUIZ_ID = QR.QUIZ_ID) 
				as SuccessRate,
				CASE WHEN COUNT(Q.QUESTION_ID) >0 THEN  /* не хочу делить на ноль */
					CAST(SUM (CASE WHEN Q.CORRECT_OPTION_FLAG = QA.ANSWER_FLAG THEN 1 ELSE 0 END ) as real) /* Сумма правильных ответов/ сумму вопросов*/
					/ CAST(COUNT(Q.QUESTION_ID) as real)
				ELSE 0 END
				as CompletedRate,
				QR.STARTED_DATE as [Start_Date],
				GETDATE() as Completed_Date 
		  FROM [dbo].[M_QUESTIONS] Q
		  JOIN [dbo].[M_QUIZ_RESULTS] QR ON QR.QUIZ_ID = Q.QUIZ_ID
		  LEFT JOIN [dbo].[M_QUIZ_ANSWERS] QA ON QR.QUIZ_RESULT_ID = QA.QUIZ_RESULT_ID AND QA.QUESTION_ID = Q.QUESTION_ID
		  WHERE 
			QR.QUIZ_RESULT_ID = @QuizResultID
		  GROUP BY 
			QR.QUIZ_RESULT_ID , QR.QUIZ_ID , QR.STARTED_DATE
		)
		UPDATE dbo.M_QUIZ_RESULTS 
		SET COMPLETED_RATE = QR.CompletedRate,
			COMPLETED_DATE = GETDATE(),
			TIME_TAKEN = dbo.F_PRINT_DATEDIFF(QR.START_DATE,GETDATE()),
			QUIZ_STATUS = CASE WHEN SuccessRate > CompletedRate THEN 4 ELSE 3 END /* 4 = failed , 3 = passed */
		FROM 
			Save_Quiz_Result QR
		WHERE 
			QUIZ_RESULT_ID = QR.QUIZRESULTID
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END
END
