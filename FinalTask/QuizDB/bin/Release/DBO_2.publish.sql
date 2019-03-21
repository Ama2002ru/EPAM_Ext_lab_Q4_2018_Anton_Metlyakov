/*
Скрипт развертывания для QuizDB

Этот код был создан программным средством.
Изменения, внесенные в этот файл, могут привести к неверному выполнению кода и будут потеряны
в случае его повторного формирования.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "QuizDB"
:setvar DefaultFilePrefix "QuizDB"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL14.R2D3\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL14.R2D3\MSSQL\DATA\"

GO
:on error exit
GO
/*
Проверьте режим SQLCMD и отключите выполнение скрипта, если режим SQLCMD не поддерживается.
Чтобы повторно включить скрипт после включения режима SQLCMD выполните следующую инструкцию:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'Для успешного выполнения этого скрипта должен быть включен режим SQLCMD.';
        SET NOEXEC ON;
    END


GO
USE [master];


GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Выполняется создание $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)]
    ON 
    PRIMARY(NAME = [$(DatabaseName)], FILENAME = N'$(DefaultDataPath)$(DefaultFilePrefix)_Primary.mdf')
    LOG ON (NAME = [$(DatabaseName)_log], FILENAME = N'$(DefaultLogPath)$(DefaultFilePrefix)_Primary.ldf') COLLATE SQL_Latin1_General_CP1_CI_AS
GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CLOSE OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'Параметры базы данных изменить нельзя. Применить эти параметры может только пользователь SysAdmin.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'Параметры базы данных изменить нельзя. Применить эти параметры может только пользователь SysAdmin.';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF),
                CONTAINMENT = NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CREATE_STATISTICS ON(INCREMENTAL = OFF),
                MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT = OFF,
                DELAYED_DURABILITY = DISABLED 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET TEMPORAL_HISTORY_RETENTION ON 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF OBJECT_ID('DBO.M_QUIZ_ANSWERS', 'U') IS NOT NULL
	DELETE FROM DBO.M_QUIZ_ANSWERS;
GO
IF OBJECT_ID('DBO.M_QUIZ_RESULTS', 'U') IS NOT NULL
	DELETE FROM DBO.M_QUIZ_RESULTS;
GO
IF OBJECT_ID('DBO.M_VARIANTS', 'U') IS NOT NULL
	DELETE FROM DBO.M_VARIANTS;
GO
IF OBJECT_ID('DBO.M_QUESTIONS', 'U') IS NOT NULL
	DELETE FROM DBO.M_QUESTIONS;
GO
IF OBJECT_ID('DBO.M_QUIZES', 'U') IS NOT NULL
	DELETE FROM DBO.M_QUIZES;
GO
IF OBJECT_ID('DBO.M_USERS', 'U') IS NOT NULL
	DELETE FROM DBO.M_USERS;
GO
IF OBJECT_ID('DBO.M_ROLES', 'U') IS NOT NULL
	DELETE FROM DBO.M_ROLES;
GO
IF OBJECT_ID('DBO.S_PK_GENERATOR', 'U') IS NOT NULL
	DELETE FROM DBO.S_PK_GENERATOR;
GO

GO

GO
PRINT N'Выполняется создание [DBO].[M_QUESTIONS]...';


GO
CREATE TABLE [DBO].[M_QUESTIONS] (
    [QUESTION_ID]         INT             NOT NULL,
    [QUIZ_ID]             INT             NOT NULL,
    [INFO]                NVARCHAR (1000) NOT NULL,
    [TEXT]                NVARCHAR (1000) NOT NULL,
    [CORRECT_OPTION_FLAG] INT             NOT NULL,
    CONSTRAINT [PK_M_QUESTIONS] PRIMARY KEY CLUSTERED ([QUESTION_ID] ASC)
);


GO
PRINT N'Выполняется создание [DBO].[M_QUIZ_ANSWERS]...';


GO
CREATE TABLE [DBO].[M_QUIZ_ANSWERS] (
    [QUIZ_ANSWERS_ID] INT      NOT NULL,
    [QUIZ_RESULT_ID]  INT      NOT NULL,
    [QUIZ_ID]         INT      NOT NULL,
    [QUESTION_ID]     INT      NOT NULL,
    [ANSWER_FLAG]     INT      NOT NULL,
    [TIMESTAMP]       DATETIME NOT NULL,
    CONSTRAINT [PK_M_QUIZ_ANSWERS] PRIMARY KEY CLUSTERED ([QUIZ_ANSWERS_ID] ASC)
);


GO
PRINT N'Выполняется создание [DBO].[M_QUIZ_RESULTS]...';


GO
CREATE TABLE [DBO].[M_QUIZ_RESULTS] (
    [QUIZ_RESULT_ID] INT           NOT NULL,
    [USER_ID]        INT           NOT NULL,
    [QUIZ_ID]        INT           NOT NULL,
    [QUIZ_STATUS]    INT           NOT NULL,
    [ASSIGNED_BY_ID] INT           NOT NULL,
    [ASSIGNED_DATE]  DATETIME      NOT NULL,
    [STARTED_DATE]   DATETIME      NULL,
    [TIME_TAKEN]     NVARCHAR (30) NULL,
    [COMPLETED_RATE] REAL          NULL,
    [COMPLETED_DATE] DATETIME      NULL,
    CONSTRAINT [PK_M_QUIZ_RESULTS] PRIMARY KEY CLUSTERED ([QUIZ_RESULT_ID] ASC)
);


GO
PRINT N'Выполняется создание [DBO].[M_QUIZES]...';


GO
CREATE TABLE [DBO].[M_QUIZES] (
    [QUIZ_ID]      INT            NOT NULL,
    [QUIZ_NAME]    NVARCHAR (100) NOT NULL,
    [AUTHOR_ID]    INT            NOT NULL,
    [CREATED_DATE] DATETIME       NOT NULL,
    [SUCCESS_RATE] REAL           NOT NULL,
    CONSTRAINT [PK_M_QUIZES] PRIMARY KEY CLUSTERED ([QUIZ_ID] ASC),
    CONSTRAINT [NK1_M_QUIZES] UNIQUE NONCLUSTERED ([QUIZ_NAME] ASC)
);


GO
PRINT N'Выполняется создание [DBO].[M_ROLES]...';


GO
CREATE TABLE [DBO].[M_ROLES] (
    [ROLE_ID]         INT            NOT NULL,
    [ROLE_NAME]       NVARCHAR (100) NOT NULL,
    [ROLE_FLAG]       INT            NOT NULL,
    [ALLOWED_METHODS] NVARCHAR (MAX) NOT NULL
);


GO
PRINT N'Выполняется создание [DBO].[M_USERS]...';


GO
CREATE TABLE [DBO].[M_USERS] (
    [USER_ID]           INT            NOT NULL,
    [FIRSTNAME]         NVARCHAR (100) NOT NULL,
    [LASTNAME]          NVARCHAR (100) NOT NULL,
    [USERNAME]          NVARCHAR (100) NOT NULL,
    [HASHEDPASSWORD]    NVARCHAR (100) NOT NULL,
    [SALT]              NVARCHAR (100) NOT NULL,
    [ROLESFLAG]         INT            NOT NULL,
    [REGISTRATION_DATE] DATETIME       NOT NULL,
    [LAST_LOGON_DATE]   DATETIME       NULL,
    CONSTRAINT [PK_M_USERS] PRIMARY KEY CLUSTERED ([USER_ID] ASC),
    CONSTRAINT [NK1_M_USERS] UNIQUE NONCLUSTERED ([USERNAME] ASC)
) ON [PRIMARY];


GO
PRINT N'Выполняется создание [DBO].[M_VARIANTS]...';


GO
CREATE TABLE [DBO].[M_VARIANTS] (
    [VARIANT_ID]  INT             NOT NULL,
    [QUESTION_ID] INT             NOT NULL,
    [TEXT]        NVARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_M_VARIANTS] PRIMARY KEY CLUSTERED ([VARIANT_ID] ASC)
);


GO
PRINT N'Выполняется создание [DBO].[S_PK_GENERATOR]...';


GO
CREATE TABLE [DBO].[S_PK_GENERATOR] (
    [TABLE_NAME] VARCHAR (30) NOT NULL,
    [TABLE_ID]   NUMERIC (18) NOT NULL,
    CONSTRAINT [XPK_S_PK_GENERATOR] PRIMARY KEY CLUSTERED ([TABLE_NAME] ASC)
) ON [PRIMARY];


GO
PRINT N'Выполняется создание [DBO].[T_LOG]...';


GO
CREATE TABLE [DBO].[T_LOG] (
    [Id]        INT             NOT NULL,
    [Date]      DATETIME        NOT NULL,
    [Thread]    VARCHAR (255)   NOT NULL,
    [Level]     VARCHAR (50)    NOT NULL,
    [Logger]    VARCHAR (255)   NOT NULL,
    [Message]   NVARCHAR (4000) NOT NULL,
    [Exception] NVARCHAR (2000) NULL
) ON [PRIMARY];


GO
PRINT N'Выполняется создание [DBO].[FK1_M_QUESTIONS]...';


GO
ALTER TABLE [DBO].[M_QUESTIONS]
    ADD CONSTRAINT [FK1_M_QUESTIONS] FOREIGN KEY ([QUIZ_ID]) REFERENCES [DBO].[M_QUIZES] ([QUIZ_ID]) ON DELETE CASCADE;


GO
PRINT N'Выполняется создание [DBO].[FK1_M_QUIZ_ANSWERS]...';


GO
ALTER TABLE [DBO].[M_QUIZ_ANSWERS]
    ADD CONSTRAINT [FK1_M_QUIZ_ANSWERS] FOREIGN KEY ([QUIZ_ID]) REFERENCES [DBO].[M_QUIZES] ([QUIZ_ID]);


GO
PRINT N'Выполняется создание [DBO].[FK2_M_QUIZ_ANSWERS]...';


GO
ALTER TABLE [DBO].[M_QUIZ_ANSWERS]
    ADD CONSTRAINT [FK2_M_QUIZ_ANSWERS] FOREIGN KEY ([QUESTION_ID]) REFERENCES [DBO].[M_QUESTIONS] ([QUESTION_ID]);


GO
PRINT N'Выполняется создание [DBO].[FK3_M_QUIZ_ANSWERS]...';


GO
ALTER TABLE [DBO].[M_QUIZ_ANSWERS]
    ADD CONSTRAINT [FK3_M_QUIZ_ANSWERS] FOREIGN KEY ([QUIZ_RESULT_ID]) REFERENCES [DBO].[M_QUIZ_RESULTS] ([QUIZ_RESULT_ID]) ON DELETE CASCADE;


GO
PRINT N'Выполняется создание [DBO].[FK1_M_QUIZ_RESULTS]...';


GO
ALTER TABLE [DBO].[M_QUIZ_RESULTS]
    ADD CONSTRAINT [FK1_M_QUIZ_RESULTS] FOREIGN KEY ([USER_ID]) REFERENCES [DBO].[M_USERS] ([USER_ID]);


GO
PRINT N'Выполняется создание [DBO].[FK2_M_QUIZ_RESULTS]...';


GO
ALTER TABLE [DBO].[M_QUIZ_RESULTS]
    ADD CONSTRAINT [FK2_M_QUIZ_RESULTS] FOREIGN KEY ([QUIZ_ID]) REFERENCES [DBO].[M_QUIZES] ([QUIZ_ID]) ON DELETE CASCADE;


GO
PRINT N'Выполняется создание [DBO].[FK3_M_QUIZ_RESULTS]...';


GO
ALTER TABLE [DBO].[M_QUIZ_RESULTS]
    ADD CONSTRAINT [FK3_M_QUIZ_RESULTS] FOREIGN KEY ([ASSIGNED_BY_ID]) REFERENCES [DBO].[M_USERS] ([USER_ID]) ON DELETE CASCADE;


GO
PRINT N'Выполняется создание [DBO].[FK1_M_QUIZES]...';


GO
ALTER TABLE [DBO].[M_QUIZES]
    ADD CONSTRAINT [FK1_M_QUIZES] FOREIGN KEY ([AUTHOR_ID]) REFERENCES [DBO].[M_USERS] ([USER_ID]);


GO
PRINT N'Выполняется создание [DBO].[FK1_M_VARIANTS]...';


GO
ALTER TABLE [DBO].[M_VARIANTS]
    ADD CONSTRAINT [FK1_M_VARIANTS] FOREIGN KEY ([QUESTION_ID]) REFERENCES [DBO].[M_QUESTIONS] ([QUESTION_ID]) ON DELETE CASCADE;


GO
PRINT N'Выполняется создание [DBO].[CK1_M_QUIZ_RESULTS]...';


GO
ALTER TABLE [DBO].[M_QUIZ_RESULTS]
    ADD CONSTRAINT [CK1_M_QUIZ_RESULTS] CHECK ([COMPLETED_RATE] >=0 AND [COMPLETED_RATE] <= 1);


GO
PRINT N'Выполняется создание [DBO].[CK1_M_QUIZES]...';


GO
ALTER TABLE [DBO].[M_QUIZES]
    ADD CONSTRAINT [CK1_M_QUIZES] CHECK ([SUCCESS_RATE] >0 AND [SUCCESS_RATE] <= 1);


GO
PRINT N'Выполняется создание [DBO].[V_M_QUESTIONS]...';


GO
CREATE VIEW [DBO].[V_M_QUESTIONS]
AS 
SELECT [QUESTION_ID]
      ,[QUIZ_ID]
      ,[INFO]
      ,[TEXT]
      ,[CORRECT_OPTION_FLAG]
  FROM [DBO].[M_QUESTIONS]
GO
PRINT N'Выполняется создание [DBO].[V_M_QUIZES]...';


GO
CREATE VIEW [DBO].[V_M_QUIZES]
AS
	SELECT 
		[QUIZ_ID]
      ,[QUIZ_NAME]
      ,[AUTHOR_ID]
	  ,(SELECT ISNULL(FIRSTNAME,'') + ' ' + ISNULL(LASTNAME,'') FROM DBO.M_USERS WHERE M_USERS.USER_ID = M_QUIZES.AUTHOR_ID) as AUTHOR
      ,CONVERT(nvarchar(23),[CREATED_DATE],121) as [CREATED_DATE]
      ,[SUCCESS_RATE]
  FROM [DBO].[M_QUIZES]
GO
PRINT N'Выполняется создание [DBO].[V_M_ROLES]...';


GO
CREATE VIEW [DBO].[V_M_ROLES]
	AS SELECT ROLE_ID, ROLE_NAME, ROLE_FLAG, ALLOWED_METHODS FROM DBO.[M_ROLES]
GO
PRINT N'Выполняется создание [DBO].[V_M_USERS]...';


GO
CREATE VIEW [DBO].[V_M_USERS]
AS
SELECT [USER_ID], [FIRSTNAME], [LASTNAME], [USERNAME], [HASHEDPASSWORD], [SALT], [ROLESFLAG], [REGISTRATION_DATE], [LAST_LOGON_DATE]
FROM  DBO.M_USERS
GO
PRINT N'Выполняется создание [DBO].[F_PRINT_DATEDIFF]...';


GO
CREATE FUNCTION [DBO].[F_PRINT_DATEDIFF]
(
	@D1 datetime,
	@D2 datetime
)
RETURNS nvarchar(8)
AS
BEGIN
	DECLARE @seconds int = DATEDIFF(second,@D1,@D2)
	DECLARE @hours int, @minutes int , @sec int
	SELECT @hours = @seconds/3600 , 
	 @minutes = (@seconds - ( @seconds/3600) * 3600) / 60 ,
	 @sec = (@seconds - ((@seconds/3600) * 3600 + ((@seconds - ( @seconds/3600) * 3600)/60)* 60))

	RETURN RIGHT('00'+CONVERT(varchar(2), @hours),2)+ ':'+
		RIGHT('00'+ CONVERT(varchar(2), @minutes),2)+':'+
		RIGHT('00' + CONVERT(varchar(2), @sec),2)
END
GO
PRINT N'Выполняется создание [DBO].[P_DELETEQUESTION]...';


GO
CREATE PROCEDURE [DBO].[P_DELETEQUESTION]
@QuizID int , 
                                        @QuestionID int , 
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM DBO.M_QUIZES WHERE QUIZ_ID  = @QuizID)
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz '+ CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES table';
	ELSE
	BEGIN
		BEGIN TRY
			IF (EXISTS(SELECT * FROM DBO.[M_QUESTIONS] Q WHERE Q.QUESTION_ID = @QuestionID AND Q.QUIZ_ID = @QuizID))
			BEGIN
				DELETE FROM DBO.[M_QUESTIONS] WHERE QUIZ_ID = @QuizID AND QUESTION_ID = @QuestionID
			END
			ELSE
			BEGIN
				SELECT @ERROR = -1, @ERRORTEXT = N'No Question '+ CAST(@QuestionID as nvarchar(10)) + N' in M_QUESTIONS table';
			END
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END
GO
PRINT N'Выполняется создание [DBO].[P_DELETEQUIZ]...';


GO
CREATE PROCEDURE [DBO].[P_DELETEQUIZ]
										@QuizID int , 
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM DBO.M_QUIZES WHERE QUIZ_ID  = @QuizID)
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz '+ CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES table';
	ELSE
	BEGIN
		BEGIN TRY
			DELETE FROM DBO.[M_QUIZES] WHERE QUIZ_ID = @QuizID
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END
GO
PRINT N'Выполняется создание [DBO].[P_DELETEUSER]...';


GO
CREATE PROCEDURE [DBO].[P_DELETEUSER]
	@USERID int = 0,
	@ERROR int out,
	@ERRORTEXT nvarchar(100) out

AS
BEGIN
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Delete Ok';
	BEGIN TRY
		IF EXISTS(SELECT * FROM DBO.M_USERS WHERE USER_ID = @USERID)
			DELETE FROM DBO.M_USERS WHERE USER_ID = @USERID
		ELSE
			SELECT @ERROR = -1,	@ERRORTEXT = N'No UserID '+CAST(@USERID as nvarchar(10)) + N' in M_USERS table';
	END TRY
	BEGIN CATCH
		SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
	END CATCH
	RETURN 0
END
GO
PRINT N'Выполняется создание [DBO].[P_DELETEVARIANT]...';


GO
CREATE PROCEDURE [DBO].[P_DELETEVARIANT]
                                        @QuizID int , 
                                        @QuestionID int , 
										@VariantID int,
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM DBO.M_QUIZES WHERE QUIZ_ID  = @QuizID) OR NOT EXISTS(SELECT * FROM DBO.M_QUESTIONS WHERE QUESTION_ID  = @QuestionID)
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz/Questiuon '+ CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES/M_QUESTIONS table';
	ELSE
	BEGIN
		BEGIN TRY
			IF (EXISTS(SELECT * FROM DBO.[M_VARIANTS] V WHERE V.QUESTION_ID = @QuestionID AND V.VARIANT_ID = @VariantID))
			BEGIN
				DELETE FROM DBO.[M_VARIANTS] WHERE QUESTION_ID = @QuestionID AND VARIANT_ID = @VariantID
			END
			ELSE
			BEGIN
				SELECT @ERROR = -1, @ERRORTEXT = N'No Variant '+ CAST(@VariantID as nvarchar(10)) + N' in M_VARIANTS table';
			END
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END
GO
PRINT N'Выполняется создание [DBO].[P_GETALLQUIZES]...';


GO
CREATE PROCEDURE [DBO].[P_GETALLQUIZES]
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
			FROM  DBO.M_VARIANTS C WHERE C.QUESTION_ID= B.QUESTION_ID
		FOR XML PATH('Variant'),type) as Options 
	FROM  DBO.M_QUESTIONS B WHERE B.QUIZ_ID= A.QUIZ_ID
	FOR XML PATH('Question'),type)  as "Questions"
FROM DBO.M_QUIZES A
FOR XML PATH('Quiz'), type, root('ArrayOfQuiz')
END
GO
PRINT N'Выполняется создание [DBO].[P_GETALLUSERS]...';


GO
-- =============================================
-- Author:		Anton Metlyakov
-- Create date: 02/12/2019
-- Description:	RETURNS USER LIST
-- Input : @RecordsCount, -1 - no limit
-- returns : table
-- =============================================
CREATE PROCEDURE [DBO].[P_GETALLUSERS]
@RecordsCount int = 2147483647 /* max INT value*/
AS
SET NOCOUNT ON
BEGIN												
	/* treating all negatives as max INT */
	DECLARE @InternalRecordsCount int = CASE WHEN @RecordsCount<0 THEN 2147483647 ELSE @RecordsCount END
	SELECT	TOP (@InternalRecordsCount)  
		[USER_ID], 
		[FIRSTNAME], 
		[LASTNAME], 
		[USERNAME], 
		[HASHEDPASSWORD], 
		[SALT],
		[ROLESFLAG], 
		CONVERT(nvarchar(23),[REGISTRATION_DATE],121) as [REGISTRATION_DATE] , 
		CONVERT(nvarchar(23),[LAST_LOGON_DATE],121) as [LAST_LOGON_DATE]
	FROM  DBO.V_M_USERS
