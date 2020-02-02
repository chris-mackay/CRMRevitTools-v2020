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
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace CRMTools
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]

    public class AddPanel : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {

            // Create a custom ribbon tab
            string tabName = "CRM Tools";
            application.CreateRibbonTab(tabName);

            string REVIT_VERSION = "v2020";

            string commandsPath = "";
            commandsPath = @"C:\Users\" + Environment.UserName + @"\Documents\CRMRevitTools\" + REVIT_VERSION + @"\Commands\";

            string iconsPath = "";
            iconsPath = @"C:\Users\" + Environment.UserName + @"\Documents\CRMRevitTools\" + REVIT_VERSION + @"\RevitIcons\";

            #region CreateRevitSheets

            // Create a push button
            PushButtonData btnCreateRevitSheets = new PushButtonData("cmdCreateRevitSheets", " Create \nSheets", commandsPath + "CreateRevitSheets.dll", "CreateRevitSheets.Class1");
            btnCreateRevitSheets.ToolTip = "Create multiple Sheets at once. Assign Views to Sheets.";
            btnCreateRevitSheets.LongDescription = "Manually create Sheets or load them from a (.csv) file. " +
                                                   "CSV files can be created with Microsoft Excel. The Sheet Number must be in Column A and the Sheet Name must be in Column B. Each Sheet should have its own Row. " +
                                                   "Sheet 1 in Row 1, Sheet 2 in Row 2, etc.";

            // create bitmap image for button
            Uri uriLargeImage_CreateRevitSheets = new Uri(iconsPath + @"32x32\cmdRevitSheetsImage_32x32.bmp");
            BitmapImage largeImage_CreateRevitSheets = new BitmapImage(uriLargeImage_CreateRevitSheets);

            // create bitmap image for button
            Uri uriSmallImage_CreateRevitSheets = new Uri(iconsPath + @"16x16\cmdRevitSheetsImage_16x16.bmp");
            BitmapImage smallImage_CreateRevitSheets = new BitmapImage(uriSmallImage_CreateRevitSheets);

            btnCreateRevitSheets.LargeImage = largeImage_CreateRevitSheets;
            btnCreateRevitSheets.Image = smallImage_CreateRevitSheets;

            #endregion

            #region SharedParameterCreator

            // Create a push button
            PushButtonData btnSharedParameterCreator = new PushButtonData("cmdSharedParameterCreator", "Shared \nParameter Creator", commandsPath + "SharedParameterCreator.dll", "SharedParameterCreator.Class1");
            btnSharedParameterCreator.ToolTip = "Create a Shared Parameter file from a CSV (Comma delimited) (.csv) file list";
            btnSharedParameterCreator.LongDescription = "Create large numbers of Shared Parameters. To use this program, first a file with the .csv extension must be created, which can be stored anywhere. " +
                                                        "CSV files can be created with Microsoft Excel.\n\n" +
                                                        "Column A: Category (e.g. Mechanical Equipment)\n" +
                                                        "Column B: Shared Parameter Group (User Determined)\n" +
                                                        "Column C: Data Type (e.g. Number, Integer, Text, YesNo)\n" +
                                                        "Column D: Binding Type (e.g. Instance, Type)\n" +
                                                        "Column E: Parameter Name (User Determined)\n\n" +
                                                        "Parameters are grouped under Data in Properties if Insert into Project Parameters is selected";

            // create bitmap image for button
            Uri uriLargeImage_SharedParameterCreator = new Uri(iconsPath + @"32x32\cmdSharedParameterCreator_32x32.bmp");
            BitmapImage largeImage_SharedParameterCreator = new BitmapImage(uriLargeImage_SharedParameterCreator);

            // create bitmap image for button
            Uri uriSmallImage_SharedParameterCreator = new Uri(iconsPath + @"16x16\cmdSharedParameterCreator_16x16.bmp");
            BitmapImage smallImage_SharedParameterCreator = new BitmapImage(uriSmallImage_SharedParameterCreator);

            btnSharedParameterCreator.LargeImage = largeImage_SharedParameterCreator;
            btnSharedParameterCreator.Image = smallImage_SharedParameterCreator;

            #endregion

            #region CreateSheetSet

            // Create a push button
            PushButtonData btnCreateSheetSet = new PushButtonData("cmdCreateSheetSet", "Create \nSheet Set", commandsPath + "CreateSheetSet.dll", "CreateSheetSet.Class1");
            btnCreateSheetSet.ToolTip = "Create a Sheet Set based on revision properties";
            btnCreateSheetSet.LongDescription = "Select between Revision Description, Revision Number, and Revision Date to create a Sheet Set containing all the drawings associated with that property";

            // create bitmap image for button
            Uri uriLargeImage_CreateSheetSet = new Uri(iconsPath + @"32x32\cmdCreateSheetSet_32x32.bmp");
            BitmapImage largeImage_CreateSheetSet = new BitmapImage(uriLargeImage_CreateSheetSet);

            // create bitmap image for button
            Uri uriSmallImage_CreateSheetSet = new Uri(iconsPath + @"16x16\cmdCreateSheetSet_16x16.bmp");
            BitmapImage smallImage_CreateSheetSet = new BitmapImage(uriSmallImage_CreateSheetSet);

            btnCreateSheetSet.LargeImage = largeImage_CreateSheetSet;
            btnCreateSheetSet.Image = smallImage_CreateSheetSet;

            #endregion

            #region SheetRenamer

            PushButtonData btnSheetRenamer = new PushButtonData("cmdSheetRenamer", "Sheet \nRenamer", commandsPath + "SheetRenamer.dll", "SheetRenamer.Class1");
            btnSheetRenamer.ToolTip = "Renames all PDF files in a directory to the following naming convention: Project Number-Sheet Number_Current Revision";
            btnSheetRenamer.LongDescription = "Create a sheet set within Revit and assign the sheets you want to print. Once the sheets are printed, browse to the directory where they are saved. " +
                                              "Select the sheet set that you used to print and click OK.\n\n" +
                                              "NOTE: Ensure that the Project Number is set within Project Information for proper file naming.";

            // create bitmap image for button
            Uri uriLargeImage_SheetRenamer = new Uri(iconsPath + @"32x32\cmdSheetRenamer_32x32.bmp");
            BitmapImage largeImage_SheetRenamer = new BitmapImage(uriLargeImage_SheetRenamer);

            // create bitmap image for button
            Uri uriSmallImage_SheetRenamer = new Uri(iconsPath + @"16x16\cmdSheetRenamer_16x16.bmp");
            BitmapImage smallImage_SheetRenamer = new BitmapImage(uriSmallImage_SheetRenamer);

            btnSheetRenamer.LargeImage = largeImage_SheetRenamer;
            btnSheetRenamer.Image = smallImage_SheetRenamer;

            #endregion

            #region ProjectParameters

            PushButtonData btnProjectParameters = new PushButtonData("cmdProjectParameters", "Insert into \nProject Parameters", commandsPath + "ProjectParameters.dll", "ProjectParameters.Class1");
            btnProjectParameters.ToolTip = "Insert Shared Parameters into Project Parameters";
            btnProjectParameters.LongDescription = "1) Make sure a Shared Parameter file is loaded in Manage->Shared Parameters\n" +
                                                   "2) Click Load to fill the view with the Shared Parameters from the file\n" +
                                                   "3) Right click to set the Binding type (e.g. Instance or Type). This value must be set before selecting a Category.\n" +
                                                   "4) Right click to set the element Category (e.g. Mechanical Equipment, Walls, etc.)\n" +
                                                   "5) Right click to set the Properties Group. This is the category it will be grouped under in the Properties window.\n" +
                                                   "6) Click Insert to insert the Shared Parameters into Project Parameters";

            // create bitmap image for button
            Uri uriLargeImage_ProjectParameters = new Uri(iconsPath + @"32x32\cmdProjectParameters_32x32.bmp");
            BitmapImage largeImage_ProjectParameters = new BitmapImage(uriLargeImage_ProjectParameters);

            // create bitmap image for button
            Uri uriSmallImage_ProjectParameters = new Uri(iconsPath + @"16x16\cmdProjectParameters_16x16.bmp");
            BitmapImage smallImage_ProjectParameters = new BitmapImage(uriSmallImage_ProjectParameters);

            btnProjectParameters.LargeImage = largeImage_ProjectParameters;
            btnProjectParameters.Image = smallImage_ProjectParameters;

            #endregion

            #region RevisionOnSheets

            // Create a push button
            PushButtonData btnRevisionOnSheets = new PushButtonData("cmdRevisionOnSheets", "Revision \nOn Sheets", commandsPath + "RevisionOnSheets.dll", "RevisionOnSheets.Class1");
            btnRevisionOnSheets.ToolTip = "Apply or unapply a revision to multiple sheets at once";
            btnRevisionOnSheets.LongDescription = "Select the revision sequence from the drop-down list that you want to apply or unapply from the list of sheets in the project";

            // create bitmap image for button
            Uri uriLargeImage_RevisionOnSheets = new Uri(iconsPath + @"32x32\cmdRevisionOnSheets_32x32.bmp");
            BitmapImage largeImage_RevisionOnSheets = new BitmapImage(uriLargeImage_RevisionOnSheets);

            // create bitmap image for button
            Uri uriSmallImage_RevisionOnSheets = new Uri(iconsPath + @"16x16\cmdRevisionOnSheets_16x16.bmp");
            BitmapImage smallImage_RevisionOnSheets = new BitmapImage(uriSmallImage_RevisionOnSheets);

            btnRevisionOnSheets.LargeImage = largeImage_RevisionOnSheets;
            btnRevisionOnSheets.Image = smallImage_RevisionOnSheets;

            #endregion

            #region ProductionPanelItems

            // Create a ribbon panel
            RibbonPanel pnlProductionPanel = application.CreateRibbonPanel(tabName, "Production");
            // Add the buttons to the panel
            List<RibbonItem> productionButtons = new List<RibbonItem>();
            // Add the buttons to the panel
            productionButtons.Add(pnlProductionPanel.AddItem(btnCreateRevitSheets));
            productionButtons.Add(pnlProductionPanel.AddItem(btnSharedParameterCreator));
            productionButtons.Add(pnlProductionPanel.AddItem(btnCreateSheetSet));
            productionButtons.Add(pnlProductionPanel.AddItem(btnSheetRenamer));
            productionButtons.Add(pnlProductionPanel.AddItem(btnProjectParameters));
            productionButtons.Add(pnlProductionPanel.AddItem(btnRevisionOnSheets));

            #endregion

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up
            return Result.Succeeded;
        }
    }
}