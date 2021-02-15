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

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevisionOnSheets
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region CLASS_LEVEL_VARIABLES

        UIApplication myRevitUIApp = null;
        Document myRevitDoc = null;

        public IList<Element> viewSheets_ENTIRE_PROJECT = null;
        public IList<Element> revisions_ENTIRE_PROJECT = null;
        public string REVIT_VERSION = "v2019";
        private bool shiftKeyIsDown = false;

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(UIApplication incomingUIApp)
        {
            InitializeComponent();

            myRevitUIApp = incomingUIApp;
            myRevitDoc = myRevitUIApp.ActiveUIDocument.Document;

            FilteredElementCollector sheetsCol = new FilteredElementCollector(myRevitDoc);
            viewSheets_ENTIRE_PROJECT = sheetsCol.OfClass(typeof(ViewSheet)).ToElements();

            LoadRevisions(cbRevisions);

            cbRevisions.SelectedIndex = 0;
            int seq = RevisionSequenceNumber(cbRevisions.SelectedItem.ToString());

            if (viewSheets_ENTIRE_PROJECT.Count > 0)
            {
                LoadSheets(dgvSheets);
                SetCheckboxes(dgvSheets, seq);
                dgvSheets[1, 0].Selected = true;
                dgvSheets.ClearSelection();
            }
        }

        #region Voids

        private void ColorRows()
        {
            DrawingControl.SetDoubleBuffered(dgvSheets);
            DrawingControl.SuspendDrawing(dgvSheets);

            foreach (DataGridViewRow row in dgvSheets.Rows)
            {
                bool set = bool.Parse(row.Cells["Set"].Value.ToString());

                if (set)
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                else
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            }

            DrawingControl.ResumeDrawing(dgvSheets);
        }

        private void LoadRevisions(System.Windows.Forms.ComboBox comboBox)
        {
            FilteredElementCollector revCol = new FilteredElementCollector(myRevitDoc);
            revisions_ENTIRE_PROJECT = revCol.OfClass(typeof(Revision)).ToElements();

            foreach (Revision revision in revisions_ENTIRE_PROJECT)
            {
                string seq = RevisionSequenceName(revision, revision.Description);

                if (!comboBox.Items.Contains(seq)) comboBox.Items.Add(seq);
            }
        }

        private void RemoveRevisionOnSheet(ViewSheet viewSheet, Revision revisionToRemove)
        {
            IList<ElementId> revisionIds = null;
            revisionIds = viewSheet.GetAllRevisionIds();
            revisionIds.Remove(revisionToRemove.Id);

            viewSheet.SetAdditionalRevisionIds(revisionIds);
        }

        private void AddRevisionOnSheet(ViewSheet viewSheet, Revision revisionToAdd)
        {
            IList<ElementId> revisionIds = null;
            revisionIds = viewSheet.GetAllRevisionIds();
            revisionIds.Add(revisionToAdd.Id);

            viewSheet.SetAdditionalRevisionIds(revisionIds);
        }

        private void SetRevisionOnSheets()
        {
            try
            {
                Transaction trans = new Transaction(myRevitDoc, "Revision On Sheets");
                trans.Start();

                string selectedSequenceName = cbRevisions.SelectedItem.ToString();

                foreach (DataGridViewRow row in dgvSheets.Rows)
                    foreach (ViewSheet viewSheet in viewSheets_ENTIRE_PROJECT)
                    {
                        string sheetNumber = row.Cells["SheetNumber"].Value.ToString();
                        bool set = bool.Parse(row.Cells["Set"].Value.ToString());

                        if (viewSheet.SheetNumber == sheetNumber && set == true)
                        {
                            int seq = RevisionSequenceNumber(selectedSequenceName);

                            foreach (Revision revision in revisions_ENTIRE_PROJECT)
                                if (revision.SequenceNumber == seq) AddRevisionOnSheet(viewSheet, revision);
                        }
                        else if (viewSheet.SheetNumber == sheetNumber && set == false)
                        {
                            int seq = RevisionSequenceNumber(selectedSequenceName);

                            foreach (Revision revision in revisions_ENTIRE_PROJECT)
                                if (revision.SequenceNumber == seq) RemoveRevisionOnSheet(viewSheet, revision);
                        }
                    }

                trans.Commit();
            }
            catch (Exception ex)
            {
                TaskDialog td = new TaskDialog("Error");
                td.MainInstruction = "Failed to set revision";
                td.MainContent = ex.Message;
                td.Show();
                return;
            }
        }

        private void SetCheckboxes(DataGridView dataGridView, int sequence)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
                foreach (ViewSheet viewSheet in viewSheets_ENTIRE_PROJECT)
                    if (row.Cells["SheetNumber"].Value.ToString() == viewSheet.SheetNumber)
                        if (RevisionIsOnSheet(viewSheet, sequence))
                        {
                            row.Cells["Set"].Value = true;
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                        }
                        else
                        {
                            row.Cells["Set"].Value = false;
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                        }
        }

        private void LoadSheets(DataGridView dataGridView)
        {
            DrawingControl.SetDoubleBuffered(dataGridView);
            DrawingControl.SuspendDrawing(dataGridView);

            foreach (ViewSheet viewSheet in viewSheets_ENTIRE_PROJECT)
            {
                string number = viewSheet.SheetNumber;
                string name = viewSheet.Name;
                dataGridView.Rows.Add(number, name, false);
            }

            DrawingControl.ResumeDrawing(dataGridView);
        }

        #endregion

        #region Functions

        private int RevisionSequenceNumber(string selectedSequenceName)
        {
            int seqNum = 0;

            int from = selectedSequenceName.IndexOf("Seq. ") + "Seq. ".Length;
            int to = selectedSequenceName.IndexOf(" - ");

            string num = selectedSequenceName.Substring(from, to - from);
            num = num.Trim();

            seqNum = int.Parse(num);

            return seqNum;
        }

        private ContextMenu SheetsContextMenu()
        {
            ContextMenu mnu = new ContextMenu();
            MenuItem cxmnuSetAll = new MenuItem("Set All");
            MenuItem cxmnuUnsetAll = new MenuItem("Unset All");

            cxmnuSetAll.Click += new EventHandler(cxmnuSelectAll_Click);
            cxmnuUnsetAll.Click += new EventHandler(cxmnuUnselectAll_Click);

            mnu.MenuItems.Add(cxmnuSetAll);
            mnu.MenuItems.Add(cxmnuUnsetAll);

            return mnu;
        }

        private bool RevisionIsOnSheet(ViewSheet viewSheet, int sequence)
        {
            IList<ElementId> revisionIds = viewSheet.GetAllRevisionIds();
            bool flag = false;

            foreach (ElementId i in revisionIds)
            {
                Element elem = myRevitDoc.GetElement(i);
                Revision r = elem as Revision;

                if (r.SequenceNumber == sequence) flag = true; else flag = false;
                if (flag) break;
            }
            return flag;
        }

        private string RevisionSequenceName(Revision revision, string desc)
        {
            string seqName = string.Empty;
            seqName = "Seq. " + revision.SequenceNumber + " - " + desc;

            return seqName;
        }

        #endregion

        #region Events

        private void cxmnuUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSheets.Rows)
                row.Cells["Set"].Value = false;
        }

        private void cxmnuSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSheets.Rows)
                row.Cells["Set"].Value = true;
        }

        private void dgvSheets_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu = SheetsContextMenu();
                contextMenu.Show(dgvSheets, new System.Drawing.Point(e.X, e.Y));
            }
        }

        private void cbRevisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seq = RevisionSequenceNumber(cbRevisions.SelectedItem.ToString());
            SetCheckboxes(dgvSheets, seq);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SetRevisionOnSheets();
        }

        private void dgvSheets_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.ShiftKey | Keys.Shift))
                shiftKeyIsDown = true;

            if (dgvSheets.SelectedRows.Count == 1)
            {
                if (e.KeyData == (Keys.Space))
                {
                    DataGridViewSelectedRowCollection rows = dgvSheets.SelectedRows;

                    foreach (DataGridViewRow row in rows)
                    {
                        bool set = bool.Parse(row.Cells["Set"].Value.ToString());
                        row.Cells["Set"].Value = !set;

                        if (bool.Parse(row.Cells["Set"].Value.ToString()) == true)
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                        else
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        private void dgvSheets_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.ShiftKey | Keys.Shift))
                shiftKeyIsDown = false;
        }

        private void dgvSheets_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && shiftKeyIsDown
                && dgvSheets.CurrentCell is DataGridViewCheckBoxCell)
            {
                foreach (DataGridViewColumn col in dgvSheets.Columns)
                    foreach (DataGridViewRow row in dgvSheets.Rows)
                        if (dgvSheets[col.Index, row.Index] is DataGridViewCheckBoxCell)
                            if (row.Selected)
                            {
                                bool set = bool.Parse(dgvSheets[col.Index, row.Index].Value.ToString());
                                row.Cells["Set"].Value = !set;

                                if (bool.Parse(row.Cells["Set"].Value.ToString()))
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                                else
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                            }

            }

            ColorRows();
        }

        private void btnAppy_Click(object sender, EventArgs e)
        {
            SetRevisionOnSheets();
        }

        #endregion

        public static class DrawingControl
        {
            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr _hWnd, Int32 _wMsg, bool _wParam, Int32 _lParam);

            private const int WM_SETREDRAW = 11;

            public static void SetDoubleBuffered(System.Windows.Forms.Control _ctrl)
            {
                if (!SystemInformation.TerminalServerSession)
                {
                    typeof(System.Windows.Forms.Control).InvokeMember("DoubleBuffered", (System.Reflection.BindingFlags.SetProperty
                                    | (System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)), null, _ctrl, new object[] {
                            true});
                }
            }

            public static void SetDoubleBuffered_ListControls(List<System.Windows.Forms.Control> _ctrlList)
            {
                if (!SystemInformation.TerminalServerSession)
                {
                    foreach (System.Windows.Forms.Control ctrl in _ctrlList)
                    {
                        typeof(System.Windows.Forms.Control).InvokeMember("DoubleBuffered", (System.Reflection.BindingFlags.SetProperty
                                        | (System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)), null, ctrl, new object[] {
                                true});
                    }
                }
            }

            public static void SuspendDrawing(System.Windows.Forms.Control _ctrl)
            {
                SendMessage(_ctrl.Handle, WM_SETREDRAW, false, 0);
            }

            public static void SuspendDrawing_ListControls(List<System.Windows.Forms.Control> _ctrlList)
            {
                foreach (System.Windows.Forms.Control ctrl in _ctrlList)
                {
                    SendMessage(ctrl.Handle, WM_SETREDRAW, false, 0);
                }
            }

            public static void ResumeDrawing(System.Windows.Forms.Control _ctrl)
            {
                SendMessage(_ctrl.Handle, WM_SETREDRAW, true, 0);
                _ctrl.Refresh();
            }

            public static void ResumeDrawing_ListControls(List<System.Windows.Forms.Control> _ctrlList)
            {
                foreach (System.Windows.Forms.Control ctrl in _ctrlList)
                {
                    SendMessage(ctrl.Handle, WM_SETREDRAW, true, 0);
                    ctrl.Refresh();
                }
            }
        }
        
    }
}