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
