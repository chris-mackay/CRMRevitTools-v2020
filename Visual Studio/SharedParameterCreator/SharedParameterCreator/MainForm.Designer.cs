//    Copyright(C) 2019 Christopher Ryan Mackay

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.If not, see<https://www.gnu.org/licenses/>.

namespace SharedParameterCreator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtCSVFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblExcelFile = new System.Windows.Forms.Label();
            this.chkInsert = new System.Windows.Forms.CheckBox();
            this.gbSelectFile = new System.Windows.Forms.GroupBox();
            this.gbCreateSharedParameters = new System.Windows.Forms.GroupBox();
            this.lblNotice = new System.Windows.Forms.Label();
            this.btnOpenTemplate = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.gbSelectFile.SuspendLayout();
            this.gbCreateSharedParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCSVFile
            // 
            this.txtCSVFile.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtCSVFile.Enabled = false;
            this.txtCSVFile.Location = new System.Drawing.Point(19, 45);
            this.txtCSVFile.Name = "txtCSVFile";
            this.txtCSVFile.ReadOnly = true;
            this.txtCSVFile.Size = new System.Drawing.Size(436, 20);
            this.txtCSVFile.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(461, 43);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(491, 253);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(410, 253);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblExcelFile
            // 
            this.lblExcelFile.AutoSize = true;
            this.lblExcelFile.BackColor = System.Drawing.Color.Transparent;
            this.lblExcelFile.Location = new System.Drawing.Point(19, 25);
            this.lblExcelFile.Name = "lblExcelFile";
            this.lblExcelFile.Size = new System.Drawing.Size(126, 13);
            this.lblExcelFile.TabIndex = 0;
            this.lblExcelFile.Text = "Excel (CSV) File Location";
            // 
            // chkInsert
            // 
            this.chkInsert.AutoSize = true;
            this.chkInsert.Location = new System.Drawing.Point(12, 259);
            this.chkInsert.Name = "chkInsert";
            this.chkInsert.Size = new System.Drawing.Size(164, 17);
            this.chkInsert.TabIndex = 2;
            this.chkInsert.Text = "Insert into Project Parameters";
            this.chkInsert.UseVisualStyleBackColor = true;
            // 
            // gbSelectFile
            // 
            this.gbSelectFile.Controls.Add(this.txtCSVFile);
            this.gbSelectFile.Controls.Add(this.lblExcelFile);
            this.gbSelectFile.Controls.Add(this.btnBrowse);
            this.gbSelectFile.Location = new System.Drawing.Point(12, 149);
            this.gbSelectFile.Name = "gbSelectFile";
            this.gbSelectFile.Size = new System.Drawing.Size(554, 91);
            this.gbSelectFile.TabIndex = 1;
            this.gbSelectFile.TabStop = false;
            this.gbSelectFile.Text = "Select CSV File";
            // 
            // gbCreateSharedParameters
            // 
            this.gbCreateSharedParameters.Controls.Add(this.lblNotice);
            this.gbCreateSharedParameters.Controls.Add(this.btnOpenTemplate);
            this.gbCreateSharedParameters.Controls.Add(this.lblInstructions);
            this.gbCreateSharedParameters.Location = new System.Drawing.Point(12, 12);
            this.gbCreateSharedParameters.Name = "gbCreateSharedParameters";
            this.gbCreateSharedParameters.Size = new System.Drawing.Size(554, 131);
            this.gbCreateSharedParameters.TabIndex = 0;
            this.gbCreateSharedParameters.TabStop = false;
            this.gbCreateSharedParameters.Text = "Create (.csv) file";
            // 
            // lblNotice
            // 
            this.lblNotice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotice.Location = new System.Drawing.Point(19, 83);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(377, 42);
            this.lblNotice.TabIndex = 1;
            this.lblNotice.Text = "The Shared Parameter file (.txt) will be saved in the same directory where the (." +
    "csv) file exists.";
            // 
            // btnOpenTemplate
            // 
            this.btnOpenTemplate.Location = new System.Drawing.Point(420, 83);
            this.btnOpenTemplate.Name = "btnOpenTemplate";
            this.btnOpenTemplate.Size = new System.Drawing.Size(116, 23);
            this.btnOpenTemplate.TabIndex = 2;
            this.btnOpenTemplate.Text = "Open Template";
            this.btnOpenTemplate.UseVisualStyleBackColor = true;
            this.btnOpenTemplate.Click += new System.EventHandler(this.btnOpenTemplate_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.Location = new System.Drawing.Point(19, 24);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(490, 47);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = resources.GetString("lblInstructions.Text");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 288);
            this.Controls.Add(this.gbCreateSharedParameters);
            this.Controls.Add(this.gbSelectFile);
            this.Controls.Add(this.chkInsert);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shared Parameter Creator";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MainForm_HelpButtonClicked);
            this.gbSelectFile.ResumeLayout(false);
            this.gbSelectFile.PerformLayout();
            this.gbCreateSharedParameters.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCSVFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblExcelFile;
        private System.Windows.Forms.CheckBox chkInsert;
        private System.Windows.Forms.GroupBox gbSelectFile;
        private System.Windows.Forms.GroupBox gbCreateSharedParameters;
        private System.Windows.Forms.Button btnOpenTemplate;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Label lblNotice;
    }
}