END
GO
PRINT N'Выполняется создание [DBO].[P_GETNEXTPK]...';


GO

-- =============================================
-- Author:		Anton Metlyakov
-- Create date: 02/12/2019
-- Description:	Generates new identity number for given table name
-- Input : table name
-- returns : @ID int - next counter value
-- =============================================
CREATE PROCEDURE [DBO].[P_GETNEXTPK]
@TABLE_NAME VARCHAR(30),
 @ID NUMERIC OUT
AS
SET NOCOUNT ON
BEGIN
	IF NOT EXISTS (SELECT * FROM  [DBO].[S_PK_GENERATOR]  WHERE TABLE_NAME = @TABLE_NAME) 
		RAISERROR('Please add record for ''%s'' in PK generator table', 16, 1, @TABLE_NAME)
	 UPDATE [DBO].[S_PK_GENERATOR]  
	 SET TABLE_ID = TABLE_ID + 1,@ID = TABLE_ID + 1   
	 WHERE TABLE_NAME = @TABLE_NAME;
END
GO
PRINT N'Выполняется создание [DBO].[P_GETNEXTQUESTION]...';


GO
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
GO
PRINT N'Выполняется создание [DBO].[P_GETQUIZ]...';


GO
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
GO
PRINT N'Выполняется создание [DBO].[P_GETQUIZASSIGNMENT]...';


