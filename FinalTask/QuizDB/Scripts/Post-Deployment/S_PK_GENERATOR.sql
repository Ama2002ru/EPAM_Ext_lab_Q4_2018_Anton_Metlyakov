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

DELETE FROM  [dbo].[S_PK_GENERATOR]

INSERT INTO [dbo].[S_PK_GENERATOR] ([TABLE_NAME],[TABLE_ID]) 
VALUES 
	('M_ROLES',4),
	('M_USERS',5),
	('M_QUIZES',1),
	('M_QUESTIONS',1),
	('M_VARIANTS',1),
	('M_COURSES',4),
	('T_LOG',1)
GO

