﻿CREATE TABLE [dbo].[M_QUESTIONS]
(
	[QUESTION_ID] [int] NOT NULL,
	[QUIZ_ID] [int] NOT NULL,
	[INFO] [nvarchar](1000) NOT NULL,
	[TEXT] [nvarchar](1000) NOT NULL,
 	[CORRECT_OPTION_FLAG] [int] NOT NULL

    CONSTRAINT [PK_M_QUESTIONS] PRIMARY KEY CLUSTERED (	[QUESTION_ID] ASC),
	CONSTRAINT [FK1_M_QUESTIONS] FOREIGN KEY ([QUIZ_ID]) REFERENCES [dbo].[M_QUIZES] ([QUIZ_ID]),
) 
