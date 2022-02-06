using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DbImportExport
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


            // fileName = "C:\\Daten\\TeslaProben\\alle_nachUA_CSV\\62005831.csv";


            if (!string.IsNullOrEmpty(fileName)) // ! = not , < = kl, > = gr, == ist gleichheitsvergleich, = ist zuweisung
            {
                // Import Verarbeitung
                ProcessImport(fileName);
            }
        }




        private void ProcessImport(string filename)
        {
            Log("Importing " + filename);

            var linesFromFile = File.ReadAllLines(filename);

            var lines = linesFromFile
                .Where(line => !string.IsNullOrEmpty(line))
                .Skip(1) //Überspringt x Zeilen, z.B. Überschrift: Skip(1)
                .ToArray();

            Log("Opning SQL connection");

            var sqlConnection = new SqlConnection("Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; ");
            sqlConnection.Open();

            using (var transaction = sqlConnection.BeginTransaction())
            {

                try
                {

                    Log("Importing lines: " + lines.Length);

                    foreach (var line in lines)
                    {
                        Log("Importing: " + line);
                        ImportLine(line, sqlConnection, transaction);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        private void ImportLine(string line, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = @"
INSERT INTO dbo.UA_csv 
(      Best_Hit
      ,Component_RT
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
      ,Comment)
VALUES (@P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10, @P11, @P12, @P13, @P14, @P15)
";
            var lineItems = SplitSpecial(line);

            Log("Items:" + lineItems.Length);

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;

                command.CommandText = sql;

                //command.Parameters.AddWithValue("@P1", lineItems[24]);//ID_Peak
                command.Parameters.AddWithValue("@P2", ConverterTool.ToBool(lineItems[1]));//Best_Hit
                command.Parameters.AddWithValue("@P3", lineItems[2]);//Component_RT
                command.Parameters.AddWithValue("@P4", lineItems[3]);//Base_Peak_MZ
                command.Parameters.AddWithValue("@P5", lineItems[4]);//CAS
                command.Parameters.AddWithValue("@P6", lineItems[5]);//Library_RI
                command.Parameters.AddWithValue("@P7", ToDecimal(lineItems[6]));//Component_RI
                command.Parameters.AddWithValue("@P8", ToDecimal(lineItems[7]));//Match_Faktor
                
                command.Parameters.AddWithValue("@P9", (Int64)(ToDecimal(lineItems[8])));//Compound_Name
                
                command.Parameters.AddWithValue("@P10", (int)(ToDecimal(lineItems[9])));//Formula
                
                command.Parameters.AddWithValue("@P11", (ToDecimal(lineItems[10])));//Library_File
                command.Parameters.AddWithValue("@P12", lineItems[11]);//Component_Area


                var nullableDec = ToNullableDecimal(lineItems[12]);
                var nullableInt = (int?)nullableDec;
                var obj = (object)nullableInt;
                var area = obj ?? DBNull.Value;

                command.Parameters.AddWithValue("@P13", area);//Base_Peak_Area


                
                command.Parameters.AddWithValue("@P14", (int)(ToDecimal(lineItems[13])));//Type
                command.Parameters.AddWithValue("@P15", (ToDecimal(lineItems[14])));//Comment
                


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

        private string[] SplitSpecial(string line)
        {
            var values = line.Split(',');

            List<string> result = new List<string>();

            string textString = null;

            foreach(var value in values)
            {
                if (textString != null)
                {
                    if (value.EndsWith("\""))
                    {
                        textString = textString + value.TrimEnd('"');
                        result.Add(textString);
                        textString = null;
                    }
                    else
                    {
                        textString = textString + value;
                    }
                }
                else
                {
                    if (value.StartsWith("\""))
                    {
                        textString = value.Substring(1);
                    }
                    else
                    {
                        result.Add(value);
                    }
                }
            }

            if (textString != null)
            {
                result.Add(textString);
            }

            return result.ToArray();
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
