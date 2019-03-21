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

DELETE FROM  [DBO].[S_PK_GENERATOR]

INSERT INTO [DBO].[S_PK_GENERATOR] ([TABLE_NAME],[TABLE_ID]) 
VALUES 
	('M_ROLES',4),
	('M_USERS',12),
	('M_QUIZES',20),
	('M_QUESTIONS',100),
	('M_VARIANTS',400),
	('M_QUIZ_RESULTS',20),
	('M_QUIZ_ANSWERS',40),
	('T_LOG',1)
GO


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
DELETE FROM DBO.[M_ROLES];
INSERT INTO DBO.[M_ROLES]
([ROLE_ID], [ROLE_NAME], [ROLE_FLAG], [ALLOWED_METHODS])
VALUES
(1,N'Student',1,''),
(2,N'Instructor',2,''),
(3,N'Admin',4,'')
GO
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

DELETE FROM [DBO].[M_USERS]
INSERT INTO [DBO].[M_USERS]
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
DELETE FROM DBO.M_QUIZES;
INSERT INTO DBO.M_QUIZES
([QUIZ_ID],[QUIZ_NAME],[AUTHOR_ID]      ,[CREATED_DATE]      ,[SUCCESS_RATE])
VALUES
(6, N'C#', 2,'2019-03-12 20:35:06.910',0.7),
(7, N'C# part 2', 2,'2019-03-12 20:38:06.910',0.7),
(8, N'.NET', 2,'2019-03-12 20:38:06.910',0.5)



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
DELETE FROM DBO.M_QUESTIONS;
  INSERT INTO [DBO].[M_QUESTIONS]
(QUESTION_ID, QUIZ_ID, info, text,CORRECT_OPTION_FLAG)
values
(2,	6,	N'Вопрос 1',	N'Какой тип переменной используется в коде: int a = 5',	8),
(3,	6,	N'Вопрос 2',	N'Что делает оператор «%»',	1),
(4,	6,	N'Вопрос 3',	N'Что сделает программа выполнив следующий код: Console.WriteLine(«Hello, World!»)',	8),
(5,	6,	N'Вопрос 4',	N'Как сделать инкремент числа?',	1),
(6,	6,	N'Вопрос 5',	N'Как сделать декремент числа?',	1),
(7,	6,	N'Вопрос 6',	N'Как найти квадратный корень из числа x ?',	8),
(8,	6,	N'Вопрос 7',	N'Обозначения оператора «НЕ»',	1),
(9,	6,	N'Вопрос 8',	N'Обозначение оператора «ИЛИ»',	8),
(10, 6,	N'Вопрос 9',	N'Обозначение оператора «И»',	2),
(11, 6,	N'Вопрос 10',	N'Чему будет равен с, если int a = 10; int b = 4; int c = a % b', 1),

(12,7,N'Вопрос 1', N'Чему равен d, если int a = 0; int b = a++; int c = 0; int d = a + b + c + 3', 1),
(13,7,N'Вопрос 2', N'Для чего нужны условные операторы', 1),
(14,7,N'Вопрос 3', N'Что вернет функция Termin после выполения. Код:int Termin(){int a = 1;int b = 3;if (a != 5) return a + b;else return 0;}', 8),
(15,7,N'Вопрос 4', N'Как называется оператор «?:»', 8),
(16,7,N'Вопрос 5', N'Что такое массив', 4),
(17,7,N'Вопрос 6', N'Какие бывают массивы ?', 8),
(18,7,N'Вопрос 7', N'Что такое цикл и для чего они нужны', 8),
(19,7,N'Вопрос 8', N'Какие бывают циклы?', 1),
(20,7,N'Вопрос 9', N'Какой оператор возвращает значение из метода ?', 1),
(21,7,N'Вопрос 10', N'Что такое константа ?', 1),

