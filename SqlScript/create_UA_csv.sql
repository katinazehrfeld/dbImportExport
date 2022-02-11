USE [BWB]
GO

/****** Object:  Table [dbo].[UA_csv]    Script Date: 2/11/2022 4:20:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UA_csv](
	[ID_Messung] [int] IDENTITY(1,1) NOT NULL,
	[Component_RT] [float] NOT NULL,
	[Base_Peak_MZ] [float] NOT NULL,
	[CAS] [nvarchar](50) NULL,
	[Library_RI] [float] NULL,
	[Component_RI] [float] NOT NULL,
	[Match_Factor] [float] NOT NULL,
	[Compound_Name] [nvarchar](150) NOT NULL,
	[Formula] [nvarchar](50) NULL,
	[Library_File] [nvarchar](50) NOT NULL,
	[Component_Area] [float] NOT NULL,
	[Base_Peak_Area] [float] NOT NULL,
	[Type] [nchar](10) NOT NULL,
	[Comment] [nvarchar](50) NOT NULL,
	[Import_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_UA_csv] PRIMARY KEY CLUSTERED 
(
	[ID_Messung] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


