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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateRevitSheets
{
    public partial class frmAddSheet : Form
    {
        public frmAddSheet()
        {
            InitializeComponent();
        }

        private void frmAddSheet_Load(object sender, EventArgs e)
        {
            if (txtSheetNumber.TextLength > 0 && txtSheetName.TextLength > 0)
            {
                btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        private void txtSheetNumber_TextChanged(object sender, EventArgs e)
        {
            if (txtSheetNumber.TextLength > 0 && txtSheetName.TextLength > 0)
            {
                btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        private void txtSheetName_TextChanged(object sender, EventArgs e)
        {
            if (txtSheetNumber.TextLength > 0 && txtSheetName.TextLength > 0)
            {
                btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }
    }
}
