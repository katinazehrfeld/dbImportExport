using System;
using System.IO;
using System.Windows.Forms;
using DbImportExport.Importer;
using DbImportExport.Importer.UpdateValues;
using DbImportExport.Report;

namespace DbImportExport
{
    public partial class DbImportExportMainForm : Form
    {
        const string bwbConnectionString = "Data Source = KATINALAPTOP2; Initial Catalog = BWB; Integrated Security = true; ";

        private DBImportMessung _messungImporter = new DBImportMessung();
        private DBImportProbeninfo _probenInfoImporter = new DBImportProbeninfo();
        private DBImportBWZuordnung _bwZuordnungImporter = new DBImportBWZuordnung();
        private DBImportLimsinfo _limsInfoImporter = new DBImportLimsinfo();
        private DBTestCASImport _testCASImporter = new DBTestCASImport();
        private DBUpdateValues _werteKorrektur = new DBUpdateValues();
        private DBGetReport _getReport = new DBGetReport();
        private DBImportCAS _casImporter = new DBImportCAS();
        private DBImportUAstoffe _uaImporter = new DBImportUAstoffe();
        private DBImportBWBstoffe _bwbImporter = new DBImportBWBstoffe();
        public DbImportExportMainForm()
        {
            InitializeComponent();
        }

        private void btnImportMessung(object sender, EventArgs e)
        {
            try 
            {
                var filename = GetFileName();
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    _messungImporter.Import(Log, filename, bwbConnectionString);
                }
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING MESSUNG:" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        private void btnImportBWZuordnung(object sender, EventArgs e)
        {
            try
            {
                var filename = GetFileName();
                if (!string.IsNullOrWhiteSpace(filename))
                {

                    _bwZuordnungImporter.Import(Log, filename, bwbConnectionString);
                }
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING MESSUNG:" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        private void btnImportProbeninfo(object sender, EventArgs e)
        {
            try
            {
                var filename = GetFileName();
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    _probenInfoImporter.Import(Log, filename, bwbConnectionString);
                }
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING PROBENINFO:" + ex.Message);
            }
        }
      
        private void btnImportLimsinfo(object sender, EventArgs e)
        {
            try
            {
                var filename = GetFileName();
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    _limsInfoImporter.Import(Log, filename, bwbConnectionString);
                }
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING PROBENINFO:" + ex.Message);
            }
        }

        private string GetFileName()
        {
            string fileName = null;
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileName = dialog.FileName;
            }
            return fileName;
        }


        private void btnTestImportCAS(object sender, EventArgs e)
        {
            try
            {
                _testCASImporter.Import(Log);
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING PROBENINFO:" + ex.Message);
            }
        }

        private void Log(string message)
        {
            rtbLog.AppendText(message + Environment.NewLine);
        }

        private void button_updateValues_Click(object sender, EventArgs e)
        {
            try
            {
                _werteKorrektur.UpdateData(Log);
            }
            catch (Exception ex)
            {
                Log("ERROR UPDATING:" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var report = _getReport.GetReport(tbWasserwerk.Text, Log);
                var filename = "c:\\temp\\queryReportResult.csv";

                File.WriteAllText(filename, report);

                Log($"report saved as {filename}");
            }
            catch (Exception ex)
            {
                Log("ERROR UPDATING:" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void btnImportCAS(object sender, EventArgs e)
        {
            try
            {
                var filename = GetFileName();
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    _casImporter.Import(Log, filename, bwbConnectionString);
                }
                
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING PROBENINFO:" + ex.Message);
            }
        }

        private void btnImportUA(object sender, EventArgs e)
        {
            try
            {
                var filename = GetFileName();
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    _uaImporter.Import(Log, filename, bwbConnectionString);
                }

            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING PROBENINFO:" + ex.Message);
            }
        }

        private void btnImportBWB(object sender, EventArgs e)
        {
            try
            {
                var filename = GetFileName();
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    _bwbImporter.Import(Log, filename, bwbConnectionString);
                }
                
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING PROBENINFO:" + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                var testConnectionString = "Data Source = KATINALAPTOP2; Initial Catalog = BWB_TEST; Integrated Security = true; ";


                _probenInfoImporter.Import(Log, @"C:\Projekte\DbImportExport\DbTestDataEinzelprobe\tb_PInfos_nur1_Pr.csv", testConnectionString);
                _limsInfoImporter.Import(Log, @"C:\Projekte\DbImportExport\DbTestDataEinzelprobe\tb_LInfos_nur1_Pr.csv", testConnectionString);
                _messungImporter.Import(Log, @"C:\Projekte\DbImportExport\DbTestDataEinzelprobe\72102609.csv", testConnectionString);
                _messungImporter.Import(Log, @"C:\Projekte\DbImportExport\DbTestDataEinzelprobe\BW_2021_11_04.csv", testConnectionString);
                _bwZuordnungImporter.Import(Log, @"C:\Projekte\DbImportExport\DbTestDataEinzelprobe\tb_BW_Zuordg_nur1_Pr.csv", testConnectionString);
                _casImporter.Import(Log, @"C:\Projekte\DbImportExport\DbTestDataEinzelprobe\InChiKey_CAS_Nist_EG_nur1Stoff.csv", testConnectionString);
                _uaImporter.Import(Log, @"C:\Projekte\DbImportExport\DbTestDataEinzelprobe\UA_Lib_SoffListe.csv", testConnectionString);
                _bwbImporter.Import(Log, @"C:\Projekte\DbImportExport\DbTestDataEinzelprobe\Stoffsammlung_BWB_Umwelt_GC_nur1Stoff.csv", testConnectionString);

            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING xxxx:" + ex.Message);

                Log("at:" + ex.StackTrace);
            }

        }
    }
}
