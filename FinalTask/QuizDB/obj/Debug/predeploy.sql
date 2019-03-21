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

IF OBJECT_ID('dbo.M_QUIZ_ANSWERS', 'U') IS NOT NULL
	DELETE FROM dbo.M_QUIZ_ANSWERS;
GO
IF OBJECT_ID('dbo.M_QUIZ_RESULTS', 'U') IS NOT NULL
	DELETE FROM dbo.M_QUIZ_RESULTS;
GO
IF OBJECT_ID('dbo.M_VARIANTS', 'U') IS NOT NULL
	DELETE FROM dbo.M_VARIANTS;
GO
IF OBJECT_ID('dbo.M_QUESTIONS', 'U') IS NOT NULL
	DELETE FROM dbo.M_QUESTIONS;
GO
IF OBJECT_ID('dbo.M_QUIZES', 'U') IS NOT NULL
	DELETE FROM dbo.M_QUIZES;
GO
IF OBJECT_ID('dbo.M_USERS', 'U') IS NOT NULL
	DELETE FROM dbo.M_USERS;
GO
IF OBJECT_ID('dbo.M_ROLES', 'U') IS NOT NULL
	DELETE FROM dbo.M_ROLES;
GO
IF OBJECT_ID('dbo.S_PK_GENERATOR', 'U') IS NOT NULL
	DELETE FROM dbo.S_PK_GENERATOR;
GO

GO
