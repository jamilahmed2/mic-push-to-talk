# Installation Guide - Mic Push-To-Talk

## For End Users (Simple Installation)

### Option 1: Download Pre-built Release (Recommended)

1. **Download** the latest release from the Releases page
2. **Extract** the ZIP file to any folder (e.g., `C:\Program Files\MicPushToTalk`)
3. **Run** `MicPushToTalk.exe`
4. **Done!** No .NET installation required

### Option 2: Build from Source

If you want to build it yourself:

1. **Install .NET 8 SDK** (or later)
   - Download from: https://dotnet.microsoft.com/download
   - Choose ".NET SDK" (not just runtime)

2. **Clone or Download** this repository

3. **Run the build script:**
   ```bash
   # Windows Command Prompt
   publish.bat
   
   # OR PowerShell
   .\publish.ps1
   ```

4. **Find the executable** in `.\publish\MicPushToTalk\MicPushToTalk.exe`

## System Requirements

- **OS**: Windows 10 (1809+) or Windows 11
- **Architecture**: 64-bit (x64)
- **RAM**: 50 MB minimum
- **Disk Space**: 100 MB
- **Permissions**: Microphone access

## First Run Setup

1. **Launch** the application
2. **Allow microphone access** if Windows prompts you
3. **Configure hotkey** (default: Left Alt)
   - Right-click the tray icon → Settings
4. **Test** by holding your hotkey and speaking

## Troubleshooting

### "Windows protected your PC" message
- Click "More info" → "Run anyway"
- This appears because the app isn't digitally signed (costs money)

### Microphone not muting
- Check Windows microphone permissions
- Ensure the correct microphone is selected in Settings
- Try running as Administrator

### Overlay not visible
- Check if it's hidden: Right-click tray icon → Toggle Overlay
- Try resetting position in Settings

### Hotkey not working
- Another app might be using the same key
- Try a different hotkey in Settings
- Some games block global hotkeys

## Uninstallation

1. **Exit** the application (right-click tray icon → Exit)
2. **Delete** the application folder
3. **Optional**: Remove settings folder at `%AppData%\MicPushToTalk`

## Auto-Start on Windows Boot

1. Open Settings (right-click tray icon)
2. Enable "Start on boot"
3. Click Save

Or manually:
1. Press `Win + R`
2. Type `shell:startup` and press Enter
3. Create a shortcut to `MicPushToTalk.exe` in that folder

## Distribution

### For Developers: Creating a Release

1. **Build** using the publish script:
   ```bash
   publish.bat
   ```

2. **Test** the executable in `.\publish\MicPushToTalk\`

3. **Create ZIP** of the publish folder:
   ```bash
   # PowerShell
   Compress-Archive -Path ".\publish\MicPushToTalk\*" -DestinationPath ".\MicPushToTalk-v1.0.0-win-x64.zip"
   ```

4. **Upload** to GitHub Releases or your distribution platform

### What to Include in Release

- `MicPushToTalk.exe` (main executable)
- All DLL files in the publish folder
- `README.md` (user documentation)
- `INSTALLATION.md` (this file)
- `LICENSE` (if applicable)

### File Size

- Single executable: ~150-200 MB (includes .NET runtime)
- Compressed ZIP: ~50-70 MB

## Advanced: Framework-Dependent Deployment

If you want a smaller executable (requires users to install .NET 8):

```bash
dotnet publish MicPushToTalk\MicPushToTalk.csproj -c Release -r win-x64 --self-contained false -o ".\publish\MicPushToTalk-FDD"
```

This creates a ~5 MB executable, but users must install .NET 8 Desktop Runtime.

## Security Notes

- The app requires microphone permissions
- Global hotkeys use low-level keyboard hooks (standard for this type of app)
- Settings are stored locally in `%AppData%\MicPushToTalk\settings.json`
- No telemetry or internet connection required
- All code is open source and auditable

## Support

For issues, questions, or feature requests:
- Open an issue on GitHub
- Check existing issues for solutions
- Include your Windows version and error messages
