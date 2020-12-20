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
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Text;
using System.Diagnostics;

namespace CreateRevitSheets
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region CLASS LEVEL VARIABLES

        //REVIT
        public UIApplication revitUIApp;
        public Document revitDoc;

        //TITLEBLOCKS
        private FilteredElementCollector titleblockCollector;

        //VIEWS
        private ElementCategoryFilter viewFilter;
        private FilteredElementCollector viewCollector;

        //VIEW SHEETS
        private FilteredElementCollector viewSheetCollector;

        //ILISTS
        private IList<Element> viewList;

        //ISETS
        private ISet<ElementId> usedViewIdsOnSheet;

        //LISTS
        private List<string> usedViewNames;
        private List<string> usedViewSheetNumbers;

        //VIEW DICTIONARY
        private Dictionary<string, ElementId> viewDictionary;
        
        //STRINGS
        private string titleBlockName;
        private string REVIT_VERSION = "v2020";
        
        #endregion
       
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(UIApplication incomingUIApp)
        {
            InitializeComponent();
            revitUIApp = incomingUIApp;
            revitDoc = revitUIApp.ActiveUIDocument.Document;

            dgvSheetToCreate.Rows.Clear();

            //TITLEBLOCKS
            titleblockCollector = new FilteredElementCollector(revitDoc);
            titleblockCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            titleblockCollector.WhereElementIsElementType(); //CONTAINS ALL THE TITLEBLOCKS IN THE PROJECT

            //VIEWS
            viewCollector = new FilteredElementCollector(revitDoc);
            viewFilter = new ElementCategoryFilter(BuiltInCategory.OST_Views);
            viewList = viewCollector.WherePasses(viewFilter).ToElements(); //CONTAINS ALL THE VIEWS IN THE PROJECT
            
            //VIEW SHEETS
            viewSheetCollector = new FilteredElementCollector(revitDoc);
            viewSheetCollector.OfClass(typeof(Autodesk.Revit.DB.ViewSheet));

            //HASH SETS
            usedViewIdsOnSheet = new HashSet<ElementId>();

            //LISTS
            usedViewNames = new List<string>();
            usedViewSheetNumbers = new List<string>();

            //VIEW DICTIONARY
            viewDictionary = new Dictionary<string, ElementId>();
         
        }
        
        private void cbTitleblocks_SelectedIndexChanged(object sender, EventArgs e)
        {

            titleBlockName = cbTitleblocks.SelectedItem.ToString();
            titleBlockName = titleBlockName.Substring(titleBlockName.IndexOf(":") + 1).Trim();

            if (cbTitleblocks.SelectedIndex != -1 && dgvSheetToCreate.Rows.Count > 0) //CHECKS TO MAKE SURE A TITLEBLOCK IS SELECTED AND THERE IS AT LEAST 1 SHEET TO CREATE
            {
                btnCreate.Enabled = true;
            }
            else
            {
                btnCreate.Enabled = false;
            }

        }
                
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.GetAllAvailableViewNamesAndIds();

            this.GetAllTitleblocks(titleblockCollector); //FILLS COMBOBOX WITH ALL THE TITLEBLOCKS IN THE DOCUMENT

            cbViewTypes.SelectedIndex = 0; //SELECT FLOOR PLANS BY DEFAULT
            btnCreate.Enabled = false;
        }
        
        public Result LoadTitleblock()
        {

            //GET THE REVIT LIBRARY PATH AS DEFINED VIA THE OPTIONS DIALOG - FILE LOCATIONS TAB - PLACES BUTTON
            string libraryPath = "";
            revitUIApp.Application.GetLibraryPaths().TryGetValue("Imperial Library", out libraryPath);

            if (string.IsNullOrEmpty(libraryPath))
            {
                libraryPath = "c:\\";   //DEFAULT PATH
            }

            //ALLOW THE USER TO SELECT A FAMILY FILE.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = libraryPath;
            openFileDialog1.Filter = "Family Files (*.rfa)|*.rfa";

            //LOAD THE FAMILY FILE
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {

                //CREATE TRANSACTION
                Transaction trans = new Transaction(revitDoc, "Load Titleblock");
                trans.Start();

                Autodesk.Revit.DB.Family family = null;
                if (revitDoc.LoadFamily(openFileDialog1.FileName, out family))
                {
                    
                    //LOADS ALL TITLEBLOCKS
                    //CREATE A FILTER TO GET ALL THE TITLEBLOCK TYPES
                    FilteredElementCollector titleblockCollector = new FilteredElementCollector(revitDoc);
                    titleblockCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);
                    titleblockCollector.WhereElementIsElementType();

                    this.cbTitleblocks.Items.Clear(); //CLEARS ALL TITLEBLOCKS IN THE LIST AND RELOADS ALL TO INCLUDE THE RECENTLY ADDED TITLEBLOCK

                    this.GetAllTitleblocks(titleblockCollector); //FILLS COMBOBOX WITH ALL THE TITLEBLOCKS IN THE DOCUMENT

                }
                else
                {
                    TaskDialog.Show("Revit", "Can't load the family file.");
                    return Result.Failed;
                }

                trans.Commit();

            }

            return Result.Succeeded;
        }

        private ContextMenu TableContextMenu()
        {
            ContextMenu mnu = new ContextMenu();
            MenuItem cxmnuAddSheet = new MenuItem("Add Sheet");
            MenuItem cxmnuEditSheet = new MenuItem("Edit Sheet");
            MenuItem cxmnuRemoveSheet = new MenuItem("Remove Sheet");
            MenuItem cxmnuAddView = new MenuItem("Add View -->");
            MenuItem cxmnuRemoveView = new MenuItem("<-- Remove View");

            cxmnuAddSheet.Click += new EventHandler(cxmnuAddSheet_Click);
            cxmnuEditSheet.Click += new EventHandler(cxmnuEditSheet_Click);
            cxmnuRemoveSheet.Click += new EventHandler(cxmnuRemoveSheet_Click);
            cxmnuAddView.Click += new EventHandler(cxmnuAddView_Click);
            cxmnuRemoveView.Click += new EventHandler(cxmnuRemoveView_Click);

            mnu.MenuItems.Add(cxmnuAddSheet);
            mnu.MenuItems.Add(cxmnuEditSheet);
            mnu.MenuItems.Add(cxmnuRemoveSheet);
            mnu.MenuItems.Add("-");
            mnu.MenuItems.Add(cxmnuAddView);
            mnu.MenuItems.Add(cxmnuRemoveView);

            return mnu;
        }

        private void dgvSheetToCreate_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu = TableContextMenu();
                contextMenu.Show(dgvSheetToCreate, new System.Drawing.Point(e.X, e.Y));
            }
        }

        public void GetAllSheetsToCreateFromCSV(string _filename)
        {
            List<string> disregardedSheetsNumbersList = new List<string>();

            StringBuilder disregardedSheetNumbers = new StringBuilder();

            dgvSheetToCreate.Rows.Clear();

            if (!CSVHasDuplicates(_filename))
            {
                string csvLine = null;
                StreamReader reader = new StreamReader(_filename);

                while ((csvLine = reader.ReadLine()) != null)
                {
                    char[] separator = new char[] { ',' };
                    string[] values = csvLine.Split(separator, StringSplitOptions.None);

                    try
                    {
                        //MAKE SURE BOTH VALUES ARE VALID
                        if (values[0] != "" && values[1] != "")
                        {
                            string sheetNumber = null;
                            string sheetName = null;

                            sheetNumber = values[0];
                            sheetName = values[1];

                            string entry = string.Empty;

                            entry = sheetNumber + ":" + sheetName;

                            if (usedViewSheetNumbers.Contains(sheetNumber))
                            {
                                disregardedSheetsNumbersList.Add(sheetNumber);
                                disregardedSheetNumbers.Append(sheetNumber + "\n");
                            }
                            else
                            {
                                this.dgvSheetToCreate.Rows.Add(entry, ""); //ADDS THE NEW SHEET TO THE LIST
                            }
                        }
                        else
                        {
                            TaskDialog taskDialog = new TaskDialog("Create Sheets");

                            taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                            taskDialog.MainInstruction = "A Sheet Name and Sheet Number must be provided for each entry.";
                            taskDialog.MainContent = "Load Sheets has been cancelled. Check your file and try again.";
                            taskDialog.Show();
                            break;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        TaskDialog taskDialog = new TaskDialog("Create Sheets");

                        taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                        taskDialog.MainInstruction = "A Sheet Name and Sheet Number must be provided for each entry.";
                        taskDialog.MainContent = "Load Sheets has been cancelled. Check your file and try again.";
                        taskDialog.Show();
                        break;
                    }
                }

                reader.Close();
                reader = null;
            }
            else
            {
                TaskDialog taskDialog = new TaskDialog("Create Sheets");

                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                taskDialog.MainInstruction = "There are duplicate sheet numbers in the csv file.";
                taskDialog.MainContent = "Remove the duplicates in your file and try again.";
                taskDialog.Show();
            }

            if (disregardedSheetsNumbersList.Count > 0)
            {   
                TaskDialog taskDialog = new TaskDialog("Create Sheets");

                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
                taskDialog.MainInstruction = "The following sheets already exist in the project and will not be added to the list.";
                taskDialog.MainContent = disregardedSheetNumbers.ToString();
                taskDialog.Show();
            }

            if (cbTitleblocks.SelectedIndex != -1 && dgvSheetToCreate.Rows.Count > 0) //CHECKS TO MAKE SURE A TITLEBLOCK IS SELECTED AND THERE IS AT LEAST 1 SHEET TO CREATE
            {
                btnCreate.Enabled = true;
            }
            else
            {
                btnCreate.Enabled = false;
            }
        }

        public void GetAllAvailableViewNamesAndIds()
        {
            foreach (ViewSheet sheet in viewSheetCollector)
            {
                usedViewSheetNumbers.Add(sheet.SheetNumber);
                usedViewIdsOnSheet = sheet.GetAllPlacedViews();

                foreach (ElementId id in usedViewIdsOnSheet)
                {

                    Element elem = null;
                    elem = revitDoc.GetElement(id);

                    Autodesk.Revit.DB.View v = null;
                    v = elem as Autodesk.Revit.DB.View;

                    string vName = string.Empty;
                    vName = v.Name + " (" + v.ViewType + ")";

                    if (!v.IsTemplate)
                    {
                        usedViewNames.Add(vName);
                    }

                }
            }
            
            foreach (Autodesk.Revit.DB.View v in viewList)
            {
                string vName = string.Empty;
                vName = v.Name + " (" + v.ViewType + ")";

                if (!v.IsTemplate)
                {
                    if (!usedViewNames.Contains(vName))
                    {
                        viewDictionary.Add(vName, v.Id);
                    }
                }

            }
        }

        public void GetAllTitleblocks(FilteredElementCollector _collector)
        {
            foreach (FamilySymbol f in _collector)
            {
                if (f.Name != null && f.Name != "Start Page")
                {
                    this.cbTitleblocks.Items.Add(f.Family.Name + " : " + f.Name); //FILLS COMBOBOX WITH ALL THE TITLEBLOCKS IN THE DOCUMENT
                }
            }
        }

        public void GetAllAvailableViewsAndFill_lstAvailableViews(IList<Element> _viewList, ViewType _viewType)
        {

            List<string> viewsToAdd = new List<string>();

            if (dgvSheetToCreate.Rows.Count > 0)
            {
                //FILL LIST OF VIEWS
                foreach (DataGridViewRow item in dgvSheetToCreate.Rows)
                {
                    string view = string.Empty;
                    view = Convert.ToString(item.Cells["View"].Value);
                    viewsToAdd.Add(view);
                }
            }

            foreach (Autodesk.Revit.DB.View v in _viewList)
            {
                if (v.ViewType == _viewType)
                {
                    string vName = string.Empty;
                    vName = v.Name + " (" + v.ViewType + ")";

                    if (!v.IsTemplate)
                    {
                        if (!lstAvailableViews.Items.Contains(vName) && !viewsToAdd.Contains(vName) && !usedViewNames.Contains(vName))
                        {
                            this.lstAvailableViews.Items.Add(vName); //FILLS LISTBOX WITH ALL THE AVAILABLE VIEWS IN THE DOCUMENT BASED ON SELECTED VIEW TYPE
                        }
                    }
                }
            }
        }
        
        private void cbViews_SelectedIndexChanged(object sender, EventArgs e)
        {

            string viewTypeSelection = string.Empty;
            viewTypeSelection = cbViewTypes.SelectedItem.ToString();

            //GET ALL VIEW TYPES BASED ON USER SELECTION
            //FLOOR PLANS
            //CEILING PLANS
            //DRAFTING VIEWS
            //LEGENDS
            //SECTIONS
            //ELEVATIONS
            switch (viewTypeSelection)
            {
                case "Floor Plans":
                    lstAvailableViews.Items.Clear();
                    this.GetAllAvailableViewsAndFill_lstAvailableViews(viewList, ViewType.FloorPlan); //FILLS LISTBOX WITH ALL THE FLOOR PLANS IN THE DOCUMENT  
                    break;
                case "Ceiling Plans":
                    lstAvailableViews.Items.Clear();
                    this.GetAllAvailableViewsAndFill_lstAvailableViews(viewList, ViewType.CeilingPlan); //FILLS LISTBOX WITH ALL THE LEGENDS IN THE DOCUMENT  
                    break;
                case "Drafting Views":
                    lstAvailableViews.Items.Clear();
                    this.GetAllAvailableViewsAndFill_lstAvailableViews(viewList, ViewType.DraftingView); //FILLS LISTBOX WITH ALL THE DRAFTING VIEWS IN THE DOCUMENT  
                    break;
                case "Legends":
                    lstAvailableViews.Items.Clear();
                    this.GetAllAvailableViewsAndFill_lstAvailableViews(viewList, ViewType.Legend); //FILLS LISTBOX WITH ALL THE LEGENDS IN THE DOCUMENT  
                    break;
                case "Sections":
                    lstAvailableViews.Items.Clear();
                    this.GetAllAvailableViewsAndFill_lstAvailableViews(viewList, ViewType.Section); //FILLS LISTBOX WITH ALL THE SECTIONS IN THE DOCUMENT  
                    break;
                case "Elevations":
                    lstAvailableViews.Items.Clear();
                    this.GetAllAvailableViewsAndFill_lstAvailableViews(viewList, ViewType.Elevation); //FILLS LISTBOX WITH ALL THE ELEVATIONS IN THE DOCUMENT 
                    break;
                default:
                    break;
            }            
        }
        
        #region BUTTON EVENTS

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            //ALLOW THE USER TO SELECT A SHEET LIST FILE.
            OpenFileDialog ofd = new OpenFileDialog();

            string libraryPath = string.Empty;
            libraryPath = "c:\\"; //DEFAULT PATH
            ofd.InitialDirectory = libraryPath;

            ofd.Filter = "CSV (Comma delimited) (.csv) Files (*.csv)|*.csv";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = null;
                fileName = ofd.FileName;
                txtFilename.Text = fileName;

                TaskDialog tdConfirmSheetLoad = new TaskDialog("Load Sheets");
                tdConfirmSheetLoad.MainInstruction = "Are you sure you want to load sheets from " + fileName + "?";
                tdConfirmSheetLoad.MainContent = "If you have already created sheets using 'Add Sheet', these will be overwritten.";
                tdConfirmSheetLoad.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;

                if (tdConfirmSheetLoad.Show() == TaskDialogResult.Yes)
                {
                    this.GetAllSheetsToCreateFromCSV(fileName); //FILLS THE LISTBOX WITH THE SHEETS YOU WANT TO CREATE
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateSheets(); //MAIN FUNCTION THAT CREATES SHEETS AND ASSIGNS VIEW TO SHEETS
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddSheet_Click(object sender, EventArgs e)
        {
            AddSheet();
        }

        private void btnRemoveSheet_Click(object sender, EventArgs e)
        {
            RemoveSheet();
        }

        private void btnEditSheet_Click(object sender, EventArgs e)
        {
            EditSheet();
        }

        private void btnAddView_Click(object sender, EventArgs e)
        {
            AddView();
        }

        private void btnRemoveView_Click(object sender, EventArgs e)
        {
            RemoveView();
        }

        private void cxmnuAddSheet_Click(object sender, EventArgs e)
        {
            AddSheet();
        }

        private void cxmnuEditSheet_Click(object sender, EventArgs e)
        {
            EditSheet();
        }

        private void cxmnuRemoveSheet_Click(object sender, EventArgs e)
        {
            RemoveSheet();
        }

        private void cxmnuAddView_Click(object sender, EventArgs e)
        {
            AddView();
        }

        private void cxmnuRemoveView_Click(object sender, EventArgs e)
        {
            RemoveView();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string helpFile = string.Empty;
            helpFile = @"C:\Users\" + Environment.UserName + @"\Documents\CRMRevitTools\" + REVIT_VERSION + @"\CRMRevitTools_Help\create_revit_sheets.html";

            if (File.Exists(helpFile))
            {
                Process.Start(helpFile);
            }
            else
            {
                TaskDialog taskDialog = new TaskDialog("Create Sheets");

                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                taskDialog.MainInstruction = "The Help file for Create Sheets could not be found. It may have been moved or deleted.";
                taskDialog.Show();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTitleblock();
            }
            catch (Exception ex)
            {

                TaskDialog.Show("Error", ex.Message + "\n\n" + ex.Source);
            }
        }

        #endregion

        #region FUNCTIONS

        private bool CSVHasDuplicates(string _filename)
        {
            bool flag = false;
            string csvLine = null;
            StreamReader reader = new StreamReader(_filename);
            List<string> numbers = new List<string>();

            while ((csvLine = reader.ReadLine()) != null)
            {
                char[] separator = new char[] { ',' };
                string[] values = csvLine.Split(separator, StringSplitOptions.None);
                numbers.Add(values[0]);
            }

            var hashset = new HashSet<string>();
            foreach (var num in numbers)
            {
                if (!hashset.Add(num))
                {
                    flag = true;
                    break;
                }
            }

            reader.Close();
            reader = null;

            return flag;
        }

        #endregion

        #region HELPER VOIDS

        private void CreateSheets()
        {
            TaskDialog taskDialog = new TaskDialog("Create Sheets");

            taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
            taskDialog.MainInstruction = "Are you sure you want to create these sheets?";
            taskDialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;

            string EXviewId = string.Empty;
            string EXviewToAdd = string.Empty;

            if (this.cbTitleblocks.Items.Count == 0)
            {
                TaskDialog.Show("No Titleblocks Loaded", "Make sure you have a titleblock loaded and selected before continuing.");
            }
            else
            {
                if (taskDialog.Show() == TaskDialogResult.Yes)
                {
                    Transaction trans = new Transaction(revitDoc, "Create Sheets");

                    try
                    {

                        #region SELECTS SPECIFIC TITLEBLOCK AND VIEW FROM DOCUMENT
                                               
                        var query = from element in titleblockCollector where element.Name == this.titleBlockName select element;
                        List<Element> titleblockList = query.ToList<Element>();
                        ElementId titleBlockid = titleblockList[0].Id;

                        #endregion

                        #region  READ FROM lstSheetsToCreate AND CREATE SHEETS

                        trans.Start(); //STARTS THE Create Sheets TRANSACTION

                        foreach (DataGridViewRow row in dgvSheetToCreate.Rows)
                        {
                            string sheet = string.Empty;
                            sheet = Convert.ToString(row.Cells["Sheet"].Value);

                            char[] separator = new char[] { ':' };
                            string[] values = sheet.Split(separator, StringSplitOptions.None);

                            string viewToAdd = string.Empty;
                            viewToAdd = Convert.ToString(row.Cells["View"].Value); //SELECT A SPECIFIC VIEW
                            EXviewToAdd = viewToAdd;

                            if (viewToAdd != string.Empty) //IF THERE IS A VIEW ASSIGNED TO A SHEET THEN CREATE A VIEWPORT
                            {

                                ViewSheet vsSheet = ViewSheet.Create(revitDoc, titleBlockid); //CREATES A NEW SHEET

                                vsSheet.SheetNumber = values[0]; //SETS THE SHEET NUMBER
                                vsSheet.Name = values[1]; //SETS THE SHEET NAME

                                ElementId viewId = null;
                                viewId = viewDictionary[viewToAdd];
                                EXviewId = viewId.ToString();

                                //GETS THE CENTER OF THE SCREEN TO ADD THE VIEW
                                UV location = new UV((vsSheet.Outline.Max.U - vsSheet.Outline.Min.U) / 2, (vsSheet.Outline.Max.V - vsSheet.Outline.Min.V) / 2);

                                Viewport.Create(revitDoc, vsSheet.Id, viewId, new XYZ(location.U, location.V, 0)); //PLACES THE VIEW ONTO THE SHEET

                            }
                            else //IF THERE IS NOT A VIEW ASSIGNED TO A SHEET THEN JUST CREATE AN EMPTY SHEET
                            {
                                ViewSheet vsSheet = ViewSheet.Create(revitDoc, titleBlockid); //CREATES A NEW SHEET

                                vsSheet.SheetNumber = values[0]; //SETS THE SHEET NUMBER
                                vsSheet.Name = values[1]; //SETS THE SHEET NAME

                            }

                        }

                        trans.Commit();
                        this.Close();

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        TaskDialog errorMessage = new TaskDialog("Create Sheet Error");
                        errorMessage.MainInstruction = "An error occurrued while creating sheets." + Environment.NewLine + "Please read the following error message below";
                        errorMessage.MainContent = ex.Message + Environment.NewLine + "viewId: " + EXviewId + Environment.NewLine + "View Name: " + EXviewToAdd;
                        errorMessage.Show();

                        trans.Dispose();
                    }
                }
            }
        }

        private void AddSheet()
        {
            frmAddSheet new_frmAddSheet = new frmAddSheet();

            if (new_frmAddSheet.ShowDialog() == DialogResult.OK)
            {
                List<string> usedAddSheetNumbers = new List<string>();

                string sheetNumber = string.Empty;
                string sheetName = string.Empty;

                sheetNumber = new_frmAddSheet.txtSheetNumber.Text;
                sheetName = new_frmAddSheet.txtSheetName.Text;

                string newEntry = string.Empty;

                newEntry = sheetNumber + ":" + sheetName;

                foreach (DataGridViewRow row in dgvSheetToCreate.Rows)
                {
                    string sheet = string.Empty;
                    sheet = Convert.ToString(row.Cells["Sheet"].Value);

                    char[] separator = new char[] { ':' };
                    string[] values = sheet.Split(separator, StringSplitOptions.None);

                    usedAddSheetNumbers.Add(values[0]);
                }

                if (usedViewSheetNumbers.Contains(sheetNumber))
                {
                    TaskDialog taskDialog = new TaskDialog("Create Sheets");

                    taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                    taskDialog.MainInstruction = "The sheet number you are trying to create already exists in the project";
                    taskDialog.Show();
                }
                else if (usedAddSheetNumbers.Contains(sheetNumber))
                {
                    TaskDialog taskDialog = new TaskDialog("Create Sheets");

                    taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                    taskDialog.MainInstruction = "The sheet number you are trying to create already exists in the list";
                    taskDialog.Show();
                }
                else
                {
                    this.dgvSheetToCreate.Rows.Add(newEntry, "");
                    usedAddSheetNumbers.Add(sheetNumber);
                }

                if (cbTitleblocks.SelectedIndex != -1 && dgvSheetToCreate.Rows.Count > 0) //CHECKS TO MAKE SURE A TITLEBLOCK IS SELECTED AND THERE IS AT LEAST 1 SHEET TO CREATE
                {
                    btnCreate.Enabled = true;
                }
                else
                {
                    btnCreate.Enabled = false;
                }

            }
        }

        private void EditSheet()
        {
            frmAddSheet new_frmAddSheet = new frmAddSheet();
            new_frmAddSheet.Text = "Edit Sheet";

            if (dgvSheetToCreate.SelectedRows.Count > 0)
            {
                string sheetToEdit = string.Empty;
                int sheetIndex = 0;
                sheetIndex = dgvSheetToCreate.CurrentCell.RowIndex;
                sheetToEdit = Convert.ToString(dgvSheetToCreate.Rows[sheetIndex].Cells["Sheet"].Value);

                string oldSheetNumber = string.Empty;
                string oldSheetName = string.Empty;

                char[] separator = new char[] { ':' };
                string[] values = sheetToEdit.Split(separator, StringSplitOptions.None);

                oldSheetNumber = values[0];
                oldSheetName = values[1];

                new_frmAddSheet.txtSheetNumber.Text = oldSheetNumber;
                new_frmAddSheet.txtSheetName.Text = oldSheetName;

                if (new_frmAddSheet.ShowDialog() == DialogResult.OK)
                {
                    List<string> usedAddSheetNumbers = new List<string>();

                    string newSheetNumber = string.Empty;
                    string newSheetName = string.Empty;

                    newSheetNumber = new_frmAddSheet.txtSheetNumber.Text;
                    newSheetName = new_frmAddSheet.txtSheetName.Text;

                    string newEntry = string.Empty;

                    newEntry = newSheetNumber + ":" + newSheetName;

                    if (newSheetNumber != oldSheetNumber)
                    {
                        foreach (DataGridViewRow row in dgvSheetToCreate.Rows)
                        {
                            string sheet = string.Empty;
                            sheet = Convert.ToString(row.Cells["Sheet"].Value);

                            char[] separator2 = new char[] { ':' };
                            string[] values2 = sheet.Split(separator, StringSplitOptions.None);

                            usedAddSheetNumbers.Add(values2[0]);
                        }

                        if (usedViewSheetNumbers.Contains(newSheetNumber))
                        {
                            TaskDialog taskDialog = new TaskDialog("Create Sheets");

                            taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                            taskDialog.MainInstruction = "The sheet number you are trying to create already exists in the project";
                            taskDialog.Show();
                        }
                        else if (usedAddSheetNumbers.Contains(newSheetNumber))
                        {
                            TaskDialog taskDialog = new TaskDialog("Create Sheets");

                            taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                            taskDialog.MainInstruction = "The sheet number you are trying to create already exists in the list";
                            taskDialog.Show();
                        }
                        else
                        {
                            dgvSheetToCreate.Rows[sheetIndex].Cells["Sheet"].Value = newEntry;
                        }
                    }
                    else
                    {
                        dgvSheetToCreate.Rows[sheetIndex].Cells["Sheet"].Value = newEntry;
                    }
                }
            }
        }

        private void RemoveSheet()
        {
            if (this.dgvSheetToCreate.SelectedRows.Count > 0)
            {

                //GET CURRENTLY SELECTED INDEX OF SELECTED SHEET
                int selectedSheetIndex = 0;
                selectedSheetIndex = dgvSheetToCreate.CurrentCell.RowIndex;

                string viewToRemove = string.Empty;
                viewToRemove = Convert.ToString(dgvSheetToCreate.Rows[selectedSheetIndex].Cells["View"].Value);

                string sheetToRemove = string.Empty;
                sheetToRemove = Convert.ToString(dgvSheetToCreate.Rows[selectedSheetIndex].Cells["Sheet"].Value);

                TaskDialog tdConfirmRemoveSheet = new TaskDialog("Remove Sheet");
                tdConfirmRemoveSheet.MainInstruction = "Are you sure you want to remove this sheet?";
                tdConfirmRemoveSheet.MainContent = sheetToRemove;
                tdConfirmRemoveSheet.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;

                if (tdConfirmRemoveSheet.Show() == TaskDialogResult.Yes)
                {
                    if (viewToRemove != "")
                    {
                        //REMOVE THE ASSIGNED VIEW AND ADD IT BACK TO AVAILABLE VIEWS
                        lstAvailableViews.Items.Add(viewToRemove);
                        dgvSheetToCreate.Rows[selectedSheetIndex].Cells["View"].Value = "";
                    }
                    else
                    {
                        //IF THE VIEW IS A DEFAULT EMPTY VIEW THEN JUST REMOVE IT
                        dgvSheetToCreate.Rows[selectedSheetIndex].Cells["View"].Value = "";
                    }

                    //REMOVE THE SHEET
                    dgvSheetToCreate.Rows.RemoveAt(selectedSheetIndex);
                }

                if (cbTitleblocks.SelectedIndex != -1 && dgvSheetToCreate.Rows.Count > 0) //CHECKS TO MAKE SURE A TITLEBLOCK IS SELECTED AND THERE IS AT LEAST 1 SHEET TO CREATE
                {
                    btnCreate.Enabled = true;
                }
                else
                {
                    btnCreate.Enabled = false;
                }

            }
        }

        private void AddView()
        {
            if (this.lstAvailableViews.SelectedItems.Count > 0)
            {

                string selectedView = string.Empty;
                selectedView = lstAvailableViews.SelectedItem.ToString();

                if (dgvSheetToCreate.SelectedRows.Count > 0)
                {
                    //GET CURRENTLY SELECTED INDEX OF SELECTED SHEET
                    int selectedSheetIndex = 0;
                    selectedSheetIndex = dgvSheetToCreate.CurrentCell.RowIndex;

                    //CHECK IF A SHEET IS SELECTED
                    if (selectedSheetIndex < 0)
                    {
                        TaskDialog.Show("Add View", "Select a sheet that you want to add the view to");
                    }
                    else
                    {
                        string viewToAdd = string.Empty;
                        viewToAdd = Convert.ToString(dgvSheetToCreate.Rows[selectedSheetIndex].Cells["View"].Value);

                        if (viewToAdd != "")
                        {
                            TaskDialog.Show("Add View", "The selected sheet already has a view assigned to it");
                        }
                        else
                        {
                            //INSERT VIEW AT CURRENTLY SELECTED SHEET
                            dgvSheetToCreate.Rows[selectedSheetIndex].Cells["View"].Value = selectedView;
                            lstAvailableViews.Items.Remove(selectedView);
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }

        private void RemoveView()
        {
            if (this.dgvSheetToCreate.SelectedRows.Count > 0)
            {
                //GET CURRENTLY SELECTED INDEX OF SELECTED SHEET
                int selectedSheetIndex = 0;
                selectedSheetIndex = dgvSheetToCreate.CurrentCell.RowIndex;

                string selectedView = string.Empty;
                selectedView = Convert.ToString(dgvSheetToCreate.Rows[selectedSheetIndex].Cells["View"].Value);

                if (selectedView != "")
                {
                    dgvSheetToCreate.Rows[selectedSheetIndex].Cells["View"].Value = "";
                    lstAvailableViews.Items.Add(selectedView);
                }

            }

            else
            {
                return;
            }
        }

        #endregion
            
    }
}
