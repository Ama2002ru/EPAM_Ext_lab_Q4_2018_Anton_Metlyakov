USE Northwind
Go
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--[ki. Accepted]
-- 1.1	Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (колонка ShippedDate)
-- включительно и которые доставлены с ShipVia >= 2.

SELECT 
    O.OrderID,
    O.ShippedDate,
    O.ShipVia
FROM 
    dbo.Orders O
WHERE 
/* Поля, где ShippedDate is Null не попадут в выходной набор потому, что 
WHERE отфильтрует только те строки, где условие = TRUE.
Сравнение NULL с датой 1998-05-06 дает UNKNOWN */
    O.ShippedDate >= CONVERT(DATETIME, '19980506', 101)   AND
    O.ShipVia >=2

-- 1.2	Написать запрос, который выводит только недоставленные заказы из таблицы Orders. 

--[ki. Accepted]
SELECT 
    O.OrderID,
--   'Not Shipped' as ShippedDate, /* Case КМК избыточен, но клиент всегда прав :) */ 
    CASE 
	WHEN O.ShippedDate IS NULL THEN N'Not Shipped'
	END as ShippedDate
FROM 
	dbo.Orders O
WHERE 
	O.ShippedDate IS NULL

--[ki. Accepted]
-- 1.3	Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (ShippedDate) не включая эту дату или  ...

SELECT 
	O.OrderID as [Order Number],
    CASE 
		WHEN O.ShippedDate IS NULL THEN N'Not Shipped'
		ELSE CAST(CAST(O.ShippedDate as date) as nvarchar(30))
	END	
	as [Shipped Date]
FROM
	dbo.Orders O
WHERE 
	O.ShippedDate > '19980601' OR
	O.ShippedDate IS NULL

-- [ki. Accepted]
-- 2.1	Выбрать из таблицы Customers всех заказчиков, проживающих в USA и Canada. 
-- Запрос сделать с только помощью оператора IN...

SELECT 
	C.ContactName, 
	C.Country
FROM 
	dbo.Customers C
WHERE
	C.Country IN (N'USA', N'Canada')
ORDER BY
	ContactName,
	Country,
	City

--[ki. Accepted]
--	2.2	Выбрать из таблицы Customers всех заказчиков, не проживающих в USA и Canada.
--  Запрос сделать с помощью оператора IN. 

SELECT 
	C.ContactName, 
	C.Country
FROM 
	dbo.Customers C
WHERE
	NOT (C.Country IN (N'USA', N'Canada'))
ORDER BY
	ContactName

--[ki. Accepted]
-- 2.3	Выбрать из таблицы Customers все страны, в которых проживают заказчики. 

SELECT DISTINCT
	C.Country
FROM 
	dbo.Customers C
ORDER BY
	Country DESC

--[ki not accepted Не выполненно условие "заказы не должны повторяться"]
-- 3.1	3.1	Выбрать все заказы (OrderID) из таблицы Order Details (заказы не должны повторяться),
-- где встречаются продукты с количеством от 3 до 10 включительно – это колонка Quantity в таблице
-- Order Details. Использовать оператор BETWEEN. Запрос должен высвечивать только колонку OrderID.

SELECT 
	OD.OrderID
FROM
	dbo.[Order Details] OD
WHERE 
	OD.Quantity BETWEEN 3 AND 10
go

--[ki. accepted]
-- 3.2	Выбрать всех заказчиков из таблицы Customers,
-- у которых название страны начинается на буквы из диапазона b и g. Использовать оператор BETWEEN. 
-- Проверить, что в результаты запроса попадает Germany.

SET showplan_all On;
go
SELECT 
	C.CustomerID,
	C.Country 
FROM 
	dbo.Customers C
WHERE 
	LEFT(C.Country,1) BETWEEN N'b' AND N'g' /* Predicate :substring([Northwind].[dbo].[Customers].[Country] as [C].[Country],(1),(1))>=N'b' AND substring([Northwind].[dbo].[Customers].[Country] as [C].[Country],(1),(1))<=N'g' */
ORDER BY 
	C.Country 
