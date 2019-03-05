-- =============================================
-- Author:		Anton Metlyakov
-- Create date: 02/17/2019
-- Description:	Saves log record to table T_LOG 
-- Input : log info fields
-- return : @error number, if any
-- =============================================
CREATE PROCEDURE [dbo].[P_RECORDLOG]
	@log_date datetime,
	@thread varchar(255), 
	@log_level varchar(50),
	@logger varchar(255),
	@message nvarchar(4000), 
	@exception nvarchar(2000) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
				DECLARE @NEWID NUMERIC;
				EXECUTE dbo.P_GETNEXTPK @TABLE_NAME = 'T_LOG', @ID = @NEWID OUT
				INSERT INTO dbo.T_LOG 
					([Id],[Date],[Thread],[Level],[Logger],[Message],[Exception])
					VALUES (@NEWID, @log_date, @thread, @log_level, @logger, @message, @exception)	
	RETURN 0
END
