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

        public void Import(Action<string> log, string fileName, string connectionString)
        {
            Log = log;

            if (!string.IsNullOrEmpty(fileName)) // ! = not
            {
                ProcessImport(fileName, connectionString);
            }
        }


        private void ProcessImport(string filename, string connectionString)  //void bedeutet, es kommt etwas rein,aber nichts raus
        {
            Log("Importing " + filename);       //neuer LogEintrag

            var linesFromFile = File.ReadAllLines(filename);    //erstellt Zeilenobjekt (Array oder Liste)

            var lines = linesFromFile                       //hier wird zeilenweise geprüft
                .Where(line => !string.IsNullOrEmpty(line)) //Zeile ist nicht 0 oder leer
                .Skip(1)                                    //Überspringt x Zeilen, z.B. Überschrift: Skip(1)
                .ToArray();                                 //Übergabe an Array

            Log("Opning SQL connection");       //neuer LogEintrag

            var sqlConnection = new SqlConnection(connectionString);  //definert Datenbankobjekt und verbindet zur entsprechenden DB
            sqlConnection.Open();               //öffnet gewählte DB

            using (var transaction = sqlConnection.BeginTransaction())  //Starten Datenübergabe in ein ÜbergabeObjekt
            {
                try
                {
                    Log("Importing lines: " + lines.Length);       //zählt die übertragenen Zeilen
                   
                    foreach (var line in lines)                    //überträgt zeilenweise in einen Zwischenspeicherobjekt mittels Schleife
                    {                       
                        ImportLine(line, sqlConnection, transaction);
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
       ID_cas
      ,CAS_NIST_name
      ,InChiKey
      ,Woher
      ,EC_No
      ,Norman_SusDat_ID
      ,Import_Date
      )
VALUES ( @ID_cas, @CAS_NIST_name, @InChiKey, @Woher, @EC_No, @Norman_SusDat_ID, @Import_Date)
";
            //Spalten in der CSV_Datei
            //ID_cas [0] CAS_NIST_name[1] InChiKey[2] Woher[3] EC_No[4] Norman_SusDat_ID[5]
            //	

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

                    command.Parameters.AddWithValue("@ID_cas", lineItems[0]);
                    command.Parameters.AddWithValue("@CAS_NIST_name", lineItems[1]);//CAS_NIST_name mit Vorsatz
                    command.Parameters.AddWithValue("@InChiKey", lineItems[2]);//
                    command.Parameters.AddWithValue("@Woher", lineItems[3]);//                                                           //
                    command.Parameters.AddWithValue("@EC_No", lineItems[4]);//EC Nummer
                    command.Parameters.AddWithValue("@Norman_SusDat_ID", lineItems[5]);//

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
