using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbImportExport
{
    public class DbImportStart
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
            

           // fileName = "C:\\Daten\\TeslaProben\\alle_CSV_nachAWDB1\\AWDB_1_62005831_AT_1L.csv";


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
                ImportLine(line, sqlConnection);
            }
        } 

        private void ImportLine(string line, SqlConnection connection)
        {
            var sql = @"
INSERT INTO dbo.Peak 
(BPMZ_Pr, 
CAS_V1, 
Name_V1,
Formel_V1, 
LibFile, 
PeakArea,
BPArea, 
PeakArea_mal_Faktoren, 
FlaechenProzent,
RT_korr_Pr,
BPMZ_RT_korr, 
RI_Lib_0,
RI_korr_Pr,
MF_Pr_0, 
GC_RI_BPMZ,
LimsNr,
PeakType)
VALUES (@P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10, @P11, @P12, @P13, @P14, @P15, @P16, @P17, @P18)
";
            var lineItems = line.Split(new[] { ';' });



            Log("Items:" + lineItems.Length);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                
                               // command.Parameters.AddWithValue("@P1", lineItems[24]);//ID_Peak
                                command.Parameters.AddWithValue("@P2", (int)(ToDecimal(lineItems[2])));//BPMZ_Pr
                                command.Parameters.AddWithValue("@P3", lineItems[3]);//CAS_V1
                                command.Parameters.AddWithValue("@P4", lineItems[7]);//Name_V1
                                command.Parameters.AddWithValue("@P5", lineItems[8]);//Formel_V1
                                command.Parameters.AddWithValue("@P6", lineItems[9]);//LibFile
                                command.Parameters.AddWithValue("@P7", (ToDecimal(lineItems[10])));//PeakArea 
                                command.Parameters.AddWithValue("@P8", (ToDecimal(lineItems[11])));//BPArea
                                command.Parameters.AddWithValue("@P9", (Int64)(ToDecimal(lineItems[15])));//PeakArea_mal_Faktoren
                                command.Parameters.AddWithValue("@P10", (int)(ToDecimal(lineItems[16])));//FlaechenProzent
                                command.Parameters.AddWithValue("@P11", (ToDecimal(lineItems[17])));//RT_korr_Pr
                                command.Parameters.AddWithValue("@P12", lineItems[18]);//BPMZ_RT_korr
                                command.Parameters.AddWithValue("@P13", ((object)(int?)ToNullableDecimal(lineItems[19]))?? DBNull.Value );//RI_Lib_0
                                command.Parameters.AddWithValue("@P14", (int)(ToDecimal(lineItems[20])));//RI_korr_Pr
                                command.Parameters.AddWithValue("@P15", (ToDecimal(lineItems[21])));//MF_Pr_0
                                command.Parameters.AddWithValue("@P16", lineItems[22]);//GC_RI_BPMZ
                                command.Parameters.AddWithValue("@P17", (int)(ToDecimal(lineItems[23])));//LimsNr
                                command.Parameters.AddWithValue("@P18", lineItems[24]);//PeakType


                command.ExecuteNonQuery();

                //  var x = ToFloat("1.23");

                /*
                // command.Parameters.AddWithValue("@P1", lineItems[24]);//ID_Peak
                command.Parameters.AddWithValue("@P2",  ToFloat("66.678"));//RT_korr_Pr
                command.Parameters.AddWithValue("@P3", Math.Round( 66.66));//BPMZ_Pr
                command.Parameters.AddWithValue("@P4",  "2");//CAS_V1
                command.Parameters.AddWithValue("@P5",  3);//RI_Lib
                command.Parameters.AddWithValue("@P6",  4);//RI_korr_Pr
                command.Parameters.AddWithValue("@P7",  5);//MF_Pr
                command.Parameters.AddWithValue("@P8",  "6");//Name_V1
                command.Parameters.AddWithValue("@P9",  "7");//Formel_V1
                command.Parameters.AddWithValue("@P10", 8);//PeakArea
                command.Parameters.AddWithValue("@P11", 9);//BPArea
                command.Parameters.AddWithValue("@P12", "10");//PeakType
                command.Parameters.AddWithValue("@P13", 11);//PeakArea_mal_Faktoren
                command.Parameters.AddWithValue("@P14", 12);//FlaechenProzent
                command.Parameters.AddWithValue("@P15", "13");//BPMZ_Rtkorr
                command.Parameters.AddWithValue("@P16", "14");//GC_RI_BPMZ
                command.Parameters.AddWithValue("@P17", 15);//LimsNr
                command.Parameters.AddWithValue("@P18", "16");//LibFile
                command.ExecuteNonQuery();
           */
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