GO
CREATE PROCEDURE [DBO].[P_GETQUIZASSIGNMENT]
@UserID int = 1
AS
SET NOCOUNT ON
BEGIN												
SELECT 
	@UserID as [User_Id],
	(SELECT USERNAME FROM DBO.M_USERS WHERE [USER_ID]=@UserID) as [User_Name],
	(SELECT 
		UserQuizList.QUIZ_ID as [Quiz_Id], 
		UserQuizList.QUIZ_NAME as Quiz_Name,
		ISNULL(QR.QUIZ_STATUS ,0) as Quiz_Status,
		ISNULL(QR.QUIZ_RESULT_ID,-1) as QuizResult_Id
		FROM
	(SELECT  U.USER_ID,U.USERNAME,  Q.QUIZ_NAME, Q.QUIZ_ID
	FROM DBO.M_USERS U
	CROSS JOIN DBO.M_QUIZES Q
	WHERE U.USER_ID = @UserID ) as UserQuizList
	LEFT JOIN DBO.M_QUIZ_RESULTS QR ON QR.QUIZ_ID = UserQuizList.QUIZ_ID AND QR.USER_ID = @UserID
	ORDER BY Quiz_Name
	FOR XML PATH('AssignQuizList'),type)  as [Assignquizlist]
FOR XML PATH('AssignQuiz'), type 
END
GO
PRINT N'Выполняется создание [DBO].[P_GETQUIZRESULT]...';


