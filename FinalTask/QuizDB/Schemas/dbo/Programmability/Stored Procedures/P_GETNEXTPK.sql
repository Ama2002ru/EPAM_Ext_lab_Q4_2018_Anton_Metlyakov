
-- =============================================
-- Author:		Anton Metlyakov
-- Create date: 02/12/2019
-- Description:	Generates new identity number for given table name
-- Input : table name
-- returns : @ID int - next counter value
-- =============================================
CREATE PROCEDURE [dbo].[P_GETNEXTPK]
@TABLE_NAME VARCHAR(30),
 @ID NUMERIC OUT
AS
SET NOCOUNT ON
BEGIN
	IF NOT EXISTS (SELECT * FROM  [dbo].[S_PK_GENERATOR]  WHERE TABLE_NAME = @TABLE_NAME) 
		RAISERROR('Please add record for ''%s'' in PK generator table', 16, 1, @TABLE_NAME)
	 UPDATE [dbo].[S_PK_GENERATOR]  
	 SET TABLE_ID = TABLE_ID + 1,@ID = TABLE_ID + 1   
	 WHERE TABLE_NAME = @TABLE_NAME;
END