# CRMRevitTools-v2020

![Ribbon](CRMRevitTools_Help/ribbon.png?raw=true "Ribbon")

#### This repository is meant to be a template for creating your own Revit tab menu with custom commands.
*Cloning this entire repository is required to build the (.exe) installer in the Inno Setup folder*
### See folder content descriptions below

### CRMRevitTools_Help
* Contains a folder structure for creating `.html` instruction pages that simply open in a browser. These instruction pages are opened using the Help (?) button in the Titlebar of the running command.
  * *css*: Cascading Style Sheet for `.html` files
  *  *images*: Contains all images for `.html` files

### Inno Setup
 * **CRMRevitTools**
   * *Addin File*: The Revit `ADDIN File` that is placed in `C:\ProgramData\Autodesk\Revit\Addins\2020`. This file does not need to be edited to build the installer.
   * *Commands*: Location where the latest `command_name.dll` files need to be placed to build the installer
   * *MenuCreator*: Location where the latest `CRMTools.dll` file needs to be placed to build the installer
   * *RevitIcons*: Location where all the referenced `image_name.bmp` files for `CRMTools.dll` need to be placed to build the installer 
     * There is a directory for both 16x16 and 32x32 size images. Revit requires images to be `Bitmap image` files.
 * `CRMRevitTools-v2020-v1.0.2 Setup.exe`. The file that is produced after compiling `Setup Script-v2020.iss`. The version number is determined by the `MyAppVersion` and `MyVersionInfoVersion` `#define` directives in Inno Setup.
 * `CRMRevitToolsInit-v2020.exe`: A simple console application that runs at the end of the installation to replace `REPLACEUSERNAME` in the `ADDIN File` to `System.Environment.UserName`.
 * `LICENSE.txt`: Inno Setup license file
 * `Setup Script-v2020.iss`: Inno Setup Script file

### RevitAPI-v2020
 * Contains the Revit API references. This is where Visual Studio references the API. `Copy Local` should be set to `False` in Properties.

### Visual Studio
 * This folder contains the project directories created by Visual Studio
 * CRMTools is the menu creator. This is where the menu and buttons are created. Each command is separated with a `Region`.

### LICENSE
 * The GPL-3.0 License that is attached to this repository and all it's contents

### Parameter_Template-v2020.xlsx
 * The template that is used with the **Shared Parameter Creator** command.

# Included Commands
## Create Sheets
![CreateSheets](CRMRevitTools_Help/images/create_revit_sheets/main_form.png?raw=true "CreateSheets")
## Shared Parameter Creator
![SharedParameterCreator](CRMRevitTools_Help/images/shared_parameter_creator/main_form.png?raw=true "SharedParameterCreator")
## Create Sheet Set
![CreateSheetSet](CRMRevitTools_Help/create_sheet_set.png?raw=true "CreateSheetSet")
## Sheet Renamer
![SheetRenamer](CRMRevitTools_Help/images/sheet_renamer/main_form.png?raw=true "SheetRenamer")
## Insert into Project Parameters
![InsertIntoProjectParameters](CRMRevitTools_Help/project_parameters.png?raw=true "InsertIntoProjectParameters")
## Revision On Sheets
![RevisionOnSheets](CRMRevitTools_Help/revision_on_sheets.png?raw=true "RevisionOnSheets")
