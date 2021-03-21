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
using Autodesk.Revit.UI;
using System.Diagnostics;

namespace About
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private string version = "v1.0.7";

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "Version " + version;
            lblCurrentChanglog.Text = "See what changed in " + version;
            lblChangelog.Text = "Check for a newer version";
            lblSource.Text = "Source code";
        }

        private void lblCurrentChangelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string site = "https://github.com/chris-mackay/CRMRevitTools-v2020/releases/tag/" + version;
                Process.Start(site);
            }
            catch (Exception ex)
            {
                TaskDialog td = new TaskDialog("About");
                td.MainInstruction = "Could not open the webpage";
                td.MainContent = ex.Message;
                td.MainIcon = TaskDialogIcon.TaskDialogIconError;
            }
        }

        private void lblChangelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string site = "https://github.com/chris-mackay/CRMRevitTools-v2020/releases";
                Process.Start(site);
            }
            catch (Exception ex)
            {
                TaskDialog td = new TaskDialog("About");
                td.MainInstruction = "Could not open the webpage";
                td.MainContent = ex.Message;
                td.MainIcon = TaskDialogIcon.TaskDialogIconError;
            }
        }

        private void lblSource_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string site = "https://github.com/chris-mackay/CRMRevitTools-v2020";
                Process.Start(site);
            }
            catch (Exception ex)
            {
                TaskDialog td = new TaskDialog("About");
                td.MainInstruction = "Could not open the webpage";
                td.MainContent = ex.Message;
                td.MainIcon = TaskDialogIcon.TaskDialogIconError;
            }
        }
    }
}
