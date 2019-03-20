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
DELETE FROM dbo.M_QUIZES;
INSERT INTO dbo.M_QUIZES
([QUIZ_ID],[QUIZ_NAME],[AUTHOR_ID]      ,[CREATED_DATE]      ,[SUCCESS_RATE])
VALUES
(6, N'C#', 2,'2019-03-12 20:35:06.910',0.7),
(7, N'C# part 2', 2,'2019-03-12 20:38:06.910',0.7),
(8, N'.NET', 2,'2019-03-12 20:38:06.910',0.5)


