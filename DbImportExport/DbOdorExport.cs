using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbImportExport
{
    public class DbOdorExport
    {
        private Action<string> Log;
 
        public void Export(Action<string> log, string query) {
            Log = log;

            List<string> lines = GetLinesFromDb(query);

            Log("Got lines: " + lines.Count);

            var filename = "c:\\temp\\output.csv";
            
/*            var dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filename = dialog.FileName;
            }
*/

            Log("Saving output to: " + filename);

            File.WriteAllLines(filename, lines);
        }

        private List<string> GetLinesFromDb(string query)
        {
            Log("Opening SQL connection");

            var connection = new SqlConnection("Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; ");
            connection.Open();

            var sql = @"
SELECT BWArea, BasePeakArea, BezeichnungVorschlag 
FROM dbo.Peak 
WHERE BezeichnungVorschlag = @P1
";

            List<string> result = new List<string>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue("@P1", query);

                var reader = command.ExecuteReader();


                while (reader.Read())
                {
                    var area = (string) reader["BWArea"];
                    area = area.Trim();


                    var peakArea = reader["BasePeakArea"];
                    var bezeichnerVorschlag = reader["BezeichnungVorschlag"];

                    var resultLine = $"{area};{peakArea};{bezeichnerVorschlag}";

                    result.Add(resultLine);
                }

                return result;
            }
        }
    }
}
