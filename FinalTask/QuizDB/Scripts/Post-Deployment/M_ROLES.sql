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
DELETE FROM dbo.[M_ROLES];
INSERT INTO dbo.[M_ROLES]
([ROLE_ID], [ROLE_NAME], [ROLE_FLAG], [ALLOWED_METHODS])
VALUES
(1,N'Student',1,'QuizClass.Show;'),
(2,N'Instructor',2,'PersonRepository.Add;'),
(3,N'Admin',4,'')
GO