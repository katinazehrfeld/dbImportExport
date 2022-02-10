using System;
using System.Collections.Generic;
using System.Linq;
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
        /*
        public static decimal ToDecimal(string value)
        {
            value = value.Replace('.', ',');

            if (!decimal.TryParse(value, out var result))
            {
                result = 0;
            }

            return result;
        }
        */
        public static decimal? ToNullableDecimal(string value)
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
