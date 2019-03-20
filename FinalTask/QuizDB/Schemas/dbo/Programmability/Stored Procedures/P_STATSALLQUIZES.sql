CREATE PROCEDURE [dbo].[P_STATSALLQUIZES]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	;WITH RawViewCTE AS
	(
		SELECT 
		Q.QUIZ_NAME as Quiz_Name,
		Q.SUCCESS_RATE as Success_Rate,
			(SELECT COUNT(*) fROM dbo.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID) 
		as Total_Assigned,
			(SELECT COUNT(*) fROM dbo.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.QUIZ_STATUS = 3) 
		as Total_Passed,
			(SELECT COUNT(*) fROM dbo.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.QUIZ_STATUS = 4) 
		as Total_Failed,
			ISNULL(CAST(CAST((SELECT AVG(COMPLETED_RATE) fROM dbo.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID) as numeric(5,2)) as nvarchar(5)),'n/a') 
		as Average_Rate
		FROM dbo.M_QUIZES Q
	)
	SELECT R.Quiz_Name, R.Success_Rate, R.Total_Assigned, R.Total_Passed, R.Total_Failed,R.Average_Rate,
		CAST(CASE WHEN R.Total_Assigned =0 THEN  0
		ELSE CAST (R.Total_Passed as real)/ CAST(R.Total_Assigned as real) * 100.0
		END as numeric(6,2))
	as Percent_Passed
	FROM RawViewCTE R
	ORDER BY [Quiz_Name]
END