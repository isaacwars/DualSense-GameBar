@echo off
set CSC="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"

if not exist %CSC% (
    echo No se encontro el compilador de C# de .NET Framework.
    pause
    exit /b
)

echo Compilando DualSense GameBar...
%CSC% /nologo /target:winexe /win32icon:icon.ico /win32manifest:app.manifest /out:DualSenseGameBar.exe Program.cs

if %errorlevel% neq 0 (
    echo.
    echo Ocurrio un error al compilar.
    pause
    exit /b
)

echo.
echo Compilacion exitosa. Se ha generado DualSenseGameBar.exe.
echo Puedes mover este ejecutable a tu carpeta de inicio (shell:startup) para que arranque con Windows.
pause
