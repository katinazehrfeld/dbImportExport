using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;



namespace DbImportExport.Importer
{
    public class DBImportCAS
    {
        private Action<string> Log;

        public void Import(Action<string> log)
        {
            Log = log;

            string fileName = null;
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileName = dialog.FileName;
            }
            if (!string.IsNullOrEmpty(fileName)) // ! = not
            {
                ProcessImport(fileName);
            }
        }


        private void ProcessImport(string filename)  //void bedeutet, es kommt etwas rein,aber nichts raus
        {
            Log("Importing " + filename);       //neuer LogEintrag

            var linesFromFile = File.ReadAllLines(filename);    //erstellt Zeilenobjekt (Array oder Liste)

            var lines = linesFromFile                       //hier wird zeilenweise geprüft
                .Where(line => !string.IsNullOrEmpty(line)) //Zeile ist nicht 0 oder leer
                .Skip(1)                                    //Überspringt x Zeilen, z.B. Überschrift: Skip(1)
                .ToArray();                                 //Übergabe an Array

            Log("Opning SQL connection");       //neuer LogEintrag

            var sqlConnection = new SqlConnection("Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; ");  //definert Datenbankobjekt und verbindet zur entsprechenden DB
            sqlConnection.Open();               //öffnet gewählte DB

            using (var transaction = sqlConnection.BeginTransaction())  //Starten Datenübergabe in ein ÜbergabeObjekt
            {
                try
                {
                    Log("Importing lines: " + lines.Length);       //zählt die übertragenen Zeilen
                    int c = 0;
                    var perc = lines.Length / 100;
                    foreach (var line in lines)                    //überträgt zeilenweise in einen Zwischenspeicherobjekt mittels Schleife
                    {
                        c++;
                        ImportLine(line, sqlConnection, transaction);

                        if (c % perc == 0)
                        {
                            Log($"Done {c / perc }%");
                        }

                    }

                    transaction.Commit();                          //speichert jede neue Zeile
                }
                catch
                {
                    transaction.Rollback();                        //überträgt nichts an die DB, falls der Datensatz einen Fehler hat 
                    throw;
                }
            }
        }
        private void ImportLine(string line, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = @"
INSERT INTO dbo.tb_cas_ikey 
      (      
       CAS_NIST_name
      ,InChiKey
      ,EC_No
      ,Import_Date
      )
VALUES ( @CAS_NIST_name, @InChiKey, @EC_No, @Import_Date)
";
            //Spalten in der CSV_Datei
            //EC_No[0] CAS_NIST_mit_Vorsatz[1] CAS_NIST[2] InChiKey[3] t[4]	LIB[5]	

            var lineItems = SplitSpecial(line);  //Code zu SplitSpzial siehe weiter unten 

            int lineCount = 0;
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "SELECT count(*) FROM dbo.tb_cas_ikey WHERE CAS_NIST_name = @CAS_NIST_name";
                command.Parameters.AddWithValue("@CAS_NIST_name", lineItems[1]);

                lineCount = (int)command.ExecuteScalar();
            }

            if (lineCount == 0)
            {
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;

                    command.CommandText = sql;
                    // die Zahl bei "lineItems[8]" in den eckigen Klammern gibt an aus welcher Spalte der csvDatei die Daten eingelesen werden sollen (1.Spalte=0, 2.Sp =1,...)

                    command.Parameters.AddWithValue("@CAS_NIST_name", lineItems[1]);//CAS_NIST_name mit Vorsatz
                    command.Parameters.AddWithValue("@InChiKey", lineItems[3]);//InChiKey
                    command.Parameters.AddWithValue("@EC_No", lineItems[0]);//EC Nummer
                    
                    command.Parameters.AddWithValue("@Import_Date", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        private string[] SplitSpecial(string line)  // Tool um die Stoffnamen, die auch oft Kommata enthalten, von den SpaltenKommata zu unterscheiden 
        {
            var values = line.Split(',');           //zerlegt eine Zeile in Teilstücke, getrennt durch Kommas

            List<string> result = new List<string>(); //erzeugt eine leere Liste

            string textString = null;                 //erzeugt ein leeres Teilstück

            foreach (var value in values)        // für jedes Teilstück der Zeile
            {
                if (textString != null)         // wenn Teilstück nicht leer ist
                {
                    if (value.EndsWith("\""))   // und wenn Teilstück am Ende " hat
                    {
                        textString = textString + "," + value.TrimEnd('"'); //dann vorhandenes Teilstück + Komma + aktuelles Teilstück + "
                        result.Add(textString);         // und in Liste einfügen
                        textString = null;              // Teilstück auf leer setzen
                    }
                    else                       // wenn nicht " am Ende, 
                    {
                        textString = textString + "," + value; //dann bisheriges Textstück + Komma + nächstes Textstück
                    }
                }
                else    // wenn Teilstück noch leer ist
                {
                    if (value.StartsWith("\"") && !value.EndsWith("\""))    //wenn Teilstück mit " beginnt und Nicht mit " endet
                    {
                        textString = value.Substring(1);                    //dann Teilstück minus erstes Zeichen in Teilstück speichern
                    }
                    else
                    {
                        result.Add(value.TrimStart('"').TrimEnd('"'));      //sonst von Teilstück vorne und hinten " abtrennen und speichern
                    }
                }
            }
            if (textString != null)             // wenn Teilstück nicht leer ist und keine Semikolons
            {
                result.Add(textString);         // dann Teilstück speichern in "result"
            }
            return result.ToArray();            // "result" an Array übergeben
        }
    }
}
