using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DbImportExport.Importer.UpdateValues
{
    class MzRtNamePlusKlasse
    {
        private Action<string> Log;
        public MzRtNamePlusKlasse(Action<string> log)
        {
            Log = log;
        }

        //MzRtNamePlus(connection)
        public void MzRtNamePlus(SqlConnection connection)
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
            var mzrtp02Neu = new List<string>();
            var mzrtm01Neu = new List<string>();
            var mzrtm02Neu = new List<string>();

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

                        var mzrtp01NeuValue = SetztMzRtp01(korrRt, Mz);    //Sprung in die Berechnung,siehe unten (mit erforderlichen Parametern)
                        var mzrtp02NeuValue = SetztMzRtp02(korrRt, Mz);
                        var mzrtm01NeuValue = SetztMzRtm01(korrRt, Mz);
                        var mzrtm02NeuValue = SetztMzRtm02(korrRt, Mz);

                        ids.Add(id);
                        mzrtp01Neu.Add(mzrtp01NeuValue);
                        mzrtp02Neu.Add(mzrtp02NeuValue);
                        mzrtm01Neu.Add(mzrtm01NeuValue);
                        mzrtm02Neu.Add(mzrtm02NeuValue);
                    }
                }
            }

            c = 0;
            foreach (var idMessung in ids)
            {
                string mzrtp01Wert = mzrtp01Neu[c];
                string mzrtp02Wert = mzrtp02Neu[c];
                string mzrtm01Wert = mzrtm01Neu[c];
                string mzrtm02Wert = mzrtm02Neu[c];

                UpdateMzRtp01Line(connection, idMessung, mzrtp01Wert, mzrtp02Wert, mzrtm01Wert, mzrtm02Wert); //

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
        private string SetztMzRtp02(double korrRt, double Mz)    //rausgezogene Berechnung
        {
            Mz = Math.Round(Mz, 0);
            korrRt = korrRt + 0.2;      //hier Korrektur + 0,2 Minute
            string mzrtp02NeuValue = (Mz + "-" + korrRt);
            return mzrtp02NeuValue;
        }
        private string SetztMzRtm01(double korrRt, double Mz)    //rausgezogene Berechnung
        {
            Mz = Math.Round(Mz, 0);
            korrRt = korrRt - 0.1;      //hier Korrektur - 0,1 Minute
            string mzrtm01NeuValue = (Mz + "-" + korrRt);
            return mzrtm01NeuValue;
        }
        private string SetztMzRtm02(double korrRt, double Mz)    //rausgezogene Berechnung
        {
            Mz = Math.Round(Mz, 0);
            korrRt = korrRt - 0.2;      //hier Korrektur - 0,2 Minute
            string mzrtm02NeuValue = (Mz + "-" + korrRt);
            return mzrtm02NeuValue;
        }

        private void UpdateMzRtp01Line(SqlConnection connection, int idMessung, string mzrtp01Wert, string mzrtp02Wert, string mzrtm01Wert, string mzrtm02Wert)
        {
            var sqlUpdateRow = @"UPDATE dbo.tbPeaks
                        SET 
                            BPMZ_RT_p01 = @BPMZ_RT_p01,
                            BPMZ_RT_p02 = @BPMZ_RT_p02,
                            BPMZ_RT_m01 = @BPMZ_RT_m01,
                            BPMZ_RT_m02 = @BPMZ_RT_m02
                        WHERE 
                            ID_Peak = @ID_Peak
            ";

            using (var commandUpdate = connection.CreateCommand())
            {
                commandUpdate.CommandText = sqlUpdateRow;

                commandUpdate.Parameters.AddWithValue("@ID_Peak", idMessung);
                commandUpdate.Parameters.AddWithValue("@BPMZ_RT_p01", mzrtp01Wert);
                commandUpdate.Parameters.AddWithValue("@BPMZ_RT_p02", mzrtp02Wert);
                commandUpdate.Parameters.AddWithValue("@BPMZ_RT_m01", mzrtm01Wert);
                commandUpdate.Parameters.AddWithValue("@BPMZ_RT_m02", mzrtm02Wert);

                var result = commandUpdate.ExecuteNonQuery();

                Log($"Updated {result} lines");
            }
        }


    }
}
