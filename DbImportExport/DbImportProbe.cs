using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;


namespace DbImportExport //Name der Funktion
{
    public class DbImportProbe
    {
        private Action<string> Log;

        public void Import(Action<string> log)
        {
            Log = log;

            // Datei auswählen - test
            string fileName = null;


            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileName = dialog.FileName;
            }


           
            if (!string.IsNullOrEmpty(fileName)) // ! = not , < = kl, > = gr, == ist gleichheitsvergleich, = ist zuweisung
            {
                // Import Verarbeitung
                ProcessImport(fileName);
            }
        }

        private void ProcessImport(string filename)
        {
            Log("Importing " + filename);

            var lines = File.ReadAllLines(filename)
                .Where(line => !string.IsNullOrEmpty(line))
                .Skip(1) //Überspringt x Zeilen, z.B. Überschrift: Skip(1)
                .ToList();

            Log("Opning SQL connection");

            var sqlConnection = new SqlConnection("Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; ");
            sqlConnection.Open();

            Log("Importing lines: " + lines.Count);

            foreach (var line in lines)
            {
                Log("Importing: " + line);

                try
                {
                    ImportLine(line, sqlConnection);
                }
                catch(Exception ex)
                {
                    Log("Error importing: " + line + ". Message: " + ex.Message + " AT:" + ex.StackTrace );
                }
            }
        }

        private void ImportLine(string line, SqlConnection connection)
        {
            var sql = @"
INSERT INTO dbo.Probe
(Lims_Nr, 
PN_Datum, 
Ort_kurz,
Pr_Vorbereitung, 
Pr_Vol_ml, 
Verd_Faktor,
IS_in_ml)
VALUES (@P2, @P3, @P4, @P5, @P6, @P7, @P8)
";
            var lineItems = line.Split(new[] { ';' });



            Log("Items:" + lineItems.Length);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;

                // command.Parameters.AddWithValue("@P1", lineItems[24]);//ID_Probe
                command.Parameters.AddWithValue("@P2", (int)(ToDecimal(lineItems[0])));//Lims_Nr
                command.Parameters.AddWithValue("@P3", lineItems[1]);//PN_Datum
                command.Parameters.AddWithValue("@P4", lineItems[2]);//Ort_kurz
                command.Parameters.AddWithValue("@P5", lineItems[3]);//Pr_Vorbereitung
                command.Parameters.AddWithValue("@P6", (int)(ToDecimal(lineItems[4])));//Pr_Vol_ml
                command.Parameters.AddWithValue("@P7", (ToDecimal(lineItems[5])));//Verd_Faktor
                command.Parameters.AddWithValue("@P8", (ToDecimal(lineItems[6])));//IS_in_ml
                

                command.ExecuteNonQuery();

               
            }

        }

        private int ToInt(string value)
        {
            if (!int.TryParse(value, out var result))
            {
                result = 0;
            }

            return result;
        }

        private decimal ToDecimal(string value)
        {
            value = value.Replace('.', ',');

            if (!decimal.TryParse(value, out var result))
            {
                result = 0;
            }

            return result;
        }



        private decimal? ToNullableDecimal(string value)
        {
            value = value.Replace('.', ',');

            if (!decimal.TryParse(value, out var result))
            {
                return null;
            }

            return result;
        }





    }
}

