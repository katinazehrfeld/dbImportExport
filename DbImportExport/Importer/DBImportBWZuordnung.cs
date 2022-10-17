using System;                           //globale geladene Funktionen
using System.Collections.Generic;       //vereinfachen später den Code
using System.Data.SqlClient;            //weil verkürzte Schreibweise dadurch möglich
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbImportExport.Importer        //Namensklasse in der keine Namensgleichheit vorkommen darf
{
    public class DBImportBWZuordnung    // auf eine public class kann von außen zugegriffen werden
    {
        private Action<string> Log; // private ist nur in dieser public class ansprechbar
                                    // erzeugt einen Log string
        public void Import(Action<string> log, string fileName, string connectionString)
        {
            Log = log;
            if (!string.IsNullOrEmpty(fileName))
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
                        Log("Importing: " + line);
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
        {   //das braune ist sql Code, deshalb die andere Notation (zB: Kommentare --)
            var sql = @"   -- definiert Spalten
INSERT INTO dbo.tbBWZuordg     -- definiert in welche Tabelle der DB die Daten übertragen werden
      (
       PKenng
      ,BWZuordg
      ,Alkane_Zuordg
      ,File_mess
      ,Acq_Date_Time
      ,Import_Date
       )
VALUES ( @PKenng, @BWZuordg, @Alkane_Zuordg, @File_mess, @Acq_Date_Time, @Import_Date)
";
            var lineItems = SplitSpecial(line);  //Code zu SplitSpzial siehe weiter unten 

            int lineCount = 0;
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;

                command.CommandText = "SELECT count(*) FROM dbo.tbBWZuordg WHERE PKenng = @PKenng";
                command.Parameters.AddWithValue("@PKenng", lineItems[0]);//Pr_Kennung
                                                                         // die Zahl bei "lineItems[8]" in den eckigen Klammern gibt an aus welcher Spalte der csvDatei die Daten eingelesen werden sollen (1.Spalte=0, 2.Sp =1,...)


                lineCount = (int)command.ExecuteScalar();
            }
            if (lineCount == 0)
            {
                Log("Items:" + lineItems.Length);
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;

                    command.CommandText = sql;

                    command.Parameters.AddWithValue("@PKenng", lineItems[0]);//Pr_Kennung
                    command.Parameters.AddWithValue("@BWZuordg", lineItems[1]);//BW_Zuordnung
                    command.Parameters.AddWithValue("@Alkane_Zuordg", lineItems[2]);//Alkane_Zuordg
                    command.Parameters.AddWithValue("@File_mess", lineItems[3]);//File_name
                    command.Parameters.AddWithValue("@Acq_Date_Time", lineItems[6]);//Acq_Date_Time

                    command.Parameters.AddWithValue("@Import_Date", DateTime.Now);

                    command.ExecuteNonQuery();

                    /*
                      mögliche KonvertierungsBeispiele
                      var x = ToFloat("1.23");
                      command.Parameters.AddWithValue("@P3", Math.Round( 66.66));//BPMZ_Pr
                      command.Parameters.AddWithValue("@PKenng", (ToDecimal(lineItems[14])));
                      command.Parameters.AddWithValue("@PKenng", ConverterTool.ToBool(lineItems[0]));//Pr_Kennung
                      command.Parameters.AddWithValue("@BWZuordg", ConverterTool.ToDecimal(lineItems[1]));//BW_Zuordnung
                      command.Parameters.AddWithValue("@Formula", (int)(ToDecimal(lineItems[9])));
                      command.Parameters.AddWithValue("@SName", (Int64)(ToDecimal(lineItems[8])))
                    */
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
