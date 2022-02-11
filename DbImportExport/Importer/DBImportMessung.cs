using System;                           //globale geladene Funktionen
using System.Collections.Generic;       //vereinfachen später den Code
using System.Data.SqlClient;            //weil verkürzte Schreibweise dadurch möglich
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbImportExport.Importer        //Namensklasse in der keine Namensgleichheit vorkommen darf
{
    public class DBImportMessung    // auf eine public class kann von außen zugegriffen werden
    {
        private Action<string> Log; // private ist nur in dieser public class ansprechbar
                                    // erzeugt einen Log string
        public void Import(Action<string> log)  //öffentliche Ausgabe des Import Log 
        {
            Log = log;              // zeigt die erfolgten ToDos und Fehler

            // Datei auswählen - test
            string fileName = null;
            var dialog = new OpenFileDialog();      //öffnet Dateiauswahl Fenster
            if (dialog.ShowDialog() == DialogResult.OK) //Schleife: wenn ok gedrückt Dateiname übernehmen
            {
                fileName = dialog.FileName;
            }
            // Bsp: fileName = "C:\\Daten\\TeslaProben\\alle_nachUA_CSV\\62005831.csv";
            // Schleife weiter: wenn NICHT 0 oder leer, gehe zu ProcessImport
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
        {   //das braune ist sql Code, deshalb die andere Notation (zB: Kommentare --)
            var sql = @"   -- definiert Spalten
INSERT INTO dbo.UA_csv     -- definiert in welche Tabelle der DB die Daten übertragen werden
      (
                            -- Best_Hit,
      Component_RT
      ,Base_Peak_MZ
      ,CAS
      ,Library_RI
      ,Component_RI
      ,Match_Factor
      ,Compound_Name
      ,Formula
      ,Library_File
      ,Component_Area
      ,Base_Peak_Area
      ,Type
      ,Comment
      ,Import_Date)
VALUES (  @Component_RT, @Base_Peak_MZ, @CAS, @Library_RI, @Component_RI, @Match_Factor, @Compound_Name, @Formula, @Library_File, @Component_Area, @Base_Peak_Area, @Type, @Comment, @Import_Date)
";
            var lineItems = SplitSpecial(line);  //Code zu SplitSpzial siehe weiter unten 

            Log("Items:" + lineItems.Length);

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;

                command.CommandText = sql;
                // die Zahl bei "lineItems[8]" in den eckigen Klammern gibt an aus welcher Spalte der csvDatei die Daten eingelesen werden sollen (1.Spalte=0, 2.Sp =1,...)
                //command.Parameters.AddWithValue("@Best_Hit", ConverterTool.ToBool(lineItems[0]));//Best_Hit
                command.Parameters.AddWithValue("@Component_RT", ConverterTool.ToDecimal(lineItems[1]));//Component_RT
                command.Parameters.AddWithValue("@Base_Peak_MZ", ConverterTool.ToDecimal(lineItems[2]));//Base_Peak_MZ
                command.Parameters.AddWithValue("@CAS", lineItems[3]);//CAS

                object value = ConverterTool.ToNullableDecimal(lineItems[4]);
                command.Parameters.AddWithValue("@Library_RI", value ?? DBNull.Value );//Library_RI
      
                command.Parameters.AddWithValue("@Component_RI", ConverterTool.ToDecimal(lineItems[5]));//Component_RI
                command.Parameters.AddWithValue("@Match_Factor", ConverterTool.ToDecimal(lineItems[6]));//Match_Faktor
                
                command.Parameters.AddWithValue("@Compound_Name", lineItems[7]);//Compound_Name
                command.Parameters.AddWithValue("@Formula", lineItems[8]);//Formula
                command.Parameters.AddWithValue("@Library_File", lineItems[9]);//Library_File

                command.Parameters.AddWithValue("@Component_Area", ConverterTool.ToDecimal(lineItems[10]));//Component_Area
                command.Parameters.AddWithValue("@Base_Peak_Area", ConverterTool.ToDecimal(lineItems[11]));//Base_Peak_Area
                                                                                                           //
                command.Parameters.AddWithValue("@Type", lineItems[12]);//Type
                command.Parameters.AddWithValue("@Comment", lineItems[13]);//Comment

                command.Parameters.AddWithValue("@Import_Date", DateTime.Now);

                command.ExecuteNonQuery();

                /*
                  mögliche KonvertierungsBeispiele
                  var x = ToFloat("1.23");
                  command.Parameters.AddWithValue("@P3", Math.Round( 66.66));//BPMZ_Pr
                  command.Parameters.AddWithValue("@Comment", (ToDecimal(lineItems[14])));
                  command.Parameters.AddWithValue("@Formula", (int)(ToDecimal(lineItems[9])));
                  command.Parameters.AddWithValue("@Compound_Name", (Int64)(ToDecimal(lineItems[8])))
                
                  object value = ConverterTool.ToNullableDecimal(lineItems[4]);                         //diese zwei Zeilen gehören zusammen, um Felder in Zahlenspalten
                  command.Parameters.AddWithValue("@Library_RI", value ?? DBNull.Value );//Library_RI   //als leer zu definieren
                */
            }

        }
        
        private string[] SplitSpecial(string line)  // Tool um die Stoffnamen, die auch oft Kommata enthalten, von den SpaltenKommata zu unterscheiden 
        {
            var values = line.Split(',');           //zerlegt eine Zeile in Teilstücke, getrennt durch Kommas

            List<string> result = new List<string>(); //erzeugt eine leere Liste

            string textString = null;                 //erzeugt ein leeres Teilstück

            foreach(var value in values)        // für jedes Teilstück der Zeile
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
                        textString = textString + "," + value ; //dann bisheriges Textstück + Komma + nächstes Textstück
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

        /*
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
            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            {
                result = 0;
            }

            return result;
        }
        private decimal? ToNullableDecimal(string value)
        {

            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            {
                return null;
            }

            return result; 
        }
        
                var nullableDec = ToNullableDecimal(lineItems[12]);
                var nullableInt = (int?)nullableDec;
                var obj = (object)nullableInt;
                var area = obj ?? DBNull.Value;
               
        */


    }
}
