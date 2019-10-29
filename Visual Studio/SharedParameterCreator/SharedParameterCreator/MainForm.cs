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
using System.Diagnostics;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace SharedParameterCreator
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region CLASS_LEVEL_VARIABLES

        UIApplication myRevitUIApp;
        Document myRevitDoc;

        public string fullFile_Excel = string.Empty;
        public string fullFile_Parameters = string.Empty;
        public ExternalCommandData myCommandData = null;
        public bool insertIntoProjectParameters = false;

        public string workingDirectory = string.Empty;
        public string REVIT_VERSION = string.Empty;

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
            myCommandData = Class1.m_commandData;

            REVIT_VERSION = "v2020";

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            string libraryPath = string.Empty;
            libraryPath = workingDirectory; //DEFAULT PATH

            if (Directory.Exists(workingDirectory))
            {
                ofd.InitialDirectory = libraryPath;
            }
            else
            {
                ofd.InitialDirectory = "c:\\";
            }

            ofd.Filter = "CSV (Comma delimited) (.csv) Files (*.csv)|*.csv";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // TODO: ADD FILE FILTER FOR .csv
                string fileName = string.Empty;
                fileName = ofd.FileName;
                txtCSVFile.Text = fileName;

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            TaskDialog taskDialog = new TaskDialog("Shared Parameter Creator");

            taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
            taskDialog.MainInstruction = "Are you sure you want to create the Shared Parameters file?";
            taskDialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;

            fullFile_Excel = txtCSVFile.Text;

            insertIntoProjectParameters = chkInsert.Checked;

            if (fullFile_Excel == string.Empty)
            {
                TaskDialog.Show("No File Provided", "Make sure you have created a file with the .csv extension.");
            }
            else
            {
                if (taskDialog.Show() == TaskDialogResult.Yes)
                {

                    try
                    {
                        Category category = null;
                        CategorySet categorySet = null;
                        InstanceBinding instanceBinding = null;
                        Autodesk.Revit.DB.Binding typeBinding = null;
                        BindingMap bindingMap = null;

                        categorySet = myCommandData.Application.Application.Create.NewCategorySet();
                        instanceBinding = myCommandData.Application.Application.Create.NewInstanceBinding(categorySet);
                        typeBinding = myCommandData.Application.Application.Create.NewInstanceBinding(categorySet);
                        bindingMap = myCommandData.Application.ActiveUIDocument.Document.ParameterBindings;

                        DefinitionFile sharedParametersFile;

                        string fileName = string.Empty;
                        string filePath = string.Empty;

                        filePath = Path.GetDirectoryName(fullFile_Excel);
                        fileName = Path.GetFileNameWithoutExtension(fullFile_Excel);

                        fullFile_Parameters = filePath + "\\" + fileName + ".txt";

                        // THE SHARED PARAMETER FILE
                        sharedParametersFile = OpenSharedParametersFile(myCommandData.Application.Application);

                        DefinitionGroup sharedParameterDefinition = null;
                        Definition definition = null;

                        string strTextLine = string.Empty;

                        StreamReader objReader = new System.IO.StreamReader(fullFile_Excel);
                        System.Collections.Specialized.StringCollection parameterCollection = new System.Collections.Specialized.StringCollection();

                        while (objReader.Peek() != -1)
                        {
                            strTextLine = objReader.ReadLine();

                            parameterCollection.Add(strTextLine);

                        }

                        //REVIT TRANSACTION
                        Transaction trans = new Transaction(myRevitDoc, "Create Shared Parameters");

                        trans.Start();

                        foreach (string param in parameterCollection)
                        {
                            // A, appliesTo, CATEGORY
                            // B, sharedParameterGroup, SHARED PARAMETER GROUP
                            // C, parameterDataType, DATA TYPE
                            // D, bindType, BINDING TYPE
                            // E, PARAMETER NAME

                            string appliesTo = string.Empty;
                            string groupUnder = string.Empty;
                            string sharedParameterGroup = string.Empty;
                            ParameterType parameterDataType;
                            parameterDataType = ParameterType.Text;
                            string parameterDataType_Test = string.Empty;
                            string bindType = string.Empty;
                            string parameterName = string.Empty;

                            char[] chrSeparator = new char[] { ',' };
                            string[] arrValues = param.Split(chrSeparator, StringSplitOptions.None);

                            appliesTo = arrValues[0];
                            sharedParameterGroup = arrValues[1];
                            parameterDataType_Test = arrValues[2];
                            bindType = arrValues[3];
                            parameterName = arrValues[4];

                            switch (parameterDataType_Test)
                            {
                                case "Text":
                                    parameterDataType = ParameterType.Text;
                                    break;
                                case "Integer":
                                    parameterDataType = ParameterType.Integer;
                                    break;
                                case "Number":
                                    parameterDataType = ParameterType.Number;
                                    break;
                                case "Length":
                                    parameterDataType = ParameterType.Length;
                                    break;
                                case "Area":
                                    parameterDataType = ParameterType.Area;
                                    break;
                                case "Volume":
                                    parameterDataType = ParameterType.Volume;
                                    break;
                                case "Angle":
                                    parameterDataType = ParameterType.Angle;
                                    break;
                                case "Slope":
                                    parameterDataType = ParameterType.Slope;
                                    break;
                                case "Currency":
                                    parameterDataType = ParameterType.Currency;
                                    break;
                                case "Mass Density":
                                    parameterDataType = ParameterType.MassDensity;
                                    break;
                                case "URL":
                                    parameterDataType = ParameterType.URL;
                                    break;
                                case "Material":
                                    parameterDataType = ParameterType.Material;
                                    break;
                                case "Image":
                                    parameterDataType = ParameterType.Image;
                                    break;
                                case "Yes/No":
                                    parameterDataType = ParameterType.YesNo;
                                    break;
                                default:
                                    parameterDataType = ParameterType.Text;
                                    break;
                            }

                            sharedParameterDefinition = sharedParametersFile.Groups.get_Item(sharedParameterGroup);

                            if ((sharedParameterDefinition == null))
                            {
                                sharedParameterDefinition = sharedParametersFile.Groups.Create(sharedParameterGroup);
                            }

                            category = myCommandData.Application.ActiveUIDocument.Document.Settings.Categories.get_Item(appliesTo);
                            categorySet = myCommandData.Application.Application.Create.NewCategorySet();
                            categorySet.Insert(category);
                            instanceBinding = myCommandData.Application.Application.Create.NewInstanceBinding(categorySet);
                            typeBinding = myCommandData.Application.Application.Create.NewTypeBinding(categorySet);

                            if ((parameterName != null))
                            {
                                definition = OpenDefinition(sharedParameterDefinition, parameterName, parameterDataType);
                            }

                            if (insertIntoProjectParameters)
                            {
                                if (bindType == "Type")
                                {
                                    bindingMap.Insert(definition, typeBinding, BuiltInParameterGroup.PG_DATA);
                                }
                                else
                                {
                                    bindingMap.Insert(definition, instanceBinding, BuiltInParameterGroup.PG_DATA);
                                }
                            }
                        }

                        //END REVIT TRANSACTION
                        trans.Commit();

                        objReader.Close();

                        TaskDialog.Show("File Created Sucessfully", "The Shared Parameter file was created successfully");

                    }
                    catch (Exception ex)
                    {
                        TaskDialog errorMessage = new TaskDialog("Create Shared Parameter Error");
                        errorMessage.MainInstruction = "An error occurrued while creating the Shared Parameter file." + "\n" + "Please read the following error message below";
                        errorMessage.MainContent = ex.Message + Environment.NewLine + ex.Source;
                        errorMessage.Show();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DefinitionFile OpenSharedParametersFile(Autodesk.Revit.ApplicationServices.Application application)
        {

            StreamWriter stream = default(StreamWriter);

            stream = new StreamWriter(fullFile_Parameters);
            stream.Close();

            application.SharedParametersFilename = fullFile_Parameters;

            Autodesk.Revit.DB.DefinitionFile sharedParametersFile = default(Autodesk.Revit.DB.DefinitionFile);

            sharedParametersFile = application.OpenSharedParameterFile();

            return sharedParametersFile;
        }

        private Autodesk.Revit.DB.Definition OpenDefinition(Autodesk.Revit.DB.DefinitionGroup defGroup, string param, ParameterType paramType)
        {

            ExternalDefinitionCreationOptions defOptions = new ExternalDefinitionCreationOptions(param, (ParameterType)paramType);
            defOptions.Name = param;
            defOptions.Type = paramType;

            Definition def = null;

            if (def == null)
            {

                def = defGroup.Definitions.Create(defOptions);

            }

            return def;
        }

        private void btnOpenTemplate_Click(object sender, EventArgs e)
        {
            string userName = string.Empty;
            userName = Environment.UserName;

            string templateFile = string.Empty;

            templateFile = @"C:\Users\" + userName + @"\Documents\CRMRevitTools\" + REVIT_VERSION + @"\Parameter_Template-" + REVIT_VERSION + ".xlsx";

            if (File.Exists(templateFile))
            {
                try
                {
                    DateTime date = new DateTime();
                    date = DateTime.Now;

                    string timeStamp = date.ToString("yyyyMMddHHmmss");
                    workingDirectory = @"C:\Users\" + userName + @"\Desktop\" + timeStamp + "-" + REVIT_VERSION + "-Shared_Parameters";

                    if (!Directory.Exists(workingDirectory))
                    {
                        Directory.CreateDirectory(workingDirectory);
                    }

                    string workingFile = string.Empty;
                    workingFile = workingDirectory + @"\" + timeStamp + "-" + REVIT_VERSION + "-Shared_Parameters.xlsx";

                    File.Copy(templateFile, workingFile);

                    if (File.Exists(workingFile))
                    {
                        try
                        {
                            Process.Start(workingFile);
                        }
                        catch (Exception ex)
                        {
                            TaskDialog taskDialog = new TaskDialog("Shared Parameter Creator");

                            taskDialog.MainInstruction = "An error occurrued while opening the parameter working file." + "\n" + "Please read the following error message below";
                            taskDialog.MainContent = ex.Message + Environment.NewLine + ex.Source;

                            taskDialog.Show();
                        }
                    }
                    else
                    {
                        TaskDialog taskDialog = new TaskDialog("Shared Parameter Creator");

                        taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                        taskDialog.MainInstruction = "The parameter working file could not be found. It may have been moved or deleted.";

                        taskDialog.Show();
                    }

                }
                catch (Exception ex)
                {
                    TaskDialog taskDialog = new TaskDialog("Shared Parameter Creator");

                    taskDialog.MainInstruction = "An error occurrued while copying the parameter template file." + "\n" + "Please read the following error message below";
                    taskDialog.MainContent = ex.Message + Environment.NewLine + ex.Source;

                    taskDialog.Show();
                }

            }
            else
            {
                TaskDialog taskDialog = new TaskDialog("Shared Parameter Creator");

                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                taskDialog.MainInstruction = "The parameter template file could not be found. It may have been moved or deleted.";

                taskDialog.Show();
            }
        }

        private void MainForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {

            string helpFile = string.Empty;
            helpFile = @"C:\Users\" + Environment.UserName + @"\Documents\CRMRevitTools\" + REVIT_VERSION + @"\CRMRevitTools_Help\shared_parameter_creator.html";

            if (File.Exists(helpFile))
            {
                Process.Start(helpFile);
            }
            else
            {
                TaskDialog taskDialog = new TaskDialog("Shared Parameter Creator");

                taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                taskDialog.MainInstruction = "The Help file for Shared Parameter Creator could not be found. It may have been moved or deleted.";
                taskDialog.Show();
            }

        }
    }
}