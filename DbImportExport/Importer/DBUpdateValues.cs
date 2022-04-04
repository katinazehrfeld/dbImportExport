﻿using System;                           //globale geladene Funktionen
using System.Collections.Generic;       //vereinfachen später den Code
using System.Data.SqlClient;            //weil verkürzte Schreibweise dadurch möglich
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbImportExport.Importer        //Namensklasse in der keine Namensgleichheit vorkommen darf
{
    public class DBUpdateValues    // auf eine public class kann von außen zugegriffen werden
    {
        private Action<string> Log; // private ist nur in dieser public class ansprechbar
                                    // erzeugt einen Log string
        
        internal void UpdateData(Action<string> log)
        {
            Log = log;              // zeigt die erfolgten ToDos und Fehler

            var connection = new SqlConnection("Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; ");  //Variable definiert für Verbindung zu entsprechender DB
            connection.Open();               //öffnet gewählte DB

            log("Updating data");

            RtKorrigieren(connection);
            RiKorrigieren(connection);
            MzRtName(connection);
            MzRtp01Name(connection);
            MzRtm01Name(connection);
            //MzRtp02Name(connection);
            //MzRtm02Name(connection);
            MzRiName(connection);
            //AreaBW(connection);
            //PeaksMinusBW(connection);

        }



// Berechnung der korrigierten Rt Werte

        private void RtKorrigieren(SqlConnection connection)
        {
            var sql_select = @"
                            SELECT
	                            messung.ID_Peak,
	                            messung.RTmess,
	                            probeInfo.RT_IS_Pr,
                                probeInfo.BWabgezogen
                            FROM
	                            dbo.tbPeaks messung
	                            LEFT JOIN dbo.tbPInfos probeInfo ON messung.PKenng = probeInfo.PKenng
                            WHERE 
                                messung.RTkorr IS NULL
                                AND probeInfo.BWabgezogen IS NULL
                                AND probeInfo.RT_IS_Pr IS NOT NULL
                                
                            ";


            var ids = new List<int>();
            var rtNeu = new List<double>();

            int c = 0;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql_select;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c++;
                        var id = (int)reader["ID_Peak"];
                        var messRt = (double)reader["RTmess"];
                        var messRtIS = (double)reader["RT_IS_Pr"];




                        //hier rechnen
                        var rtNeuValue = BerechneRt(messRt, messRtIS);    //Sprung in die Berechnung,siehe unten (mit erforderlichen Parametern)

                        ids.Add(id);
                        rtNeu.Add(rtNeuValue);
                    }
                }
            }

            c = 0;
            foreach (var idMessung in ids)
            {
                var rtKorr = rtNeu[c];

                UpdateRtLine(connection, idMessung, rtKorr); //

                c++;
            }

        }




        private double BerechneRt(double messRt, double messRtIS)    //rausgezogene Berechnung
        {

            var rtNeuValue = messRt - (Konstanten.RT_SOLL_IS_DEzimal - messRtIS);
            
            rtNeuValue = Math.Round(rtNeuValue, 1);
            
            return rtNeuValue;
        }




        private void UpdateRtLine(SqlConnection connection, int idMessung, double rtKorr)
        {
            var sqlUpdateRow = @"UPDATE dbo.tbPeaks
                        SET 
                            RTkorr = @RTkorr
                        WHERE 
                            ID_Peak = @ID_Peak
            ";


            using (var commandUpdate = connection.CreateCommand())
            {
                commandUpdate.CommandText = sqlUpdateRow;

                commandUpdate.Parameters.AddWithValue("@ID_Peak", idMessung);
                commandUpdate.Parameters.AddWithValue("@RTkorr", rtKorr);          

                var result = commandUpdate.ExecuteNonQuery();

                Log($"Updated {result} lines");
            }
        }

