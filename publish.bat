@echo off
echo ========================================
echo  Mic Push-To-Talk - Build Script
echo ========================================
echo.

REM Clean previous builds
echo [1/4] Cleaning previous builds...
if exist ".\publish" rmdir /s /q ".\publish"
if exist ".\MicPushToTalk\bin\Release" rmdir /s /q ".\MicPushToTalk\bin\Release"
if exist ".\MicPushToTalk\obj\Release" rmdir /s /q ".\MicPushToTalk\obj\Release"

REM Restore packages
echo [2/4] Restoring NuGet packages...
dotnet restore MicPushToTalk\MicPushToTalk.csproj

REM Build Release
echo [3/4] Building Release configuration...
dotnet build MicPushToTalk\MicPushToTalk.csproj -c Release

REM Publish self-contained
echo [4/4] Publishing self-contained executable...
dotnet publish MicPushToTalk\MicPushToTalk.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishReadyToRun=true -o ".\publish\MicPushToTalk"

echo.
echo ========================================
echo  Build Complete!
echo ========================================
echo.
echo Output location: .\publish\MicPushToTalk\
echo Main executable: MicPushToTalk.exe
echo.
echo You can now distribute the contents of the publish folder.
echo Users do NOT need to install .NET separately.
echo.
pause