(22,8,N'Вопрос 1', N'Когда вызываются статические конструкторы классов в C#?', 1),
(23,8,N'Вопрос 2', N'Каким образом можно перехватить добавление и удаление делегата из события?', 8),
(24,8,N'Вопрос 3', N'Что произойдет при исполнении следующего кода? int i = 5; object o = i; long j = (long)o;', 1),
(25,8,N'Вопрос 4', N'Выберите средство, которое предоставляет C# для условной компиляции', 1),
(26,8,N'Вопрос 5', N'Выберите правильный вариант, в которых пространство имен System содержит пространство имен Customizer', 2),
(27,8,N'Вопрос 6', N'Чтобы использовать unsafe код в приложении, необходимо …', 2),
(28,8,N'Вопрос 7', N'Реализацией какого паттерна (шаблона проектирования) являются события в C#?', 8),
(29,8,N'Вопрос 8', N'Чем отличаются константы и доступные только для чтения поля?', 1),
(30,8,N'Вопрос 9', N'Элемент, который нельзя пометить атрибутом', 8),
(31,8,N'Вопрос 10', N'Как называется технология, благодаря которой возможно взаимодействие управляемого кода (managed code) с Win32 API функциями и COM-объектами?', 1)


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
DELETE FROM DBO.M_VARIANTS;
 insert into [DBO].[M_VARIANTS]
  ([VARIANT_ID]      ,[QUESTION_ID]       ,[TEXT])
VALUES 

(2,	2,	N'"1 байт"'),
(3,	2,	N'"Знаковое 64-бит целое"'),
(4,	2,	N'"Знаковое 8-бит целое"'),
(5,	2,	N'"Знаковое 32-бит целое"'),
(6,	3,	N'"Возвращает остаток от деления"'),
(7,	3,	N'"Возвращает процент от суммы"'),
(8,	3,	N'"Возвращает тригонометрическую функцию"'),
(9,	3,	N'"Ничего из выше перечисленного"'),
(10,	4,	N'"Напишет на новой строчке Hello, World!"'),
(11,	4,	N'"Вырежет слово Hello, World! из всего текста"'),
(12,	4,	N'"Удалит все значения с Hello, World!"'),
(13,	4,	N'"Напишет Hello, World!"'),
(14,	5,	N'"++"'),
(15,	5,	N'"!="'),
(16,	5,	N'"—"'),
(17,	5,	N'"%%"'),
(18,	6,	N'"—"'),
(19,	6,	N'"%%"'),
(20,	6,	N'"!="'),
(21,	6,	N'"++"'),
(22,	7,	N'"Sqrt(x)"'),
(23,	7,	N'"Arifmetic.sqrt"'),
(24,	7,	N'"Summ.Koren(x)"'),
(25,	7,	N'"Math.Sqrt(x)"'),
(26,	8,	N'"!"'),
(27,	8,	N'"No"'),
(28,	8,	N'"!="'),
(29,	8,	N'"Not"'),
(30,	9,	N'"Or"'),
(31,	9,	N'"!="'),
(32,	9,	N'"!"'),
(33,	9,	N'"||"'),
(34,	10,	N'"Все выше перечисленные"'),
(35,	10,	N'"&&"'),
(36,	10,	N'"AND"'),
(37,	10,	N'"&"'),
(38,	11,	N'"2"'),
(39,	11,	N'"11"'),
(40,	11,	N'"1"'),
(41,	11,	N'"3"'),

(42,	12,	N'"4"'),
(43,	12,	N'"False"'),
(44,	12,	N'"True"'),
(45,	12,	N'"3"'),

(46,	13,	N'"Для ветвления программы"'),
(47,	13,	N'"Чтобы были"'),
(48,	13,	N'"Чтобы устанавливать условия пользователю"'),
(49,	13,	N'"Для оптимизации программы"'),

(50,	14,	N'"0"'),
(51,	14,	N'"3"'),
(52,	14,	N'"5"'),
(53,	14,	N'"4"'),

(54,	15,	N'"Территориальный оператор"'),
(55,	15,	N'"Прямой оператор"'),
(56,	15,	N'"Вопросительный"'),
(57,	15,	N'"Тернарный оператор"'),

