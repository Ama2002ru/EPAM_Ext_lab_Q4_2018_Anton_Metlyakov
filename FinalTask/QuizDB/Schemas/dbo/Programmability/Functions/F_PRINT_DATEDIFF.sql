CREATE FUNCTION [DBO].[F_PRINT_DATEDIFF]
(
	@D1 datetime,
	@D2 datetime
)
RETURNS nvarchar(8)
AS
BEGIN
	DECLARE @seconds int = DATEDIFF(second,@D1,@D2)
	DECLARE @hours int, @minutes int , @sec int
	SELECT @hours = @seconds/3600 , 
	 @minutes = (@seconds - ( @seconds/3600) * 3600) / 60 ,
	 @sec = (@seconds - ((@seconds/3600) * 3600 + ((@seconds - ( @seconds/3600) * 3600)/60)* 60))

	RETURN RIGHT('00'+CONVERT(varchar(2), @hours),2)+ ':'+
		RIGHT('00'+ CONVERT(varchar(2), @minutes),2)+':'+
		RIGHT('00' + CONVERT(varchar(2), @sec),2)
END
