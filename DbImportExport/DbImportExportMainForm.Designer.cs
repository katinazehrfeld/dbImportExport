
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
            this.btImport = new System.Windows.Forms.Button();
            this.btExport = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bt_Odor_in = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btImport
            // 
            this.btImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btImport.Location = new System.Drawing.Point(1004, 15);
            this.btImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btImport.Name = "btImport";
            this.btImport.Size = new System.Drawing.Size(175, 47);
            this.btImport.TabIndex = 0;
            this.btImport.Text = "Import";
            this.btImport.UseVisualStyleBackColor = true;
            this.btImport.Click += new System.EventHandler(this.btImport_Click);
            // 
            // btExport
            // 
            this.btExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExport.Location = new System.Drawing.Point(1004, 69);
            this.btExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(175, 44);
            this.btExport.TabIndex = 1;
            this.btExport.Text = "Export";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.Location = new System.Drawing.Point(16, 15);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(960, 624);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1004, 121);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(173, 22);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "aaaa";
            // 
            // bt_Odor_in
            // 
            this.bt_Odor_in.Location = new System.Drawing.Point(1004, 162);
            this.bt_Odor_in.Name = "bt_Odor_in";
            this.bt_Odor_in.Size = new System.Drawing.Size(173, 38);
            this.bt_Odor_in.TabIndex = 4;
            this.bt_Odor_in.Text = "Odor import";
            this.bt_Odor_in.UseVisualStyleBackColor = true;
            // 
            // DbImportExportMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 655);
            this.Controls.Add(this.bt_Odor_in);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.btImport);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DbImportExportMainForm";
            this.Text = "Db Import Export";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btImport;
        private System.Windows.Forms.Button btExport;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button bt_Odor_in;
    }
}

