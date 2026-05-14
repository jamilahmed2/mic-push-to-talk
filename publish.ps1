# Mic Push-To-Talk - PowerShell Build Script
Write-Host "========================================" -ForegroundColor Cyan
Write-Host " Mic Push-To-Talk - Build Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Clean previous builds
Write-Host "[1/4] Cleaning previous builds..." -ForegroundColor Yellow
if (Test-Path ".\publish") { Remove-Item ".\publish" -Recurse -Force }
if (Test-Path ".\MicPushToTalk\bin\Release") { Remove-Item ".\MicPushToTalk\bin\Release" -Recurse -Force }
if (Test-Path ".\MicPushToTalk\obj\Release") { Remove-Item ".\MicPushToTalk\obj\Release" -Recurse -Force }

# Restore packages
Write-Host "[2/4] Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore MicPushToTalk\MicPushToTalk.csproj

# Build Release
Write-Host "[3/4] Building Release configuration..." -ForegroundColor Yellow
dotnet build MicPushToTalk\MicPushToTalk.csproj -c Release

# Publish self-contained
Write-Host "[4/4] Publishing self-contained executable..." -ForegroundColor Yellow
dotnet publish MicPushToTalk\MicPushToTalk.csproj `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:PublishReadyToRun=true `
    -o ".\publish\MicPushToTalk"

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host " Build Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Output location: .\publish\MicPushToTalk\" -ForegroundColor White
Write-Host "Main executable: MicPushToTalk.exe" -ForegroundColor White
Write-Host ""
Write-Host "You can now distribute the contents of the publish folder." -ForegroundColor Cyan
Write-Host "Users do NOT need to install .NET separately." -ForegroundColor Cyan
Write-Host ""
