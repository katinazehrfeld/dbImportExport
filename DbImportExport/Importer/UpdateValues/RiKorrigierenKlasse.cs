using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbImportExport.Importer.UpdateValues
{
    class RiKorrigierenKlasse
    {
        private Action<string> Log;
        public RiKorrigierenKlasse(Action<string> log)
        {
            Log = log;
        }

        // Berechnung der korrigierten RI Werte
        public void RiKorrigieren(SqlConnection connection)
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

                        var riNeuValue = BerechneRi(messRi, messRiIS);  // Sprung in Berechnung

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

    }
}
