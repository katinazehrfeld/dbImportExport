USE [BWB]
GO

/****** Object:  Table [dbo].[E2_Pr_Info]    Script Date: 2/11/2022 4:19:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[E2_Pr_Info](
	[ID_Pr_Info] [int] IDENTITY(1,1) NOT NULL,
	[Pr_Kennung] [nvarchar](50) NOT NULL,
	[Thema] [nvarchar](50) NULL,
	[LM] [nvarchar](50) NULL,
	[Vorbereitung_Meth] [nvarchar](50) NULL,
	[RT_IS_Pr] [float] NOT NULL,
	[RI_IS_Pr] [float] NOT NULL,
	[IS_Area_Peak] [float] NOT NULL,
	[IS_Area_BasePeak] [float] NOT NULL,
	[V_Extraktion_mL] [float] NOT NULL,
	[Verdg_im_Vial] [float] NOT NULL,
	[IS_Volumen_ml] [float] NOT NULL,
	[InjektionsVolumen_ml] [float] NOT NULL,
	[Import_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Kunden] PRIMARY KEY CLUSTERED 
(
	[ID_Pr_Info] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


