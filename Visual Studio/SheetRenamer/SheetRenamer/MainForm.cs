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
        public string REVIT_VERSION = "v2020";

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

            string dir = txtDrawingDirectory.Text.Trim();

            string[] fileEntries = Directory.GetFiles(dir); //GET ALL THE FILES IN THE SELECTED DIRECTORY FOR RENAMING

            foreach (string oldFile in fileEntries)
            {
                oldFilesInDirectory.Add(oldFile);
            }

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
                    List<string> newFiles = new List<string>();
                    ViewSet viewSet = null;

                    //GET ALL THE SHEETS FROM THE SHEETSET SELECTED
                    foreach (ViewSheetSet vs in viewSheetSets)
                    {
                        if (vs.Name == cbSheetSets.SelectedItem.ToString())
                        {
                            viewSet = vs.Views;
                        }
                    }

                    List<string> reOrderedFiles = new List<string>();

                    //LOOP THROUGH ALL THE SHEETS FROM THE SHEETSET, CREATE NEW SHEET NAMES, AND FILL NEW FILE LIST
                    foreach (ViewSheet oldSheet in viewSet)
                    {

                        string sheetNumber = string.Empty;
                        string sheetName = string.Empty;

                        sheetNumber = oldSheet.SheetNumber;
                        sheetName = oldSheet.Name;

                        // SHEET NUMBER NEEDS TO BE CHECKED FOR THE FOLLOWING SPECIAL CHARACTERS BELOW

                        // THESE NEED TO BE REPLACED WITH '-'
                        // / * " .

                        // REVIT CHECKS FOR THE FOLLOWING CHARACTERS BELOW AND DON'T NEED TO BE HANDLED
                        // \ : {} [] ; < > ? ` ~

                        // REVIT & WINDOWS ALLOW THE CHARACTERS BELOW
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

                        rev = oldSheet.LookupParameter("Current Revision").AsString();

                        string newFileName = string.Empty;
                        string newFile = string.Empty;

                        newFileName = projectNumber + "-" + sheetNumber + "_" + rev + ".pdf"; //DPS STANDARD FILE NAMING CONVENTION (E.G. 816075-HE-100_0.pdf)
                        newFile = dir + "\\" + newFileName;

                        newFiles.Add(newFile);

                        foreach (string file in oldFilesInDirectory)
                        {
                            if (file.Contains(sheetNumber))
                            {
                                reOrderedFiles.Add(file);
                            }
                        }
                    }

                    int index = 0;

                    //LOOP THROUGH EACH FILE IN THE DIRECTORY AND RENAME THE FILE
                    foreach (string oldFile in reOrderedFiles)
                    {
                        try
                        {
                            string newFile = string.Empty;
                            newFile = newFiles[index];

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

                        index += 1;

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
    }
}