﻿ALTER TABLE [dbo].[aspnet_Roles]
    ADD CONSTRAINT [PK_aspnet_Roles] PRIMARY KEY NONCLUSTERED ([RoleId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];
