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
                _messungImporter.Import(Log);
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
                //ToDo File open dialog


                _bwZuordnungImporter.Import(Log);
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
                _probenInfoImporter.Import(Log);
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
                _limsInfoImporter.Import(Log);
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING PROBENINFO:" + ex.Message);
            }
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
                _casImporter.Import(Log);
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
                _uaImporter.Import(Log);
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
                _bwbImporter.Import(Log);
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING PROBENINFO:" + ex.Message);
            }
        }
    }
}
