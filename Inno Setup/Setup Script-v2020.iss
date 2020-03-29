#define RevitVersion "2020"
#define MyAppName "CRMRevitTools-v2020"
#define MyAppVersion "1.0.3"
#define MyVersionInfoVersion "1.0.3"
#define MyAppPublisher "Christopher Ryan Mackay"

[Setup]
AppId={{8369C0A4-4FB0-4531-992F-109221FE6584}
AppName={#MyAppName}
AppCopyright=Copyright © 2019-2020 Christopher Ryan Mackay
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyVersionInfoVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={userdocs}\{#MyAppName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=.
OutputBaseFilename=CRMRevitTools-v{#RevitVersion}-v{#MyAppVersion} Setup
Compression=lzma
SolidCompression=yes
LicenseFile=LICENSE.txt
PrivilegesRequired=lowest

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "CRMRevitTools\Commands\*"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\Commands"; Flags: ignoreversion
Source: "CRMRevitTools\MenuCreator\*"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\MenuCreator"; Flags: ignoreversion
Source: "CRMRevitTools\RevitIcons\16x16\*"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\RevitIcons\16x16\"; Flags: ignoreversion
Source: "CRMRevitTools\RevitIcons\32x32\*"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\RevitIcons\32x32\"; Flags: ignoreversion
Source: "CRMRevitTools\Addin File\*"; DestDir: "C:\ProgramData\Autodesk\Revit\Addins\{#RevitVersion}"; Flags: ignoreversion
Source: "CRMRevitToolsInit-v{#RevitVersion}.exe"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}"; Flags: ignoreversion
Source: "LICENSE.txt"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}"; Flags: ignoreversion

;CRMRevitTools_Help
Source: "..\CRMRevitTools_Help\*"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\CRMRevitTools_Help"; Flags: ignoreversion
Source: "..\CRMRevitTools_Help\css\*"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\CRMRevitTools_Help\css"; Flags: ignoreversion

;Create Revit Sheets
Source: "..\CRMRevitTools_Help\images\create_revit_sheets\*"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\CRMRevitTools_Help\images\create_revit_sheets"; Flags: ignoreversion

;Shared Parameter Creator
Source: "..\Parameter_Template-v{#RevitVersion}.xlsx"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\"; Flags: ignoreversion
Source: "..\CRMRevitTools_Help\images\shared_parameter_creator\*"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\CRMRevitTools_Help\images\shared_parameter_creator"; Flags: ignoreversion

;Sheet Renamer
Source: "..\CRMRevitTools_Help\images\sheet_renamer\*"; DestDir: "{userdocs}\CRMRevitTools\v{#RevitVersion}\CRMRevitTools_Help\images\sheet_renamer"; Flags: ignoreversion

[UninstallDelete] 
Type: dirifempty; Name: {userdocs}\CRMRevitTools;

[Run]
Filename: {userdocs}\CRMRevitTools\v{#RevitVersion}\CRMRevitToolsInit-v{#RevitVersion}.exe;

