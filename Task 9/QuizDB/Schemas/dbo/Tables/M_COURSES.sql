﻿CREATE TABLE [dbo].[M_COURSES]
(
	[COURSE_ID] [int] NOT NULL,
	[COURSE_NAME] [nvarchar](100) NOT NULL,

    CONSTRAINT [PK_M_COURSES] PRIMARY KEY CLUSTERED (	[COURSE_ID] ASC),
	CONSTRAINT [NK1_M_COURSES] UNIQUE NONCLUSTERED (	[COURSE_NAME] ASC )
) 