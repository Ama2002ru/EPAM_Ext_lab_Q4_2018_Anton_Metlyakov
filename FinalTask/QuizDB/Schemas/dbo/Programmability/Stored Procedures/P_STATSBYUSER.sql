﻿CREATE PROCEDURE P_STATSBYUSER
@USER_ID int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		Q.QUIZ_ID as [Quiz_Id],
		Q.QUIZ_NAME as [Quiz_Name],
		CASE  ISNULL((SELECT QR.QUIZ_STATUS FROM dbo.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID),0)
			WHEN 0 THEN 'Not assigned'
			WHEN 1 THEN 'Assigned'
			WHEN 2 THEN 'In progress'
			WHEN 3 THEN 'Passed'
			WHEN 4 THEN 'Failed'
		END as Quiz_Status,
		ISNULL(
			CAST(
				CAST(
					(SELECT QR.COMPLETED_RATE FROM dbo.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID)
				 as numeric(5,2)
			) as nvarchar(5)
		),'') 
		as [Completed_Rate], 
		ISNULL(
			CONVERT(nvarchar(19),
				(SELECT QR.COMPLETED_DATE FROM dbo.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID)
				,121)
		,'') 
		as [Completed_Date],
		ISNULL(CONVERT(nvarchar(19),(SELECT QR.STARTED_DATE FROM dbo.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID),121),'') 
		as [Started_Date],
		ISNULL(
			  (SELECT dbo.[F_PRINT_DATEDIFF](QR.STARTED_DATE,QR.COMPLETED_DATE) FROM dbo.M_QUIZ_RESULTS QR 
					WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID)
		,'') as [Time_taken]
	FROM dbo.M_QUIZES Q
END
