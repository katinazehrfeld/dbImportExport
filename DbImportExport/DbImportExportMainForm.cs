using System;
using System.Windows.Forms;

namespace DbImportExport
{
    public partial class DbImportExportMainForm : Form
    {
        private DbImportStart _startImport = new DbImportStart();
        private DbExportStart _startExport = new DbExportStart();

        public DbImportExportMainForm()
        {
            InitializeComponent();
        }

        private void btImport_Click(object sender, EventArgs e)
        {
            try 
            {
                _startImport.Import(Log);
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING:" + ex.Message);
            }
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            try
            {
                _startExport.Export(Log);  // Export - funktion
            }
            catch(Exception ex)
            {
                Log("ERROR EXPORTING:" + ex.Message);
            }
        }

        private void Log(string message)
        {
            rtbLog.AppendText(message + Environment.NewLine);
        }
    }
}
