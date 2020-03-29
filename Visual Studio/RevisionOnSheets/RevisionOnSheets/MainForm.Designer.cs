//    Copyright(C) 2020 Christopher Ryan Mackay

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

namespace RevisionOnSheets
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dgvSheets = new System.Windows.Forms.DataGridView();
            this.SheetNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SheetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Set = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cbRevisions = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSheets)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSheets
            // 
            this.dgvSheets.AllowUserToAddRows = false;
            this.dgvSheets.AllowUserToDeleteRows = false;
            this.dgvSheets.AllowUserToOrderColumns = true;
            this.dgvSheets.AllowUserToResizeColumns = false;
            this.dgvSheets.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.dgvSheets.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSheets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSheets.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvSheets.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSheets.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSheets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSheets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSheets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SheetNumber,
            this.SheetName,
            this.Set});
            this.dgvSheets.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvSheets.Location = new System.Drawing.Point(12, 140);
            this.dgvSheets.Name = "dgvSheets";
            this.dgvSheets.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvSheets.RowHeadersVisible = false;
            this.dgvSheets.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.dgvSheets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSheets.Size = new System.Drawing.Size(574, 436);
            this.dgvSheets.TabIndex = 3;
            this.dgvSheets.TabStop = false;
            this.dgvSheets.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSheets_KeyDown);
            this.dgvSheets.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvSheets_KeyUp);
            this.dgvSheets.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvSheets_MouseClick);
            this.dgvSheets.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvSheets_MouseUp);
            // 
            // SheetNumber
            // 
            this.SheetNumber.FillWeight = 114.2132F;
            this.SheetNumber.HeaderText = "Sheet #";
            this.SheetNumber.Name = "SheetNumber";
            this.SheetNumber.ReadOnly = true;
            this.SheetNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SheetNumber.Width = 75;
            // 
            // SheetName
            // 
            this.SheetName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SheetName.FillWeight = 92.8934F;
            this.SheetName.HeaderText = "Sheet Name";
            this.SheetName.Name = "SheetName";
            this.SheetName.ReadOnly = true;
            // 
            // Set
            // 
            this.Set.FillWeight = 92.8934F;
            this.Set.HeaderText = "Set";
            this.Set.Name = "Set";
            this.Set.Width = 50;
            // 
            // cbRevisions
            // 
            this.cbRevisions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRevisions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRevisions.FormattingEnabled = true;
            this.cbRevisions.Location = new System.Drawing.Point(106, 102);
            this.cbRevisions.Name = "cbRevisions";
            this.cbRevisions.Size = new System.Drawing.Size(480, 21);
            this.cbRevisions.Sorted = true;
            this.cbRevisions.TabIndex = 2;
            this.cbRevisions.TabStop = false;
            this.cbRevisions.SelectedIndexChanged += new System.EventHandler(this.cbRevisions_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(511, 587);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(430, 587);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.TabStop = false;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Revision to set";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(486, 65);
            this.label2.TabIndex = 0;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 622);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbRevisions);
            this.Controls.Add(this.dgvSheets);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(614, 661);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Revision On Sheets";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSheets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ComboBox cbRevisions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGridView dgvSheets;
        private System.Windows.Forms.DataGridViewTextBoxColumn SheetNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn SheetName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Set;
        private System.Windows.Forms.Label label2;
    }
}