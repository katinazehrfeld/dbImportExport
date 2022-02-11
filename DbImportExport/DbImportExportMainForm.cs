using System;
using System.Windows.Forms;
using DbImportExport.Importer;

namespace DbImportExport
{
    public partial class DbImportExportMainForm : Form
    {
        private DBImportMessung _messungImporter = new DBImportMessung();
        private DBImportProbeninfo _probenInfoImporter = new DBImportProbeninfo();
        private DBImportBWZuordnung _dBImportBWZuordnung = new DBImportBWZuordnung();

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
      
        private void Log(string message)
        {
            rtbLog.AppendText(message + Environment.NewLine);
        }
    }
}
