CREATE PROCEDURE [DBO].[P_STATSALLUSERS]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	;WITH RawViewCTE AS
	(
		SELECT 
		U.USERNAME,
			(SELECT COUNT(*) fROM DBO.M_QUIZ_RESULTS QR WHERE QR.USER_ID = U.USER_ID) 
		as Total_Assigned,
			(SELECT COUNT(*) fROM DBO.M_QUIZ_RESULTS QR WHERE  QR.USER_ID = U.USER_ID AND QR.QUIZ_STATUS = 3) 
		as Total_Passed,
			(SELECT COUNT(*) fROM DBO.M_QUIZ_RESULTS QR WHERE  QR.USER_ID = U.USER_ID AND QR.QUIZ_STATUS = 4) 
		as Total_Failed,
			ISNULL(CAST(CAST((SELECT AVG(COMPLETED_RATE) fROM DBO.M_QUIZ_RESULTS QR WHERE  QR.USER_ID = U.USER_ID) as numeric(6,2)) as nvarchar(5)),'') 
		as AVG_RATE
		FROM DBO.M_USERS U
	)
	SELECT
		 R.USERNAME as [User_Name],
		 R.Total_Assigned, 
		 R.Total_Passed, 
		 R.Total_Failed,
		 R.AVG_RATE as Average_Rate,
		CAST(CASE WHEN R.Total_Assigned =0 THEN  0
		ELSE CAST (R.Total_Passed as real)/ CAST(R.Total_Assigned as real) * 100.0
		END as numeric(6,2))
	as Percent_Passed
	FROM RawViewCTE R
	ORDER BY [User_Name]
END