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

DELETE FROM [dbo].[M_COURSES];
INSERT INTO  [dbo].[M_COURSES] ([COURSE_ID],[COURSE_NAME])
VALUES 
	(1,N'Английский язык'),
	(2,N'Русский язык'),
	(3,N'C# & Dot.NET')

