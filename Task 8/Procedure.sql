USE [Northwind]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.SF_GetBiggestOrderID', 'FN') IS NOT NULL
  DROP FUNCTION dbo.SF_GetBiggestOrderID
GO

-- =============================================
-- Author:		Anton Metlyakov
-- Create date: feb 09 2019
-- Description:	Считает суммы заказов за год 
-- по конкретному продавцу и возвращает ID заказа c максимальной суммой
-- =============================================
CREATE FUNCTION [dbo].[SF_GetBiggestOrderID]
(
	-- Add the parameters for the function here
	@employee_id int = 0,
	@report_year int = 1998
)
RETURNS int 
AS
BEGIN

	DECLARE @OrderID int = 0;
	DECLARE @sum money = 0;
	SELECT TOP 1 
		@OrderID = O.OrderID,
		@sum = SUM(OD.Quantity * OD.UnitPrice * (1 - OD.Discount) )
	FROM 
		dbo.Orders O
	JOIN dbo.[Order Details] OD 
		ON O.OrderID = OD.OrderID
	WHERE 
		O.EmployeeID = @employee_id AND
		/*Проще было бы использовать DATEPART(year, O.OrderDate) = @report_year 
		 но это исключит возможность использования индекса по O.OrderDate,
		 если таковой будет построен */
		O.OrderDate >= CAST(@report_year as nvarchar(10))+ '0101' AND
		O.OrderDate < CAST(@report_year+1 as nvarchar(10))+ '0101' 
		
	GROUP BY 
		O.OrderID
	ORDER BY 
		SUM(OD.Quantity * OD.UnitPrice * (1 - OD.Discount)) DESC
	RETURN @OrderID

END
GO



IF OBJECT_ID('dbo.SF_IsBoss', 'FN') IS NOT NULL
  DROP FUNCTION dbo.SF_IsBoss
GO

-- =============================================
-- Author:		Anton Metlyakov
-- Create date: feb 09 2019
-- Description: Функция, определяет, есть ли у продавца подчиненные. Возвращает тип данных BIT.
--				В качестве входного параметра функции используется EmployeeID. 
-- =============================================
CREATE FUNCTION [dbo].[SF_IsBoss]
(
	@EmployeeID int = 0
)
RETURNS Bit
AS
BEGIN

	-- Declare the return variable here
	DECLARE @isboss bit = 0;
	IF (EXISTS(SELECT * 
				FROM dbo.Employees E 
					WHERE E.ReportsTo = @EmployeeID))
	BEGIN
		SET @isboss = 1;
	END
	-- Return the result of the function
	RETURN @isboss;

END
GO



IF OBJECT_ID('dbo.SF_GetEmployeeByManager', 'TF') IS NOT NULL
  DROP FUNCTION dbo.SF_GetEmployeeByManager
GO

-- =============================================
-- Author:		Anton Metlyakov
-- Create date: feb 09 2019
-- Description:	РЕкурсивная TVF, возвращает текущего работника согласно отступа @level 
--              + рекурсивно вызывается для каждого его подчиненного (если есть).
-- =============================================
CREATE FUNCTION [dbo].[SF_GetEmployeeByManager]
(
	@manager int = 2,
	@level int = 0
)
RETURNS 
@OrderedEmployees TABLE
(
	ID int identity(1,1), /* для упорядочивания результата */
	EmployeeID int,
	SupervisorID int,
	Name nvarchar(200),
	Level int
)
AS
BEGIN

	INSERT INTO @OrderedEmployees (EmployeeID, SupervisorID,Name, Level)
		SELECT  E.EmployeeID, E.ReportsTo AS Supervisor,
				REPLICATE('      ',@level) + (ISNULL(E.LastName,'') + ' ' + ISNULL(E.Firstname,'')) AS Name ,
				@level as Level
		FROM dbo.Employees E
		WHERE E.EmployeeID = @manager
		UNION ALL
		SELECT E2.EmployeeID, E2.SupervisorID,E2.Name, E2.Level
			FROM dbo.Employees E1  
		CROSS APPLY 
			dbo.SF_GetEmployeeByManager(E1.EmployeeID, @level + 1) E2
		WHERE 
			E1.ReportsTo = @manager
	RETURN 

END
GO



IF OBJECT_ID('dbo.P_GreatestOrders', 'P') IS NOT NULL
  DROP PROCEDURE dbo.P_GreatestOrders
GO

-- =============================================
-- Author: Anton Metlyakov
-- Create date: feb 09 2019
-- Description:	
-- 13.1	Написать процедуру, которая возвращает самый крупный заказ для каждого из продавцов 
--      за определенный год. 
--	    Для улучшения читаемости вынес поиск самого крупного заказа в функцию SF_GetBiggestOrderID(E.EmployeeID, @report_year)
-- =============================================
CREATE PROCEDURE [dbo].[P_GreatestOrders] @report_year int = 1998, @records_to_show int = 100
AS
BEGIN

	DECLARE @Err nvarchar(max);
	DECLARE @ErrNumber int = 0;
	SET NOCOUNT ON;
	BEGIN TRY
		SELECT TOP (@records_to_show)
			ISNULL(E.FirstName, '') + ' '+ ISNULL(E.LastName, '') 
		as [User Name],
			dbo.SF_GetBiggestOrderID(E.EmployeeID, @report_year) 
		as OrderID,	
			ISNULL((SELECT CAST(SUM(OD.Quantity * OD.UnitPrice * (1 - OD.Discount)) as Decimal(18,2)) 
				FROM dbo.[Order Details] OD 
				WHERE OD.OrderID = dbo.SF_GetBiggestOrderID(E.EmployeeID, @report_year)
			), 0)
		as Value
		FROM 
			dbo.Employees E
		ORDER BY Value DESC
	END TRY
	/* Если что-то пошло не так (например мега-корпорация с 32 уровнями управления) */
	BEGIN CATCH
		SELECT @Err = 'Error is : ' + ERROR_MESSAGE(), @ErrNumber = ERROR_NUMBER()
		PRINT @Err
	END CATCH
	RETURN @ErrNumber

