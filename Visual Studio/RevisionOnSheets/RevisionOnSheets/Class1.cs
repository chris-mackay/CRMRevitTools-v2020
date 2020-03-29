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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace RevisionOnSheets
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

            MainForm myMainForm = new MainForm(uiApp);
            myMainForm.ShowDialog();

            return Result.Succeeded;
        }
    }
}