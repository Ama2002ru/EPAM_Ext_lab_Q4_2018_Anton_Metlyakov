CREATE PROCEDURE [DBO].[P_SAVEQUIZASSIGNMENT]
										@XmlQuizes nvarchar(max)
                                        ,@ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	SELECT @XmlQuizes = REPLACE(@XmlQuizes,'\','')			-- убираю лишние символы - \
	SELECT @XmlQuizes = SUBSTRING(@XmlQuizes,2,LEN(@XmlQuizes)-2) -- убираю лишние символы - ".."
	DECLARE @xml xml = @XmlQuizes 
	DECLARE @NEWID numeric
	DECLARE @AssignedUser_Id int
	DECLARE @User_Id int
	DECLARE @Quiz_Id int
	DECLARE @Quiz_Status int
	DECLARE @QuizResult_Id int

	BEGIN TRY
		SELECT @User_Id = Table1.user_id.value('.','int') 
		FROM   @xml.nodes('/AssignQuiz/User_Id') Table1(user_id)
		SELECT @AssignedUser_Id = Table1.user_id.value('.','int') 
		FROM   @xml.nodes('/AssignQuiz/AssignedUser_Id') Table1(user_id)
		DECLARE QuizAssignment CURSOR FOR
				SELECT @User_Id as User_id,
					col.query('Quiz_Id').value('.','int') as Quiz_Id,
					col.query('Quiz_Status').value('.','int') as Quiz_Status,
					col.query('QuizResult_Id').value('.','int') as QuizResult_Id
				FROM @xml.nodes('/AssignQuiz/Assignquizlist/AssignQuizList') as i(col)
		OPEN QuizAssignment

		WHILE (1=1)
		BEGIN
			FETCH NEXT FROM QuizAssignment INTO @User_Id, @Quiz_Id, @Quiz_Status, @QuizResult_Id
			IF (@@FETCH_STATUS !=0) BREAK
	-- обработка строки назначенных квизов
	-- логика  :
	--	Quiz_Status и QuizResult_Id ==0 - такую комбинацию пропускаю (не назначено)
	--	QuizResult_Id ==0, Quiz_Status ==1 - добавить в M_QuizResults
	--	QuizResult_Id >0  Quiz_Status == 0 - удалить из M_QuizResults
	--	QuizResult_Id <=0  Quiz_Status == 1  такую комбинацию пропускаю  (было ранее назначено)
			IF (@QuizResult_Id <= 0) AND (@Quiz_Status = 1)
			BEGIN
			-- insert into M_QUIZ_RESULTS	
				EXECUTE DBO.P_GETNEXTPK @TABLE_NAME = 'M_QUIZ_RESULTS', @ID = @NEWID OUT
				INSERT INTO DBO.M_QUIZ_RESULTS
					(QUIZ_RESULT_ID, USER_ID, QUIZ_ID,QUIZ_STATUS, ASSIGNED_BY_ID,ASSIGNED_DATE, COMPLETED_RATE,COMPLETED_DATE)
					VALUES 
					(@NEWID, @User_Id,@Quiz_Id,1 /*Assigned*/,@AssignedUser_Id, GETDATE(), NULL,NULL)
			END
			IF (@QuizResult_Id > 0) AND (@Quiz_Status = 0)
			BEGIN
			-- delete FROM M_QUIZ_RESULTS	
				DELETE FROM DBO.M_QUIZ_RESULTS
					WHERE QUIZ_RESULT_ID = @QuizResult_Id AND
						QUIZ_STATUS = 1	-- Assigned (Passed, Failed, InProgress - do not delete)
			END
		END
		CLOSE QuizAssignment
		DEALLOCATE QuizAssignment
	END TRY
	BEGIN CATCH
		SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
	END CATCH
END
