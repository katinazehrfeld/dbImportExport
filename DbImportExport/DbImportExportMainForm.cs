using System;
using System.Windows.Forms;

namespace DbImportExport
{
    public partial class DbImportExportMainForm : Form
    {
        private DBImportMessung _messungImporter = new DBImportMessung();
        private DBImportProbeninfo _probenInfoImporter = new DBImportProbeninfo();

        private DbExportStart _startExport = new DbExportStart();
        private DbImportProbe _probeImport = new DbImportProbe();


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



        private void btExport_Click(object sender, EventArgs e)
        {
            try
            {
                _startExport.Export(Log, this.textBox1.Text);  // Export - funktion
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _probeImport.Import(Log);
            }
            catch (Exception ex)
            {
                Log("ERROR IMPORTING PROBE:" + ex.Message);
            }
        }

      
    }
}
