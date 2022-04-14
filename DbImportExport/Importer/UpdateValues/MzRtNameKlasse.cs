using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DbImportExport.Importer.UpdateValues
{
    class MzRtNameKlasse
    {
        private Action<string> Log;
        public MzRtNameKlasse(Action<string> log)
        {
            Log = log;
        }

        // Spalte mit den BPMZ-Rt-Namen für jeden Peak
        public void MzRtName(SqlConnection connection)
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
    }
}
