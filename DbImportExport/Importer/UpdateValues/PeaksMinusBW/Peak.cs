using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbImportExport.Importer.UpdateValues.PeaksMinusBW
{
    class Peak
    {
        public string PKenng {get; set;}
		public string BWZuordg { get; set; }
        public int ID_Peak { get; set; }
        public string Type { get; set; }
        public string BPMZ_RT { get; set; }
        public string BPMZ_RT_p01 { get; set; }
        public string BPMZ_RT_p02 { get; set; }
        public string BPMZ_RT_m01 { get; set; }
        public string BPMZ_RT_m02 { get; set; }
        public double AreaP { get; set; }
        public double AreaBW { get; set; }
    }
}
