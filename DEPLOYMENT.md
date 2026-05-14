# Deployment Guide for Developers

This guide explains how to build and distribute Mic Push-To-Talk for end users.

## Quick Start

```bash
# Build self-contained executable (no .NET required for users)
.\publish.bat

# Output will be in: .\publish\MicPushToTalk\
```

## Deployment Options

### Option 1: Self-Contained Single File (Recommended)

**Pros:**
- Users don't need to install .NET
- Single executable file
- Works on any Windows 10/11 x64 system

**Cons:**
- Larger file size (~150-200 MB)
- Longer first-run startup time

**Build Command:**
```bash
dotnet publish MicPushToTalk\MicPushToTalk.csproj ^
    -c Release ^
    -r win-x64 ^
    --self-contained true ^
    -p:PublishSingleFile=true ^
    -p:IncludeNativeLibrariesForSelfExtract=true ^
    -p:PublishReadyToRun=true ^
    -o ".\publish\MicPushToTalk"
```

### Option 2: Framework-Dependent

**Pros:**
- Smaller file size (~5-10 MB)
- Faster startup

**Cons:**
- Users must install .NET 8 Desktop Runtime

**Build Command:**
```bash
dotnet publish MicPushToTalk\MicPushToTalk.csproj ^
    -c Release ^
    -r win-x64 ^
    --self-contained false ^
    -o ".\publish\MicPushToTalk-FDD"
```

### Option 3: Installer (Professional)

**Requirements:**
- Install [Inno Setup](https://jrsoftware.org/isdl.php)

**Steps:**
1. Build the self-contained version first
2. Open `create-installer.iss` in Inno Setup
3. Click "Build" → "Compile"
4. Installer will be created in `.\installer\` folder

**Features:**
- Professional Windows installer
- Start menu shortcuts
- Desktop icon option
- Auto-start option
- Proper uninstaller
- Registry integration

## Distribution Checklist

### Before Release

- [ ] Update version number in `MicPushToTalk.csproj`
- [ ] Update version in `create-installer.iss`
- [ ] Update `README.md` with latest features
- [ ] Test on clean Windows 10 VM
- [ ] Test on clean Windows 11 VM
- [ ] Test all hotkey combinations
- [ ] Test microphone mute/unmute
- [ ] Test settings persistence
- [ ] Test tray icon functionality
- [ ] Verify no crashes or errors

### Build Release

```bash
# 1. Clean build
.\publish.bat

# 2. Test the executable
.\publish\MicPushToTalk\MicPushToTalk.exe

# 3. Create ZIP for GitHub release
Compress-Archive -Path ".\publish\MicPushToTalk\*" -DestinationPath ".\MicPushToTalk-v1.0.0-win-x64.zip"

# 4. (Optional) Create installer
# Open create-installer.iss in Inno Setup and compile
```

### GitHub Release

1. Create a new release on GitHub
2. Tag version: `v1.0.0`
3. Upload files:
   - `MicPushToTalk-v1.0.0-win-x64.zip` (portable version)
   - `MicPushToTalk-Setup-v1.0.0.exe` (installer)
4. Write release notes
5. Publish release

## File Structure for Distribution

### Portable ZIP
```
MicPushToTalk-v1.0.0-win-x64.zip
├── MicPushToTalk.exe          (main executable)
├── *.dll                       (dependencies)
├── README.md                   (user guide)
└── INSTALLATION.md             (setup instructions)
```

### Installer
```
MicPushToTalk-Setup-v1.0.0.exe (single installer file)
```

## Code Signing (Optional but Recommended)

To avoid "Windows protected your PC" warnings:

1. **Purchase a code signing certificate** (~$100-300/year)
   - DigiCert
   - Sectigo
   - GlobalSign

2. **Sign the executable:**
   ```bash
   signtool sign /f "certificate.pfx" /p "password" /t http://timestamp.digicert.com ".\publish\MicPushToTalk\MicPushToTalk.exe"
   ```

3. **Sign the installer:**
   ```bash
   signtool sign /f "certificate.pfx" /p "password" /t http://timestamp.digicert.com ".\installer\MicPushToTalk-Setup-v1.0.0.exe"
   ```

## Auto-Update System (Future Enhancement)

Consider implementing:
- Check for updates on startup
- Download and install updates automatically
- Use Squirrel.Windows or similar framework

## Platform Support

### Current
- Windows 10 (1809+) x64
- Windows 11 x64

### Future Possibilities
- Windows ARM64 (change RuntimeIdentifier to `win-arm64`)
- Multiple architectures (use `win-x64;win-arm64`)

## Performance Optimization

### Build Optimizations
```xml
<PropertyGroup>
  <PublishTrimmed>true</PublishTrimmed>
  <TrimMode>link</TrimMode>
  <EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>
</PropertyGroup>
```

**Warning:** Test thoroughly after enabling trimming, as it can break reflection-based code.

## Troubleshooting Build Issues

### Issue: "Could not find a part of the path"
**Solution:** Ensure all file paths in the project are correct

### Issue: Large executable size
**Solution:** This is normal for self-contained apps. Consider framework-dependent deployment.

### Issue: Slow first startup
**Solution:** This is normal for single-file apps. Subsequent starts are faster.

### Issue: Antivirus false positives
**Solution:** 
- Code sign the executable
- Submit to antivirus vendors for whitelisting
- Use VirusTotal to verify

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Build and Release

on:
  push:
    tags:
      - 'v*'

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      
      - name: Publish
        run: |
          dotnet publish MicPushToTalk\MicPushToTalk.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o .\publish
      
      - name: Create ZIP
        run: |
          Compress-Archive -Path ".\publish\*" -DestinationPath ".\MicPushToTalk-win-x64.zip"
      
      - name: Create Release
        uses: softprops/action-gh-release@v1
        with:
          files: MicPushToTalk-win-x64.zip
```

## Support and Maintenance

- Monitor GitHub issues
- Respond to user feedback
- Update dependencies regularly
- Test on new Windows versions
- Keep documentation updated

## Legal Considerations

- Include LICENSE file
- Respect NAudio license (MIT)
- Respect CommunityToolkit.Mvvm license (MIT)
- Add copyright notices
- Consider privacy policy if collecting telemetry
