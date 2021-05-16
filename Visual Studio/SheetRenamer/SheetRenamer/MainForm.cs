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
using System.Windows.Forms;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;

namespace SheetRenamer
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region CLASS_LEVEL_VARIABLES

        UIApplication myRevitUIApp = null;
        Document myRevitDoc = null;

        public string projectNumber = string.Empty;
        List<string> oldFilesInDirectory = new List<string>();

        public IList<Element> viewSheetSets = null;
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

            FilteredElementCollector sheetSetsCol = new FilteredElementCollector(myRevitDoc);

            viewSheetSets = sheetSetsCol.OfClass(typeof(ViewSheetSet)).ToElements(); //GET ALL THE SHEETSETS IN THE PROJECT

            projectNumber = myRevitDoc.ProjectInformation.LookupParameter("Project Number").AsString();

            //LOOPS THROUGH ALL THE SHEETSETS IN THE PROJECT AND FILL COMBOBOX FOR SELECTION
            foreach (ViewSheetSet vss in viewSheetSets)
            {
                cbSheetSets.Items.Add(vss.Name);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fldrBrowser = new FolderBrowserDialog();
            fldrBrowser.Description = "Select the directory where the sheets you want to rename are located";

            //GET DIRECTORY WHERE THE DRAWINGS ARE SAVED
            if (fldrBrowser.ShowDialog() == DialogResult.OK)
            {
                string dir = string.Empty;
                dir = fldrBrowser.SelectedPath;
                txtDrawingDirectory.Text = dir.Trim();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TaskDialog taskDialog = new TaskDialog("Sheet Renamer");

            string dir = txtDrawingDirectory.Text;

            if (dir == string.Empty)
            {
                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                taskDialog.MainInstruction = "No directory provided.";
                taskDialog.Show();
            }
            else if (!System.IO.Directory.Exists(dir))
            {
                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                taskDialog.MainInstruction = "The directory provided does not exist.";
                taskDialog.Show();
            }
            else if (cbSheetSets.SelectedIndex < 0)
            {
                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                taskDialog.MainInstruction = "No sheet set provided.";
                taskDialog.Show();
            }
            else
            {
                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                taskDialog.MainInstruction = "Are you sure you want to rename all the sheets in the directory below?";
                taskDialog.MainContent = dir;
                taskDialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;

                if (taskDialog.Show() == TaskDialogResult.Yes)
                {
                    ViewSet viewSet = null;

                    foreach (ViewSheetSet vs in viewSheetSets) // Get the selected sheet set
                    {
                        if (vs.Name == cbSheetSets.SelectedItem.ToString())
                        {
                            viewSet = vs.Views; // Get all the sheets in the sheet set
                        }
                    }

                    string[] files = Directory.GetFiles(dir, "*.pdf", SearchOption.TopDirectoryOnly);
                    List<string> oldFiles = new List<string>();

                    foreach (string file in files) oldFiles.Add(file);

                    // <Key>   Old file to be renamed
                    // <Value> New file name
                    Dictionary<string, string> fileDic = new Dictionary<string, string>();

                    foreach (ViewSheet v in viewSet) // Loop through all the sheets in the sheet set
                    {
                        string sheetNumber = string.Empty;
                        string sheetName = string.Empty;

                        sheetNumber = v.SheetNumber;
                        sheetName = v.Name;

                        // SHEET NUMBER needs to be checked for the following special characters below

                        // These need to be replaced with '-'
                        // / * " .

                        // Revit checks for the following characters below and don't need to be handled
                        // \ : {} [] ; < > ? ` ~

                        // REVIT & WINDOWS all the following characters below in file names
                        // ! @ # $ % ^ & * ( ) _ + = - ' ,

                        if (sheetNumber.Contains(@"/"))
                        {
                            sheetNumber = sheetNumber.Replace(@"/", "-");
                        }

                        if (sheetNumber.Contains("*"))
                        {
                            sheetNumber = sheetNumber.Replace("*", "-");
                        }

                        if (sheetNumber.Contains("\""))
                        {
                            sheetNumber = sheetNumber.Replace("\"", "-");
                        }

                        if (sheetNumber.Contains("."))
                        {
                            sheetNumber = sheetNumber.Replace(".", "-");
                        }

                        string rev = string.Empty;

                        rev = v.LookupParameter("Current Revision").AsString();

                        string newFileName = string.Empty;
                        string newFile = string.Empty;

                        newFileName = projectNumber + "-" + sheetNumber + "_" + rev + ".pdf";
                        newFile = dir + "\\" + newFileName;

                        string pattern = "- " + sheetNumber + " -";
                        string oldFile = oldFiles.Find(a => a.Contains(pattern));
                        fileDic.Add(oldFile, newFile);
                    }

                    foreach (KeyValuePair<string, string> entry in fileDic)
                    {
                        string oldFile = entry.Key;
                        string newFile = entry.Value;

                        try
                        {
                            if (File.Exists(newFile))
                            {
                                File.Delete(newFile);
                            }

                            File.Move(oldFile, newFile);
                        }
                        catch (Exception ex)
                        {
                            TaskDialog errorTaskDialog = new TaskDialog("Sheet Renamer");
                            errorTaskDialog.MainInstruction = "An error occured while renaming the files. See message below.";
                            errorTaskDialog.MainContent = "Error Message: " + ex.Message + "\nError Source: " + ex.Source;
                            errorTaskDialog.CommonButtons = TaskDialogCommonButtons.Ok;
                            errorTaskDialog.Show();
                            return;
                        }
                    }
                    TaskDialog completeTaskDialog = new TaskDialog("Sheet Renamer");
                    completeTaskDialog.MainInstruction = "The sheets have been renamed successfully";
                    completeTaskDialog.MainContent = "";
                    completeTaskDialog.CommonButtons = TaskDialogCommonButtons.Ok;
                    completeTaskDialog.Show();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string helpFile = string.Empty;
            helpFile = @"C:\Users\" + Environment.UserName + @"\Documents\CRMRevitTools\" + REVIT_VERSION + @"\CRMRevitTools_Help\sheet_renamer.html";

            if (File.Exists(helpFile))
            {
                Process.Start(helpFile);
            }
            else
            {
                TaskDialog taskDialog = new TaskDialog("Sheet Renamer");

                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                taskDialog.MainInstruction = "The Help file for Sheet Renamer could not be found. It may have been moved or deleted.";
                taskDialog.Show();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Segoe UI", 12))
            {
                var brush = new SolidBrush(System.Drawing.Color.FromArgb(0, 51, 188));

                e.Graphics.DrawString("Specify the directory where the PDF files have been printed.\n" +
                                      "Select the Sheet Set that was used to print the PDF files.\n\n" + 
                                      "Click OK to rename the PDF files. Make sure none of the\n" +
                                      "files are open before renaming.", myFont, brush, new System.Drawing.Point(0, 0));
            }
        }

        private void txtDrawingDirectory_TextChanged(object sender, EventArgs e)
        {
            string dir = txtDrawingDirectory.Text;

            if (!DrawingDirectoryIsDefault(dir))
            {
                ckbDefault.Checked = false;
                ckbDefault.Enabled = true;
            }
            else
            {
                ckbDefault.Checked = true;
                ckbDefault.Enabled = false;
            }
        }

        private bool DrawingDirectoryIsDefault(string dir)
        {
            bool flag = false;
            dir = txtDrawingDirectory.Text;
            
            string savedDir = XMLSettings.GetSettingsValue(XMLSettings.ApplicationSettings.DrawingDirectory);

            if (dir != string.Empty && System.IO.Directory.Exists(dir))
            {
                if (dir == savedDir)
                    flag = true;
                else
                    flag = false;
            }

            return flag;
        }

        private void ckbDefault_CheckedChanged(object sender, EventArgs e)
        {
            string dir = txtDrawingDirectory.Text;
            bool isChecked = ckbDefault.Checked;

            if (isChecked)
                if (dir != string.Empty && System.IO.Directory.Exists(dir))
                {
                    XMLSettings.SetSettingsValue(XMLSettings.ApplicationSettings.DrawingDirectory, dir);
                    ckbDefault.Enabled = false;
                }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            XMLSettings.CreateAppSettings_SetDefaults();

            string dir = XMLSettings.GetSettingsValue(XMLSettings.ApplicationSettings.DrawingDirectory);
            txtDrawingDirectory.Text = dir;
            txtDrawingDirectory.Select(dir.Length + 1, 0);

            ckbDefault.Checked = DrawingDirectoryIsDefault(dir);
        }
    }
}