CREATE PROCEDURE [DBO].[P_GETNEXTQUESTION]
@RecordCount int = 1,
@QuizResultId int

AS
SET NOCOUNT ON
BEGIN												
-- execute [DBO].[P_GETNEXTQUESTION] @RecordCount=2, @QuizResultId=1
UPDATE [DBO].[M_QUIZ_RESULTS]
SET QUIZ_STATUS = 2, -- In progress
STARTED_DATE = GETDATE()
WHERE 
	QUIZ_RESULT_ID = @QuizResultId AND
	QUIZ_STATUS = 1  -- Assigned

  SELECT TOP (@RecordCount)
		 QR.USER_ID
		 ,Q.[QUESTION_ID] as [Question_Id]
		,Q.[QUIZ_ID] as [Quiz_Id]
		,Q.[INFO] as [Info]
		,Q.[TEXT] as [Text]
        ,Q.[CORRECT_OPTION_FLAG] as CorrectOptionFlag
		,(SELECT	
			V.VARIANT_ID as Variant_Id,
			V.QUESTION_ID as Question_Id,
			Q.QUIZ_ID as Quiz_Id,
			V.[TEXT] as [Text]
			FROM  DBO.M_VARIANTS V WHERE V.QUESTION_ID = Q.QUESTION_ID
		FOR XML PATH('Variant'),type) as Options 
   FROM [DBO].[M_QUIZ_RESULTS] QR 
  JOIN DBO.[M_QUESTIONS] Q ON QR.QUIZ_ID = Q.QUIZ_ID
  LEFT JOIN [DBO].[M_QUIZ_ANSWERS] QA ON Q.QUESTION_ID = QA.QUESTION_ID  AND QA.QUIZ_RESULT_ID = QR.QUIZ_RESULT_ID 
    WHERE 
	QR.QUIZ_RESULT_ID = @QuizResultId AND
	QA.QUIZ_ANSWERS_ID IS NULL
	ORDER BY Q.[QUESTION_ID]
	FOR XML PATH('Question'),type, root('ArrayOfQuestion')
END