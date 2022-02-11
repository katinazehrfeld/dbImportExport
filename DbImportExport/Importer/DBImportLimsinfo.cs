using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbImportExport.Importer
{
    
    public class DBImportLimsinfo
    {
        private Action<string> Log;

        public void Import(Action<string> log)
        {
            Log = log;

            // Datei auswählen 
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
INSERT INTO dbo.E3_Lims_Info 
      (      
       LimsNr
      ,ProbenArt
      ,Ort_kurz
      ,Entnahmedatum
      ,Import_Date
      )
VALUES ( @LimsNr, @ProbenArt, @Ort_kurz, @Entnahmedatum, @Import_Date)
";
            //Spalten in der CSV_Datei
            //  Probe_Nr[0] Annahmedatum[1]	Kom[2]	Status[3]	Termin[4]	Teilproj_Nr[5]	Teilprojekt[6]	Pruefauftrag[7]	Pg_Kz_1[8]	Pg_Kz_2[9]
            //	Ort_Kz[10]	Ort[11]	PLZ[12]	Straße[13]	HNr[14]	Zusatz[15]	Bezeichnung[16]	Meßstelle[17]	Detail[18]	int_Nr[19]	ext_Nr[20]
            //	Firma[21]	Abt[22]	KSt[23]	SAP_Nr[24]	Entnahmedatum[25]	Flag[26]	fert[27]	Labor[28]

            var lineItems = SplitSpecial(line);  //Code zu SplitSpzial siehe weiter unten 

            Log("Items:" + lineItems.Length);

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;

                command.CommandText = sql;
                // die Zahl bei "lineItems[8]" in den eckigen Klammern gibt an aus welcher Spalte der csvDatei die Daten eingelesen werden sollen (1.Spalte=0, 2.Sp =1,...)

                command.Parameters.AddWithValue("@LimsNr", lineItems[0]);//Probe_Nr im Lims
                command.Parameters.AddWithValue("@ProbenArt", lineItems[9]);//Pg_Kz_2 , Probenarten: Abwasser,Rohzulauf,gereinigtes Abwasser,Trinkwasser,Grundwasser,Reinwasser,Wasser aus TW-Installation,
                command.Parameters.AddWithValue("@Ort_kurz", lineItems[10]);//Ort_Kz , Kurzzeichen für den Ort
                command.Parameters.AddWithValue("@Entnahmedatum", lineItems[25]);//Entnahmedatum der Probe
              
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


