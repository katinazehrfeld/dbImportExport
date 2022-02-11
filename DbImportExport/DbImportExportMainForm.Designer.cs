
namespace DbImportExport
{
    partial class DbImportExportMainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btImport = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.import_BW_zuordg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btImport
            // 
            this.btImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btImport.Location = new System.Drawing.Point(753, 19);
            this.btImport.Name = "btImport";
            this.btImport.Size = new System.Drawing.Size(131, 38);
            this.btImport.TabIndex = 0;
            this.btImport.Text = "Import Messung";
            this.btImport.UseVisualStyleBackColor = true;
            this.btImport.Click += new System.EventHandler(this.btnImportMessung);
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.Location = new System.Drawing.Point(12, 12);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(693, 508);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // import_BW_zuordg
            // 
            this.import_BW_zuordg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.import_BW_zuordg.Location = new System.Drawing.Point(753, 63);
            this.import_BW_zuordg.Name = "import_BW_zuordg";
            this.import_BW_zuordg.Size = new System.Drawing.Size(131, 38);
            this.import_BW_zuordg.TabIndex = 7;
            this.import_BW_zuordg.Text = "Import BW Zuordnung";
            this.import_BW_zuordg.UseVisualStyleBackColor = true;
            this.import_BW_zuordg.Click += new System.EventHandler(this.btnImportProbeninfo);
            // 
            // DbImportExportMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 532);
            this.Controls.Add(this.import_BW_zuordg);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btImport);
            this.Name = "DbImportExportMainForm";
            this.Text = "Db Import Export";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btImport;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button import_BW_zuordg;
    }
}

