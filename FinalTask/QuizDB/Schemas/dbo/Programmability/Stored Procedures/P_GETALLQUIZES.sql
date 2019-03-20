CREATE PROCEDURE [dbo].[P_GETALLQUIZES]
@RecordsCount int = 2147483647 /* max INT value*/
AS
SET NOCOUNT ON
BEGIN												
	/* treating all negatives as max INT */
	DECLARE @InternalRecordsCount int = CASE WHEN @RecordsCount<0 THEN 2147483647 ELSE @RecordsCount END
SELECT	TOP (@InternalRecordsCount)  
A.QUIZ_ID as ID,
A.AUTHOR_ID as Author_Id ,
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
		B.CORRECT_OPTION_FLAG as Correct_Option_Flag,
		(
		SELECT	
			C.VARIANT_ID as Variant_Id,
			C.QUESTION_ID as Question_Id,
			B.QUIZ_ID as Quiz_Id,
			C.[TEXT] as [Text]
			FROM  dbo.M_VARIANTS C WHERE C.QUESTION_ID= B.QUESTION_ID
		FOR XML PATH('Variant'),type) as Options 
	FROM  dbo.M_QUESTIONS B WHERE B.QUIZ_ID= A.QUIZ_ID
	FOR XML PATH('Question'),type)  as "Questions"
FROM dbo.M_QUIZES A
FOR XML PATH('Quiz'), type, root('ArrayOfQuiz')
END
