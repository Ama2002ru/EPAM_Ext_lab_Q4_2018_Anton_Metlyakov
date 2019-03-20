CREATE PROCEDURE [dbo].[P_GETQUIZASSIGNMENT]
@UserID int = 1
AS
SET NOCOUNT ON
BEGIN												
SELECT 
	@UserID as [User_Id],
	(SELECT USERNAME FROM dbo.M_USERS WHERE [USER_ID]=@UserID) as [User_Name],
	(SELECT 
		UserQuizList.QUIZ_ID as [Quiz_Id], 
		UserQuizList.QUIZ_NAME as Quiz_Name,
		ISNULL(QR.QUIZ_STATUS ,0) as Quiz_Status,
		ISNULL(QR.QUIZ_RESULT_ID,-1) as QuizResult_Id
		FROM
	(SELECT  U.USER_ID,U.USERNAME,  Q.QUIZ_NAME, Q.QUIZ_ID
	FROM M_USERS U
	CROSS JOIN M_QUIZES Q
	WHERE U.USER_ID = @UserID ) as UserQuizList
	LEFT JOIN dbo.M_QUIZ_RESULTS QR ON QR.QUIZ_ID = UserQuizList.QUIZ_ID AND QR.USER_ID = @UserID
	ORDER BY Quiz_Name
	FOR XML PATH('AssignQuizList'),type)  as [Assignquizlist]
FOR XML PATH('AssignQuiz'), type 
END
