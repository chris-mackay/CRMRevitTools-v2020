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

namespace SheetRenamer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSheetSet = new System.Windows.Forms.Label();
            this.cbSheetSets = new System.Windows.Forms.ComboBox();
            this.lblDrawingDirectory = new System.Windows.Forms.Label();
            this.txtDrawingDirectory = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblSheetSet);
            this.panel1.Controls.Add(this.cbSheetSets);
            this.panel1.Controls.Add(this.lblDrawingDirectory);
            this.panel1.Controls.Add(this.txtDrawingDirectory);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(554, 144);
            this.panel1.TabIndex = 0;
            // 
            // lblSheetSet
            // 
            this.lblSheetSet.AutoSize = true;
            this.lblSheetSet.BackColor = System.Drawing.Color.Transparent;
            this.lblSheetSet.Location = new System.Drawing.Point(18, 72);
            this.lblSheetSet.Name = "lblSheetSet";
            this.lblSheetSet.Size = new System.Drawing.Size(54, 13);
            this.lblSheetSet.TabIndex = 3;
            this.lblSheetSet.Text = "Sheet Set";
            // 
            // cbSheetSets
            // 
            this.cbSheetSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSheetSets.FormattingEnabled = true;
            this.cbSheetSets.Location = new System.Drawing.Point(18, 94);
            this.cbSheetSets.Name = "cbSheetSets";
            this.cbSheetSets.Size = new System.Drawing.Size(517, 21);
            this.cbSheetSets.TabIndex = 4;
            // 
            // lblDrawingDirectory
            // 
            this.lblDrawingDirectory.AutoSize = true;
            this.lblDrawingDirectory.BackColor = System.Drawing.Color.Transparent;
            this.lblDrawingDirectory.Location = new System.Drawing.Point(18, 19);
            this.lblDrawingDirectory.Name = "lblDrawingDirectory";
            this.lblDrawingDirectory.Size = new System.Drawing.Size(91, 13);
            this.lblDrawingDirectory.TabIndex = 0;
            this.lblDrawingDirectory.Text = "Drawing Directory";
            // 
            // txtDrawingDirectory
            // 
            this.txtDrawingDirectory.Location = new System.Drawing.Point(18, 45);
            this.txtDrawingDirectory.Name = "txtDrawingDirectory";
            this.txtDrawingDirectory.Size = new System.Drawing.Size(436, 20);
            this.txtDrawingDirectory.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(460, 44);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(410, 167);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(491, 167);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 202);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sheet Renamer";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MainForm_HelpButtonClicked);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDrawingDirectory;
        private System.Windows.Forms.TextBox txtDrawingDirectory;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSheetSet;
        private System.Windows.Forms.ComboBox cbSheetSets;
    }
}