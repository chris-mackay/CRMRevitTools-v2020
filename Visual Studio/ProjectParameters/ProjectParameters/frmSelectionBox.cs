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

using System;
using System.Windows.Forms;

namespace ProjectParameters
{
    public partial class frmSelectionBox : Form
    {
        public frmSelectionBox()
        {
            InitializeComponent();
        }

        private void frmSelectionBox_Load(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
        }

        private void cbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbItems.SelectedIndex == -1)
                btnOK.Enabled = false;
            else
                btnOK.Enabled = true;
        }
    }
}
