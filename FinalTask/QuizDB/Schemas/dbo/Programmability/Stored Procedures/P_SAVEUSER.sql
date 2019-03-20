
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
	@SALT nvarchar(100),
	@ROLESFLAG int = 1,
	@LastLogonDate nvarchar(50),
	@ERROR int out,
	@ERRORTEXT nvarchar(100) out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @ERROR = 0;
	SET @ERRORTEXT = N'Ok';
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
				INSERT INTO dbo.M_USERS (USER_ID,FIRSTNAME,LASTNAME,USERNAME, HASHEDPASSWORD, SALT, ROLESFLAG,[REGISTRATION_DATE],[LAST_LOGON_DATE] )
					VALUES (@NEWID,@FIRSTNAME,@LASTNAME,@USERNAME,@HASHEDPASSWORD,@SALT,@ROLESFLAG, GETDATE(), NULL)
			END
		END
		ELSE
		BEGIN
		/* Update existing user info */
			IF NOT EXISTS(SELECT * FROM dbo.M_USERS WHERE USER_ID = @USERID)
			/* DB consistency error */
				SELECT @ERROR = -1,	@ERRORTEXT = N'No Username '+@USERNAME + N' in M_USERS table';
			ELSE
			BEGIN
			/* change username of existing user. Is new username free ?*/
				IF EXISTS(SELECT * FROM dbo.M_USERS WHERE USERNAME = @USERNAME AND USER_ID != @USERID )
					SELECT @ERROR = -1,	@ERRORTEXT = N'Can''t change Username to '+@USERNAME + N' in M_USERS table, this username already exists';
				ELSE
					UPDATE dbo.M_USERS 
					SET USERNAME = @USERNAME,
						FIRSTNAME = @FIRSTNAME,
						LASTNAME = @LASTNAME,
						HASHEDPASSWORD = @HASHEDPASSWORD,
						ROLESFLAG = @ROLESFLAG
					WHERE USER_ID = @USERID
			END
		END
		IF (LEN(@LastLogonDate) >0)
		BEGIN
			UPDATE dbo.M_USERS 
			SET LAST_LOGON_DATE = CAST(@LastLogonDate as datetime)
			WHERE USER_ID  = @USERID
		END
	END TRY
	BEGIN CATCH
		SELECT @ERROR = ERROR_NUMBER(), @ERRORTEXT = ERROR_MESSAGE();
	END CATCH
END
