﻿ALTER TABLE [dbo].[aspnet_Applications]
    ADD CONSTRAINT [UQ_aspnet_Applications_LoweredApplicationName] UNIQUE NONCLUSTERED ([LoweredApplicationName] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];

