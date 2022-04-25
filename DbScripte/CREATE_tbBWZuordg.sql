USE [BWB]
GO

/****** Object:  Table [dbo].[tbBWZuordg]    Script Date: 4/25/2022 8:26:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbBWZuordg](
	[PKenng] [nvarchar](50) NOT NULL,
	[BWZuordg] [nvarchar](50) NOT NULL,
	[Alkane_Zuordg] [nvarchar](50) NULL,
	[File_mess] [nvarchar](50) NULL,
	[Acq_Date_Time] [nvarchar](50) NULL,
	[Import_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_tbBWZuordg] PRIMARY KEY CLUSTERED 
(
	[PKenng] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