go
SET showplan_all Off;
go
  
  --Sort(ORDER BY:([C].[Country] ASC))
       --Clustered Index Scan(OBJECT:([Northwind].[dbo].[Customers].[PK_Customers] AS [C]), WHERE:(substring([Northwind].[dbo].[Customers].[Country] as [C].[Country],(1),(1))>=N'b' AND substring([Northwind].[dbo].[Customers].[Country] as [C].[Country],(1),(1))<=N'g'))

--[ki. accepted]
-- 3.3	Выбрать всех заказчиков из таблицы Customers, у которых название страны начинается на буквы 
-- из диапазона b и g, не используя оператор BETWEEN. С помощью опции “Execution Plan” определить какой 
-- запрос предпочтительнее 3.2 или 3.3 – для этого надо ввести в скрипт выполнение текстового Execution Plan-a 
-- для двух этих запросов, результаты выполнения Execution Plan надо ввести в скрипт в виде комментария 
-- и по их результатам дать ответ на вопрос – по какому параметру было проведено сравнение

/* Предикаты двух запросов соответственно :  
3.2.  substring([Northwind].[dbo].[Customers].[Country] as [C].[Country],(1),(1))>=N'b' AND substring([Northwind].[dbo].[Customers].[Country] as [C].[Country],(1),(1))<=N'g' 
3.3.  [Northwind].[dbo].[Customers].[Country] as [C].[Country] like N'[b-g]%'
Отличия в стоимости запроса - в шестом знаке после запятой.

Я бы сказал, что использовать BETWEEN для varchar не самая лучшая идея при заданных входных условиях.
при использовании LIKE оптимизатор сможет использовать индекс, если сочтёт нужным
[ki. никакой разницы в скорости выполнения запросов я не замеча, когда писал их - и там и там используется кластерный индекс]
 */

SET showplan_all On;
go
SELECT 
	C.CustomerID,
	C.Country 
FROM 
	dbo.Customers C
WHERE 
	C.Country like N'[b-g]%'  /*Predicate : [Northwind].[dbo].[Customers].[Country] as [C].[Country] like N'[b-g]%'*/
ORDER BY
	C.Country
go
SET showplan_all Off;
go

  --Sort(ORDER BY:([C].[Country] ASC))
     --Clustered Index Scan(OBJECT:([Northwind].[dbo].[Customers].[PK_Customers] AS [C]), WHERE:([Northwind].[dbo].[Customers].[Country] as [C].[Country] like N'[b-g]%'))


--[ki. accepted]
-- 4.1	В таблице Products найти все продукты (колонка ProductName), где встречается подстрока 'chocolade'
-- Известно, что в подстроке 'chocolade' может быть изменена одна буква 'c' в середине - найти все продукты,
-- которые удовлетворяют этому условию. Подсказка: результаты запроса должны высвечивать 2 строки.

SELECT 
	P.ProductName
FROM 
	dbo.Products P
WHERE
	P.ProductName like N'%cho_olade%'

--[ki. accepted]
--5.1	Найти общую сумму всех заказов из таблицы Order Details с учетом количества закупленных товаров 
-- и скидок по ним. Результат округлить до сотых и высветить в стиле 1 для типа данных money.  

DECLARE @money_style int = 1
SELECT 
	CONVERT (nvarchar(100), CAST(SUM (OD.UnitPrice * (1 - OD.Discount) * Quantity) as money), @money_style) as Totals
FROM dbo.[Order Details] OD

--[ki. accepted]
--5.2	По таблице Orders найти количество заказов, которые еще не были доставлены 
-- (т.е. в колонке ShippedDate нет значения даты доставки). Использовать при этом запросе 
-- только оператор COUNT. Не использовать предложения WHERE и GROUP.

SELECT 
	COUNT(*) - COUNT(O.ShippedDate)
FROM 
	dbo.Orders O

--[ki. accepted]
-- 5.3	По таблице Orders найти количество различных покупателей (CustomerID), сделавших заказы. 
-- Использовать функцию COUNT и не использовать предложения WHERE и GROUP.

SELECT 
	COUNT(DISTINCT O.CustomerID)
FROM 
	dbo.Orders O

--[ki. Accepted]
-- 6.1	По таблице Orders найти количество заказов с группировкой по годам. 
-- В результатах запроса надо высвечивать две колонки c названиями Year и Total. 
-- Написать проверочный запрос, который вычисляет количество всех заказов

