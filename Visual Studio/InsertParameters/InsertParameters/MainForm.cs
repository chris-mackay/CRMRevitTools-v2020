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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Text;
using System.Runtime.InteropServices;

namespace InsertParameters
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region CLASS_LEVEL_VARIABLES

        UIApplication myRevitUIApp;
        Document myRevitDoc;
        public ExternalCommandData myCommandData = null;

        public string sharedParamFile;

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
            
            sharedParamFile = OpenSharedParametersFile(myCommandData.Application.Application).Filename.ToString();
            txtSharedParameterFile.Text = sharedParamFile;

            if (myRevitDoc.IsFamilyDocument) dgvSharedParameters.Columns["clmCategory"].Visible = false;
        }

        #region VOIDS

        private void InsertIntoProjectParameters()
        {
            DefinitionFile sharedParametersFile = null;
            sharedParametersFile = OpenSharedParametersFile(myCommandData.Application.Application);
            Dictionary<string, BuiltInParameterGroup> groupDict = new Dictionary<string, BuiltInParameterGroup>();
            groupDict = BrowserGroupDictionary();
            Category revitCategory = null;
            CategorySet categorySet = null;
            InstanceBinding instanceBinding = null;
            Autodesk.Revit.DB.Binding typeBinding = null;

            bool emptyValueFlag = false;

            foreach (DataGridViewRow row in dgvSharedParameters.Rows)
            {
                string category = row.Cells["clmCategory"].Value.ToString();
                string binding = row.Cells["clmBinding"].Value.ToString();
                string browserGroup = row.Cells["clmPropertiesGroup"].Value.ToString();

                if (myRevitDoc.IsFamilyDocument)
                {
                    if (binding == "" || browserGroup == "")
                    {
                        emptyValueFlag = true;
                        break;
                    }
                }
                else
                {
                    if (category == "" || binding == "" || browserGroup == "")
                    {
                        emptyValueFlag = true;
                        break;
                    }
                }
            }

            if (!emptyValueFlag)
            {
                Transaction trans = new Transaction(myRevitDoc, "Insert Parameters");

                trans.Start();

                foreach (DataGridViewRow row in dgvSharedParameters.Rows)
                {
                    string name = row.Cells["clmParamName"].Value.ToString();
                    string group = row.Cells["clmGroup"].Value.ToString();
                    string category = row.Cells["clmCategory"].Value.ToString();
                    string binding = row.Cells["clmBinding"].Value.ToString();
                    string browserGroup = row.Cells["clmPropertiesGroup"].Value.ToString();

                    if (!myRevitDoc.IsFamilyDocument)
                    {
                        revitCategory = myCommandData.Application.ActiveUIDocument.Document.Settings.Categories.get_Item(category);
                        categorySet = myCommandData.Application.Application.Create.NewCategorySet();
                        categorySet.Insert(revitCategory);
                        instanceBinding = myCommandData.Application.Application.Create.NewInstanceBinding(categorySet);
                        typeBinding = myCommandData.Application.Application.Create.NewTypeBinding(categorySet);
                    }

                    ExternalDefinition extDef = GetSharedParameter(sharedParametersFile, group, name);

                    BuiltInParameterGroup paramGroup;

                    paramGroup = groupDict[browserGroup];

                    if (myRevitDoc.IsFamilyDocument)
                    {
                        FamilyManager fm = myRevitDoc.FamilyManager;
                        
                        bool IsInstanceBinding = true;
                        if (binding == "Type") IsInstanceBinding = false; else IsInstanceBinding = true;

                        fm.AddParameter(extDef, paramGroup, IsInstanceBinding);
                    }
                    else
                    {
                        BindingMap bindingMap = null;
                        bindingMap = myCommandData.Application.ActiveUIDocument.Document.ParameterBindings;

                        if (binding == "Type")
                        {
                            bindingMap.Insert(extDef, typeBinding, paramGroup);
                        }
                        else
                        {
                            bindingMap.Insert(extDef, instanceBinding, paramGroup);
                        }
                    }
                }

                trans.Commit();

                TaskDialog.Show("Insert Parameters", "The Shared Parameters were inserted successfully");
                this.Close();
            }
            else
            {
                TaskDialog td = new TaskDialog("Insert Parameters");
                td.MainInstruction = "Invalid value(s)";
                td.MainContent = "Specify a value for Binding, Category, and Properties Group for each parameter before inserting parameters";
                td.Show();
            }
        }

        #endregion

        #region FUNCTIONS
        
        private ContextMenu TableContextMenu()
        {
            ContextMenu mnu = new ContextMenu();
            MenuItem cxmnuEditBinding = new MenuItem("Edit Binding");
            MenuItem cxmnuEditCategory = new MenuItem("Edit Category");
            MenuItem cxmnuEditPropertiesGroup = new MenuItem("Edit Properties Group");
            MenuItem cxmnuExclude = new MenuItem("Exclude Parameter");
            
            cxmnuEditBinding.Click += new EventHandler(cxmnuEditBinding_Click);
            cxmnuEditCategory.Click += new EventHandler(cxmnuEditCategory_Click);
            cxmnuEditPropertiesGroup.Click += new EventHandler(cxmnuEditPropertiesGroup_Click);
            cxmnuExclude.Click += new EventHandler(cxmnuExclude_Click);

            mnu.MenuItems.Add(cxmnuEditBinding);
            mnu.MenuItems.Add(cxmnuEditCategory);
            mnu.MenuItems.Add(cxmnuEditPropertiesGroup);
            mnu.MenuItems.Add("-");
            mnu.MenuItems.Add(cxmnuExclude);

            return mnu;
        }
        
        private List<string> FamilyParameters()
        {
            FamilyManager fm = myRevitDoc.FamilyManager;
            List<string> familyParameters = new List<string>();

            IList<FamilyParameter> fp;
            fp = fm.GetParameters();

            foreach (FamilyParameter familyParameter in fp)
            {
                familyParameters.Add(familyParameter.Definition.Name);
            }

            return familyParameters;
        }

        private List<string> ProjectParameterList()
        {
            List<string> projectParameters = new List<string>();

            BindingMap bindingMap;
            bindingMap = myRevitDoc.ParameterBindings;

            if (!bindingMap.IsEmpty)
            {
                DefinitionBindingMapIterator iterator;
                iterator = bindingMap.ForwardIterator();
                iterator.Reset();

                while (iterator.MoveNext())
                {
                    string paramName = "";
                    paramName = iterator.Key.Name;

                    projectParameters.Add(paramName);
                }

            }

            return projectParameters;
        }

        private List<string> TypeCategories()
        {
            List<string> list = new List<string>();

            list.Add("Air Terminals");
            list.Add("Analytical Links");
            list.Add("Analytical Pipe Connections");
            list.Add("Assemblies");
            list.Add("Cable Tray Fittings");
            list.Add("Cable Tray Runs");
            list.Add("Cable Trays");
            list.Add("Casework");
            list.Add("Ceilings");
            list.Add("Columns");
            list.Add("Communication Devices");
            list.Add("Conduit Fittings");
            list.Add("Conduit Runs");
            list.Add("Conduits");
            list.Add("Curtain Panels");
            list.Add("Curtain Systems");
            list.Add("Curtain Wall Mullions");
            list.Add("Data Devices");
            list.Add("Detail Items");
            list.Add("Doors");
            list.Add("Duct Accessories");
            list.Add("Duct Fittings");
            list.Add("Duct Insulations");
            list.Add("Duct Linings");
            list.Add("Duct Placeholders");
            list.Add("Duct Systems");
            list.Add("Ducts");
            list.Add("Electrical Equipment");
            list.Add("Electrical Fixtures");
            list.Add("Entourage");
            list.Add("Fire Alarm Devices");
            list.Add("Flex Ducts");
            list.Add("Flex Pipes");
            list.Add("Floors");
            list.Add("Furniture");
            list.Add("Furniture Systems");
            list.Add("Generic Models");
            list.Add("Grids");
            list.Add("Levels");
            list.Add("Lighting Devices");
            list.Add("Lighting Fixtures");
            list.Add("MEP Fabrication Containment");
            list.Add("MEP Fabrication Ductwork");
            list.Add("MEP Fabrication Hangers");
            list.Add("MEP Fabrication Pipework");
            list.Add("Mass");
            list.Add("Mechanical Equipment");
            list.Add("Model Groups");
            list.Add("Nurse Call Devices");
            list.Add("Parking");
            list.Add("Pipe Accessories");
            list.Add("Pipe Fittings");
            list.Add("Pipe Insulations");
            list.Add("Pipe Placeholders");
            list.Add("Pipes");
            list.Add("Piping Systems");
            list.Add("Planting");
            list.Add("Plumbing Fixtures");
            list.Add("RVT Links");
            list.Add("Railings");
            list.Add("Ramps");
            list.Add("Rebar Shape");
            list.Add("Roofs");
            list.Add("Security Devices");
            list.Add("Site");
            list.Add("Specialty Equipment");
            list.Add("Sprinklers");
            list.Add("Stairs");
            list.Add("Structural Area Reinforcement");
            list.Add("Structural Beam Systems");
            list.Add("Structural Columns");
            list.Add("Structural Connections");
            list.Add("Structural Fabric Areas");
            list.Add("Structural Fabric Reinforcement");
            list.Add("Structural Foundations");
            list.Add("Structural Framing");
            list.Add("Structural Path Reinforcement");
            list.Add("Structural Rebar");
            list.Add("Structural Rebar Couplers");
            list.Add("Structural Stiffeners");
            list.Add("Telephone Devices");
            list.Add("Topography");
            list.Add("Walls");
            list.Add("Windows");
            list.Add("Wires");

            return list;
        }

        private List<string> InstanceCategories()
        {
            List<string> list = new List<string>();

            list.Add("Air Terminals");
            list.Add("Analytical Beams");
            list.Add("Analytical Braces");
            list.Add("Analytical Columns");
            list.Add("Analytical Floors");
            list.Add("Analytical Foundation Slabs");
            list.Add("Analytical Isolated Foundations");
            list.Add("Analytical Links");
            list.Add("Analytical Nodes");
            list.Add("Analytical Pipe Connections");
            list.Add("Analytical Spaces");
            list.Add("Analytical Surfaces");
            list.Add("Analytical Wall Foundations");
            list.Add("Analytical Walls");
            list.Add("Areas");
            list.Add("Assemblies");
            list.Add("Cable Tray Fittings");
            list.Add("Cable Tray Runs");
            list.Add("Cable Trays");
            list.Add("Casework");
            list.Add("Ceilings");
            list.Add("Columns");
            list.Add("Communication Devices");
            list.Add("Conduit Fittings");
            list.Add("Conduit Runs");
            list.Add("Conduits");
            list.Add("Curtain Panels");
            list.Add("Curtain Systems");
            list.Add("Curtain Wall Mullions");
            list.Add("Data Devices");
            list.Add("Detail Items");
            list.Add("Doors");
            list.Add("Duct Accessories");
            list.Add("Duct Fittings");
            list.Add("Duct Insulations");
            list.Add("Duct Linings");
            list.Add("Duct Placeholders");
            list.Add("Duct Systems");
            list.Add("Ducts");
            list.Add("Electrical Circuits");
            list.Add("Electrical Equipment");
            list.Add("Electrical Fixtures");
            list.Add("Entourage");
            list.Add("Fire Alarm Devices");
            list.Add("Flex Ducts");
            list.Add("Flex Pipes");
            list.Add("Floors");
            list.Add("Furniture");
            list.Add("Furniture Systems");
            list.Add("Generic Models");
            list.Add("Grids");
            list.Add("HVAC Zones");
            list.Add("Levels");
            list.Add("Lighting Devices");
            list.Add("Lighting Fixtures");
            list.Add("MEP Fabrication Containment");
            list.Add("MEP Fabrication Ductwork");
            list.Add("MEP Fabrication Hangers");
            list.Add("MEP Fabrication Pipework");
            list.Add("Mass");
            list.Add("Materials");
            list.Add("Mechanical Equipment");
            list.Add("Model Groups");
            list.Add("Nurse Call Devices");
            list.Add("Parking");
            list.Add("Parts");
            list.Add("Pipe Accessories");
            list.Add("Pipe Fittings");
            list.Add("Pipe Insulations");
            list.Add("Pipe Placeholders");
            list.Add("Pipes");
            list.Add("Piping Systems");
            list.Add("Planting");
            list.Add("Plumbing Fixtures");
            list.Add("Project Information");
            list.Add("RVT Links");
            list.Add("Railings");
            list.Add("Ramps");
            list.Add("Rebar Shape");
            list.Add("Roads");
            list.Add("Roofs");
            list.Add("Rooms");
            list.Add("Schedules");
            list.Add("Security Devices");
            list.Add("Shaft Openings");
            list.Add("Sheets");
            list.Add("Site");
            list.Add("Spaces");
            list.Add("Specialty Equipment");
            list.Add("Sprinklers");
            list.Add("Stairs");
            list.Add("Structural Area Reinforcement");
            list.Add("Structural Beam Systems");
            list.Add("Structural Columns");
            list.Add("Structural Connections");
            list.Add("Structural Fabric Areas");
            list.Add("Structural Fabric Reinforcement");
            list.Add("Structural Foundations");
            list.Add("Structural Framing");
            list.Add("Structural Path Reinforcement");
            list.Add("Structural Rebar");
            list.Add("Structural Rebar Couplers");
            list.Add("Structural Stiffeners");
            list.Add("Switch System");
            list.Add("Telephone Devices");
            list.Add("Topography");
            list.Add("Views");
            list.Add("Walls");
            list.Add("Windows");
            list.Add("Wires");

            return list;
        }

        private List<string> SharedParameterList()
        {
            List<string> paramList = new List<string>();
            Dictionary<string, string> groupDict = new Dictionary<string, string>();

            groupDict = ParameterGroupDictionary();

            StreamReader objReader = new System.IO.StreamReader(sharedParamFile);

            string line = "";

            while ((line = objReader.ReadLine()) != null)
            {
                char[] chrSeparator = new char[] { '\t' };
                string[] arrValues = line.Split(chrSeparator, StringSplitOptions.None);

                if (arrValues[0] == "PARAM")
                {
                    char[] chrSep = new char[] { '\t' };
                    string[] arr = line.Split(chrSeparator, StringSplitOptions.None);
                    
                    string guid = arr[1];
                    string paramName = arr[2];
                    string paramType = arr[3];
                    string groupNum = arr[5];
                    string groupName = groupDict[groupNum];

                    string entry = guid + "\t" +
                                   paramName + "\t" +
                                   paramType + "\t" +
                                   groupName + "\t";

                    paramList.Add(entry);
                    
                }
            }

            objReader.Close();

            return paramList;
        }

        private Dictionary<string, BuiltInParameterGroup> BrowserGroupDictionary()
        {
            Dictionary<string, BuiltInParameterGroup> groupDict = new Dictionary<string, BuiltInParameterGroup>();

            groupDict.Add("Analysis Results", BuiltInParameterGroup.PG_ANALYSIS_RESULTS);
            groupDict.Add("Analytical Alignment", BuiltInParameterGroup.PG_ANALYTICAL_ALIGNMENT);
            groupDict.Add("Analytical Model", BuiltInParameterGroup.PG_ANALYTICAL_MODEL);
            groupDict.Add("Constraints", BuiltInParameterGroup.PG_CONSTRAINTS);
            groupDict.Add("Construction", BuiltInParameterGroup.PG_CONSTRUCTION);
            groupDict.Add("Data", BuiltInParameterGroup.PG_DATA);
            groupDict.Add("Dimensions", BuiltInParameterGroup.PG_GEOMETRY);
            groupDict.Add("Division Geometry", BuiltInParameterGroup.PG_DIVISION_GEOMETRY);
            groupDict.Add("Electrical", BuiltInParameterGroup.PG_AELECTRICAL);
            groupDict.Add("Electrical - Circuiting", BuiltInParameterGroup.PG_ELECTRICAL_CIRCUITING);
            groupDict.Add("Electrical - Lighting", BuiltInParameterGroup.PG_ELECTRICAL_LIGHTING);
            groupDict.Add("Electrical - Loads", BuiltInParameterGroup.PG_ELECTRICAL_LOADS);
            groupDict.Add("Electrical Engineering", BuiltInParameterGroup.PG_ELECTRICAL);
            groupDict.Add("Energy Analysis", BuiltInParameterGroup.PG_ENERGY_ANALYSIS);
            groupDict.Add("Fire Protection", BuiltInParameterGroup.PG_FIRE_PROTECTION);
            groupDict.Add("Forces", BuiltInParameterGroup.PG_FORCES);
            groupDict.Add("General", BuiltInParameterGroup.PG_GENERAL);
            groupDict.Add("Graphics", BuiltInParameterGroup.PG_GRAPHICS);
            groupDict.Add("Green Building Properties", BuiltInParameterGroup.PG_GREEN_BUILDING);
            groupDict.Add("Identity Data", BuiltInParameterGroup.PG_IDENTITY_DATA);
            groupDict.Add("IFC Parameters", BuiltInParameterGroup.PG_IFC);
            groupDict.Add("Layers", BuiltInParameterGroup.PG_REBAR_SYSTEM_LAYERS);
            groupDict.Add("Materials and Finishes", BuiltInParameterGroup.PG_MATERIALS);
            groupDict.Add("Mechanical", BuiltInParameterGroup.PG_MECHANICAL);
            groupDict.Add("Mechanical - Flow", BuiltInParameterGroup.PG_MECHANICAL_AIRFLOW);
            groupDict.Add("Mechanical - Loads", BuiltInParameterGroup.PG_MECHANICAL_LOADS);
            groupDict.Add("Model Properties", BuiltInParameterGroup.PG_ADSK_MODEL_PROPERTIES);
            groupDict.Add("Moments", BuiltInParameterGroup.PG_MOMENTS);
            groupDict.Add("Other", BuiltInParameterGroup.INVALID);
            groupDict.Add("Overall Legend", BuiltInParameterGroup.PG_OVERALL_LEGEND);
            groupDict.Add("Phasing", BuiltInParameterGroup.PG_PHASING);
            groupDict.Add("Photometrics", BuiltInParameterGroup.PG_LIGHT_PHOTOMETRICS);
            groupDict.Add("Plumbing", BuiltInParameterGroup.PG_PLUMBING);
            groupDict.Add("Primary End", BuiltInParameterGroup.PG_PRIMARY_END);
            groupDict.Add("Rebar Set", BuiltInParameterGroup.PG_REBAR_ARRAY);
            groupDict.Add("Releases / Member Forces", BuiltInParameterGroup.PG_RELEASES_MEMBER_FORCES);
            groupDict.Add("Secondary End", BuiltInParameterGroup.PG_SECONDARY_END);
            groupDict.Add("Segments and Fittings", BuiltInParameterGroup.PG_SECONDARY_END);
            groupDict.Add("Set", BuiltInParameterGroup.PG_SEGMENTS_FITTINGS);
            groupDict.Add("Slab Shape Edit", BuiltInParameterGroup.PG_SLAB_SHAPE_EDIT);
            groupDict.Add("Structural", BuiltInParameterGroup.PG_STRUCTURAL);
            groupDict.Add("Structural Analysis", BuiltInParameterGroup.PG_STRUCTURAL_ANALYSIS);
            groupDict.Add("Text", BuiltInParameterGroup.PG_TEXT);
            groupDict.Add("Title Text", BuiltInParameterGroup.PG_TITLE);
            groupDict.Add("Visibility", BuiltInParameterGroup.PG_VISIBILITY);

            return groupDict;
        }

        private Dictionary<string, string> ParameterGroupDictionary()
        {
            Dictionary<string, string> groupDict = new Dictionary<string, string>();

            StreamReader objReader = new System.IO.StreamReader(sharedParamFile);

            string line = "";

            while ((line = objReader.ReadLine()) != null)
            {
                char[] chrSeparator = new char[] { '\t' };
                string[] arrValues = line.Split(chrSeparator, StringSplitOptions.None);

                if (arrValues[0] == "GROUP")
                {
                    char[] chrSep = new char[] { '\t' };
                    string[] arr = line.Split(chrSeparator, StringSplitOptions.None);

                    string key = "";
                    key = arr[1];

                    string value = "";
                    value = arr[2];

                    groupDict.Add(key, value);

                }
            }

            objReader.Close();

            return groupDict;
        }

        private ExternalDefinition GetSharedParameter(DefinitionFile file, string group, string name)
        {
            DefinitionGroups myGroups = file.Groups;
            DefinitionGroup myGroup = myGroups.get_Item(group);

            ExternalDefinition extDef = null;

            if (myGroup != null)
                extDef = myGroup.Definitions.get_Item(name) as ExternalDefinition;
            
            return extDef;
        }

        private DefinitionFile OpenSharedParametersFile(Autodesk.Revit.ApplicationServices.Application application)
        {
            Autodesk.Revit.DB.DefinitionFile sharedParametersFile = default(Autodesk.Revit.DB.DefinitionFile);

            sharedParametersFile = application.OpenSharedParameterFile();

            return sharedParametersFile;
        }

        #endregion

        #region EVENTS

        private void cxmnuEditPropertiesGroup_Click(object sender, EventArgs e)
        {
            frmSelectionBox new_frmSelectionBox = new frmSelectionBox();
            new_frmSelectionBox.cbItems.Items.Clear();

            new_frmSelectionBox.cbItems.Items.Add("Analysis Results");
            new_frmSelectionBox.cbItems.Items.Add("Analytical Alignment");
            new_frmSelectionBox.cbItems.Items.Add("Analytical Model");
            new_frmSelectionBox.cbItems.Items.Add("Constraints");
            new_frmSelectionBox.cbItems.Items.Add("Construction");
            new_frmSelectionBox.cbItems.Items.Add("Data");
            new_frmSelectionBox.cbItems.Items.Add("Dimensions");
            new_frmSelectionBox.cbItems.Items.Add("Division Geometry");
            new_frmSelectionBox.cbItems.Items.Add("Electrical");
            new_frmSelectionBox.cbItems.Items.Add("Electrical - Circuiting");
            new_frmSelectionBox.cbItems.Items.Add("Electrical - Lighting");
            new_frmSelectionBox.cbItems.Items.Add("Electrical - Loads");
            new_frmSelectionBox.cbItems.Items.Add("Electrical Engineering");
            new_frmSelectionBox.cbItems.Items.Add("Energy Analysis");
            new_frmSelectionBox.cbItems.Items.Add("Fire Protection");
            new_frmSelectionBox.cbItems.Items.Add("Forces");
            new_frmSelectionBox.cbItems.Items.Add("General");
            new_frmSelectionBox.cbItems.Items.Add("Graphics");
            new_frmSelectionBox.cbItems.Items.Add("Green Building Properties");
            new_frmSelectionBox.cbItems.Items.Add("Identity Data");
            new_frmSelectionBox.cbItems.Items.Add("IFC Parameters");
            new_frmSelectionBox.cbItems.Items.Add("Layers");
            new_frmSelectionBox.cbItems.Items.Add("Materials and Finishes");
            new_frmSelectionBox.cbItems.Items.Add("Mechanical");
            new_frmSelectionBox.cbItems.Items.Add("Mechanical - Flow");
            new_frmSelectionBox.cbItems.Items.Add("Mechanical - Loads");
            new_frmSelectionBox.cbItems.Items.Add("Model Properties");
            new_frmSelectionBox.cbItems.Items.Add("Moments");
            new_frmSelectionBox.cbItems.Items.Add("Other");
            new_frmSelectionBox.cbItems.Items.Add("Overall Legend");
            new_frmSelectionBox.cbItems.Items.Add("Phasing");
            new_frmSelectionBox.cbItems.Items.Add("Photometrics");
            new_frmSelectionBox.cbItems.Items.Add("Plumbing");
            new_frmSelectionBox.cbItems.Items.Add("Primary End");
            new_frmSelectionBox.cbItems.Items.Add("Rebar Set");
            new_frmSelectionBox.cbItems.Items.Add("Releases / Member Forces");
            new_frmSelectionBox.cbItems.Items.Add("Secondary End");
            new_frmSelectionBox.cbItems.Items.Add("Segments and Fittings");
            new_frmSelectionBox.cbItems.Items.Add("Set");
            new_frmSelectionBox.cbItems.Items.Add("Slab Shape Edit");
            new_frmSelectionBox.cbItems.Items.Add("Structural");
            new_frmSelectionBox.cbItems.Items.Add("Structural Analysis");
            new_frmSelectionBox.cbItems.Items.Add("Text");
            new_frmSelectionBox.cbItems.Items.Add("Title Text");
            new_frmSelectionBox.cbItems.Items.Add("Visibility");

            new_frmSelectionBox.Text = "Edit Properties Group";
            new_frmSelectionBox.lblInstructions.Text = "Select the properties group from the \ndrop-down list below";

            if (new_frmSelectionBox.ShowDialog() == DialogResult.OK)
            {
                string propertiesGroup = new_frmSelectionBox.cbItems.SelectedItem.ToString();

                foreach (DataGridViewRow row in dgvSharedParameters.SelectedRows)
                    row.Cells["clmPropertiesGroup"].Value = propertiesGroup;
            }
        }

        private void cxmnuExclude_Click(object sender, EventArgs e)
        {
            TaskDialog td = new TaskDialog("Exclude Parameter");
            td.MainInstruction = "Are you sure you want to exclude the selected parameters from insertion?";
            td.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;

            if (td.Show() == TaskDialogResult.Yes)
            {
                DrawingControl.SetDoubleBuffered(dgvSharedParameters);
                DrawingControl.SuspendDrawing(dgvSharedParameters);

                foreach (DataGridViewRow row in dgvSharedParameters.SelectedRows)
                    dgvSharedParameters.Rows.Remove(row);

                DrawingControl.ResumeDrawing(dgvSharedParameters);
            }
        }

        private void cxmnuEditBinding_Click(object sender, EventArgs e)
        {
            List<string> instanceCategories = new List<string>();
            instanceCategories = InstanceCategories();

            List<string> typeCategories = new List<string>();
            typeCategories = TypeCategories();

            frmSelectionBox new_frmSelectionBox = new frmSelectionBox();

            new_frmSelectionBox.cbItems.Items.Clear();
            new_frmSelectionBox.cbItems.Items.Add("Instance");
            new_frmSelectionBox.cbItems.Items.Add("Type");

            new_frmSelectionBox.Text = "Edit Binding";
            new_frmSelectionBox.lblInstructions.Text = "Select the binding type from the \ndrop-down list below";

            List<string> invalidPairs = new List<string>();

            if (new_frmSelectionBox.ShowDialog() == DialogResult.OK)
            {
                string binding = new_frmSelectionBox.cbItems.SelectedItem.ToString();
                
                foreach (DataGridViewRow row in dgvSharedParameters.SelectedRows)
                {
                    string category = row.Cells["clmCategory"].Value.ToString();

                    if (binding == "Type" && !typeCategories.Contains(category) && category != "")
                    {
                        invalidPairs.Add(binding + "," + category);
                    }

                    if (binding == "Instance" && !instanceCategories.Contains(category) && category != "")
                    {
                        invalidPairs.Add(binding + "," + category);
                    }
                }

                if (invalidPairs.Count == 0)
                {
                    foreach (DataGridViewRow row in dgvSharedParameters.SelectedRows)
                        row.Cells["clmBinding"].Value = binding;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (string item in invalidPairs)
                    {
                        sb.Append(item + "\n");
                    }

                    TaskDialog td = new TaskDialog("Edit Binding");
                    td.MainInstruction = "Binding type and Category are an invalid pair";
                    td.MainContent = "Invalid pair(s) are listed below\n\n" + sb.ToString();
                    td.Show();
                }
            }
        }

        private void cxmnuEditCategory_Click(object sender, EventArgs e)
        {
            frmSelectionBox new_frmSelectionBox = new frmSelectionBox();
            new_frmSelectionBox.cbItems.Items.Clear();

            List<string> bindingList = new List<string>();
            string binding = "";

            foreach (DataGridViewRow row in dgvSharedParameters.SelectedRows)
            {
                binding = row.Cells["clmBinding"].Value.ToString();
                bindingList.Add(binding);
            }

            if (bindingList.All(o => o == bindingList[0]) && bindingList[0] == "Type")
            {
                List<string> list = new List<string>();
                list = TypeCategories();

                foreach (string item in list)
                    new_frmSelectionBox.cbItems.Items.Add(item);
            }
            else if (bindingList.All(o => o == bindingList[0]) && bindingList[0] == "Instance")
            {
                List<string> list = new List<string>();
                list = InstanceCategories();

                foreach (string item in list)
                    new_frmSelectionBox.cbItems.Items.Add(item);
            }
            else if (bindingList.Contains(""))
            {
                TaskDialog td = new TaskDialog("Edit Category");
                td.MainInstruction = "Binding type not specified";
                td.MainContent = "Binding type must be specified before setting a Category";
                td.Show();

                return;
            }
            else
            {
                TaskDialog td = new TaskDialog("Edit Category");
                td.MainInstruction = "Multiple binding types specified";
                td.MainContent = "Mulitple parameters can only be edited at once if all the binding types are the same";
                td.Show();

                return;
            }

            new_frmSelectionBox.Text = "Edit Category";
            new_frmSelectionBox.lblInstructions.Text = "Select a category from the \ndrop-down list below";

            if (new_frmSelectionBox.ShowDialog() == DialogResult.OK)
            {
                string category = new_frmSelectionBox.cbItems.SelectedItem.ToString();

                foreach (DataGridViewRow row in dgvSharedParameters.SelectedRows)
                    row.Cells["clmCategory"].Value = category;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertIntoProjectParameters();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dgvSharedParameters.Rows.Clear();

            List<string> sharedParamList = SharedParameterList();

            DrawingControl.SetDoubleBuffered(dgvSharedParameters);
            DrawingControl.SuspendDrawing(dgvSharedParameters);
            
            if (myRevitDoc.IsFamilyDocument)
            {
                List<string> familyParamList = FamilyParameters();
                
                foreach (string item in sharedParamList)
                {
                    char[] chrSeparator = new char[] { '\t' };
                    string[] arrValues = item.Split(chrSeparator, StringSplitOptions.None);

                    string paramName = arrValues[1];
                    string paramType = arrValues[2];
                    string paramGroup = arrValues[3];

                    if (!familyParamList.Contains(paramName))
                        dgvSharedParameters.Rows.Add(paramName, paramGroup, paramType, "", "");
                }
            }
            else
            {
                List<string> projectParamList = ProjectParameterList();

                foreach (string item in sharedParamList)
                {
                    char[] chrSeparator = new char[] { '\t' };
                    string[] arrValues = item.Split(chrSeparator, StringSplitOptions.None);

                    string paramName = arrValues[1];
                    string paramType = arrValues[2];
                    string paramGroup = arrValues[3];

                    if (!projectParamList.Contains(paramName))
                        dgvSharedParameters.Rows.Add(paramName, paramGroup, paramType, "", "", "");
                }
            }

            DrawingControl.ResumeDrawing(dgvSharedParameters);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvSharedParameters_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu = TableContextMenu();
                if (myRevitDoc.IsFamilyDocument) contextMenu.MenuItems.RemoveAt(1);

                contextMenu.Show(dgvSharedParameters, new System.Drawing.Point(e.X, e.Y));
            }
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
