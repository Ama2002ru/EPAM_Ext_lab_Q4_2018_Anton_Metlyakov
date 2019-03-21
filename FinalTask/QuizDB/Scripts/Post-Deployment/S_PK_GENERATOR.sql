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

