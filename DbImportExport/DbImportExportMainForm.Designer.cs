﻿
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
            this.btExport = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bt_Odor_in = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // btImport
            // 
            this.btImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btImport.Location = new System.Drawing.Point(1130, 19);
            this.btImport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btImport.Name = "btImport";
            this.btImport.Size = new System.Drawing.Size(197, 59);
            this.btImport.TabIndex = 0;
            this.btImport.Text = "Import";
            this.btImport.UseVisualStyleBackColor = true;
            this.btImport.Click += new System.EventHandler(this.btImport_Click);
            // 
            // btExport
            // 
            this.btExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExport.Location = new System.Drawing.Point(1130, 86);
            this.btExport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(197, 55);
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
            this.rtbLog.Location = new System.Drawing.Point(18, 19);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(1080, 779);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1130, 151);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(194, 26);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "aaaa";
            // 
            // bt_Odor_in
            // 
            this.bt_Odor_in.Location = new System.Drawing.Point(1130, 202);
            this.bt_Odor_in.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bt_Odor_in.Name = "bt_Odor_in";
            this.bt_Odor_in.Size = new System.Drawing.Size(195, 48);
            this.bt_Odor_in.TabIndex = 4;
            this.bt_Odor_in.Text = "Odor import";
            this.bt_Odor_in.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1127, 309);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(197, 59);
            this.button1.TabIndex = 5;
            this.button1.Text = "ImportProbe";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1127, 391);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(197, 26);
            this.textBox2.TabIndex = 6;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // DbImportExportMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 819);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bt_Odor_in);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.btImport);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}

