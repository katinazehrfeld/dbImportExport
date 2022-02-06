using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbImportExport
{
    class ConverterTool
    {
        public static bool ToBool(string value)
        {
            if (!bool.TryParse(value, out var result))
            {
                result = false;
            }

            return result;
        }


    }
}
