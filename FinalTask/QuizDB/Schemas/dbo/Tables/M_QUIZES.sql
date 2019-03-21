﻿CREATE TABLE [DBO].[M_QUIZES]
(
	[QUIZ_ID] [int] NOT NULL,
	[QUIZ_NAME] [nvarchar](100) NOT NULL,
 	[AUTHOR_ID] [int] NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[SUCCESS_RATE] [real] NOT NULL

    CONSTRAINT [PK_M_QUIZES] PRIMARY KEY CLUSTERED (	[QUIZ_ID] ASC),
    CONSTRAINT [NK1_M_QUIZES] UNIQUE NONCLUSTERED (	[QUIZ_NAME] ASC ),
	CONSTRAINT [FK1_M_QUIZES] FOREIGN KEY ([AUTHOR_ID]) REFERENCES [DBO].[M_USERS] ([USER_ID]) ON DELETE NO ACTION,
	CONSTRAINT [CK1_M_QUIZES] CHECK ([SUCCESS_RATE] >0 AND [SUCCESS_RATE] <= 1 )
) 
