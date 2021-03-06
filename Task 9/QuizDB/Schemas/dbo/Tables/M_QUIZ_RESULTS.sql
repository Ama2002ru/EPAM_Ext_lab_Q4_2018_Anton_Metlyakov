﻿CREATE TABLE [dbo].[M_QUIZ_RESULTS]
(
	[QUIZ_RESULTS_ID] [int] NOT NULL,
	[USER_ID] [int] NOT NULL,
	[QUIZ_ID] [int] NOT NULL,
	[QUIZ_STATUS] [int] NOT NULL,
	[ASSIGNED_BY_ID] [int] NOT NULL,
	[ASSIGNED_DATE] [datetime] NOT NULL,
	[COMPLETED_RATE] [real] NULL,
	[COMPLETED_DATE] [datetime] NULL

    CONSTRAINT [PK_M_QUIZ_RESULTS] PRIMARY KEY CLUSTERED (	[QUIZ_RESULTS_ID] ASC),
	CONSTRAINT [FK1_M_QUIZ_RESULTS] FOREIGN KEY ([USER_ID]) REFERENCES [dbo].[M_USERS] ([USER_ID]),
	CONSTRAINT [FK2_M_QUIZ_RESULTS] FOREIGN KEY ([QUIZ_ID]) REFERENCES [dbo].[M_QUIZES] ([QUIZ_ID]),
	CONSTRAINT [FK3_M_QUIZ_RESULTS] FOREIGN KEY ([ASSIGNED_BY_ID]) REFERENCES [dbo].[M_USERS] ([USER_ID]),
	CONSTRAINT [CK1_M_QUIZ_RESULTS] CHECK ([COMPLETED_DATE] >0 AND [COMPLETED_DATE] <= 1 )
) 