(58,	16,	N'"Переменная"'),
(59,	16,	N'"Набор текстовых значений в формате Unicode, которые расположены в случайном порядке"'),
(60,	16,	N'"Набор однотипных данных, которые располагаются в памяти последовательно друг за другом"'),
(61,	16,	N'"Набор данных типа int (32-бит целое)"'),

(62,	17,	N'"статичные"'),
(63,	17,	N'"Разнообразные"'),
(64,	17,	N'"Связные"'),
(65,	17,	N'"Одномерные и многомерные"'),

(66,	18,	N'"Циклы нужны чтобы выполнить код без ошибок"'),
(67,	18,	N'"Циклы нужны для многократного размещения данных"'),
(68,	18,	N'"Циклы нужны для многократного запуска программы"'),
(69,	18,	N'"Циклы нужны для многократного выполнения кода"'),

(70,	19,	N'"for, while, do-while, foreach"'),
(71,	19,	N'"ref, out, static, root"'),
(72,	19,	N'"Большие и маленькие"'),
(73,	19,	N'"Цикл, Форич, Двойной цикл, Многократный"'),

(74,	20,	N'"return"'),
(75,	20,	N'"end"'),
(76,	20,	N'"out"'),
(77,	20,	N'"back"'),

(78,	21,	N'"Переменная значение которой нельзя изменить"'),
(79,	21,	N'"Глобальная переменная"'),
(80,	21,	N'"Переменная которая может быть изменена в любое время"'),
(81,	21,	N'"Переменная типа string"'),

(83,	22,	N'"Один раз при первом создании экземпляра класса или при первом обращении к статическим членам класса"'),
(84,	22,	N'"После каждого обращения к статическим полям, методам и свойствам"'),
(85,	22,	N'"Статических конструкторов в C# нет"'),
(86,	22,	N'"Строгий порядок вызова не определен"'),

(87,	23,	N'"Переопределить операторы + и – для делегата"'),
(88,	23,	N'"Использовать ключевые слова get и set"'),
(89,	23,	N'"Для этого существуют специальные ключевые слова add и remove"'),
(90,	23,	N'"Такая возможность не предусмотрена"'),

(91,	24,	N'"Ошибок не произойдет. Переменная j будет иметь значение 5"'),
(92,	24,	N'"Произойдет ошибка времени компиляции"'),
(93,	24,	N'"Значение переменной j предсказать нельзя"'),
(94,	24,	N'Произойдет исключение'),

(95,	25,	N'"Директива #if"'),
(96,	25,	N'"Директива #typedef"'),
(97,	25,	N'"Директива #elseif"'),
(98,	25,	N'"Директива #switch"'),

(99,	26,	N'"namespace System { namespace Customizer { } }"'),
(100,	26,	N'"namespace System { namespace Customizer { } }"'),
(101,	26,	N'"Нельзя создавать собственные пространства имен в пространстве имен System"'),

(102,	27,	N'"Пометить методы, где используется небезопасный код с помощью ключевого слова fixed"'),
(103,	27,	N'"Пометить методы, где используется небезопасный код с помощью ключевого слова unsafe"'),
(104,	27,	N'"Декоратор (Decorator)"'),

(105,	28,	N'"Декоратор (Decorator)"'),
(106,	28,	N'"Посетитель (Visitor)"'),
(107,	28,	N'"Шаблонный метод (Template Method)"'),
(108,	28,	N'"Издатель-подписчик (Publisher-Subscriber)"'),

(109,	29,	N'"Константы инициализируются во время компиляции, доступные только для чтения поля — во время выполнения"'),
(110,	29,	N'"Константы можно изменять, а доступные только для чтения поля нет"'),
(111,	29,	N'"Ничем не отличаются"'),
(112,	29,	N'"Доступные только для чтения поля инициализируются во время компиляции, константы — во время выполнения"'),
(113,	30,	N'"Структуры"'),
(114,	30,	N'"Методы"'),
(115,	30,	N'"Классы"'),
(116,	30,	N'"Все перечисленное можно пометить атрибутом"'),

(117,	31,	N'"CodeDOM"'),
(118,	31,	N'"WebServices"'),
(119,	31,	N'"Interop"'),
(120,	31,	N'"Remoting"')

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




GO
