﻿
-- =============================================
-- Author:		Anton Metlyakov
-- Create date: 02/12/2019
-- Description:	Saves user to table M_USERS (new (id=-1)/update (id>=0))
-- Input : user info fields
-- return : @error number, if any
-- =============================================
CREATE PROCEDURE [dbo].[P_SAVEUSER]
	@USERID int=-1,
	@USERNAME nvarchar(100),
	@FIRSTNAME nvarchar(100),
	@LASTNAME nvarchar(100),
	@HASHEDPASSWORD nvarchar(100),
	@ROLESFLAG int = 1,
	@ERROR int out,
	@ERRORTEXT nvarchar(100) out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = -10;
	SET @ERRORTEXT = N'тест возвращаемого значения';
	BEGIN TRY
		IF (@USERID = -1) 
		BEGIN
		/* new user */
			IF EXISTS(SELECT * FROM dbo.M_USERS WHERE USERNAME = @USERNAME)
				SELECT @ERROR = -1,	@ERRORTEXT = N'Dublicate Username '+@USERNAME + N' in M_USERS table';
			ELSE
			BEGIN
				DECLARE @NEWID NUMERIC;
				EXECUTE dbo.P_GETNEXTPK @TABLE_NAME = 'M_USERS', @ID = @NEWID OUT
				INSERT INTO dbo.M_USERS (USER_ID,FIRSTNAME,LASTNAME,USERNAME, HASHEDPASSWORD, ROLESFLAG,[REGISTRATION_DATE],[LAST_LOGON_DATE] )
					VALUES (@NEWID,@FIRSTNAME,@LASTNAME,@USERNAME,@HASHEDPASSWORD,@ROLESFLAG, GETDATE(), NULL)
			END
		END
		ELSE
		BEGIN
		/* Update existing user info */
			IF NOT EXISTS(SELECT * FROM dbo.M_USERS WHERE USERNAME = @USERNAME)
				SELECT @ERROR = -1,	@ERRORTEXT = N'No Username '+@USERNAME + N' in M_USERS table';
			ELSE
			BEGIN
				UPDATE dbo.M_USERS 
				SET FIRSTNAME = @FIRSTNAME,
					LASTNAME = @LASTNAME,
					HASHEDPASSWORD = @HASHEDPASSWORD,
					ROLESFLAG = @ROLESFLAG
				WHERE USER_ID = @USERID
			END
		END
	END TRY
	BEGIN CATCH
		SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
	END CATCH
END
