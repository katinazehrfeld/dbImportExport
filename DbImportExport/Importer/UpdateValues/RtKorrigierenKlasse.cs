using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DbImportExport.Importer.UpdateValues
{
    // Berechnung der korrigierten Rt Werte
    internal class RtKorrigierenKlasse
    {
        private Action<string> Log;
        public RtKorrigierenKlasse(Action<string> log)
        {
            Log = log;
        }



        public void RtKorrigieren(SqlConnection connection)
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
    }
}
