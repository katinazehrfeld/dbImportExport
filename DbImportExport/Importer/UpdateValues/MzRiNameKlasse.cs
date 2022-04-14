using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbImportExport.Importer.UpdateValues
{
    class MzRiNameKlasse
    {
        private Action<string> Log;
        public MzRiNameKlasse(Action<string> log)
        {
            Log = log;
        }

        // Spalte mit den BPMZ-RI-Namen für jeden Peak


        public void MzRiName(SqlConnection connection)
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
