
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
            this.bt_import_BW_zuordg = new System.Windows.Forms.Button();
            this.bt_import_Pr_Info = new System.Windows.Forms.Button();
            this.ImportLimsinfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btImport
            // 
            this.btImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btImport.Location = new System.Drawing.Point(1201, 19);
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
            this.rtbLog.Size = new System.Drawing.Size(1141, 774);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // bt_import_BW_zuordg
            // 
            this.bt_import_BW_zuordg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_import_BW_zuordg.Location = new System.Drawing.Point(1201, 63);
            this.bt_import_BW_zuordg.Name = "bt_import_BW_zuordg";
            this.bt_import_BW_zuordg.Size = new System.Drawing.Size(131, 38);
            this.bt_import_BW_zuordg.TabIndex = 7;
            this.bt_import_BW_zuordg.Text = "Import BW Zuordnung";
            this.bt_import_BW_zuordg.UseVisualStyleBackColor = true;
            this.bt_import_BW_zuordg.Click += new System.EventHandler(this.btnImportBWZuordnung);
            // 
            // bt_import_Pr_Info
            // 
            this.bt_import_Pr_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_import_Pr_Info.Location = new System.Drawing.Point(1201, 107);
            this.bt_import_Pr_Info.Name = "bt_import_Pr_Info";
            this.bt_import_Pr_Info.Size = new System.Drawing.Size(131, 38);
            this.bt_import_Pr_Info.TabIndex = 8;
            this.bt_import_Pr_Info.Text = "Import Proben Infos";
            this.bt_import_Pr_Info.UseVisualStyleBackColor = true;
            this.bt_import_Pr_Info.Click += new System.EventHandler(this.btnImportProbeninfo);
            // 
            // ImportLimsinfo
            // 
            this.ImportLimsinfo.Location = new System.Drawing.Point(1201, 151);
            this.ImportLimsinfo.Name = "ImportLimsinfo";
            this.ImportLimsinfo.Size = new System.Drawing.Size(131, 38);
            this.ImportLimsinfo.TabIndex = 9;
            this.ImportLimsinfo.Text = "Import Lims Infos";
            this.ImportLimsinfo.UseVisualStyleBackColor = true;
            this.ImportLimsinfo.Click += new System.EventHandler(this.btnImportLimsinfo);
            // 
            // DbImportExportMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 798);
            this.Controls.Add(this.ImportLimsinfo);
            this.Controls.Add(this.bt_import_Pr_Info);
            this.Controls.Add(this.bt_import_BW_zuordg);
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
        private System.Windows.Forms.Button bt_import_BW_zuordg;
        private System.Windows.Forms.Button bt_import_Pr_Info;
        private System.Windows.Forms.Button ImportLimsinfo;
    }
}

