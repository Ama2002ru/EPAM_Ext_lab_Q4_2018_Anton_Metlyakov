-- =============================================
-- Author:		Anton Metlyakov
-- Create date: 02/12/2019
-- Description:	RETURNS USER LIST
-- Input : @RecordsCount, -1 - no limit
-- returns : table
-- =============================================
CREATE PROCEDURE [dbo].[P_GETALLUSERS]
@RecordsCount int = 2147483647 /* max INT value*/
AS
SET NOCOUNT ON
BEGIN												
	/* treating all negatives as max INT */
	DECLARE @InternalRecordsCount int = CASE WHEN @RecordsCount<0 THEN 2147483647 ELSE @RecordsCount END
	SELECT	TOP (@InternalRecordsCount)  
		[USER_ID], 
		[FIRSTNAME], 
		[LASTNAME], 
		[USERNAME], 
		[HASHEDPASSWORD], 
		[ROLESFLAG], 
		[REGISTRATION_DATE], 
		[LAST_LOGON_DATE]
	FROM  dbo.V_M_USERS
END