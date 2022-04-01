using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DbImportExport.Importer
{
    
     public class DBTestCASImport
    {
        private Action<string> Log;

        public void Import(Action<string> log)
        {
            Log = log;

            var resultlist = new List<string>();


            var txt = File.ReadAllText(@"c:\DB_BWB\Test_compound_index.txt");

            Regex regex = new Regex(@"[0-9]+\-[0-9]+\-[0-9]+");

            foreach (Match match in regex.Matches(txt))
            {
                resultlist.Add(match.Value);
            }


            /*

            using (var stream = File.OpenText(@"c:\DB_BWB\Test_compound_index.txt"))
            {
                while (!stream.EndOfStream)
                {
                    string line = stream.ReadLine();

                    Regex regex = new Regex(@"[0-9]+\-[0-9]+\-[0-9]+");

                    foreach (Match match in regex.Matches(line))
                    {
                        resultlist.Add(match.Value);
                    }
                }
            }
            */

            resultlist = resultlist
                .GroupBy(x => x)
                .Select(k => k.Key)
                //.OrderBy(s => s)
                .ToList();


            File.WriteAllLines(@"c:\DB_BWB\Test_compound_indexOUT.txt", resultlist);
        }

      
    }
}