GO
CREATE PROCEDURE [DBO].[P_GETQUIZRESULT]
@QuizResultId int
AS
SET NOCOUNT ON
BEGIN												
SELECT
       [QUIZ_RESULT_ID] as [QuizResult_Id]
	  ,[USER_ID] as [User_Id]
	  ,(SELECT USERNAME FROM DBO.M_USERS Q WHERE Q.USER_ID =  QR.[USER_ID]) as [UserName]
      ,[QUIZ_ID] as [Quiz_Id]
	  ,(SELECT QUIZ_NAME FROM DBO.M_QUIZES Q WHERE Q.QUIZ_ID = QR.QUIZ_ID) as [Quiz_Name]
      ,(SELECT FIRSTNAME+ ' '+ LASTNAME FROM DBO.M_USERS Q WHERE Q.USER_ID =  QR.[ASSIGNED_BY_ID]) as [AssignedBy]
      ,[ASSIGNED_DATE] as [Assigned_Date]
      ,[QUIZ_STATUS] as [QuizResult_Status]
      ,[COMPLETED_DATE] as [Completed_Date]
	  ,[STARTED_DATE] as [Started_Date]
	  ,[TIME_TAKEN] as [Time_Taken]
      ,CAST([COMPLETED_RATE] as DECIMAL(6,4)) as [Completed_Rate]
	  ,(SELECT	
			QA.QUIZ_ANSWERS_ID as [Answer_Id],
			QA.QUIZ_RESULT_ID as [QuizResult_Id],
			QA.QUIZ_ID as [Quiz_Id],
			QA.QUESTION_ID as [Question_Id],
			QA.ANSWER_FLAG as [Answer_Flag],
			QA.[TIMESTAMP] as [TimeStamp]
			FROM  DBO.M_QUIZ_ANSWERS QA WHERE QA.QUIZ_RESULT_ID = QR.QUIZ_RESULT_ID
		FOR XML PATH('Answer'),type) as [Answer_List]
  FROM [DBO].[M_QUIZ_RESULTS] QR
  WHERE QR.QUIZ_RESULT_ID = @QuizResultId
  ORDER BY [Quiz_Name]
  FOR XML PATH('QuizResult'), type
END
GO
PRINT N'Выполняется создание [DBO].[P_GETQUIZRESULTS]...';


GO
CREATE PROCEDURE [DBO].[P_GETQUIZRESULTS]
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
	  ,(SELECT USERNAME FROM DBO.M_USERS Q WHERE Q.USER_ID =  QR.[USER_ID]) as [UserName]
      ,[QUIZ_ID] as [Quiz_Id]
	  ,(SELECT QUIZ_NAME FROM DBO.M_QUIZES Q WHERE Q.QUIZ_ID = QR.QUIZ_ID) as [Quiz_Name]
      ,(SELECT FIRSTNAME+ ' '+ LASTNAME FROM DBO.M_USERS Q WHERE Q.USER_ID =  QR.[ASSIGNED_BY_ID]) as [AssignedBy]
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
			FROM  DBO.M_QUIZ_ANSWERS QA WHERE QA.QUIZ_RESULT_ID = QR.QUIZ_RESULT_ID
		FOR XML PATH('Answer'),type) as [Answer_List]
  FROM [DBO].[M_QUIZ_RESULTS] QR
  WHERE QR.USER_ID = @UserId
  FOR XML PATH('QuizResult'), type, root('ArrayOfQuizResult')
END
GO
PRINT N'Выполняется создание [DBO].[P_RECORDLOG]...';


GO
-- =============================================
-- Author:		Anton Metlyakov
-- Create date: 02/17/2019
-- Description:	Saves log record to table T_LOG 
-- Input : log info fields
-- return : @error number, if any
-- =============================================
CREATE PROCEDURE [DBO].[P_RECORDLOG]
	@log_date datetime,
	@thread varchar(255), 
	@log_level varchar(50),
	@logger varchar(255),
	@message nvarchar(4000), 
	@exception nvarchar(2000) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
				DECLARE @NEWID NUMERIC;
				EXECUTE DBO.P_GETNEXTPK @TABLE_NAME = 'T_LOG', @ID = @NEWID OUT
				INSERT INTO DBO.T_LOG 
					([Id],[Date],[Thread],[Level],[Logger],[Message],[Exception])
					VALUES (@NEWID, @log_date, @thread, @log_level, @logger, @message, @exception)	
	RETURN 0
END
GO
PRINT N'Выполняется создание [DBO].[P_SAVEANSWER]...';


GO
CREATE PROCEDURE [DBO].[P_SAVEANSWER]
										@QuizResultId int,
                                        @QuestionID int , 
                                        @AnswerID int,
                                        @AnswerFlag int,
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF ( NOT EXISTS(SELECT * FROM DBO.M_QUESTIONS WHERE QUESTION_ID = @QuestionID) OR 
	   NOT EXISTS(SELECT * FROM DBO.M_QUIZ_RESULTS WHERE QUIZ_RESULT_ID = @QuizResultId))
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz|Question|Result '+ CAST(@QuizResultId as nvarchar(10)) + N' in M_QUESTIONS|M_QUIZ_RESULTS table';
	ELSE
	BEGIN
		IF (@AnswerID >0) 
		BEGIN
			SELECT @ERROR = -2,	@ERRORTEXT = N' AnswerID '+ CAST(@AnswerID as nvarchar(10)) + N' is positive. No updates allowed';
		END
		ELSE
		BEGIN
			BEGIN TRY
				/* new record */
				DECLARE @NEWID NUMERIC;
				EXECUTE DBO.P_GETNEXTPK @TABLE_NAME = 'M_QUIZ_ANSWERS', @ID = @NEWID OUT
				INSERT INTO DBO.[M_QUIZ_ANSWERS]
					([QUIZ_ANSWERS_ID], [QUIZ_RESULT_ID], [QUIZ_ID], [QUESTION_ID],[ANSWER_FLAG], [TIMESTAMP])
					VALUES (
						@NEWID,
						@QuizResultId,
						(SELECT QUIZ_ID FROM DBO.[M_QUIZ_RESULTS] QR WHERE QR.QUIZ_RESULT_ID = @QuizResultId),
						@QuestionID,
						@AnswerFlag,
						GETDATE())
			END TRY
			BEGIN CATCH
				SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
			END CATCH
		END
	END

END
GO
PRINT N'Выполняется создание [DBO].[P_SAVEQUESTION]...';


GO
CREATE PROCEDURE [DBO].[P_SAVEQUESTION]
                                        @QuizID int , 
                                        @QuestionID int , 
                                        @info nvarchar(1000) ,
                                        @text nvarchar(1000),
                                        @CorrectOptionFlag int,
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM DBO.M_QUIZES WHERE QUIZ_ID  = @QuizID)
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz '+ CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES table';
	ELSE
	BEGIN
		BEGIN TRY
			IF (EXISTS(SELECT * FROM DBO.[M_QUESTIONS] Q WHERE Q.QUESTION_ID = @QuestionID AND Q.QUIZ_ID = @QuizID)
				AND @QuestionID >0)
			BEGIN
			/* update record */
				UPDATE DBO.[M_QUESTIONS]
				SET 
					[INFO] = @info, [TEXT] = @text, [CORRECT_OPTION_FLAG] = @CorrectOptionFlag
				WHERE 
					QUESTION_ID = @QuestionID AND QUIZ_ID = @QuizID
			END
			ELSE
			BEGIN
			/* new record */
				DECLARE @NEWID NUMERIC;
				EXECUTE DBO.P_GETNEXTPK @TABLE_NAME = 'M_QUESTIONS', @ID = @NEWID OUT
				INSERT INTO DBO.[M_QUESTIONS]
				([QUIZ_ID], [QUESTION_ID],[INFO], [TEXT],[CORRECT_OPTION_FLAG])
				VALUES (@QuizID, @NEWID,@info, @text,@CorrectOptionFlag)
			END
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END
GO
PRINT N'Выполняется создание [DBO].[P_SAVEQUIZ]...';


