/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [ID_LInfos]
      ,[LimsNr]
      ,[ProbenArt]
      ,[Ort_kurz]
      ,[Entnahmedatum]
      ,[Import_Date]
  FROM [BWB].[dbo].[tbLInfos]