SELECT
	DATEPART(year,O.OrderDate) as [Year],
	COUNT(*) as Total
FROM 
	dbo.Orders O
GROUP BY
/* CUBE для сравнения с проверочным запросом - сумма всех групп */
	CUBE(DATEPART(year,O.OrderDate))
ORDER BY
	[Year]

/* ПРоверочный запрос */
SELECT 
COUNT(*) as Total
FROM
	dbo.Orders

--[ki. Accepted]
-- 6.2	По таблице Orders найти количество заказов, cделанных каждым продавцом...
-- Заказ для указанного продавца – это любая запись в таблице Orders, где в колонке 
-- EmployeeID задано значение для данного продавца

/* OUTER JOIN конечно появляется только в п.8 Задания 8, но мне кажется правильнее строить
этот запрос на таблице Employees а не Orders */

SELECT
	ISNULL(E.LastName,'') + N' ' + ISNULL(E.FirstName,'') as Seller,
	COUNT(O.EmployeeID) as Amount
FROM
	dbo.Employees E 
LEFT JOIN 
	dbo.Orders O ON E.EmployeeID = O.EmployeeID
GROUP BY
	E.LastName,
	E.FirstName
ORDER BY
	Amount DESC


-- [ki. Скорее всего, здесь ожидалось использование подзапросов]
SELECT (SELECT LastName + ' ' + FirstName FROM dbo.[Employees] WHERE EmployeeID = O.EmployeeID) [Seller] , COUNT(*) [Amount] FROM dbo.[Orders] as O
GROUP BY EmployeeID
ORDER BY [Amount] desc;

GO

--[ki. Not Accepted "колонку с именем покупателя (название колонки ‘Customer’) " а ты выводишь ID]
-- 6.3	таблице Orders найти количество заказов, cделанных каждым продавцом и для каждого покупателя. 
-- Необходимо определить это только для заказов сделанных в 1998 году

DECLARE @requested_year_bottom  int = 1998
DECLARE @requested_year_top     int = 1998
SELECT  
		ISNULL(
		(SELECT ISNULL(E.LastName,'') + N' ' + ISNULL(E.FirstName,'')
			FROM dbo.Employees E 
				WHERE E.EmployeeID = O.EmployeeID), N'ALL')
	as Seller,
		ISNULL(O.CustomerID, N'ALL') 
	as Customer,
	SUM(1) as Amount
FROM 
	dbo.Orders O
WHERE 
	DATEPART(year, O.OrderDate) BETWEEN @requested_year_bottom AND @requested_year_top
GROUP BY
	CUBE (O.EmployeeID,O.CustomerID)
ORDER BY 
	CustomerID,
	EmployeeID,
	Amount DESC

--[ki. Not Accepted. Все-таки supplier - это поставщик, а продавец в данной БД - эот Employee]
--6.4	Найти покупателей и продавцов, которые живут в одном городе. 

/* как я понял задание, ищу города, в которых есть и покупатели и продавцы*/

-- ki. Правильный запрос не отличается по логике. Но... Платон мне друг, а истина дороже
SELECT c.ContactName AS Person, N'Customer' AS Type,c.City AS City
FROM dbo.[Customers] AS c
WHERE EXISTS (
              SELECT e.City 
              FROM dbo.[Employees] AS e
              WHERE e.City=c.City
              )
UNION ALL
SELECT e.FirstName+' '+e.LastName AS Person, N'Seller' AS Type,e.City AS City
FROM dbo.[Employees] AS e
WHERE EXISTS (
              SELECT c.City 
              FROM dbo.[Customers] AS c
              WHERE e.City=c.City
              )
ORDER BY City, Person;

GO
-- ki. Конец запроса

SELECT 
	C.ContactName as Person, 
	N'Customer' as [Type],
	C.City
FROM 
	dbo.Customers C
WHERE 
	EXISTS (SELECT * FROM dbo.Suppliers S WHERE S.City = C.City)
UNION 
SELECT 
	S.ContactName, 
	N'Seller'  as [Type],
	S.City
FROM 
	dbo.Suppliers S
