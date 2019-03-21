CREATE PROCEDURE [DBO].[P_GETQUIZ]
@ID int = 1
AS
SET NOCOUNT ON
BEGIN												

SELECT
A.QUIZ_ID as ID,
A.AUTHOR_ID as Author_Id,
(SELECT FIRSTNAME+ ' '+LASTNAME FROM M_USERS C WHERE C.USER_ID = A.AUTHOR_ID ) as Author,
A.CREATED_DATE as Created_Date,
A.QUIZ_NAME as Quiz_Name,
CAST (A.SUCCESS_RATE as numeric(10,2)) as Success_Rate,
	( 
	SELECT 
		B.QUESTION_ID as Question_Id,
		B.QUIZ_ID as Quiz_Id,
		B.INFO as Info,
		B.[TEXT] as [Text],
		B.CORRECT_OPTION_FLAG as CorrectOptionFlag,
		(
		SELECT	
			C.VARIANT_ID as Variant_Id,
			C.QUESTION_ID as Question_Id,
			B.QUIZ_ID as Quiz_Id,
			C.[TEXT] as [Text]
			FROM  DBO.M_VARIANTS C WHERE C.QUESTION_ID= B.QUESTION_ID
		FOR XML PATH('Variant'),type) as Options  
	FROM  DBO.M_QUESTIONS B where B.QUIZ_ID= A.QUIZ_ID
	FOR XML PATH('Question'),type)  as "Questions"
FROM DBO.M_QUIZES A
WHERE A.QUIZ_ID = @ID
FOR XML PATH('Quiz'), type


END
