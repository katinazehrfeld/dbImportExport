USE [BWB]
GO

/****** Object:  Table [dbo].[tbPeaks]    Script Date: 4/22/2022 7:07:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbPeaks](
	[ID_Peak] [int] IDENTITY(1,1) NOT NULL,
	[RTmess] [float] NOT NULL,
	[BP_MZ] [float] NOT NULL,
	[CAS] [nvarchar](50) NULL,
	[LibRI] [float] NULL,
	[RImess] [float] NOT NULL,
	[MF] [float] NOT NULL,
	[SName] [nvarchar](150) NOT NULL,
	[Formula] [nvarchar](50) NULL,
	[LibFile] [nvarchar](50) NOT NULL,
	[AreaP] [float] NOT NULL,
	[AreaBP] [float] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[PKenng] [nvarchar](50) NOT NULL,
	[Import_Date] [datetime] NULL,
	[RTkorr] [float] NULL,
	[RIkorr] [int] NULL,
	[BPMZ_RT] [nvarchar](50) NULL,
	[BPMZ_RI] [nvarchar](50) NULL,
	[Peak_minus_BW] [float] NULL,
	[AreaBW] [float] NULL,
	[Kategorie] [int] NULL,
	[Name_BPMZ_RI] [nvarchar](250) NULL,
	[AreaPercent] [int] NULL,
 CONSTRAINT [PK_tbPeaks] PRIMARY KEY CLUSTERED 
(
	[ID_Peak] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