GO
CREATE PROCEDURE [DBO].[P_SAVEQUIZ]
                                        @QuizID int, 
                                        @QuizName nvarchar(100),
                                        @AuthorID int, 
                                        @CreatedDate datetime,
                                        @SuccessRate real,
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
BEGIN TRY
		IF (@QuizID = -1) 
		BEGIN
		/* new quiz */
			IF EXISTS(SELECT * FROM DBO.M_QUIZES WHERE QUIZ_NAME = @QuizName)
				SELECT @ERROR = -1,	@ERRORTEXT = N'Dublicate Quiz name '+@QuizName + N' in M_QUIZES table';
			ELSE
			BEGIN
				DECLARE @NEWID NUMERIC;
				EXECUTE DBO.P_GETNEXTPK @TABLE_NAME = 'M_QUIZES', @ID = @NEWID OUT
				INSERT INTO DBO.M_QUIZES 
				([QUIZ_ID],[QUIZ_NAME], [AUTHOR_ID], [CREATED_DATE], [SUCCESS_RATE])
					VALUES (@NEWID,@QuizName,@AuthorID,GETDATE(),@SuccessRate)
			END
		END
		ELSE
		BEGIN
		/* Update existing quiz info */
			IF NOT EXISTS(SELECT * FROM DBO.M_QUIZES WHERE QUIZ_ID = @QuizID)
			/* DB consistency error */
				SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz '+CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES table';
			ELSE
			BEGIN
			/* change quiz name of existing quiz. Is new Quiz name free ?*/
				IF EXISTS(SELECT * FROM DBO.M_QUIZES WHERE [QUIZ_NAME] = @QuizName AND QUIZ_ID != @QuizID )
					SELECT @ERROR = -1,	@ERRORTEXT = N'Can''t change quiz name to '+@QuizName + N' in M_QUIZES table, this quiz name already exists';
				ELSE
					UPDATE DBO.M_QUIZES 
					SET [QUIZ_NAME] = @QuizName,
						[AUTHOR_ID] = @AuthorID,
						[SUCCESS_RATE] = @SuccessRate
					WHERE [QUIZ_ID] = @QuizID
			END
		END
	END TRY
	BEGIN CATCH
		SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
	END CATCH

END
GO
PRINT N'Выполняется создание [DBO].[P_SAVEQUIZASSIGNMENT]...';


GO
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
GO
PRINT N'Выполняется создание [DBO].[P_SAVEQUIZRESULT]...';


GO
CREATE PROCEDURE [DBO].[P_SAVEQUIZRESULT]
                                        @QuizResultID int, 
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	/*  test script
	DECLARE @error int, @Errortext nvarchar(1000)
	 execute [DBO].[P_SAVEQUZRESULT] 1, @Error out, @errortext out
 */
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM DBO.M_QUIZ_RESULTS WHERE QUIZ_RESULT_ID = @QuizResultID)
				SELECT @ERROR = -1,	@ERRORTEXT = N'NO QuizResult '+CAST(@QuizResultID as nvarchar(10)) + N' in M_QUIZ_RESULTS table';
	ELSE
	BEGIN
		BEGIN TRY
		;WITH Save_Quiz_Result AS
		(SELECT 
				QR.QUIZ_RESULT_ID as QUIZRESULTID,
				(SELECT SUCCESS_RATE FROM [DBO].[M_QUIZES] QQ WHERE QQ.QUIZ_ID = QR.QUIZ_ID) 
				as SuccessRate,
				CASE WHEN COUNT(Q.QUESTION_ID) >0 THEN  /* не хочу делить на ноль */
					CAST(SUM (CASE WHEN Q.CORRECT_OPTION_FLAG = QA.ANSWER_FLAG THEN 1 ELSE 0 END ) as real) /* Сумма правильных ответов/ сумму вопросов*/
					/ CAST(COUNT(Q.QUESTION_ID) as real)
				ELSE 0 END
				as CompletedRate,
				QR.STARTED_DATE as [Start_Date],
				GETDATE() as Completed_Date 
		  FROM [DBO].[M_QUESTIONS] Q
		  JOIN [DBO].[M_QUIZ_RESULTS] QR ON QR.QUIZ_ID = Q.QUIZ_ID
		  LEFT JOIN [DBO].[M_QUIZ_ANSWERS] QA ON QR.QUIZ_RESULT_ID = QA.QUIZ_RESULT_ID AND QA.QUESTION_ID = Q.QUESTION_ID
		  WHERE 
			QR.QUIZ_RESULT_ID = @QuizResultID
		  GROUP BY 
			QR.QUIZ_RESULT_ID , QR.QUIZ_ID , QR.STARTED_DATE
		)
		UPDATE DBO.M_QUIZ_RESULTS 
		SET COMPLETED_RATE = QR.CompletedRate,
			COMPLETED_DATE = GETDATE(),
			TIME_TAKEN = DBO.F_PRINT_DATEDIFF(QR.START_DATE,GETDATE()),
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
GO
PRINT N'Выполняется создание [DBO].[P_SAVEUSER]...';


GO

-- =============================================
-- Author:		Anton Metlyakov
-- Create date: 02/12/2019
-- Description:	Saves user to table M_USERS (new (id=-1)/update (id>=0))
-- Input : user info fields
-- return : @error number, if any
-- =============================================
CREATE PROCEDURE [DBO].[P_SAVEUSER]
		@USERID int=-1,
	@USERNAME nvarchar(100),
	@FIRSTNAME nvarchar(100),
	@LASTNAME nvarchar(100),
	@HASHEDPASSWORD nvarchar(100),
	@SALT nvarchar(100),
	@ROLESFLAG int = 1,
	@LastLogonDate nvarchar(50),
	@ERROR int out,
	@ERRORTEXT nvarchar(100) out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	BEGIN TRY
		IF (@USERID = -1) 
		BEGIN
		/* new user */
			IF EXISTS(SELECT * FROM DBO.M_USERS WHERE USERNAME = @USERNAME)
				SELECT @ERROR = -1,	@ERRORTEXT = N'Dublicate Username '+@USERNAME + N' in M_USERS table';
			ELSE
			BEGIN
				DECLARE @NEWID NUMERIC;
				EXECUTE DBO.P_GETNEXTPK @TABLE_NAME = 'M_USERS', @ID = @NEWID OUT
				INSERT INTO DBO.M_USERS (USER_ID,FIRSTNAME,LASTNAME,USERNAME, HASHEDPASSWORD, SALT, ROLESFLAG,[REGISTRATION_DATE],[LAST_LOGON_DATE] )
					VALUES (@NEWID,@FIRSTNAME,@LASTNAME,@USERNAME,@HASHEDPASSWORD,@SALT,@ROLESFLAG, GETDATE(), NULL)
			END
		END
		ELSE
		BEGIN
		/* Update existing user info */
			IF NOT EXISTS(SELECT * FROM DBO.M_USERS WHERE USER_ID = @USERID)
			/* DB consistency error */
				SELECT @ERROR = -1,	@ERRORTEXT = N'No Username '+@USERNAME + N' in M_USERS table';
			ELSE
			BEGIN
			/* change username of existing user. Is new username free ?*/
				IF EXISTS(SELECT * FROM DBO.M_USERS WHERE USERNAME = @USERNAME AND USER_ID != @USERID )
					SELECT @ERROR = -1,	@ERRORTEXT = N'Can''t change Username to '+@USERNAME + N' in M_USERS table, this username already exists';
				ELSE
					UPDATE DBO.M_USERS 
					SET USERNAME = @USERNAME,
						FIRSTNAME = @FIRSTNAME,
						LASTNAME = @LASTNAME,
						HASHEDPASSWORD = @HASHEDPASSWORD,
						ROLESFLAG = @ROLESFLAG
					WHERE USER_ID = @USERID
			END
		END
		IF (LEN(@LastLogonDate) >0)
		BEGIN
			UPDATE DBO.M_USERS 
			SET LAST_LOGON_DATE = CAST(@LastLogonDate as datetime)
			WHERE USER_ID  = @USERID
		END
	END TRY
	BEGIN CATCH
		SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
	END CATCH
END
GO
PRINT N'Выполняется создание [DBO].[P_SAVEVARIANT]...';


