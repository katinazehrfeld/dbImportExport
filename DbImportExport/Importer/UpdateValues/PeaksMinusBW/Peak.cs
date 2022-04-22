namespace DbImportExport.Importer.UpdateValues.PeaksMinusBW
{
    internal class Peak
    {
        public string PKenng { get; set; }
        public string BWZuordg { get; set; }
        public int ID_Peak { get; set; }
        public string BPMZ_RT { get; set; }
        public double AreaP { get; set; }

        public double V_Extraktion_mL { get; set; }
        public double Verdg_im_Vial { get; set; }
        public double IS_Volumen_ml { get; set; }
        public double InjektionsVolumen_ml { get; set; }
        public string CAS { get; set; }

        public double MF { get; set; }
        public double? LibRI { get; set; }
        public double? RIkorr { get; set; }
        public string LibFile { get; set; }

        public string SName { get; set; }
        public string BPMZ_RI { get; set; }

        public double IS_AreaP { get; set; }
        public double RTkorr { get; set; }

        public double BP_MZ_korr { get; set; }
    }
}