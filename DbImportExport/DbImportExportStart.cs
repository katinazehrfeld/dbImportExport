using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbImportExport
{
    public class DbImportExportStart
    {
        private Action<string> Log;

        public void Import(Action<string> log)
        {
            Log = log;

            // Datei auswählen
            string fileName = null;

            /*
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileName = dialog.FileName;
            }
            */

            fileName = "C:\\temp\\testValues.csv";


            if (!string.IsNullOrEmpty(fileName)) // ! = not , < = kl, > = gr, == ist gleichheitsvergleich, = ist zuweisung
            {
                // Import Verarbeitung
                ProcessImport(fileName);
            }
        }

        public void Export(Action<string> log) {
            Log = log;
        }


        private void ProcessImport(string filename)
        {
            Log("Importing " + filename);

            var lines = File.ReadAllLines(filename)
                .Where(line => !string.IsNullOrEmpty(line))
                .Skip(0) //Überspringt x Zeilen, z.B. Überschrift: Skip(1)
                .ToList();

            Log("Opning SQL connection");

            var sqlConnection = new SqlConnection("Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; ");
            sqlConnection.Open();

            Log("Importing lines: " + lines.Count);

            foreach (var line in lines)
            {
                ImportLine(line, sqlConnection);
            }
        } 

        private void ImportLine(string line, SqlConnection connection)
        {
            var sql = @"
INSERT INTO dbo.Peak 
(BWArea, BasePeakArea, BezeichnungVorschlag)
VALUES (@P1, @P2, @P3)
";

            var lineItems = SplitLine(line);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;

                command.Parameters.AddWithValue("@P1", lineItems[0]);
                command.Parameters.AddWithValue("@P2", lineItems[1]);
                command.Parameters.AddWithValue("@P3", lineItems[2]);

                command.ExecuteNonQuery();
            }
        }

        private object[] SplitLine(string line)
        {
            var splitted = line.Split(new[] { ';' });


            var number = -1;
            if (!string.IsNullOrEmpty(splitted[1]))
            {
                number = int.Parse(splitted[1]);
            }


            var text1 = splitted[0];
            text1 = text1.Replace('-', 'X');


            return new object[]
            {
                text1,
                number,
                splitted[2]
            };
        }










    }
}
