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
    public class DBImportBWBstoffe
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
INSERT INTO dbo.tb_bwb_stoffe
      (      
       IKey_CAS_name
      ,BWBname
      ,IUPACname
      ,Formel
      ,MW
      ,SummeVerwendung
      ,Import_Date
      )
VALUES ( @IKey_CAS_name, @BWBname, @IUPACname, @Formel, @MW, @SummeVerwendung, @Import_Date)
";
            //Spalten in der CSV_Datei
            //InChiKey_CAS_NameListe[0]	Stoffname[1] CAS-Nr[2] INCHIKEY[3]	IUPAC_NAME[4]	SMILES[5]	INCHI_STRING[6]	
            //MOLECULAR_FORMULA[7]	AVERAGE_MASS[8]	MONOISOTOPIC_MASS[9] ID[10]	BPMZ[11] RT[12]	RI[13] Lib[14] Geruch[15]
            //LCMS[16] ID Kopie[17]	WGK[18]	AZM[19]	PSM[20]	Biozide[21]	REACH[22] POPs[23]	Biozidal_Active_Sub[24]
            //Verbrauch[25]	Quelle[26]	Verwendung[27] SummeVerwendung[28]	Bemerkung[29]	Bem.2[30]
            //höchster MW	höchstes Maximum	UQN- Entwurf	Priority setting (P),
            //UQN Watch List (W) *)	PNEC	PNEC <	PNEC >	PNEC/ UQN-V in µg/l	RAC	
            //Toxizitäts-Kriterium nicht erfüllt	Einheit	Reach A7	Norman_SusDat_ID																																																																																																																																											

            var lineItems = SplitSpecial(line);  //Code zu SplitSpzial siehe weiter unten 

            int lineCount = 0;
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "SELECT count(*) FROM dbo.tb_bwb_stoffe WHERE IKey_CAS_name = @IKey_CAS_name";
                command.Parameters.AddWithValue("@IKey_CAS_name", lineItems[1]);

                lineCount = (int)command.ExecuteScalar();
            }

            if (lineCount == 0)
            {
                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;

                    command.CommandText = sql;
                    // die Zahl bei "lineItems[8]" in den eckigen Klammern gibt an aus welcher Spalte der csvDatei die Daten eingelesen werden sollen (1.Spalte=0, 2.Sp =1,...)

                    command.Parameters.AddWithValue("@IKey_CAS_name", lineItems[0]);//InChiKey, inkl.CAS_NIST_name mit Vorsatz
                    command.Parameters.AddWithValue("@BWBname", lineItems[1]);//Stoffname aus BWBliste
                    command.Parameters.AddWithValue("@IUPACname", lineItems[4]);//IUPAC Name
                    command.Parameters.AddWithValue("@Formel", lineItems[7]);//SummenFormel
                    //command.Parameters.AddWithValue("@MW",lineItems[9]);//MONOISOTOPIC_MASS (weight)
                    command.Parameters.AddWithValue("@MW", ConverterTool.ToDecimal (lineItems[9]));//MONOISOTOPIC_MASS (weight)
                    command.Parameters.AddWithValue("@SummeVerwendung", lineItems[28]);//SummeVerwendung

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

