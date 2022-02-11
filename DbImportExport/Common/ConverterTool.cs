using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace DbImportExport
{
    class ConverterTool
    {
        public static bool ToBool(string value)             // setzt "Wahr oder Falsch" als Wert
        {
            if (!bool.TryParse(value, out var result))      // wenn nicht "Wahr oder Falsch" Charakter
            {
                result = false;                             // dann setze "Falsch"
            }

            return result;
        }
        public static int ToInt(string value)               // wandelt Wert in GanzZahl um
        {
            if (!int.TryParse(value, out var result))       // wenn keine Zahl
            {
                result = 0;                                 // dann Null
            }

            return result;
        }
        
        public static decimal ToDecimal(string value)
        {
            
             if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            {
                result = 0;
            }

            return result;
        }
       
        public static decimal? ToNullableDecimal(string value) //wenn in einer Spalte mit Zahlen ein Feld leer ist, dann wird es auf "NULL" gesetzt
                                                               //Textspalten brauchen diese Funktion nicht, dürfen einfach leer sein
                                                               //bei Spalten mit Zahlen stellt sich die Frage, ob eine leere Zelle als die Zahl 0 oder als leer ("NULL") verstanden werden soll
        {

            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            {
                return null;
            }

            return result;
        }
        
    }
}