// Berechnung der korrigierten RI Werte
        private void RiKorrigieren(SqlConnection connection)
        {
            var sql_select = @"
                            SELECT
	                            messung.ID_Peak,
	                            messung.RImess,
	                            probeInfo.RI_IS_Pr      -- alle PEAK-IDs, alle RI-Messungen, alle RI-IS-Messungen aller Proben, als 3Spaltentabelle, verknüpft über PKenng, wenn RIkorr leer und RI_IS nicht leer
                            FROM
	                            dbo.tbPeaks messung
	                            LEFT JOIN dbo.tbPInfos probeInfo ON messung.PKenng = probeInfo.PKenng
                            WHERE 
                                messung.RIkorr IS NULL
                                AND probeInfo.RI_IS_Pr IS NOT NULL
                            ";


            var ids = new List<int>();                      //int-Liste wird erstellt
            var riNeu = new List<double>();                 //Double-Liste wird erstellt

            int c = 0;              //int-Counter erstellt mit Null-WErt

            using (var command = connection.CreateCommand())    
            {
                command.CommandText = sql_select;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c++;
                        var id = (int)reader["ID_Peak"];
                        var messRi = (double)reader["RImess"];
                        var messRiIS = (double)reader["RI_IS_Pr"];

                        //hier rechnen
                        var riNeuValue = BerechneRi(messRi, messRiIS);

                        ids.Add(id);
                        riNeu.Add(riNeuValue);
                    }
                }
            }

            c = 0;
            foreach (var idMessung in ids)      // Schleife, um jede ID Zeile zu durchlaufen
            {
                var riKorr = riNeu[c];

                UpdateRiLine(connection, idMessung, riKorr);  //hier Übergabe der Werte an SQL

                c++;
            }

        }


        private double BerechneRi(double messRi, double messRiIS)
        {
            //hier rechnen

           // var messRiRundend = Math.Round(messRi, 0);

            var riNeuValue = messRi - (Konstanten.RI_SOLL_IS_DEzimal - messRiIS);

            riNeuValue = Math.Round(riNeuValue, 0);


            return riNeuValue;
        }


        private void UpdateRiLine(SqlConnection connection, int idMessung, double riKorr)
        {
            var sqlUpdateRow = @"UPDATE dbo.tbPeaks
                        SET 
                            RIkorr = @RIkorr 
                        WHERE 
                            ID_Peak = @ID_Peak
            ";


            using (var commandUpdate = connection.CreateCommand())
            {
                commandUpdate.CommandText = sqlUpdateRow;

                commandUpdate.Parameters.AddWithValue("@ID_Peak", idMessung);
                commandUpdate.Parameters.AddWithValue("@RIkorr", riKorr);          

                var result = commandUpdate.ExecuteNonQuery();

                Log($"Updated {result} lines");
            }
        }


        // Spalte mit den BPMZ-Rt-Namen für jeden Peak
        private void MzRtName(SqlConnection connection)
        {
            var sql_select = @"
                            SELECT
	                            messung.ID_Peak,
	                            messung.RTkorr,
	                            messung.BP_MZ
                            FROM
	                            dbo.tbPeaks messung
	                        WHERE 
                                messung.BPMZ_RT IS NULL
                                AND messung.RTkorr IS NOT NULL
                            ";


            var ids = new List<int>();
            var mzrtNeu = new List<string>();

            int c = 0;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql_select;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c++;
                        var id = (int)reader["ID_Peak"];
                        var korrRt = (double)reader["RTkorr"];
                        var Mz = (double)reader["BP_MZ"];




                        //hier rechnen
                        var mzrtNeuValue = SetztMzRt(korrRt, Mz);    //Sprung in die Berechnung,siehe unten (mit erforderlichen Parametern)

                        ids.Add(id);
                        mzrtNeu.Add(mzrtNeuValue);
                    }
                }
            }

            c = 0;
            foreach (var idMessung in ids)
            {
                string mzrtWert = mzrtNeu[c];

                UpdateMzRtLine(connection, idMessung, mzrtWert); //

                c++;
            }

        }




        private string SetztMzRt(double korrRt, double Mz)    //rausgezogene Berechnung
        {
            Mz = Math.Round(Mz, 0);

            string mzrtNeuValue = (Mz + "-" + korrRt);

            

            return mzrtNeuValue;
        }




        private void UpdateMzRtLine(SqlConnection connection, int idMessung, string mzrtWert)
        {
            var sqlUpdateRow = @"UPDATE dbo.tbPeaks
                        SET 
                            BPMZ_RT = @BPMZ_RT
                        WHERE 
                            ID_Peak = @ID_Peak
            ";


            using (var commandUpdate = connection.CreateCommand())
            {
                commandUpdate.CommandText = sqlUpdateRow;

                commandUpdate.Parameters.AddWithValue("@ID_Peak", idMessung);
                commandUpdate.Parameters.AddWithValue("@BPMZ_RT", mzrtWert);

                var result = commandUpdate.ExecuteNonQuery();

                Log($"Updated {result} lines");
            }
        }


        // Spalte mit den BPMZ-Rt_plus 0,1min-Namen für jeden Peak, aber nicht BWs
        private void MzRtp01Name(SqlConnection connection)
        {
            var sql_select = @"
                            SELECT
	                            messung.ID_Peak,
	                            messung.RTkorr,
	                            messung.BP_MZ
                            FROM
	                            dbo.tbPeaks messung
	                        WHERE 
                                messung.BPMZ_RT_p01 IS NULL
                                AND messung.RTkorr IS NOT NULL
                                AND messung.Type = 'Sample'
                            ";


            var ids = new List<int>();
            var mzrtp01Neu = new List<string>();

            int c = 0;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql_select;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c++;
                        var id = (int)reader["ID_Peak"];
                        var korrRt = (double)reader["RTkorr"];
                        var Mz = (double)reader["BP_MZ"];




                        //hier rechnen
                        var mzrtp01NeuValue = SetztMzRtp01(korrRt, Mz);    //Sprung in die Berechnung,siehe unten (mit erforderlichen Parametern)

                        ids.Add(id);
                        mzrtp01Neu.Add(mzrtp01NeuValue);
                    }
                }
            }

            c = 0;
            foreach (var idMessung in ids)
            {
                string mzrtp01Wert = mzrtp01Neu[c];

                UpdateMzRtp01Line(connection, idMessung, mzrtp01Wert); //

                c++;
            }

        }




        private string SetztMzRtp01(double korrRt, double Mz)    //rausgezogene Berechnung
        {
            Mz = Math.Round(Mz, 0);
            korrRt = korrRt + 0.1;      //hier Korrektur + 0,1 Minute

            string mzrtp01NeuValue = (Mz + "-" + korrRt);



            return mzrtp01NeuValue;
        }




        private void UpdateMzRtp01Line(SqlConnection connection, int idMessung, string mzrtp01Wert)
        {
            var sqlUpdateRow = @"UPDATE dbo.tbPeaks
                        SET 
                            BPMZ_RT_p01 = @BPMZ_RT_p01
                        WHERE 
                            ID_Peak = @ID_Peak
            ";


            using (var commandUpdate = connection.CreateCommand())
            {
                commandUpdate.CommandText = sqlUpdateRow;

                commandUpdate.Parameters.AddWithValue("@ID_Peak", idMessung);
                commandUpdate.Parameters.AddWithValue("@BPMZ_RT_p01", mzrtp01Wert);

                var result = commandUpdate.ExecuteNonQuery();

                Log($"Updated {result} lines");
            }
        }



        // Spalte mit den BPMZ-Rt_minus 0,1min-Namen für jeden Peak, aber nicht BWs
        private void MzRtm01Name(SqlConnection connection)
        {
            var sql_select = @"
                            SELECT
	                            messung.ID_Peak,
	                            messung.RTkorr,
	                            messung.BP_MZ
                            FROM
	                            dbo.tbPeaks messung
	                        WHERE 
                                messung.BPMZ_RT_m01 IS NULL
                                AND messung.RTkorr IS NOT NULL
                                AND messung.Type = 'Sample'
                            ";


            var ids = new List<int>();
            var mzrtm01Neu = new List<string>();

            int c = 0;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql_select;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c++;
                        var id = (int)reader["ID_Peak"];
                        var korrRt = (double)reader["RTkorr"];
                        var Mz = (double)reader["BP_MZ"];




                        //hier rechnen
                        var mzrtm01NeuValue = SetztMzRtm01(korrRt, Mz);    //Sprung in die Berechnung,siehe unten (mit erforderlichen Parametern)

                        ids.Add(id);
                        mzrtm01Neu.Add(mzrtm01NeuValue);
                    }
                }
            }

            c = 0;
            foreach (var idMessung in ids)
            {
                string mzrtm01Wert = mzrtm01Neu[c];

                UpdateMzRtm01Line(connection, idMessung, mzrtm01Wert); //

                c++;
            }

        }




        private string SetztMzRtm01(double korrRt, double Mz)    //rausgezogene Berechnung
        {
            Mz = Math.Round(Mz, 0);
            korrRt = korrRt - 0.1;      //hier Korrektur - 0,1 Minute

            string mzrtm01NeuValue = (Mz + "-" + korrRt);



            return mzrtm01NeuValue;
        }




        private void UpdateMzRtm01Line(SqlConnection connection, int idMessung, string mzrtm01Wert)
        {
            var sqlUpdateRow = @"UPDATE dbo.tbPeaks
                        SET 
                            BPMZ_RT_m01 = @BPMZ_RT_m01
                        WHERE 
                            ID_Peak = @ID_Peak
            ";


            using (var commandUpdate = connection.CreateCommand())
            {
                commandUpdate.CommandText = sqlUpdateRow;

                commandUpdate.Parameters.AddWithValue("@ID_Peak", idMessung);
                commandUpdate.Parameters.AddWithValue("@BPMZ_RT_m01", mzrtm01Wert);

                var result = commandUpdate.ExecuteNonQuery();

                Log($"Updated {result} lines");
            }
        }




        // Spalte mit den BPMZ-RI-Namen für jeden Peak


        private void MzRiName(SqlConnection connection)
        {
            var sql_select = @"
                            SELECT
	                            messung.ID_Peak,
	                            messung.RIkorr,
	                            messung.BP_MZ
                            FROM
	                            dbo.tbPeaks messung
	                        WHERE 
                                messung.BPMZ_RI IS NULL
                                AND messung.RIkorr IS NOT NULL
                            ";


            var ids = new List<int>();
            var mzriNeu = new List<string>();

            int c = 0;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql_select;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c++;
                        var id = (int)reader["ID_Peak"];
                        var korrRi = (int)reader["RIkorr"];
                        var Mz = (double)reader["BP_MZ"];




                        //hier rechnen
                        var mzriNeuValue = SetztMzRi(korrRi, Mz);    //Sprung in die Berechnung,siehe unten (mit erforderlichen Parametern)

                        ids.Add(id);
                        mzriNeu.Add(mzriNeuValue);
                    }
                }
            }

            c = 0;
            foreach (var idMessung in ids)
            {
                string mzriWert = mzriNeu[c];

                UpdateMzRiLine(connection, idMessung, mzriWert); //

                c++;
            }

        }




        private string SetztMzRi(double korrRi, double Mz)    //rausgezogene Berechnung
        {
            Mz = Math.Round(Mz, 2);

            string mzriNeuValue = (Mz + "-" + korrRi);



            return mzriNeuValue;
        }




        private void UpdateMzRiLine(SqlConnection connection, int idMessung, string mzriWert)
        {
            var sqlUpdateRow = @"UPDATE dbo.tbPeaks
                        SET 
                            BPMZ_RI = @BPMZ_RI
                        WHERE 
                            ID_Peak = @ID_Peak
            ";


            using (var commandUpdate = connection.CreateCommand())
            {
                commandUpdate.CommandText = sqlUpdateRow;

                commandUpdate.Parameters.AddWithValue("@ID_Peak", idMessung);
                commandUpdate.Parameters.AddWithValue("@BPMZ_RI", mzriWert);

                var result = commandUpdate.ExecuteNonQuery();

                Log($"Updated {result} lines");
            }
        }





}
/*
    // Ermittlung der Blindwert-Fläche

    private void AreaBW(SqlConnection connection)
    {
        var sql_select = @"
                         SELECT
                            messung.PKenng,
                            messung.RTkorr,
                            messung.BP_MZ,
                            messung.BPMZ_RT,
							messung.BPMZ_RT_p01,
							messung.BPMZ_RT_p02,
							messung.BPMZ_RT_m01,
							messung.BPMZ_RT_m02,
                            messung.AreaP,
                            messung.AreaBW,
                            messung.Type,
                            messung.ID_Peak

                        FROM
                            dbo.tbPeaks messung
                            LEFT JOIN dbo.tbPInfos probeInfo ON messung.PKenng = probeInfo.PKenng

                        WHERE 
                            messung.AreaBW         IS NULL  
                            AND  messung.RTkorr    IS NOT NULL
                            AND  messung.BPMZ_RT   IS NOT NULL
                            
                                
                            ";


        var ids = new List<int>();
        var aBWNeu = new List<double>();

        int c = 0;

        using (var command = connection.CreateCommand())
        {
            command.CommandText = sql_select;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    c++;
                    var id = (int)reader["ID_Peak"];
                    var korrRt = (double)reader["RTkorr"];
                    var messRtIS = (double)reader["RT_IS_Pr"];




                    //hier rechnen
                    var aBWNeuValue = BerechneaBW(messRt, messRtIS);    //Sprung in die Berechnung,siehe unten (mit erforderlichen Parametern)

                    ids.Add(id);
                    aBWNeu.Add(aBWNeuValue);
                }
            }
        }

        c = 0;
        foreach (var idMessung in ids)
        {
            var AreaBW = aBWNeu[c];

            UpdateaBWLine(connection, idMessung, rtKorr); //

            c++;
        }

    }




    private double BerechneaBW(double messRt, double messRtIS)    //rausgezogene Berechnung
    {

        var aBWNeuValue = messRt - (Konstanten.RT_SOLL_IS_DEzimal - messRtIS);

        aBWNeuValue = Math.Round(aBWNeuValue, 1);

        return aBWNeuValue;
    }




    private void UpdateaBWLine(SqlConnection connection, int idMessung, double AreaBW)
    {
        var sqlUpdateRow = @"UPDATE dbo.tbPeaks
                        SET 
                            AreaBW = @AreaBW
                        WHERE 
                            ID_Peak = @ID_Peak
            ";


        using (var commandUpdate = connection.CreateCommand())
        {
            commandUpdate.CommandText = sqlUpdateRow;

            commandUpdate.Parameters.AddWithValue("@ID_Peak", idMessung);
            commandUpdate.Parameters.AddWithValue("@AreaBW", AreaBW);

            var result = commandUpdate.ExecuteNonQuery();

            Log($"Updated {result} lines");
        }
    }


    // BW Peak von ProbenPeak abziehen, im Bereich von +/- 0,2min, wenn Werte vorhanden, sonst Null, Kontrolle ob 2x größer, Zelle leer, wenn Werte fehlen
    /*
     
    private void PeaksMinusBW(SqlConnection connection)
    {
        int cc = 0;

        var sql_select = @"
                        SELECT
                            messung.PKenng,
                            messung.RTkorr,
                            messung.BP_MZ,
                            messung.BPMZ_RT,
                            messung.AreaP,
                            messung.Type,
                            messung.ID_Peak
                        FROM
                            dbo.tbPeaks messung
                            LEFT JOIN dbo.tbPInfos probeInfo ON messung.PKenng = probeInfo.PKenng

                        WHERE 
                            messung.Peak_minus_BW  IS NULL  --die Zahl 0 bedeutet BWPeak, leere Zelle kann zwei Bedeutungen haben: 1.werte zur Berechung fehlen, 2.neuerEintrag der noch in die Berechnung muss
                                                            --für fehlende Werte ein FW eintragen, dann kann später, danach selektiert werden, ist das gut?
                            AND  messung.RTkorr    IS NOT NULL
                            AND  messung.BPMZ_RT   IS NOT NULL

                        ";


        var ids = new List<int>();
        var PeakminusBWNeu = new List<string>();

        int c = 0;

        using (var command = connection.CreateCommand())
        {
            command.CommandText = sql_select;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    c++;
                    var id = (string)reader["Pkenng"];
                    var korrRt = (double)reader["RTkorr"];
                    var Mz = (double)reader["BP_MZ"];
                    var Mz_Rt = (string)reader["BPMZ_RT"];
                    var PK_Area = (double)reader["AreaP"];



                    var PeakminusBWNeuValue = Sub_PK_BW(korrRt, Mz, Mz_Rt,PK_Area);    //Sprung in die Berechnung,siehe unten (mit erforderlichen Parametern)

                    ids.Add(id);
                    PeakminusBWNeu.Add(PeakminusBWNeuValue);
                }
            }
        }

        c = 0;
        foreach (var PKenng in ids)
        {
            double PeakminusBW_Wert = PeakminusBWNeu[c];

            UpdatePeakminusBWLine(connection, PKenng, PeakminusBW_Wert);

            c++;
        }

    }




    private string Sub_PK_BW(double korrRt, double Mz, string Mz_Rt, double PK_Area)    //rausgezogene Berechnung
    {
        Mz = Math.Round(Mz, 0);

        double PeakminusBWNeuValue = (Mz + "-" + korrRt);



        return PeakminusBWNeuValue;
    }




    private void UpdatePeakminusBWLine(SqlConnection connection, int idMessung, double PeakminusBW_Wert)
    {
        var sqlUpdateRow = @"UPDATE dbo.tbPeaks
                    SET 
                        Peak_minus_BW = @Peak_minus_BW
                    WHERE 
                        PKenng = @PKenng
        ";


        using (var commandUpdate = connection.CreateCommand())
        {
            commandUpdate.CommandText = sqlUpdateRow;

            commandUpdate.Parameters.AddWithValue("@ID_Messung", idMessung);
            commandUpdate.Parameters.AddWithValue("@Peak_minus_BW", PeakminusBW_Wert);

            var result = commandUpdate.ExecuteNonQuery();

            Log($"Updated {result} lines");
        }
    }





    
    */


}
