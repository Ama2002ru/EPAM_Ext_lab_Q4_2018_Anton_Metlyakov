CREATE PROCEDURE [dbo].[P_GETQUIZRESULTS]
@RecordsCount int = 2147483647, /* max INT value*/
@UserId int
AS
SET NOCOUNT ON
BEGIN												
	/* treating all negatives as max INT */
	DECLARE @InternalRecordsCount int = CASE WHEN @RecordsCount<0 THEN 2147483647 ELSE @RecordsCount END

SELECT TOP (@InternalRecordsCount)
       [QUIZ_RESULT_ID] as [QuizResult_Id]
	  ,[USER_ID] as [User_Id]
	  ,(SELECT USERNAME FROM dbo.M_USERS Q WHERE Q.USER_ID =  QR.[USER_ID]) as [UserName]
      ,[QUIZ_ID] as [Quiz_Id]
	  ,(SELECT QUIZ_NAME FROM dbo.M_QUIZES Q WHERE Q.QUIZ_ID = QR.QUIZ_ID) as [Quiz_Name]
      ,(SELECT FIRSTNAME+ ' '+ LASTNAME FROM dbo.M_USERS Q WHERE Q.USER_ID =  QR.[ASSIGNED_BY_ID]) as [AssignedBy]
      ,[ASSIGNED_DATE] as [Assigned_Date]
      ,[COMPLETED_DATE]as [Completed_Date]
      ,[QUIZ_STATUS] as [QuizResult_Status]
      ,[COMPLETED_RATE] as [Completed_Rate]
	  ,[STARTED_DATE] as [Started_Date]
	  ,[TIME_TAKEN] as [Time_Taken]
	  ,(SELECT	
			QA.QUIZ_ANSWERS_ID as [Answer_Id],
			QA.QUIZ_RESULT_ID as [QuizResult_Id],
			QA.QUIZ_ID as [Quiz_Id],
			QA.QUESTION_ID as [Question_Id],
			QA.ANSWER_FLAG as [Answer_Flag],
			QA.[TIMESTAMP] as [TimeStamp]
			FROM  dbo.M_QUIZ_ANSWERS QA WHERE QA.QUIZ_RESULT_ID = QR.QUIZ_RESULT_ID
		FOR XML PATH('Answer'),type) as [Answer_List]
  FROM [dbo].[M_QUIZ_RESULTS] QR
  WHERE QR.USER_ID = @UserId
  FOR XML PATH('QuizResult'), type, root('ArrayOfQuizResult')
END

