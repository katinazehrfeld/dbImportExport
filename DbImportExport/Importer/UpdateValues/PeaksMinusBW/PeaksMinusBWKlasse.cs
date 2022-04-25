using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DbImportExport.Importer.MzRtFinder;

namespace DbImportExport.Importer.UpdateValues.PeaksMinusBW
{
    public class PeaksMinusBWKlasse
    {
        private Action<string> Log;

        public PeaksMinusBWKlasse(Action<string> log)
        {
            Log = log;
        }

        public void PeaksMinusBW(SqlConnection connection)
        {
            var peaks = ReadPeaks(connection);
            var groups = peaks.GroupBy(peak => peak.PKenng);

            var gemesseneStandards =
                connection.Query<GemessenerStandad>("SELECT * FROM dbo.GemesseneStandards").ToList();
            var säuleBw = connection.Query<SäuleBw>("SELECT * FROM dbo.Säule_BW").ToList();

            foreach (var group in groups)
            {
                ProcessProbe(connection, group.Key, group.ToList(), gemesseneStandards, säuleBw);
            }

            Log("done");
        }

        private void ProcessProbe(SqlConnection connection,
            string probeKennung,
            List<Peak> peaks,
            List<GemessenerStandad> gemesseneStandards,
            List<SäuleBw> säuleBws)
        {
            using (var transaction = connection.BeginTransaction())
            {
                var bwKennung = peaks
                    .Select(p => p.BWZuordg)
                    .Distinct()
                    .Single();

                var blindwerte = ReadBlindwerte(connection, bwKennung, transaction);

                foreach (var peak in peaks)
                {
                    var blindwert = new BlindwertFinder().SucheBlindwert(peak, blindwerte);
                    if (blindwert != null)
                    {
                        UpdateBlindwert(connection, peak, blindwert, gemesseneStandards, säuleBws, transaction);
                    }
                }

                connection.Execute(
                    @"UPDATE dbo.tbPInfos SET BWabgezogen = 1 WHERE PKenng = @pKennung",
                    new { pKennung = probeKennung },
                    transaction);

                transaction.Commit();
            }
        }

        private void UpdateBlindwert(
            SqlConnection connection,
            Peak peak,
            Blindwert blindwert,
            List<GemessenerStandad> gemessenerStandads,
            List<SäuleBw> säuleBws,
            DbTransaction transaction)
        {
            var areaNeu = peak.AreaP - blindwert.AreaP;

            //                                Verdünnungsfaktor        Vlumen IS                        Volumen Spritze
            // O2*1000/Proben_Infos!$J$3  *   Proben_Infos!$K$3   *   (0.05/Proben_Infos!$L$3)   *   (0.001/Proben_Infos!$M$3)
            //
            // (areaneu * 1000) /   tbPinfos.V_extraktion_ml 
            //Verdünnungsfaktor:    pinfo.Verd_Im_Vail
            //Volumen IS =          pinfo.IS_Volumen_ML
            //Vol Spritze:          pinfo.Injektionsvolumen_ml


            areaNeu = areaNeu * 1000D / peak.V_Extraktion_mL *
                      peak.Verdg_im_Vial *
                      (0.05 / peak.IS_Volumen_ml) *
                      (0.001 / peak.InjektionsVolumen_ml);

            /*
            =WENN(ODER(O17<10000,Y17="Säule",Y17="BW",Y17="IS"),6,
             WENN(G17<80,5,
             WENN(UND(ABS(E17-U17)>100,NICHT(E17="")),4,
             WENN(UND(AI17="x",NICHT(E17="")),1,
             WENN(UND(ODER(J17="NIST20.L",J17="NIST17.L",J17="NIST11.L",J17="SWGDRUG.L",J17="WILEY275.L",J17="HPPEST.L",J17="PMW_TOX2.L",J17="ENVI96.L"),NICHT(E17="")),3,
             WENN(E17="",4,2))))))

            O17 = Areneu

            6: ODER(O17<10000,Y17="Säule",Y17="BW",Y17="IS") <-- Paek.CAS == Säule_BW.CAS
            5: G17<80  <-- TbPaekks.MF < 80
            4: UND(ABS(E17-U17)>100,NICHT(E17="")) <--  tbPeak.LibRi != NULL  &&  abs( tbPeak.LibRi - Peak.RiKorr) > 100 
            1: UND(AI17="x",NICHT(E17="")) <--  tbPeak.LibRi != NULL  && GemesseneStandards.CAS."x" =="x"
            3: UND(ODER(
                        J17="NIST20.L",
                        J17="NIST17.L",
                        J17="NIST11.L",
                        J17="SWGDRUG.L",
                        J17="WILEY275.L",
                        J17="HPPEST.L",
                        J17="PMW_TOX2.L",
                        J17="ENVI96.L"),  NICHT(E17=""))
            <-- tbPeak.LibRi != NULL && TbPeak.LibFile  == [...]
            4: E17="" <-- tbPeak.LibRi == NULL 
            2: sonst
            */

            var kategorie = 2;
            if (areaNeu < 10000 || säuleBws.Any(s => s.CAS == peak.CAS))
            {
                kategorie = 6;
            }
            else if (peak.MF < 80)
            {
                kategorie = 5;
            }
            else if (peak.LibRI.HasValue && peak.RIkorr.HasValue &&
                     (peak.LibRI.Value - peak.RIkorr.Value) > 100) //ToDo: peak.RIkorr.HasValue???
            {
                kategorie = 4;
            }
            else if (peak.LibRI.HasValue && gemessenerStandads.FirstOrDefault(s => s.CAS == peak.CAS)?.Type == "x")
            {
                kategorie = 1;
            }
            else if (peak.LibRI.HasValue && (
                         peak.LibFile == "" ||
                         peak.LibFile == "NIST20.L" ||
                         peak.LibFile == "NIST17.L" ||
                         peak.LibFile == "NIST11.L" ||
                         peak.LibFile == "SWGDRUG.L" ||
                         peak.LibFile == "WILEY275.L" ||
                         peak.LibFile == "HPPEST.L" ||
                         peak.LibFile == "PMW_TOX2.L" ||
                         peak.LibFile == "ENVI96.L"
                     ))
            {
                kategorie = 3;
            }
            else if (peak.LibRI == null)
            {
                kategorie = 4;
            }

            /*
            //Substanzname

            Neue Spalte: pbPeak.Name_BPMZ_RI

            Kategorie = 1,2,3 -> pbPeak.Name_BPMZ_RI = tbPeak.SName 
                        sonst: pbPeak.Name_BPMZ_RI = tbPeak.BPMZ_RI
            */

            var name_BPMZ_RI = kategorie == 1 || kategorie == 2 || kategorie == 3
                ? peak.SName
                : peak.BPMZ_RI;


            /*
            //Flächenprozent:
            
            - Neue Spalte
            
                   runden ohne nachkomma(  100 / tbPinfo.IS:AreaP * AreaNeu)
            */


            var flaechenProzent = (int)Math.Round(100D / peak.IS_AreaP * areaNeu);


            // ToDo: in tablePaeks Spalte Peak_minus_BW updaten (areaNeu)
            // ToDo: Column TbPeak.Kategorie -> update!

            var sqlStatementUpdatePeak = @"
UPDATE dbo.tbPeaks 
SET 
    Peak_minus_BW = @areaNeu,
    Kategorie = @kategorie,
    Name_BPMZ_RI = @name_BPMZ_RI,
    AreaPercent = @flaechenProzent
WHERE
    ID_Peak = @ID_Peak";

            connection.Execute(
                sqlStatementUpdatePeak,
                new
                {
                    ID_Peak = peak.ID_Peak,
                    areaNeu = areaNeu,
                    kategorie = kategorie,
                    name_BPMZ_RI = name_BPMZ_RI,
                    flaechenProzent = flaechenProzent
                },
                transaction);
        }

