using System.Collections.Generic;
using System.Linq;
using DbImportExport.Importer.UpdateValues.PeaksMinusBW;

namespace DbImportExport.Importer.MzRtFinder
{
    internal class BlindwertFinder
    {
        public Blindwert SucheBlindwert(Peak peak, List<Blindwert> blindwerte)
        {
            var match = GetMatches(peak, blindwerte, 0)
                .FirstOrDefault(bw => 2 * bw.AreaP < peak.AreaP);

            //--BPMZ_RT_p01
            if (match == null)
            {
                match = GetMatches(peak, blindwerte, 0.1)
                    .FirstOrDefault(bw => 2 * bw.AreaP < peak.AreaP);
            }

            //--BPMZ_RT_m01
            if (match == null)
            {
                match = GetMatches(peak, blindwerte, -0.1)
                    .FirstOrDefault(bw => 2 * bw.AreaP < peak.AreaP);
            }

            //--BPMZ_RT_p02
            if (match == null)
            {
                match = GetMatches(peak, blindwerte, 0.2)
                    .FirstOrDefault(bw => 2 * bw.AreaP < peak.AreaP);
            }

            //--BPMZ_RT_m02
            if (match == null)
            {
                match = GetMatches(peak, blindwerte, -0.2)
                    .FirstOrDefault(bw => 2 * bw.AreaP < peak.AreaP);
            }

            return match;
        }

        private IEnumerable<Blindwert> GetMatches(Peak peak, List<Blindwert> blindwerte, double diff)
        {
            return blindwerte.Where(b => b.BP_MZ_korr == peak.BP_MZ_korr && b.RTkorr == peak.RTkorr + diff);
        }
    }
}
