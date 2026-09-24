[Setup]
AppName=DualSense GameBar
AppVersion=1.0
AppPublisher=Antigravity
DefaultDirName={autopf}\DualSense GameBar
DefaultGroupName=DualSense GameBar
UninstallDisplayIcon={app}\DualSenseGameBar.exe
Compression=lzma2
SolidCompression=yes
OutputDir=C:\Users\Admin\.gemini\antigravity\scratch\DualSenseGameBar\InstaladorFinal
OutputBaseFilename=Instalar_DualSenseGameBar
ArchitecturesInstallIn64BitMode=x64
SetupIconFile=C:\Users\Admin\.gemini\antigravity\scratch\DualSenseGameBar\icon.ico
DisableProgramGroupPage=yes

[Tasks]
Name: "desktopicon"; Description: "Crear acceso directo en el escritorio"; GroupDescription: "Iconos adicionales:"; Flags: unchecked
Name: "startup"; Description: "Iniciar automáticamente con Windows"; GroupDescription: "Inicio:"

[Files]
Source: "C:\Users\Admin\.gemini\antigravity\scratch\DualSenseGameBar\DualSenseGameBar.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\DualSense GameBar"; Filename: "{app}\DualSenseGameBar.exe"
Name: "{group}\Desinstalar DualSense GameBar"; Filename: "{uninstallexe}"
Name: "{autodesktop}\DualSense GameBar"; Filename: "{app}\DualSenseGameBar.exe"; Tasks: desktopicon
Name: "{userstartup}\DualSense GameBar"; Filename: "{app}\DualSenseGameBar.exe"; Tasks: startup

[Run]
Filename: "{app}\DualSenseGameBar.exe"; Description: "Ejecutar DualSense GameBar ahora"; Flags: nowait postinstall skipifsilent
