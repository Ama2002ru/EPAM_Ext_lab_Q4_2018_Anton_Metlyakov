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
           (1,N'Student',N'Student', N'Student', 'm8Z5BDpulCjK2xpmhs75UjkK29KS7htgHUwvDQ9tBayOoVfWwRPcRRkFhzz9gk8rG0YPCKsQA90bX0z746i+7A==','TJt4YAQC3Ls=', 1, GETDATE(),NULL),
		   (2,N'Instructor', N'Instructor', N'Instructor','jZUEB2PumY0SLRB/jeLRjYejWXHLLfAc4vfFi8OGsBe/Gi4o8q/iyw+G+sx5yaJLRD8xcI0Pih1tMKn0MmtGeg==','WFPBuGkPcfc=', 2,GETDATE(),NULL),
		   (3,N'Admin',N'Admin',N'Admin','qtIfeL3/WOFxu+yUXltw1GTztjvns25a2b4lyYfvF0KsGYb6HCZvqs5vp4yV8Pbq+RiZflRwf2ROCUgEwzkXGg==','8CdaBgGjjGk=',4,GETDATE(),NULL),
		   (6,N'George',N'Washington',N'gw','N+dXH8U4Vc9ZIEjwAdX5PoBDoKLsZl/Qm2cERN1gq2G5YyZBDYKdQYq008LcHZtacYm8byWojqtYhKR0W2BKUw==','NTvvX3sBBt0=',1,GETDATE(), NULL),
		   (7,N'John',N'Adams',N'ja','eGczNcLqmZG613KtKxpkqCMsC7aQ0ZDnsBqwALmHu4KLwSKApLXs3n8a72trcohnXalRWMi6NBshH3RxB2fcPA==','ut93ixtzN+4=',1,GETDATE(), NULL),
		   (9,N'Thomas',N'Jefferson',N'tj','P78YS/rAYLNZBGFjJy7iDQrAknkQX21i4h2a3mwr3oojoHZxYzl1UiZ7q8Cn99xXVRTYjQPbQqEsBoYYEYbkOw==','FlMiIbHw1yQ=',1,GETDATE(), NULL),
		   (10,N'James',N'Madison',N'jm','39DLX27Cn3+KBNFpm8da0hCiNKU5vLkW3a350L/neWuOV0QPqOw+xxP8cp5wqHnj1fh7+fLOehZHwdb6PxmLzw==','6nlBBLSFak4=',1,GETDATE(), NULL),
		   (11,N'James',N'Monroe',N'jmonroe','LKtpU3/UGsQbkxR4O69bBahTdNq1o5v5CBZf/Nk/9XHMi3a1K5FCyX9Y2oKCSjzi1B1nU+ebGmlgE89e5FUpqg==','lJxC596qmZg=',1,GETDATE(), NULL)
