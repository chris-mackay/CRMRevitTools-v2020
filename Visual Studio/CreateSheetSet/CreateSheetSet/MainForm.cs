//    Copyright(C) 2020  Christopher Ryan Mackay

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
using System.Drawing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CreateSheetSet
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region CLASS_LEVEL_VARIABLES

        UIApplication myRevitUIApp = null;
        Document myRevitDoc = null;

        public IList<Element> viewSheets = null;
        public string REVIT_VERSION = "v2019";

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
            viewSheets = sheetsCol.OfClass(typeof(ViewSheet)).ToElements();

            rbSequence.Checked = true;
            btnCreate.Enabled = false;

            LoadItems();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string prop = cbRevisions.SelectedItem.ToString();
            int selectedSequence = RevisionSequenceNumber(prop);

            ViewSet set = new ViewSet();

            foreach (ViewSheet vss in viewSheets)
            {
                IList<ElementId> revisionIds = vss.GetAllRevisionIds();

                foreach (ElementId i in revisionIds)
                {
                    Element elem = myRevitDoc.GetElement(i);
                    Revision r = elem as Revision;

                    int sequenceNumber = r.SequenceNumber;
                    string num = vss.GetRevisionNumberOnSheet(i);
                    string date = r.RevisionDate;

                    if (rbSequence.Checked)
                    {
                        if (selectedSequence == sequenceNumber)
                            set.Insert(vss);
                    }
                    else if (rbNumber.Checked)
                    {
                        if (num == prop)
                            set.Insert(vss);
                    }
                    else
                    {
                        if (date == prop)
                            set.Insert(vss);
                    }
                }
            }

            PrintManager print = myRevitDoc.PrintManager;
            print.PrintRange = PrintRange.Select;
            ViewSheetSetting viewSheetSetting = print.ViewSheetSetting;
            viewSheetSetting.CurrentViewSheetSet.Views = set;

            Transaction trans = new Transaction(myRevitDoc, "Create Sheet Set");
            trans.Start();

            try
            {
                viewSheetSetting.SaveAs(prop);
                TaskDialog dialog = new TaskDialog("Create Sheet Set");
                dialog.MainInstruction = prop + " was created successfully";
                trans.Commit();
                dialog.Show();
            }
            catch (Exception ex)
            {
                TaskDialog dialog = new TaskDialog("Create Sheet Set");
                dialog.MainInstruction = "Failed to create " + prop;
                dialog.MainContent = ex.Message;
                trans.RollBack();
                dialog.Show();
            }
        }

        private void cbRevisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRevisions.SelectedIndex == -1)
                btnCreate.Enabled = false;
            else
                btnCreate.Enabled = true;
        }

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

        private string RevisionSequenceName(Revision revision, string desc)
        {
            string seqName = string.Empty;

            seqName = "Seq. " + revision.SequenceNumber + " - " + desc;

            return seqName;
        }

        private void LoadItems()
        {
            foreach (ViewSheet vss in viewSheets)
            {
                IList<ElementId> revisionIds = vss.GetAllRevisionIds();

                foreach (ElementId i in revisionIds)
                {
                    Element elem = myRevitDoc.GetElement(i);
                    Revision r = elem as Revision;

                    if (rbSequence.Checked)
                    {
                        string sequenceName = RevisionSequenceName(r, r.Description);

                        if (!cbRevisions.Items.Contains(sequenceName))
                            cbRevisions.Items.Add(sequenceName);
                    }
                    else if (rbNumber.Checked)
                    {
                        if (!cbRevisions.Items.Contains(vss.GetRevisionNumberOnSheet(i)))
                            cbRevisions.Items.Add(vss.GetRevisionNumberOnSheet(i));
                    }
                    else
                    {
                        if (!cbRevisions.Items.Contains(r.RevisionDate))
                            cbRevisions.Items.Add(r.RevisionDate);
                    }
                }
            }
        }

        private void radioButtonCheckChanged(object sender, EventArgs e)
        {
            cbRevisions.Items.Clear();
            cbRevisions.SelectedIndex = -1;

            if (cbRevisions.SelectedIndex == -1)
                btnCreate.Enabled = false;
            else
                btnCreate.Enabled = true;

            LoadItems();
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            using (Font myFont = new Font("Segoe UI", 12))
            {
                var brush = new SolidBrush(System.Drawing.Color.FromArgb(0, 51, 188));

                e.Graphics.DrawString("Select a revision from the dropdown list below to create\n" +
                                      "a sheet set containing all the sheets with the selected\n" +
                                      "revision property", myFont, brush, new System.Drawing.Point(0, 0));
            }
        }
    }
}