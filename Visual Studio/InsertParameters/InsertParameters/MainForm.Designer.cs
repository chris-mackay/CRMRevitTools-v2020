//    Copyright(C) 2019-2020 Christopher Ryan Mackay

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

namespace InsertParameters
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
            this.btnInsert = new System.Windows.Forms.Button();
            this.dgvSharedParameters = new System.Windows.Forms.DataGridView();
            this.lblSharedParameterFile = new System.Windows.Forms.Label();
            this.txtSharedParameterFile = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.clmParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmParamType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBinding = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmPropertiesGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSharedParameters)).BeginInit();
            this.SuspendLayout();
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Location = new System.Drawing.Point(666, 718);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 5;
            this.btnInsert.TabStop = false;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // dgvSharedParameters
            // 
            this.dgvSharedParameters.AllowUserToAddRows = false;
            this.dgvSharedParameters.AllowUserToDeleteRows = false;
            this.dgvSharedParameters.AllowUserToOrderColumns = true;
            this.dgvSharedParameters.AllowUserToResizeColumns = false;
            this.dgvSharedParameters.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.dgvSharedParameters.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSharedParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSharedParameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSharedParameters.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvSharedParameters.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSharedParameters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSharedParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSharedParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmParamName,
            this.clmGroup,
            this.clmParamType,
            this.clmBinding,
            this.clmCategory,
            this.clmPropertiesGroup});
            this.dgvSharedParameters.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvSharedParameters.Location = new System.Drawing.Point(12, 206);
            this.dgvSharedParameters.Name = "dgvSharedParameters";
            this.dgvSharedParameters.ReadOnly = true;
            this.dgvSharedParameters.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvSharedParameters.RowHeadersVisible = false;
            this.dgvSharedParameters.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.dgvSharedParameters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSharedParameters.Size = new System.Drawing.Size(810, 506);
            this.dgvSharedParameters.TabIndex = 4;
            this.dgvSharedParameters.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvSharedParameters_MouseUp);
            // 
            // lblSharedParameterFile
            // 
            this.lblSharedParameterFile.AutoSize = true;
            this.lblSharedParameterFile.BackColor = System.Drawing.Color.Transparent;
            this.lblSharedParameterFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSharedParameterFile.Location = new System.Drawing.Point(12, 162);
            this.lblSharedParameterFile.Name = "lblSharedParameterFile";
            this.lblSharedParameterFile.Size = new System.Drawing.Size(111, 13);
            this.lblSharedParameterFile.TabIndex = 1;
            this.lblSharedParameterFile.Text = "Shared Parameter File";
            // 
            // txtSharedParameterFile
            // 
            this.txtSharedParameterFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSharedParameterFile.BackColor = System.Drawing.SystemColors.Control;
            this.txtSharedParameterFile.Enabled = false;
            this.txtSharedParameterFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtSharedParameterFile.Location = new System.Drawing.Point(12, 180);
            this.txtSharedParameterFile.Name = "txtSharedParameterFile";
            this.txtSharedParameterFile.ReadOnly = true;
            this.txtSharedParameterFile.Size = new System.Drawing.Size(729, 20);
            this.txtSharedParameterFile.TabIndex = 2;
            this.txtSharedParameterFile.TabStop = false;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLoad.Location = new System.Drawing.Point(747, 179);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.TabStop = false;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.BackColor = System.Drawing.Color.Transparent;
            this.lblInstructions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblInstructions.Location = new System.Drawing.Point(9, 9);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(687, 130);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = resources.GetString("lblInstructions.Text");
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(747, 718);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // clmParamName
            // 
            this.clmParamName.HeaderText = "Parameter Name";
            this.clmParamName.Name = "clmParamName";
            this.clmParamName.ReadOnly = true;
            // 
            // clmGroup
            // 
            this.clmGroup.HeaderText = "Parameter Group";
            this.clmGroup.Name = "clmGroup";
            this.clmGroup.ReadOnly = true;
            // 
            // clmParamType
            // 
            this.clmParamType.HeaderText = "Data Type";
            this.clmParamType.Name = "clmParamType";
            this.clmParamType.ReadOnly = true;
            // 
            // clmBinding
            // 
            this.clmBinding.HeaderText = "Binding";
            this.clmBinding.Name = "clmBinding";
            this.clmBinding.ReadOnly = true;
            // 
            // clmCategory
            // 
            this.clmCategory.HeaderText = "Category";
            this.clmCategory.Name = "clmCategory";
            this.clmCategory.ReadOnly = true;
            // 
            // clmPropertiesGroup
            // 
            this.clmPropertiesGroup.HeaderText = "Properties Group";
            this.clmPropertiesGroup.Name = "clmPropertiesGroup";
            this.clmPropertiesGroup.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 753);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvSharedParameters);
            this.Controls.Add(this.txtSharedParameterFile);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.lblSharedParameterFile);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnInsert);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(850, 400);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Insert Parameters";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSharedParameters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.DataGridView dgvSharedParameters;
        private System.Windows.Forms.Label lblSharedParameterFile;
        public System.Windows.Forms.TextBox txtSharedParameterFile;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmParamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmParamType;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBinding;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmPropertiesGroup;
    }
}