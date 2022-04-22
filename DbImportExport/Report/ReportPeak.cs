using System;

namespace DbImportExport.Report
{
    internal class ReportPeak
    {
        public string CAS { get; set; }
        public double? Peak_minus_BW { get; set; }
        public double? AreaP { get; set; }
        public DateTimeOffset Import_Date { get; set; }
        public bool BWabgezogen { get; set; }
    }
}