WHERE 
	EXISTS (SELECT * FROM dbo.Customers C WHERE S.City = C.City)
ORDER BY 
	City, Person

--[ki. Accepted]
-- 6.5	Найти всех покупателей, которые живут в одном городе. 
-- В запросе использовать соединение таблицы Customers c собой - самосоединение. 

SELECT DISTINCT
	C.City,
	C.CustomerID
FROM 
	dbo.Customers C
JOIN
	dbo.Customers C2 
		ON C.City = C2.City
		AND C.CustomerID <> C2.CustomerID
ORDER BY City

/* Проверочный запрос - список городов */
SELECT 
	C.City
FROM 
	dbo.Customers C
GROUP BY 
	C.City 
HAVING COUNT(C.City)>1

--[ki. accepted]
--6.6	По таблице Employees найти для каждого продавца его руководителя...
-- Высвечены ли все продавцы в этом запросе?
/* Ответ зависит от типа JOIN'a. INNER JOIN не покажет вице-президента*/

SELECT 
	E.LastName as [User Name],
	ISNULL(Boss.LastName, '') as [Boss]
FROM 
	dbo.Employees E
LEFT JOIN 
	dbo.Employees Boss
		ON	Boss.EmployeeID = E.ReportsTo

-- [ki. Accepted]
--7.1	Определить продавцов, которые обслуживают регион 'Western' 
 
SELECT 
	E.LastName,
    T.TerritoryDescription
FROM 
	dbo.Territories T
JOIN 
	dbo.EmployeeTerritories ET 
	ON ET.TerritoryID = T.TerritoryID
JOIN 
	dbo.Region R 
	ON R.RegionID = T.RegionID 
JOIN
	dbo.Employees E
	ON	E.EmployeeID = ET.EmployeeID
WHERE 
	R.RegionDescription = N'Western'


--[ki. accepted]
-- 8.1	Высветить в результатах запроса имена всех заказчиков из таблицы Customers 
-- и суммарное количество их заказов из таблицы Orders

SELECT 
	C.ContactName as Customer,
	COUNT(O.OrderID) as TotalOrders
FROM 
	dbo.Customers C
LEFT JOIN
	dbo.Orders O
	ON	O.CustomerID = C.CustomerID
GROUP BY 
	C.ContactName
ORDER BY
	TotalOrders 

--[ki. accepted]
-- 9.1	Высветить всех поставщиков колонка CompanyName в таблице Suppliers, у которых нет хотя бы 
-- одного продукта на складе (UnitsInStock в таблице Products равно 0). 
-- Использовать вложенный SELECT для этого запроса с использованием оператора IN. 

/* Знак "=" использовать можно, если подзапрос возвращает не более 1 значения */
SELECT 
	S.CompanyName 
FROM
	dbo.Suppliers S
WHERE 
	S.SupplierID IN
	(SELECT P.SupplierID FROM dbo.Products P 
		WHERE P.UnitsInStock = 0)
ORDER BY 
	CompanyName

--[ki. not accepted. Чё-то не то выводится]
-- Вот простой и рабочий запрос -  SELECT EmployeeID FROM dbo.Employees as E WHERE (SELECT Count(*) FROM dbo.Orders WHERE EmployeeID = e.EmployeeID) > 150;
--10.1	Высветить всех продавцов, которые имеют более 150 заказов. Использовать вложенный коррелированный SELECT

DECLARE @order_limit int = 150;
SELECT 
	S.CompanyName as [Seller]
FROM 
	dbo.Suppliers S
WHERE 
	(SELECT COUNT(O.OrderID)
	FROM 
		dbo.Products P
	JOIN
		dbo.[Order Details] OD
			ON OD.ProductID = P.ProductID
	JOIN
		dbo.Orders O
			ON O.OrderID = OD.OrderID
	WHERE 
		P.SupplierID = S.SupplierID
	) > @order_limit

--[ki. accepted]
-- 11.1	Высветить всех заказчиков (таблица Customers), которые не имеют ни одного заказа 

SELECT
CompanyName as Customer
FROM 
	dbo.Customers C
WHERE 
	NOT EXISTS(SELECT * FROM dbo.Orders O 
		WHERE O.CustomerID = C.CustomerID)

