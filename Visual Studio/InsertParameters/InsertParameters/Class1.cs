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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.IO;
using System.Windows.Forms;

namespace InsertParameters
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    public class Class1 : IExternalCommand
    {
        public static ExternalCommandData m_commandData;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            m_commandData = commandData;

            UIApplication uiApp = commandData.Application;

            DefinitionFile sharedParamFile;
            sharedParamFile = OpenSharedParametersFile(commandData.Application.Application);

            if (sharedParamFile == null)
            {
                TaskDialog td = new TaskDialog("Insert Parameters");
                td.MainInstruction = "Shared Parameter file is not specified";
                td.MainContent = "Specify a Shared Parameter file now?\n\nIf not, Shared Parameter files can be specified in Manage->Shared Parameters.";
                td.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;

                if (td.Show() == TaskDialogResult.Yes)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.InitialDirectory = "c:\\";
                    ofd.Filter = "txt files (*.txt)|*.txt";
                    ofd.RestoreDirectory = true;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string file = ofd.FileName;

                        if (SharedParametersFileIsValid(file))
                        {
                            commandData.Application.Application.SharedParametersFilename = file;
                            MainForm myMainForm = new MainForm(uiApp);
                            myMainForm.ShowDialog();
                        }
                        else
                        {
                            TaskDialog td2 = new TaskDialog("Insert Parameters");
                            td2.MainInstruction = "File provided is not a valid Shared Parameters file";
                            td2.Show();
                        }
                    }
                }
            }
            else
            {
                MainForm myMainForm = new MainForm(uiApp);
                myMainForm.ShowDialog();
            }
            
            return Result.Succeeded;
        }

        private bool SharedParametersFileIsValid(string _file)
        {
            FileInfo fInfo = new FileInfo(_file);
            string[] lines = File.ReadAllLines(fInfo.FullName);

            if (lines.Length != 0)
                if (!lines[0].Contains("# This is a Revit shared parameter file.")) return false;

            return true;
        }

        private DefinitionFile OpenSharedParametersFile(Autodesk.Revit.ApplicationServices.Application application)
        {
            Autodesk.Revit.DB.DefinitionFile sharedParametersFile = default(Autodesk.Revit.DB.DefinitionFile);

            sharedParametersFile = application.OpenSharedParameterFile();

            return sharedParametersFile;
        }
    }
}
