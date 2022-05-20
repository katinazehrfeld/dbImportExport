/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [ID_Peak]
      ,[RTmess]
      ,[BP_MZ]
      ,[CAS]
      ,[LibRI]
      ,[RImess]
      ,[MF]
      ,[SName]
      ,[Formula]
      ,[LibFile]
      ,[AreaP]
      ,[AreaBP]
      ,[Type]
      ,[PKenng]
      ,[Import_Date]
      ,[RTkorr]
      ,[RIkorr]
      ,[BPMZ_RT]
      ,[BPMZ_RI]
      ,[Peak_minus_BW]
      ,[AreaBW]
      ,[Kategorie]
      ,[Name_BPMZ_RI]
      ,[AreaPercent]
  FROM [BWB].[dbo].[tbPeaks]


  UPDATE tbPeaks 
  SET 
	 tbPeaks.RTkorr = NULL
	  ,RIkorr = NULL
      ,BPMZ_RT = NULL
      ,BPMZ_RI = NULL
      ,Peak_minus_BW = NULL
      ,AreaBW = NULL
	  ,Kategorie = NULL
	  ,Name_BPMZ_RI = Null
	  ,AreaPercent = Null