END
GO


IF OBJECT_ID('dbo.P_ShippedOrdersDiff', 'P') IS NOT NULL
  DROP PROCEDURE dbo.P_ShippedOrdersDiff
GO

-- =============================================
-- Author:		Anton Metlyakov
-- Create date: feb 09 2019
-- Description:	13.2	Написать процедуру, которая возвращает заказы в таблице Orders, 
--				согласно указанному сроку доставки в днях (разница между OrderDate и ShippedDate).  
--				В результатах должны быть возвращены заказы, срок которых превышает переданное значение или
--				еще недоставленные заказы. Значению по умолчанию для передаваемого срока 35 дней
--				Возвращаемые значения :
--				OrderID, OrderDate, ShippedDate, ShippedDelay (разность в днях между ShippedDate и OrderDate), SpecifiedDelay (переданное в процедуру значение).
--=============================================
CREATE PROCEDURE [dbo].[P_ShippedOrdersDiff]  
	@specified_delay int = 35
AS
BEGIN

	DECLARE @Err nvarchar(max);
	DECLARE @ErrNumber int = 0;
	SET NOCOUNT ON;
	BEGIN TRY
		SELECT 
			O.OrderID,
			CAST (O.OrderDate as date) as OrderDate,
			ISNULL(CAST(CAST(O.ShippedDate as date) as nvarchar(11)),'Not shipped') as ShippedDate,
			ISNULL (CAST (DATEDIFF (day, O.OrderDate, O.ShippedDate) as nvarchar(11)), 'Not shipped') as ShippedDelay,
			@specified_delay as SpecifiedDelay 
		FROM
			dbo.Orders O
		WHERE 
			DATEDIFF (day, O.OrderDate, O.ShippedDate) > @specified_delay OR
			O.ShippedDate IS NULL
	END TRY
	BEGIN CATCH
		SELECT @Err = 'Error is : ' + ERROR_MESSAGE(), @ErrNumber = ERROR_NUMBER()
		PRINT @Err
	END CATCH
	RETURN @ErrNumber

END
GO


IF OBJECT_ID('dbo.P_SubordinationInfo', 'P') IS NOT NULL
  DROP PROCEDURE dbo.P_SubordinationInfo
GO

-- =============================================
-- Author:		Anton Metlyakov
-- Create date: feb 09 2019
-- Description:	Процедура выводит список  работников,
--				выделяя подчнинение отступами.
--				параметр - @EmployeeID int - работник, с которого надо начинать список
--				Работает на рекурсивной CTE
-- =============================================
CREATE PROCEDURE [dbo].[P_SubordinationInfo] 
	@EmployeeID int = 0
AS
BEGIN

	DECLARE @Err nvarchar(max);
	DECLARE @ErrNumber int = 0;
	SET NOCOUNT ON;
	BEGIN TRY
		DECLARE @NAME nvarchar(100);
		DECLARE Emp_Hierarchy CURSOR FOR 
		WITH 
				EmployeeHierarhy AS /* Recursive CTE */
			(
				SELECT E.EmployeeID, E.LastName, E.FirstName,  0 as lvl,
				CAST(E.LastName+' '+E.FirstName+';' AS VARCHAR(MAX)) AS SortPath
				FROM dbo.Employees E
				WHERE E.ReportsTo IS NULL
			UNION All
				SELECT E.EmployeeID, E.LastName, E.FirstName, lvl+1,
				CAST(SortPath + E.LastName+' '+E.FirstName+';' AS VARCHAR(max))
				FROM EmployeeHierarhy EH
				JOIN dbo.Employees E ON E.ReportsTo = EH.EmployeeID
			)
			SELECT REPLICATE('    ',lvl) +LastName + ' '+FirstName AS SortedFullName
			FROM EmployeeHierarhy
			ORDER BY SortPath /* Сортировка списка через конкатенированные имена всех начальников */
		OPEN Emp_Hierarchy
		WHILE (1=1)
		BEGIN
			FETCH NEXT FROM Emp_Hierarchy INTO @Name
			IF (@@FETCH_STATUS <>0) BREAK /* Если данных больше нет*/
			PRINT @Name
		END
		CLOSE Emp_Hierarchy
		DEALLOCATE Emp_Hierarchy

	END TRY
	BEGIN CATCH
		SELECT @Err = 'Error is : ' + ERROR_MESSAGE(), @ErrNumber = ERROR_NUMBER()
		PRINT @Err
	END CATCH
	RETURN @ErrNumber

END



