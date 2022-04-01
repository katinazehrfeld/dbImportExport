using System;                           //globale geladene Funktionen
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




            ////RT(korrigiert) berechnen und als neue Spalte in SQL einfügen
            var connection = new SqlConnection("Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; ");  //verbindet zur entsprechenden DB
            connection.Open();               //öffnet gewählte DB

            log("Updating data");



            RtKorrigieren(connection);
            RiKorrigieren(connection);
            MzRtName(connection);
            MzRiName(connection);
            //Peak_BWok(connection);
        }





// Berechnung der korrigierten Rt Werte

        private void RtKorrigieren(SqlConnection connection)
        {
            var sql_select = @"
                            SELECT
	                            messung.ID_Peak,
	                            messung.RTmess,
	                            probeInfo.RT_IS_Pr
                                messung.BWok
                            FROM
	                            dbo.tbPeaks messung
	                            LEFT JOIN dbo.tbPInfos probeInfo ON messung.PKenng = probeInfo.PKenng
                            WHERE 
                                messung.RTkorr IS NULL
                                AND messung.BWok IS NULL
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
	                            probeInfo.RI_IS_Pr 
                            FROM
	                            dbo.tbPeaks messung
	                            LEFT JOIN dbo.tbPInfos probeInfo ON messung.PKenng = probeInfo.PKenng
                            WHERE 
                                messung.RIkorr IS NULL
                                AND probeInfo.RI_IS_Pr IS NOT NULL
                            ";


            var ids = new List<int>();
            var riNeu = new List<double>();

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
            foreach (var idMessung in ids)
            {
                var riKorr = riNeu[c];

                UpdateRiLine(connection, idMessung, riKorr);

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
}
