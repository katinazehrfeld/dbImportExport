using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbImportExport.Importer
{
    public class DBImportProbeninfo
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
        {
            var sql = @"
INSERT INTO dbo.tbPInfos 
      (      
       PKenng
      ,Thema
      ,LM
      ,Vorbereitung_Meth
      ,RT_IS_Pr
      ,RI_IS_Pr
      ,IS_AreaP
      ,IS_AreaBP
      ,V_Extraktion_mL
      ,Verdg_im_Vial
      ,IS_Volumen_ml
      ,InjektionsVolumen_ml
      ,Import_Date
      )
VALUES ( @PKenng, @Thema, @LM, @Vorbereitung_Meth, @RT_IS_Pr, @RI_IS_Pr, @IS_AreaP, @IS_AreaBP, @V_Extraktion_mL, @Verdg_im_Vial, @IS_Volumen_ml, @InjektionsVolumen_ml, @Import_Date)
";
      //Spalten in der CSV_Datei
      //Pr_Kennung[0] Thema[1] LM[2] Vorbereitung_Meth[3] RT_IS_Pr[4]	RI_IS_Pr[5]	IS_Area_Peak[6]	IS_Area_BasePeak[7]	V_Extraktion_mL[8]
      //Verdg_im_Vial[9] IS_Volumen_ml[10] InjektionsVolumen_ml[11]

            var lineItems = SplitSpecial(line);  //Code zu SplitSpzial siehe weiter unten 

            Log("Items:" + lineItems.Length);

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;

                command.CommandText = sql;
                // die Zahl bei "lineItems[8]" in den eckigen Klammern gibt an aus welcher Spalte der csvDatei die Daten eingelesen werden sollen (1.Spalte=0, 2.Sp =1,...)
               
                command.Parameters.AddWithValue("@PKenng", lineItems[0]);//Pr_Kennung
                command.Parameters.AddWithValue("@Thema", lineItems[1]);//Thema
                command.Parameters.AddWithValue("@LM", lineItems[2]);//Lösungsmittel bei der Extraktion
                command.Parameters.AddWithValue("@Vorbereitung_Meth", lineItems[3]);//Vorbereitung_Methode (SPE,gesch,...)

                command.Parameters.AddWithValue("@RT_IS_Pr", ConverterTool.ToDecimal(lineItems[4]));//RetentionTime des IS in der Probe
                command.Parameters.AddWithValue("@RI_IS_Pr", ConverterTool.ToDecimal(lineItems[5]));//RetentionIndex des IS in der Probe
                command.Parameters.AddWithValue("@IS_AreaP", ConverterTool.ToDecimal(lineItems[6]));//PeakFläche des IS
                command.Parameters.AddWithValue("@IS_AreaBP", ConverterTool.ToDecimal(lineItems[7]));//BasePeakFläche des IS
                
                command.Parameters.AddWithValue("@V_Extraktion_mL", ConverterTool.ToDecimal(lineItems[8]));//ExtraktionsVolumen in mL
                command.Parameters.AddWithValue("@Verdg_im_Vial", ConverterTool.ToDecimal(lineItems[9]));//Verdünnung_im_Vial
                command.Parameters.AddWithValue("@IS_Volumen_ml", ConverterTool.ToDecimal(lineItems[10]));//dosiertes IS_Volumen in ml zur Probe
                command.Parameters.AddWithValue("@InjektionsVolumen_ml", ConverterTool.ToDecimal(lineItems[11]));//InjektionsVolumen_ml_in den GC
                
                command.Parameters.AddWithValue("@Import_Date", DateTime.Now);

                command.ExecuteNonQuery();
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
