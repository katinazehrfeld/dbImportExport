using System;
using System.Windows.Forms;

namespace DbImportExport
{
    public partial class DbImportExportMainForm : Form
    {
        private DbImportStart _startImport = new DbImportStart();
        private DbExportStart _startExport = new DbExportStart();
        private DbImportProbe _probeImport = new DbImportProbe();


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
