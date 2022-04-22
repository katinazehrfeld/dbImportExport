using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace DbImportExport.Report        
{
    public class DBGetReport   
    {
        private Action<string> Log; 

        internal string GetReport(string wasserwerk, Action<string> log)
        {
            Log = log;             

            var sqlConnection = new SqlConnection("Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; "); 
            sqlConnection.Open();

            var query = @"
SELECT 
	dbo.tbPeaks.CAS,
	dbo.tbPeaks.Peak_minus_BW,
	dbo.tbPeaks.AreaP,

	dbo.tbLInfos.Import_Date,

	dbo.tbPInfos.BWabgezogen

FROM dbo.tbPeaks
JOIN dbo.tbLInfos on tbPeaks.PKenng = tbLInfos.LimsNr
JOIN dbo.tbPInfos on tbPeaks.PKenng = dbo.tbPInfos.PKenng

WHERE tbLInfos.Ort_kurz = @Ort_kurz 
";

            var peaks = sqlConnection.Query<ReportPeak>(query, new { Ort_kurz = wasserwerk }).ToList();

            var proben = peaks
                .GroupBy(peak => peak.Import_Date)
                .Select(g => new ReportProbe(g.Key, g.ToList()))
                .ToList();
            
            var casNumbers = peaks
                .Select(peak => peak.CAS)
                .Where(cas => !string.IsNullOrEmpty(cas))
                .Distinct()
                .ToList();

            var sb = new StringBuilder();

            sb.AppendLine(GetHeaderLine(proben));

            foreach (var casNumber in casNumbers)
            {
                var line = GetStoffLine(casNumber, proben);
                sb.AppendLine(line);
            }

            return sb.ToString();
        }


        private string GetHeaderLine(List<ReportProbe> proben)
        {
            var dates = proben
                .OrderBy(probe => probe.Date)
                .Select(probe => probe.Date.ToString());

            var line = string.Join(";", dates);

            return $";;;;;{line}";
        }

        private string GetStoffLine(string cas, List<ReportProbe> proben)
        {
            var peakAreas = proben
                .Select(probe =>

                    probe.Peaks
                        .Where(peak => peak.CAS == cas)
                        .Select(peak => peak.AreaP)
                        .FirstOrDefault()?.ToString() ?? "<nö>");

            var line = string.Join(";", peakAreas);

            return $";;{cas};;;;{line}";
        }
    }
}
