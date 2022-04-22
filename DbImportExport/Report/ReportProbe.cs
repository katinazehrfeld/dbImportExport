using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbImportExport.Report
{
    class ReportProbe
    {
        public DateTimeOffset Date { get; }
        public List<ReportPeak> Peaks { get; }

        public ReportProbe(DateTimeOffset date, List<ReportPeak> peaks)
        {
            Date = date;
            Peaks = peaks;
        }
    }
}
