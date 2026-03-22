@echo off
echo ==============================================================
echo  SLIC Flood Management - Self-Contained Publisher
echo ==============================================================
echo This script will package the application so it can run on
echo ANY machine WITHOUT needing the .NET SDK installed.
echo.

echo Cleaning old builds...
dotnet clean
echo.

echo Building for Windows (64-bit)...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./Dist/Windows-x64

echo Building for macOS (ARM64 / Apple Silicon)...
dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true -o ./Dist/macOS-AppleSilicon

echo Building for macOS (Intel x64)...
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -o ./Dist/macOS-Intel

echo Building for Linux (64-bit)...
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o ./Dist/Linux-x64

echo.
echo ==============================================================
echo DONE! Check the "Dist" folder.
echo You can now copy those folders to ANY computer and run them 
echo directly without installing .NET!
echo ==============================================================
pause