GO
CREATE PROCEDURE [DBO].[P_SAVEVARIANT]
										@QuizID int , 
                                        @QuestionID int , 
                                        @VariantID int,
                                        @text nvarchar(1000),
                                        @ERROR int OUT, 
                                        @ERRORTEXT nvarchar(1000) OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
	IF NOT EXISTS(SELECT * FROM DBO.M_QUIZES WHERE QUIZ_ID  = @QuizID) OR NOT EXISTS(SELECT * FROM DBO.M_QUESTIONS WHERE QUESTION_ID  = @QuestionID)
	/* DB consistency error */
		SELECT @ERROR = -1,	@ERRORTEXT = N'No Quiz/Questiuon '+ CAST(@QuizID as nvarchar(10)) + N' in M_QUIZES/M_QUESTIONS table';
	ELSE
	BEGIN
		BEGIN TRY
			IF (EXISTS(SELECT * FROM DBO.[M_VARIANTS] V WHERE V.QUESTION_ID = @QuestionID )
				AND @VariantID >0)
			BEGIN
			/* update record */
				UPDATE DBO.[M_VARIANTS]
				SET 
					[TEXT] = @text
				WHERE 
					QUESTION_ID = @QuestionID AND VARIANT_ID = @VariantID
			END
			ELSE
			BEGIN
			/* new record */
				DECLARE @NEWID NUMERIC;
				EXECUTE DBO.P_GETNEXTPK @TABLE_NAME = 'M_VARIANTS', @ID = @NEWID OUT
				INSERT INTO DBO.[M_VARIANTS]
				([QUESTION_ID],[VARIANT_ID], [TEXT])
				VALUES (@QuestionID, @NEWID, @text)
			END
		END TRY
		BEGIN CATCH
			SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
		END CATCH
	END

END
GO
PRINT N'Выполняется создание [DBO].[P_STATSALLQUIZES]...';


GO
CREATE PROCEDURE [DBO].[P_STATSALLQUIZES]
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
			(SELECT COUNT(*) fROM DBO.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID) 
		as Total_Assigned,
			(SELECT COUNT(*) fROM DBO.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.QUIZ_STATUS = 3) 
		as Total_Passed,
			(SELECT COUNT(*) fROM DBO.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.QUIZ_STATUS = 4) 
		as Total_Failed,
			ISNULL(CAST(CAST((SELECT AVG(COMPLETED_RATE) fROM DBO.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID) as numeric(5,2)) as nvarchar(5)),'n/a') 
		as Average_Rate
		FROM DBO.M_QUIZES Q
	)
	SELECT R.Quiz_Name, R.Success_Rate, R.Total_Assigned, R.Total_Passed, R.Total_Failed,R.Average_Rate,
		CAST(CASE WHEN R.Total_Assigned =0 THEN  0
		ELSE CAST (R.Total_Passed as real)/ CAST(R.Total_Assigned as real) * 100.0
		END as numeric(6,2))
	as Percent_Passed
	FROM RawViewCTE R
	ORDER BY [Quiz_Name]
END
GO
PRINT N'Выполняется создание [DBO].[P_STATSALLUSERS]...';


GO
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
GO
PRINT N'Выполняется создание [DBO].[P_STATSBYQUIZ]...';


GO
CREATE PROCEDURE [DBO].[P_STATSBYQUIZ]
@Quiz_ID int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT 
		U.[USER_ID] as [User_Id], 
		U.USERNAME as [User_Name], 
		CASE  ISNULL((SELECT QR.QUIZ_STATUS FROM DBO.M_QUIZ_RESULTS QR WHERE QR.USER_ID = U.USER_ID AND QR.QUIZ_ID = @Quiz_ID),0)
			WHEN 0 THEN 'Not assigned'
			WHEN 1 THEN 'Assigned'
			WHEN 2 THEN 'In progress'
			WHEN 3 THEN 'Passed'
			WHEN 4 THEN 'Failed'
		END as Quiz_Status,
		ISNULL(CAST(CAST((SELECT QR.COMPLETED_RATE FROM DBO.M_QUIZ_RESULTS QR WHERE QR.USER_ID = U.USER_ID AND QR.QUIZ_ID = @Quiz_ID) as numeric(6,2)) as nvarchar(6)),'') 
		as Completed_Rate, 
		ISNULL(CONVERT(nvarchar(19),(SELECT QR.COMPLETED_DATE FROM DBO.M_QUIZ_RESULTS QR WHERE QR.USER_ID = U.USER_ID AND QR.QUIZ_ID = @Quiz_ID),121),'') 
		as Completed_Date,
		ISNULL(CONVERT(nvarchar(19),(SELECT QR.STARTED_DATE FROM DBO.M_QUIZ_RESULTS QR WHERE QR.USER_ID = U.USER_ID AND QR.QUIZ_ID = @Quiz_ID),121),'') 
		as Started_Date,
		ISNULL(
			  (SELECT DBO.[F_PRINT_DATEDIFF](QR.STARTED_DATE,QR.COMPLETED_DATE) FROM DBO.M_QUIZ_RESULTS QR 
					WHERE QR.QUIZ_ID = @QUIZ_ID AND QR.USER_ID = U.USER_ID)
		,'') as [Time_taken]
	FROM DBO.M_USERS U
	order by [User_Name]
END
GO
PRINT N'Выполняется создание [dbo].[P_STATSBYUSER]...';


GO
CREATE PROCEDURE P_STATSBYUSER
@USER_ID int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		Q.QUIZ_ID as [Quiz_Id],
		Q.QUIZ_NAME as [Quiz_Name],
		CASE  ISNULL((SELECT QR.QUIZ_STATUS FROM DBO.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID),0)
			WHEN 0 THEN 'Not assigned'
			WHEN 1 THEN 'Assigned'
			WHEN 2 THEN 'In progress'
			WHEN 3 THEN 'Passed'
			WHEN 4 THEN 'Failed'
		END as Quiz_Status,
		ISNULL(
			CAST(
				CAST(
					(SELECT QR.COMPLETED_RATE FROM DBO.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID)
				 as numeric(5,2)
			) as nvarchar(5)
		),'') 
		as [Completed_Rate], 
		ISNULL(
			CONVERT(nvarchar(19),
				(SELECT QR.COMPLETED_DATE FROM DBO.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID)
				,121)
		,'') 
		as [Completed_Date],
		ISNULL(CONVERT(nvarchar(19),(SELECT QR.STARTED_DATE FROM DBO.M_QUIZ_RESULTS QR WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID),121),'') 
		as [Started_Date],
		ISNULL(
			  (SELECT DBO.[F_PRINT_DATEDIFF](QR.STARTED_DATE,QR.COMPLETED_DATE) FROM DBO.M_QUIZ_RESULTS QR 
					WHERE QR.QUIZ_ID = Q.QUIZ_ID AND QR.USER_ID = @USER_ID)
		,'') as [Time_taken]
	FROM DBO.M_QUIZES Q
END
GO
PRINT N'Выполняется создание [DBO].[P_STATSBYUSERQUIZ]...';


GO
CREATE PROCEDURE [DBO].[P_STATSBYUSERQUIZ]
@USER_ID INT,
@QUIZ_ID int 
AS
BEGIN

	DECLARE @Quiz_Status int = ISNULL((SELECT QUIZ_STATUS FROM [DBO].[M_QUIZ_RESULTS] WHERE QUIZ_ID = @QUIZ_ID AND USER_ID= @USER_ID),0)
	SELECT 
		Q.[INFO], 
		Q.[TEXT],
		CASE WHEN 
				(SELECT QA.ANSWER_FLAG
			FROM DBO.M_QUIZ_RESULTS QR
			JOIN DBO.M_QUIZ_ANSWERS QA ON QA.QUIZ_RESULT_ID = QR.QUIZ_RESULT_ID
			WHERE QR.USER_ID = @USER_ID
			AND QR.QUIZ_ID = Q.QUIZ_ID
			AND QA.QUESTION_ID = Q.QUESTION_ID) IS NULL
			THEN 'NOT ANSWERED'
		WHEN Q.CORRECT_OPTION_FLAG =
		(
			SELECT QA.ANSWER_FLAG
			FROM DBO.M_QUIZ_RESULTS QR
			JOIN DBO.M_QUIZ_ANSWERS QA ON QA.QUIZ_RESULT_ID = QR.QUIZ_RESULT_ID
			WHERE QR.USER_ID = @USER_ID
			AND QR.QUIZ_ID = Q.QUIZ_ID
			AND QA.QUESTION_ID = Q.QUESTION_ID
		)
		THEN 'RIGHT' 
		ELSE 'WRONG' END
			AS RESULT
	 FROM M_QUESTIONS Q
	WHERE Q.QUIZ_ID = @QUIZ_ID
	ORDER BY Q.QUESTION_ID
