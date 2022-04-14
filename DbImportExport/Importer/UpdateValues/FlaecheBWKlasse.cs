using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbImportExport.Importer.UpdateValues
{
    class FlaecheBWKlasse
    {
        private Action<string> Log;
        public FlaecheBWKlasse(Action<string> log)
        {
            Log = log;
        }



        // Ermittlung der Blindwert-Fläche

        public void FlaecheBW(SqlConnection connection)
        {
            var sql_select = @"
                             SELECT
                                messung.AreaP,
                                messung.AreaBW,
                                messung.Type,
                                messung.ID_Peak

                            FROM
                                dbo.tbPeaks messung
                                
                            WHERE 
                                messung.AreaBW         IS NULL  

                                AND  messung.RTkorr    IS NOT NULL
                                AND  messung.AreaP     IS NOT NULL
                                AND  messung.Type      = 'Blank'
                                ";


            var ids = new List<int>();
            var BWNeu = new List<double>();

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
                        var AreaP = (double)reader["AreaP"];

                        //so...
                        var FlaecheBW = reader["AreaBW"] == DBNull.Value
                            ? (double)0
                            : (double)reader["AreaBW"];


                        //oder so...
                        var o = reader["AreaBW"];

                        if (o == DBNull.Value)
                        {
                        }
                        else
                        {

                        }

                        // oder so...
                        var x = o == DBNull.Value
                            ? 0
                            : (double)o;




                        //hier rechnen
                        var BWNeuValue = AreaP;

                        ids.Add(id);
                        BWNeu.Add(BWNeuValue);
                    }
                }
            }

            c = 0;
            foreach (var idMessung in ids)
            {
                var AreaBW = BWNeu[c];

                UpdateBWLine(connection, idMessung, AreaBW); //

                c++;
            }
        }


        private void UpdateBWLine(SqlConnection connection, int idMessung, double AreaBW)
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


    }
}
