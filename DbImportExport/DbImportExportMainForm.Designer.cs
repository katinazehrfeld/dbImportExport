
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
            this.bt_import_lims_info = new System.Windows.Forms.Button();
            this.bt_test_cas_import = new System.Windows.Forms.Button();
            this.btnUpdateValues = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btImport
            // 
            this.btImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btImport.Location = new System.Drawing.Point(12, 12);
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
            this.rtbLog.Location = new System.Drawing.Point(149, 12);
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
            this.bt_import_BW_zuordg.Location = new System.Drawing.Point(12, 56);
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
            this.bt_import_Pr_Info.Location = new System.Drawing.Point(12, 100);
            this.bt_import_Pr_Info.Name = "bt_import_Pr_Info";
            this.bt_import_Pr_Info.Size = new System.Drawing.Size(131, 38);
            this.bt_import_Pr_Info.TabIndex = 8;
            this.bt_import_Pr_Info.Text = "Import Proben Infos";
            this.bt_import_Pr_Info.UseVisualStyleBackColor = true;
            this.bt_import_Pr_Info.Click += new System.EventHandler(this.btnImportProbeninfo);
            // 
            // bt_import_lims_info
            // 
            this.bt_import_lims_info.Location = new System.Drawing.Point(12, 145);
            this.bt_import_lims_info.Name = "bt_import_lims_info";
            this.bt_import_lims_info.Size = new System.Drawing.Size(131, 38);
            this.bt_import_lims_info.TabIndex = 9;
            this.bt_import_lims_info.Text = "Import Lims Infos";
            this.bt_import_lims_info.UseVisualStyleBackColor = true;
            this.bt_import_lims_info.Click += new System.EventHandler(this.btnTestImportCAS);
            // 
            // bt_test_cas_import
            // 
            this.bt_test_cas_import.Location = new System.Drawing.Point(12, 189);
            this.bt_test_cas_import.Name = "bt_test_cas_import";
            this.bt_test_cas_import.Size = new System.Drawing.Size(131, 38);
            this.bt_test_cas_import.TabIndex = 10;
            this.bt_test_cas_import.Text = "Test CAS Import";
            this.bt_test_cas_import.UseVisualStyleBackColor = true;
            this.bt_test_cas_import.Click += new System.EventHandler(this.btnTestImportCAS);
            // 
            // btnUpdateValues
            // 
            this.btnUpdateValues.Location = new System.Drawing.Point(12, 292);
            this.btnUpdateValues.Name = "btnUpdateValues";
            this.btnUpdateValues.Size = new System.Drawing.Size(131, 38);
            this.btnUpdateValues.TabIndex = 13;
            this.btnUpdateValues.Text = "Werte korrigieren";
            this.btnUpdateValues.UseVisualStyleBackColor = true;
            this.btnUpdateValues.Click += new System.EventHandler(this.button_updateValues_Click);
            // 
            // DbImportExportMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 798);
            this.Controls.Add(this.btnUpdateValues);
            this.Controls.Add(this.bt_test_cas_import);
            this.Controls.Add(this.bt_import_lims_info);
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
        private System.Windows.Forms.Button bt_import_lims_info;
        private System.Windows.Forms.Button bt_test_cas_import;
        private System.Windows.Forms.Button btnUpdateValues;
    }
}

