﻿CREATE TABLE [dbo].[M_QUIZ_ANSWERS]
(
	[QUIZ_ANSWERS_ID] [int] NOT NULL,
	[QUIZ_ID] [int] NOT NULL,
	[QUESTION_ID] [int] NOT NULL,
	[ANSWER_FLAG] [int] NOT NULL,
	[ANSWER_DATE] [datetime] NOT NULL,
	[ELAPSED_TIME] [int] NOT NULL

    CONSTRAINT [PK_M_QUIZ_ANSWERS] PRIMARY KEY CLUSTERED (	[QUIZ_ANSWERS_ID] ASC),
	CONSTRAINT [FK1_M_QUIZ_ANSWERS] FOREIGN KEY ([QUIZ_ID]) REFERENCES [dbo].[M_QUIZES] ([QUIZ_ID]),
	CONSTRAINT [FK2_M_QUIZ_ANSWERS] FOREIGN KEY ([QUESTION_ID]) REFERENCES [dbo].[M_QUESTIONS] ([QUESTION_ID]),
) 