        private static List<Peak> ReadPeaks(SqlConnection connection)
        {
            var sql_select = @"
                    SELECT
                        tbPInfos.PKenng,
                        tbPInfos.BWabgezogen,
                        tbPInfos.V_Extraktion_mL,
                        tbPInfos.Verdg_im_Vial,
                        tbPInfos.IS_Volumen_ml,
                        tbPInfos.InjektionsVolumen_ml,
                        tbPInfos.IS_AreaP,
						
                        tbBWZuordg.BWZuordg,

                        tbPeaks.ID_Peak,
                        tbPeaks.AreaP,
                        tbPeaks.CAS,
                        tbPeaks.MF,
                        tbPeaks.LibRI,
                        tbPeaks.RIkorr,
                        tbPeaks.LibFile,
                        tbPeaks.SName,
                        tbPeaks.BPMZ_RI,
                        tbPeaks.RTkorr,
                        ROUND(BP_MZ, 0) BP_MZ_korr
                    FROM
                        dbo.tbPInfos tbPInfos
                        LEFT JOIN dbo.tbPeaks tbPeaks ON tbPInfos.PKenng = tbPeaks.PKenng
						LEFT JOIN dbo.tbBWZuordg tbBWZuordg ON tbPInfos.PKenng = tbBWZuordg.PKenng

                    WHERE 
                        tbPInfos.BWabgezogen = 0     --die Zahl 0 bedeutet BWPeak, leere Zelle kann zwei Bedeutungen haben: 1.werte zur Berechung fehlen, 2.neuerEintrag der noch in die Berechnung muss
                                                     --für fehlende Werte ein FW eintragen, dann kann später, danach selektiert werden, ist das gut?
						AND tbPeaks.Type = 'Sample'
";

            var peaks = connection.Query<Peak>(sql_select).ToList();
            return peaks;
        }

        private static List<Blindwert> ReadBlindwerte(
            SqlConnection connection,
            string pKennung,
            SqlTransaction transaction)
        {
            var sql_select = @"
SELECT
    tbPeaks.AreaP,
    tbPeaks.RTkorr,
    ROUND(tbPeaks.BP_MZ, 0) BP_MZ_korr,
FROM
	dbo.tbPeaks 
WHERE 
	tbPeaks.PKenng = @PKenng
	AND tbPeaks.Type = 'Blank'
";
            var parameter = new
            {
                PKenng = pKennung
            };

            var blindWerte = connection.Query<Blindwert>(
                    sql_select,
                    parameter,
                    transaction)
                .ToList();

            return blindWerte;
        }
    }
}