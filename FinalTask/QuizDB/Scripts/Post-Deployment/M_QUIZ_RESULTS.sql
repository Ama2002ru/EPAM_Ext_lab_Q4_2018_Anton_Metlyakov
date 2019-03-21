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
INSERT INTO [DBO].[M_QUIZ_RESULTS]
  ([QUIZ_RESULT_ID],USER_ID, QUIZ_ID,ASSIGNED_BY_ID,ASSIGNED_DATE, COMPLETED_DATE, QUIZ_STATUS,COMPLETED_RATE)
  VALUES
  (1,1,6,2,'2019-03-13 10:00:00', null, 1,null),
  (2,1,7,2,'2019-03-13 10:00:00', null, 1,null)