--[ki. accepted]
-- 12.1	Для формирования алфавитного указателя Employees высветить из таблицы Employees список 
-- только тех букв алфавита, с которых начинаются фамилии Employees (колонка LastName ) из этой таблицы.

SELECT DISTINCT
	UPPER(LEFT(LTRIM(E.LastName),1)) 
	as Letter
FROM
	dbo.Employees E
WHERE 
	/* Я не расист :), но мы будем использовать алфавит Шекспира */
	UPPER(LEFT(LTRIM(E.LastName),1)) LIKE N'[A-Z]' 
ORDER BY 
	Letter

--[ki. accepted]
--13.1	Написать процедуру, которая возвращает самый крупный заказ для каждого из продавцов за определенный год. 
-- В результатах не может быть несколько заказов одного продавца, должен быть только один и самый крупный. 

DECLARE @retval int
EXECUTE @retval = dbo.P_GreatestOrders @report_year = 1998, @records_to_show = 50
SELECT @retval as ReturnValue
GO

/* Проверочный скрипт.	Не знаю, как он "не должен повторять запрос, написанный в процедуре" - 
   как мне кажется, без суммирования строк заказа (таблица Order Details) не обойтись. */
DECLARE @test_year int					= 1998
DECLARE @test_lastname nvarchar(100)	= N'Fuller'
/* Беру строки спецификации заказа работника */
SELECT 
	E.LastName,
	OD.*
FROM 
	dbo.Employees E
JOIN
	dbo.Orders O ON O.EmployeeID = E.EmployeeID
JOIN	
	dbo.[Order Details] OD ON OD.OrderID = O.OrderID
WHERE 
	E.LastName = @test_lastname AND
	DATEPART(year,O.OrderDate) = @test_year
UNION
/* Суммирую строки спецификации - получаю сумму в заказе работника */
SELECT 
	E.LastName + N' - Total',
	OD.OrderID,
	0,
	CAST(SUM(OD.UnitPrice * OD.Quantity * (1 - OD.Discount)) as decimal(18,2)),
	0,
	0
FROM 
	dbo.Employees E
JOIN
	dbo.Orders O ON O.EmployeeID = E.EmployeeID
JOIN	
	dbo.[Order Details] OD ON OD.OrderID = O.OrderID
WHERE 
	E.LastName = @test_lastname AND
	DATEPART(year,O.OrderDate) = @test_year
GROUP BY 
	E.LastName,
	OD.OrderID
ORDER BY 
	OrderID,LastName

--[ki. accepted]
-- 13.2	Написать процедуру, которая возвращает заказы в таблице Orders, согласно указанному сроку
--  доставки в днях 

DECLARE @retval int
EXECUTE @retval = dbo.P_ShippedOrdersDiff  @specified_delay = 15
SELECT @retval as ReturnValue
GO

--[ki. accepted]
-- 13.3	 Написать процедуру, которая высвечивает всех подчиненных заданного продавца, 
-- как непосредственных, так и подчиненных его подчиненных. 
-- В качестве входного параметра функции используется EmployeeID. Необходимо распечатать имена
-- подчиненных и выровнять их в тексте (использовать оператор PRINT) согласно иерархии подчинения.
-- Продавец, для которого надо найти подчиненных также должен быть высвечен. Название процедуры 
-- SubordinationInfo. В качестве алгоритма для решения этой задачи надо использовать пример,
-- приведенный в Books Online и рекомендованный Microsoft для решения подобного типа задач. 
-- Продемонстрировать использование процедуры.

DECLARE @retval int
EXECUTE @retval =  dbo.P_SubordinationInfo @EmployeeID = 2
SELECT @retval as ReturnValue
GO

--[ki. accepted]
-- 13.4	 Написать функцию, которая определяет, есть ли у продавца подчиненные.
--  Возвращает тип данных BIT. 

SELECT 
	E.LastName,
	E.FirstName,
	dbo.SF_IsBoss(E.EmployeeID) as IsBoss
FROM
	dbo.Employees E
	 

--	14. Работа по финальному проекту
	
-- см файл QuizDB.sql

-- КОНЕЦ ЗАДАНИЯ 8.
