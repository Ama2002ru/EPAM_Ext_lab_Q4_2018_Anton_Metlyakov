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
DELETE FROM [dbo].[M_COURSES];
GO

INSERT INTO  [dbo].[M_COURSES] ([COURSE_ID],[COURSE_NAME])
VALUES 
	(1,N'Английский язык'),
	(2,N'Русский язык'),
	(3,N'C# & Dot.NET')


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
DELETE FROM [dbo].[M_USERS]
GO
INSERT INTO [dbo].[M_USERS]
           ([USER_ID]
           ,[FIRSTNAME]
           ,[LASTNAME]
           ,[USERNAME]
           ,[HASHEDPASSWORD]
           ,[ROLESFLAG]
	   ,[REGISTRATION_DATE]
	   ,[LAST_LOGON_DATE])
     VALUES
           (1,'John','Doe', 'Jdoe', '123', 7, '1900-01-01',NULL),
		   (2,'Nikolay', 'Piskarev', 'np','123',3,'1900-01-01',NULL),
		   (3,'Igor','Kalugin','ki','123',7,'1900-01-01',NULL),
		   (4,'Barak','Obama','bo','123',1,'1900-01-01',NULL)


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
GO
INSERT INTO [dbo].[S_PK_GENERATOR] ([TABLE_NAME],[TABLE_ID]) 
VALUES 
	('M_USERS',5),
	('M_QUIZES',1),
	('M_QUESTIONS',1),
	('M_VARIANTS',1),
	('M_COURSES',4)
GO


GO
