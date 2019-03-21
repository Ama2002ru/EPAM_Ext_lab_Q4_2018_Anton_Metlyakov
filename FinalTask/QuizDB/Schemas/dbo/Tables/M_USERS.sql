﻿CREATE TABLE [DBO].[M_USERS](
	[USER_ID] [int] NOT NULL,
	[FIRSTNAME] [nvarchar](100) NOT NULL,
	[LASTNAME] [nvarchar](100) NOT NULL,
	[USERNAME] [nvarchar](100) NOT NULL,
	[HASHEDPASSWORD] [nvarchar](100) NOT NULL,
	[SALT] [nvarchar](100) NOT NULL,
	[ROLESFLAG] [int] NOT NULL,
	[REGISTRATION_DATE] [datetime] NOT NULL,
	[LAST_LOGON_DATE] [datetime] NULL, 

    CONSTRAINT [PK_M_USERS] PRIMARY KEY CLUSTERED (	[USER_ID] ASC),
    CONSTRAINT [NK1_M_USERS] UNIQUE NONCLUSTERED (	[USERNAME] ASC)
) ON [PRIMARY]
