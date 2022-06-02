
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
            this.bt_import_cas = new System.Windows.Forms.Button();
            this.btnUpdateValues = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.tbWasserwerk = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btImport
            // 
            this.btImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btImport.Location = new System.Drawing.Point(16, 14);
            this.btImport.Margin = new System.Windows.Forms.Padding(4);
            this.btImport.Name = "btImport";
            this.btImport.Size = new System.Drawing.Size(174, 46);
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
            this.rtbLog.Location = new System.Drawing.Point(199, 14);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(4);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(1580, 616);
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
            this.bt_import_BW_zuordg.Location = new System.Drawing.Point(16, 69);
            this.bt_import_BW_zuordg.Margin = new System.Windows.Forms.Padding(4);
            this.bt_import_BW_zuordg.Name = "bt_import_BW_zuordg";
            this.bt_import_BW_zuordg.Size = new System.Drawing.Size(174, 46);
            this.bt_import_BW_zuordg.TabIndex = 7;
            this.bt_import_BW_zuordg.Text = "Import BW Zuordnung";
            this.bt_import_BW_zuordg.UseVisualStyleBackColor = true;
            this.bt_import_BW_zuordg.Click += new System.EventHandler(this.btnImportBWZuordnung);
            // 
            // bt_import_Pr_Info
            // 
            this.bt_import_Pr_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_import_Pr_Info.Location = new System.Drawing.Point(16, 123);
            this.bt_import_Pr_Info.Margin = new System.Windows.Forms.Padding(4);
            this.bt_import_Pr_Info.Name = "bt_import_Pr_Info";
            this.bt_import_Pr_Info.Size = new System.Drawing.Size(174, 46);
            this.bt_import_Pr_Info.TabIndex = 8;
            this.bt_import_Pr_Info.Text = "Import Proben Infos";
            this.bt_import_Pr_Info.UseVisualStyleBackColor = true;
            this.bt_import_Pr_Info.Click += new System.EventHandler(this.btnImportProbeninfo);
            // 
            // bt_import_lims_info
            // 
            this.bt_import_lims_info.Location = new System.Drawing.Point(16, 178);
            this.bt_import_lims_info.Margin = new System.Windows.Forms.Padding(4);
            this.bt_import_lims_info.Name = "bt_import_lims_info";
            this.bt_import_lims_info.Size = new System.Drawing.Size(174, 46);
            this.bt_import_lims_info.TabIndex = 9;
            this.bt_import_lims_info.Text = "Import Lims Infos";
            this.bt_import_lims_info.UseVisualStyleBackColor = true;
            this.bt_import_lims_info.Click += new System.EventHandler(this.btnImportLimsinfo);
            // 
            // bt_import_cas
            // 
            this.bt_import_cas.Location = new System.Drawing.Point(16, 233);
            this.bt_import_cas.Margin = new System.Windows.Forms.Padding(4);
            this.bt_import_cas.Name = "bt_import_cas";
            this.bt_import_cas.Size = new System.Drawing.Size(174, 46);
            this.bt_import_cas.TabIndex = 10;
            this.bt_import_cas.Text = "CAS Import";
            this.bt_import_cas.UseVisualStyleBackColor = true;
            this.bt_import_cas.Click += new System.EventHandler(this.bt_import_cas_Click);
            // 
            // btnUpdateValues
            // 
            this.btnUpdateValues.Location = new System.Drawing.Point(16, 359);
            this.btnUpdateValues.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdateValues.Name = "btnUpdateValues";
            this.btnUpdateValues.Size = new System.Drawing.Size(174, 46);
            this.btnUpdateValues.TabIndex = 13;
            this.btnUpdateValues.Text = "Werte korrigieren";
            this.btnUpdateValues.UseVisualStyleBackColor = true;
            this.btnUpdateValues.Click += new System.EventHandler(this.button_updateValues_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(13, 624);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(174, 46);
            this.btnQuery.TabIndex = 14;
            this.btnQuery.Text = "Abfrage";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tbWasserwerk
            // 
            this.tbWasserwerk.Location = new System.Drawing.Point(199, 636);
            this.tbWasserwerk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbWasserwerk.Name = "tbWasserwerk";
            this.tbWasserwerk.Size = new System.Drawing.Size(527, 22);
            this.tbWasserwerk.TabIndex = 15;
            this.tbWasserwerk.Text = "WDF";
            // 
            // DbImportExportMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1792, 676);
            this.Controls.Add(this.tbWasserwerk);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnUpdateValues);
            this.Controls.Add(this.bt_import_cas);
            this.Controls.Add(this.bt_import_lims_info);
            this.Controls.Add(this.bt_import_Pr_Info);
            this.Controls.Add(this.bt_import_BW_zuordg);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btImport);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DbImportExportMainForm";
            this.Text = "Db Import Export";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btImport;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button bt_import_BW_zuordg;
        private System.Windows.Forms.Button bt_import_Pr_Info;
        private System.Windows.Forms.Button bt_import_lims_info;
        private System.Windows.Forms.Button bt_import_cas;
        private System.Windows.Forms.Button btnUpdateValues;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.TextBox tbWasserwerk;
        //private System.Windows.Forms.Button bt_cas_import;
    }
}