END
GO
-- Выполняется этап рефакторинга для обновления развернутых журналов транзакций на целевом сервере

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'a83bd177-679f-407f-b5c0-9bc765bde1ad')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('a83bd177-679f-407f-b5c0-9bc765bde1ad')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '1fcd2e4c-3faf-4f46-ba3d-1c3fb38a75b5')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('1fcd2e4c-3faf-4f46-ba3d-1c3fb38a75b5')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'a8e42ba6-8f8e-455e-a863-628a475bbc63')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('a8e42ba6-8f8e-455e-a863-628a475bbc63')

GO

GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DELETE FROM  [DBO].[S_PK_GENERATOR]

INSERT INTO [DBO].[S_PK_GENERATOR] ([TABLE_NAME],[TABLE_ID]) 
VALUES 
	('M_ROLES',4),
	('M_USERS',12),
	('M_QUIZES',20),
	('M_QUESTIONS',100),
	('M_VARIANTS',400),
	('M_QUIZ_RESULTS',20),
	('M_QUIZ_ANSWERS',40),
	('T_LOG',1)
GO


/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
DELETE FROM DBO.[M_ROLES];
INSERT INTO DBO.[M_ROLES]
([ROLE_ID], [ROLE_NAME], [ROLE_FLAG], [ALLOWED_METHODS])
VALUES
(1,N'Student',1,''),
(2,N'Instructor',2,''),
(3,N'Admin',4,'')
GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DELETE FROM [DBO].[M_USERS]
INSERT INTO [DBO].[M_USERS]
           ([USER_ID]
           ,[FIRSTNAME]
           ,[LASTNAME]
           ,[USERNAME]
           ,[HASHEDPASSWORD]
		   ,[SALT]
           ,[ROLESFLAG]
	   ,[REGISTRATION_DATE]
	   ,[LAST_LOGON_DATE])
     VALUES
           (1,N'Student',N'Student', N'Student', 'm8Z5BDpulCjK2xpmhs75UjkK29KS7htgHUwvDQ9tBayOoVfWwRPcRRkFhzz9gk8rG0YPCKsQA90bX0z746i+7A==','TJt4YAQC3Ls=', 1, GETDATE(),NULL),
		   (2,N'Instructor', N'Instructor', N'Instructor','jZUEB2PumY0SLRB/jeLRjYejWXHLLfAc4vfFi8OGsBe/Gi4o8q/iyw+G+sx5yaJLRD8xcI0Pih1tMKn0MmtGeg==','WFPBuGkPcfc=', 2,GETDATE(),NULL),
		   (3,N'Admin',N'Admin',N'Admin','qtIfeL3/WOFxu+yUXltw1GTztjvns25a2b4lyYfvF0KsGYb6HCZvqs5vp4yV8Pbq+RiZflRwf2ROCUgEwzkXGg==','8CdaBgGjjGk=',4,GETDATE(),NULL),
		   (6,N'George',N'Washington',N'gw','N+dXH8U4Vc9ZIEjwAdX5PoBDoKLsZl/Qm2cERN1gq2G5YyZBDYKdQYq008LcHZtacYm8byWojqtYhKR0W2BKUw==','NTvvX3sBBt0=',1,GETDATE(), NULL),
		   (7,N'John',N'Adams',N'ja','eGczNcLqmZG613KtKxpkqCMsC7aQ0ZDnsBqwALmHu4KLwSKApLXs3n8a72trcohnXalRWMi6NBshH3RxB2fcPA==','ut93ixtzN+4=',1,GETDATE(), NULL),
		   (9,N'Thomas',N'Jefferson',N'tj','P78YS/rAYLNZBGFjJy7iDQrAknkQX21i4h2a3mwr3oojoHZxYzl1UiZ7q8Cn99xXVRTYjQPbQqEsBoYYEYbkOw==','FlMiIbHw1yQ=',1,GETDATE(), NULL),
		   (10,N'James',N'Madison',N'jm','39DLX27Cn3+KBNFpm8da0hCiNKU5vLkW3a350L/neWuOV0QPqOw+xxP8cp5wqHnj1fh7+fLOehZHwdb6PxmLzw==','6nlBBLSFak4=',1,GETDATE(), NULL),
		   (11,N'James',N'Monroe',N'jmonroe','LKtpU3/UGsQbkxR4O69bBahTdNq1o5v5CBZf/Nk/9XHMi3a1K5FCyX9Y2oKCSjzi1B1nU+ebGmlgE89e5FUpqg==','lJxC596qmZg=',1,GETDATE(), NULL)

/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
DELETE FROM DBO.M_QUIZES;
INSERT INTO DBO.M_QUIZES
([QUIZ_ID],[QUIZ_NAME],[AUTHOR_ID]      ,[CREATED_DATE]      ,[SUCCESS_RATE])
VALUES
(6, N'C#', 2,'2019-03-12 20:35:06.910',0.7),
(7, N'C# part 2', 2,'2019-03-12 20:38:06.910',0.7),
(8, N'.NET', 2,'2019-03-12 20:38:06.910',0.5)



/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
DELETE FROM DBO.M_QUESTIONS;
  INSERT INTO [DBO].[M_QUESTIONS]
(QUESTION_ID, QUIZ_ID, info, text,CORRECT_OPTION_FLAG)
values
(2,	6,	N'Вопрос 1',	N'Какой тип переменной используется в коде: int a = 5',	8),
(3,	6,	N'Вопрос 2',	N'Что делает оператор «%»',	1),
(4,	6,	N'Вопрос 3',	N'Что сделает программа выполнив следующий код: Console.WriteLine(«Hello, World!»)',	8),
(5,	6,	N'Вопрос 4',	N'Как сделать инкремент числа?',	1),
(6,	6,	N'Вопрос 5',	N'Как сделать декремент числа?',	1),
(7,	6,	N'Вопрос 6',	N'Как найти квадратный корень из числа x ?',	8),
(8,	6,	N'Вопрос 7',	N'Обозначения оператора «НЕ»',	1),
(9,	6,	N'Вопрос 8',	N'Обозначение оператора «ИЛИ»',	8),
(10, 6,	N'Вопрос 9',	N'Обозначение оператора «И»',	2),
(11, 6,	N'Вопрос 10',	N'Чему будет равен с, если int a = 10; int b = 4; int c = a % b', 1),

(12,7,N'Вопрос 1', N'Чему равен d, если int a = 0; int b = a++; int c = 0; int d = a + b + c + 3', 1),
(13,7,N'Вопрос 2', N'Для чего нужны условные операторы', 1),
(14,7,N'Вопрос 3', N'Что вернет функция Termin после выполения. Код:int Termin(){int a = 1;int b = 3;if (a != 5) return a + b;else return 0;}', 8),
(15,7,N'Вопрос 4', N'Как называется оператор «?:»', 8),
(16,7,N'Вопрос 5', N'Что такое массив', 4),
(17,7,N'Вопрос 6', N'Какие бывают массивы ?', 8),
(18,7,N'Вопрос 7', N'Что такое цикл и для чего они нужны', 8),
(19,7,N'Вопрос 8', N'Какие бывают циклы?', 1),
(20,7,N'Вопрос 9', N'Какой оператор возвращает значение из метода ?', 1),
(21,7,N'Вопрос 10', N'Что такое константа ?', 1),

(22,8,N'Вопрос 1', N'Когда вызываются статические конструкторы классов в C#?', 1),
(23,8,N'Вопрос 2', N'Каким образом можно перехватить добавление и удаление делегата из события?', 8),
(24,8,N'Вопрос 3', N'Что произойдет при исполнении следующего кода? int i = 5; object o = i; long j = (long)o;', 1),
(25,8,N'Вопрос 4', N'Выберите средство, которое предоставляет C# для условной компиляции', 1),
(26,8,N'Вопрос 5', N'Выберите правильный вариант, в которых пространство имен System содержит пространство имен Customizer', 2),
(27,8,N'Вопрос 6', N'Чтобы использовать unsafe код в приложении, необходимо …', 2),
(28,8,N'Вопрос 7', N'Реализацией какого паттерна (шаблона проектирования) являются события в C#?', 8),
(29,8,N'Вопрос 8', N'Чем отличаются константы и доступные только для чтения поля?', 1),
(30,8,N'Вопрос 9', N'Элемент, который нельзя пометить атрибутом', 8),
(31,8,N'Вопрос 10', N'Как называется технология, благодаря которой возможно взаимодействие управляемого кода (managed code) с Win32 API функциями и COM-объектами?', 1)


