USE [BWB]
GO

/****** Object:  Table [dbo].[E3_Lims_Info]    Script Date: 2/11/2022 4:19:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[E3_Lims_Info](
	[ID_Lims_Info] [int] NOT NULL,
	[LimsNr] [nvarchar](50) NOT NULL,
	[ProbenArt] [nvarchar](50) NOT NULL,
	[Ort_kurz] [nvarchar](50) NOT NULL,
	[Entnahmedatum] [nvarchar](50) NOT NULL,
	[Import_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_E3_Lims_Info] PRIMARY KEY CLUSTERED 
(
	[ID_Lims_Info] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


