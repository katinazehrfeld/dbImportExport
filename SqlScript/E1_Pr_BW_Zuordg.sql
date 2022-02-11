USE [BWB]
GO

/****** Object:  Table [dbo].[E1_Pr_BW_Zuordg]    Script Date: 2/11/2022 4:19:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[E1_Pr_BW_Zuordg](
	[ID_BW_Zuordg] [int] IDENTITY(1,1) NOT NULL,
	[Pr_Kennung] [nvarchar](50) NOT NULL,
	[BW_Zuordnung] [nvarchar](50) NOT NULL,
	[Alkane_Zuordg] [nvarchar](50) NULL,
	[File_name] [nvarchar](50) NULL,
	[Acq_Date_Time] [nvarchar](50) NULL,
	[Import_Date] [datetime] NOT NULL
) ON [PRIMARY]
GO


