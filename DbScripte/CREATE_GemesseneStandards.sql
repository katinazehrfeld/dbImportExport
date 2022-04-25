USE [BWB]
GO

/****** Object:  Table [dbo].[GemesseneStandads]    Script Date: 4/25/2022 8:18:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GemesseneStandards](
	[CAS] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[RILib] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](1000) NOT NULL,
	[Remark] [nvarchar](1000) NULL
) ON [PRIMARY]
GO


