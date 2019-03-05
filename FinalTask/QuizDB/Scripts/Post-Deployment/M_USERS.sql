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
INSERT INTO [dbo].[M_USERS]
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
           (1,'Student','Student', 'Student', 'm8Z5BDpulCjK2xpmhs75UjkK29KS7htgHUwvDQ9tBayOoVfWwRPcRRkFhzz9gk8rG0YPCKsQA90bX0z746i+7A==','TJt4YAQC3Ls=', 1, '2019-03-02 18:47:10.780',NULL),
		   (2,'Instructor', 'Instructor', 'Instructor','jZUEB2PumY0SLRB/jeLRjYejWXHLLfAc4vfFi8OGsBe/Gi4o8q/iyw+G+sx5yaJLRD8xcI0Pih1tMKn0MmtGeg==','WFPBuGkPcfc=', 2,'2019-03-02 18:49:55.883',NULL),
		   (3,'Admin','Admin','Admin','qtIfeL3/WOFxu+yUXltw1GTztjvns25a2b4lyYfvF0KsGYb6HCZvqs5vp4yV8Pbq+RiZflRwf2ROCUgEwzkXGg==','8CdaBgGjjGk=',4,'2019-03-02 18:49:55.883',NULL)
		   