/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
DELETE FROM DBO.M_VARIANTS;
 insert into [DBO].[M_VARIANTS]
  ([VARIANT_ID]      ,[QUESTION_ID]       ,[TEXT])
VALUES 

(2,	2,	N'"1 байт"'),
(3,	2,	N'"Знаковое 64-бит целое"'),
(4,	2,	N'"Знаковое 8-бит целое"'),
(5,	2,	N'"Знаковое 32-бит целое"'),
(6,	3,	N'"Возвращает остаток от деления"'),
(7,	3,	N'"Возвращает процент от суммы"'),
(8,	3,	N'"Возвращает тригонометрическую функцию"'),
(9,	3,	N'"Ничего из выше перечисленного"'),
(10,	4,	N'"Напишет на новой строчке Hello, World!"'),
(11,	4,	N'"Вырежет слово Hello, World! из всего текста"'),
(12,	4,	N'"Удалит все значения с Hello, World!"'),
(13,	4,	N'"Напишет Hello, World!"'),
(14,	5,	N'"++"'),
(15,	5,	N'"!="'),
(16,	5,	N'"—"'),
(17,	5,	N'"%%"'),
(18,	6,	N'"—"'),
(19,	6,	N'"%%"'),
(20,	6,	N'"!="'),
(21,	6,	N'"++"'),
(22,	7,	N'"Sqrt(x)"'),
(23,	7,	N'"Arifmetic.sqrt"'),
(24,	7,	N'"Summ.Koren(x)"'),
(25,	7,	N'"Math.Sqrt(x)"'),
(26,	8,	N'"!"'),
(27,	8,	N'"No"'),
(28,	8,	N'"!="'),
(29,	8,	N'"Not"'),
(30,	9,	N'"Or"'),
(31,	9,	N'"!="'),
(32,	9,	N'"!"'),
(33,	9,	N'"||"'),
(34,	10,	N'"Все выше перечисленные"'),
(35,	10,	N'"&&"'),
(36,	10,	N'"AND"'),
(37,	10,	N'"&"'),
(38,	11,	N'"2"'),
(39,	11,	N'"11"'),
(40,	11,	N'"1"'),
(41,	11,	N'"3"'),

(42,	12,	N'"4"'),
(43,	12,	N'"False"'),
(44,	12,	N'"True"'),
(45,	12,	N'"3"'),

(46,	13,	N'"Для ветвления программы"'),
(47,	13,	N'"Чтобы были"'),
(48,	13,	N'"Чтобы устанавливать условия пользователю"'),
(49,	13,	N'"Для оптимизации программы"'),

(50,	14,	N'"0"'),
(51,	14,	N'"3"'),
(52,	14,	N'"5"'),
(53,	14,	N'"4"'),

(54,	15,	N'"Территориальный оператор"'),
(55,	15,	N'"Прямой оператор"'),
(56,	15,	N'"Вопросительный"'),
(57,	15,	N'"Тернарный оператор"'),

(58,	16,	N'"Переменная"'),
(59,	16,	N'"Набор текстовых значений в формате Unicode, которые расположены в случайном порядке"'),
(60,	16,	N'"Набор однотипных данных, которые располагаются в памяти последовательно друг за другом"'),
(61,	16,	N'"Набор данных типа int (32-бит целое)"'),

(62,	17,	N'"статичные"'),
(63,	17,	N'"Разнообразные"'),
(64,	17,	N'"Связные"'),
(65,	17,	N'"Одномерные и многомерные"'),

(66,	18,	N'"Циклы нужны чтобы выполнить код без ошибок"'),
(67,	18,	N'"Циклы нужны для многократного размещения данных"'),
(68,	18,	N'"Циклы нужны для многократного запуска программы"'),
(69,	18,	N'"Циклы нужны для многократного выполнения кода"'),

(70,	19,	N'"for, while, do-while, foreach"'),
(71,	19,	N'"ref, out, static, root"'),
(72,	19,	N'"Большие и маленькие"'),
(73,	19,	N'"Цикл, Форич, Двойной цикл, Многократный"'),

(74,	20,	N'"return"'),
(75,	20,	N'"end"'),
(76,	20,	N'"out"'),
(77,	20,	N'"back"'),

(78,	21,	N'"Переменная значение которой нельзя изменить"'),
(79,	21,	N'"Глобальная переменная"'),
(80,	21,	N'"Переменная которая может быть изменена в любое время"'),
(81,	21,	N'"Переменная типа string"'),

(83,	22,	N'"Один раз при первом создании экземпляра класса или при первом обращении к статическим членам класса"'),
(84,	22,	N'"После каждого обращения к статическим полям, методам и свойствам"'),
(85,	22,	N'"Статических конструкторов в C# нет"'),
(86,	22,	N'"Строгий порядок вызова не определен"'),

(87,	23,	N'"Переопределить операторы + и – для делегата"'),
(88,	23,	N'"Использовать ключевые слова get и set"'),
(89,	23,	N'"Для этого существуют специальные ключевые слова add и remove"'),
(90,	23,	N'"Такая возможность не предусмотрена"'),

(91,	24,	N'"Ошибок не произойдет. Переменная j будет иметь значение 5"'),
(92,	24,	N'"Произойдет ошибка времени компиляции"'),
(93,	24,	N'"Значение переменной j предсказать нельзя"'),
(94,	24,	N'Произойдет исключение'),

(95,	25,	N'"Директива #if"'),
(96,	25,	N'"Директива #typedef"'),
(97,	25,	N'"Директива #elseif"'),
(98,	25,	N'"Директива #switch"'),

(99,	26,	N'"namespace System { namespace Customizer { } }"'),
(100,	26,	N'"namespace System { namespace Customizer { } }"'),
(101,	26,	N'"Нельзя создавать собственные пространства имен в пространстве имен System"'),

(102,	27,	N'"Пометить методы, где используется небезопасный код с помощью ключевого слова fixed"'),
(103,	27,	N'"Пометить методы, где используется небезопасный код с помощью ключевого слова unsafe"'),
(104,	27,	N'"Декоратор (Decorator)"'),

(105,	28,	N'"Декоратор (Decorator)"'),
(106,	28,	N'"Посетитель (Visitor)"'),
(107,	28,	N'"Шаблонный метод (Template Method)"'),
(108,	28,	N'"Издатель-подписчик (Publisher-Subscriber)"'),

(109,	29,	N'"Константы инициализируются во время компиляции, доступные только для чтения поля — во время выполнения"'),
(110,	29,	N'"Константы можно изменять, а доступные только для чтения поля нет"'),
(111,	29,	N'"Ничем не отличаются"'),
(112,	29,	N'"Доступные только для чтения поля инициализируются во время компиляции, константы — во время выполнения"'),
(113,	30,	N'"Структуры"'),
(114,	30,	N'"Методы"'),
(115,	30,	N'"Классы"'),
(116,	30,	N'"Все перечисленное можно пометить атрибутом"'),

(117,	31,	N'"CodeDOM"'),
(118,	31,	N'"WebServices"'),
(119,	31,	N'"Interop"'),
(120,	31,	N'"Remoting"')

/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
INSERT INTO [DBO].[M_QUIZ_RESULTS]
  ([QUIZ_RESULT_ID],USER_ID, QUIZ_ID,ASSIGNED_BY_ID,ASSIGNED_DATE, COMPLETED_DATE, QUIZ_STATUS,COMPLETED_RATE)
  VALUES
  (1,1,6,2,'2019-03-13 10:00:00', null, 1,null),
  (2,1,7,2,'2019-03-13 10:00:00', null, 1,null)




GO

GO
DECLARE @VarDecimalSupported AS BIT;

SELECT @VarDecimalSupported = 0;

IF ((ServerProperty(N'EngineEdition') = 3)
    AND (((@@microsoftversion / power(2, 24) = 9)
          AND (@@microsoftversion & 0xffff >= 3024))
         OR ((@@microsoftversion / power(2, 24) = 10)
             AND (@@microsoftversion & 0xffff >= 1600))))
    SELECT @VarDecimalSupported = 1;

IF (@VarDecimalSupported > 0)
    BEGIN
        EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
    END


GO
PRINT N'Обновление завершено.';


GO
