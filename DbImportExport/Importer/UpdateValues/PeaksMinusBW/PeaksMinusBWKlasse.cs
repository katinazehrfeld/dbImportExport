using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DbImportExport.Importer.UpdateValues.PeaksMinusBW
{
    class PeaksMinusBWKlasse
    {
        private Action<string> Log;
        public PeaksMinusBWKlasse(Action<string> log)
        {
            Log = log;
        }

        public void PeaksMinusBW(SqlConnection connection)
        {
            var peaks = ReadPeaks(connection);

            var groups = peaks.GroupBy(p => p.PKenng);

            foreach (var group in groups)
            {
                ProcessGroup(connection, group.Key, group.ToList());
            }
        }


        private void ProcessGroup(SqlConnection connection, string pKennung, List<Peak> peaks)
        {
            var bwKennung = peaks
                .Select(p => p.BWZuordg)
                .Distinct()
                .Single();


            var blindwerte = ReadBlindwerte(connection, bwKennung);


            foreach (var peak in peaks)
            {
                ProcessPeak(connection, peak, blindwerte);
            }

        }


        private void ProcessPeak(SqlConnection connection, Peak peak, List<Blindwert> blindwerte)
        {
            var processed = false;

            var match = blindwerte
                .Where(bw => bw.BPMZ_RT == peak.BPMZ_RT)
                .Where(bw => 2 * bw.AreaP > peak.AreaP)
                .FirstOrDefault();

            if (match != null)
            {
                processed = true;
                UpdateBw(connection, peak, match);
            }


            //--BPMZ_RT_p01
            if (!processed)
            {
                match = blindwerte
                .Where(bw => bw.BPMZ_RT == peak.BPMZ_RT_p01)
                .Where(bw => 2 * bw.AreaP > peak.AreaP)
                .FirstOrDefault();

                if (match != null)
                {
                    processed = true;
                    UpdateBw(connection, peak, match);
                }
            }

            //--BPMZ_RT_m01
            if (!processed)
            {
                match = blindwerte
                .Where(bw => bw.BPMZ_RT == peak.BPMZ_RT_m01)
                .Where(bw => 2 * bw.AreaP > peak.AreaP)
                .FirstOrDefault();

                if (match != null)
                {
                    processed = true;
                    UpdateBw(connection, peak, match);
                }
            }

            //--BPMZ_RT_p02
            if (!processed)
            {
                match = blindwerte
                .Where(bw => bw.BPMZ_RT == peak.BPMZ_RT_p02)
                .Where(bw => 2 * bw.AreaP > peak.AreaP)
                .FirstOrDefault();

                if (match != null)
                {
                    processed = true;
                    UpdateBw(connection, peak, match);
                }
            }

            //--BPMZ_RT_m02
            if (!processed)
            {
                match = blindwerte
                .Where(bw => bw.BPMZ_RT == peak.BPMZ_RT_m02)
                .Where(bw => 2 * bw.AreaP > peak.AreaP)
                .FirstOrDefault();

                if (match != null)
                {
                    processed = true;
                    UpdateBw(connection, peak, match);
                }
            }
        }

        private void UpdateBw(SqlConnection connection, Peak peak, Blindwert blindwert)
        {

            

            //hier BW Rechnung
            //






        }




        private List<Peak> ReadPeaks(SqlConnection connection)
        {
            var sql_select = @"
SELECT

                        tbPInfos.PKenng,
                        tbPInfos.BWabgezogen,
                        tbPInfos.BWmissingValues,

						tbBWZuordg.BWZuordg,

                        tbPeaks.ID_Peak,
                        tbPeaks.Type,
                        tbPeaks.BPMZ_RT,
                        tbPeaks.BPMZ_RT_p01,
                        tbPeaks.BPMZ_RT_p02,
                        tbPeaks.BPMZ_RT_m01,
						tbPeaks.BPMZ_RT_m02,
                        tbPeaks.AreaP
                    FROM
                        dbo.tbPInfos tbPInfos
                        LEFT JOIN dbo.tbPeaks tbPeaks ON tbPInfos.PKenng = tbPeaks.PKenng
						LEFT JOIN dbo.tbBWZuordg tbBWZuordg ON tbPInfos.PKenng = tbBWZuordg.PKenng

                    WHERE 
                        (tbPInfos.BWabgezogen           IS NULL     --die Zahl 0 bedeutet BWPeak, leere Zelle kann zwei Bedeutungen haben: 1.werte zur Berechung fehlen, 2.neuerEintrag der noch in die Berechnung muss
                                                                    --für fehlende Werte ein FW eintragen, dann kann später, danach selektiert werden, ist das gut?
                        OR tbPInfos.BWabgezogen        = 0)

						AND(  
							tbPInfos.BWmissingValues  IS NULL
							OR tbPInfos.BWmissingValues    = 0 
						)

						AND tbPeaks.Type = 'Sample'
";


            List<Peak> peaks = new List<Peak>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql_select;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var PKenng = (string)reader["PKenng"];
                        var BWZuordg = (string)reader["BWZuordg"];
                        var ID_Peak = (int)reader["ID_Peak"];
                        var Type = (string)reader["Type"];
                        var BPMZ_RT = (string)reader["BPMZ_RT"];
                        var BPMZ_RT_p01 = (string)reader["BPMZ_RT_p01"];
                        var BPMZ_RT_p02 = (string)reader["BPMZ_RT_p02"];
                        var BPMZ_RT_m01 = (string)reader["BPMZ_RT_m01"];
                        var BPMZ_RT_m02 = (string)reader["BPMZ_RT_m02"];
                        var AreaP = (double)reader["AreaP"];
                        

                        var peak = new Peak()
                        {
                            PKenng = PKenng,
                            BWZuordg = BWZuordg,
                            ID_Peak = ID_Peak,
                            Type = Type,
                            BPMZ_RT = BPMZ_RT,
                            BPMZ_RT_p01 = BPMZ_RT_p01,
                            BPMZ_RT_p02 = BPMZ_RT_p02,
                            BPMZ_RT_m01 = BPMZ_RT_m01,
                            BPMZ_RT_m02 = BPMZ_RT_m02,
                            AreaP = AreaP,
                        };

                        peaks.Add(peak);
                    }
                }
            }

            return peaks;
        }


        private List<Blindwert> ReadBlindwerte(SqlConnection connection, string pKennung)
        {
            var sql_select = @"
SELECT
	tbPeaks.ID_Peak,
    tbPeaks.BPMZ_RT,
    tbPeaks.AreaP
FROM
	dbo.tbPeaks 
WHERE 
	tbPeaks.PKenng = @PKenng
	AND tbPeaks.Type = 'Blank'
";


            List<Blindwert> blindWerte = new List<Blindwert>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql_select;

                command.Parameters.AddWithValue("@PKenng", pKennung);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ID_Peak = (int)reader["ID_Peak"];
                        var BPMZ_RT = (string)reader["BPMZ_RT"];
                        var AreaP = (double)reader["AreaP"];


                        var blindwertPeak = new Blindwert()
                        {
                            ID_Peak = ID_Peak,
                            BPMZ_RT = BPMZ_RT,
                            AreaP = AreaP,
                        };

                        blindWerte.Add(blindwertPeak);
                    }
                }
            }

            return blindWerte;
        }
    }
}
