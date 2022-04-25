USE [BWB]
GO

/****** Object:  Table [dbo].[Säule_BW]    Script Date: 4/25/2022 8:25:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Säule_BW](
	[CAS] [nvarchar](50) NULL,
	[SysName] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](500) NOT NULL
) ON [PRIMARY]
GO


