using System;                           //globale geladene Funktionen
using System.Data.SqlClient;            //weil verkürzte Schreibweise dadurch möglich
using DbImportExport.Importer.UpdateValues.PeaksMinusBW;


namespace DbImportExport.Importer.UpdateValues        //Namensklasse in der keine Namensgleichheit vorkommen darf
{
    public class DBUpdateValues    // auf eine public class kann von außen zugegriffen werden
    {
        private Action<string> Log; // private ist nur in dieser public class ansprechbar
                                    // erzeugt einen Log string

        internal void UpdateData(Action<string> log)
        {
            Log = log;              // zeigt die erfolgten ToDos und Fehler

            var connection = new SqlConnection("Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; ");  //Variable definiert für Verbindung zu entsprechender DB
            connection.Open();               //öffnet gewählte DB

            log("Updating data");

            new RtKorrigierenKlasse(log).RtKorrigieren(connection);
            new RiKorrigierenKlasse(log).RiKorrigieren(connection);
            new MzRtNameKlasse(log).MzRtName(connection);
            new MzRtNamePlusKlasse(log).MzRtNamePlus(connection);
            new MzRiNameKlasse(log).MzRiName(connection);
            new FlaecheBWKlasse(log).FlaecheBW(connection);

            
            new PeaksMinusBWKlasse(log).PeaksMinusBW(connection);


            // BWinnen(connection);
        }
    } 
}
