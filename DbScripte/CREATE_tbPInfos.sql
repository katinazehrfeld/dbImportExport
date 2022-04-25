USE [BWB]
GO

/****** Object:  Table [dbo].[tbPInfos]    Script Date: 4/25/2022 8:28:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbPInfos](
	[ID_PInfos] [int] IDENTITY(1,1) NOT NULL,
	[PKenng] [nvarchar](50) NOT NULL,
	[Thema] [nvarchar](50) NULL,
	[LM] [nvarchar](50) NULL,
	[Vorbereitung_Meth] [nvarchar](50) NULL,
	[RT_IS_Pr] [float] NOT NULL,
	[RI_IS_Pr] [float] NOT NULL,
	[IS_AreaP] [float] NOT NULL,
	[IS_AreaBP] [float] NOT NULL,
	[V_Extraktion_mL] [float] NOT NULL,
	[Verdg_im_Vial] [float] NOT NULL,
	[IS_Volumen_ml] [float] NOT NULL,
	[InjektionsVolumen_ml] [float] NOT NULL,
	[Import_Date] [datetime] NOT NULL,
	[BWabgezogen] [bit] NOT NULL,
 CONSTRAINT [PK_tbPInfos] PRIMARY KEY CLUSTERED 
(
	[PKenng] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbPInfos] ADD  CONSTRAINT [DF_tbPInfos_BWabgezogen]  DEFAULT ((0)) FOR [BWabgezogen]
GO


