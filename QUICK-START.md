# Quick Start Guide - Mic Push-To-Talk

## For Users (Download & Run)

### Step 1: Download
Download the latest release ZIP file from the Releases page.

### Step 2: Extract
Extract the ZIP file to any folder on your computer:
- `C:\Program Files\MicPushToTalk\` (recommended)
- Or any folder you prefer

### Step 3: Run
Double-click `MicPushToTalk.exe` to start the application.

**Note:** You may see a "Windows protected your PC" message. Click "More info" → "Run anyway". This is normal for unsigned applications.

### Step 4: Configure (Optional)
1. Look for the microphone icon in your system tray (bottom-right corner)
2. Right-click the icon → **Settings**
3. Change hotkey if desired (default: Left Alt)
4. Adjust overlay size and appearance
5. Click **Save**

### Step 5: Use It!
- **Press** your hotkey (default: Left Alt) to unmute your microphone
- **Press again** to mute
- The floating overlay shows your current status:
  - 🔴 Red = Muted
  - 🟢 Green = Active/Talking
- Simple toggle: Press once to talk, press again to mute

## For Developers (Build from Source)

### Prerequisites
- Windows 10/11
- .NET 8 SDK or later
- Visual Studio 2022 (optional)

### Build Steps

**Option 1: Using Build Script (Easiest)**
```bash
# Clone the repository
git clone https://github.com/jamilahmed2/mic-push-to-talk.git
cd mic-push-to-talk

# Run the build script
publish.bat

# Executable will be in: .\publish\MicPushToTalk\MicPushToTalk.exe
```

**Option 2: Manual Build**
```bash
# Restore packages
dotnet restore

# Build and publish
dotnet publish MicPushToTalk\MicPushToTalk.csproj -c Release

# Run
.\MicPushToTalk\bin\Release\net8.0-windows\win-x64\publish\MicPushToTalk.exe
```

**Option 3: Visual Studio**
1. Open `MicPushToTalk.sln`
2. Set configuration to **Release**
3. Right-click project → **Publish**
4. Choose **Folder** profile
5. Click **Publish**

## Features Overview

### 🎯 Core Features
- **System-wide push-to-talk** - Works in any application
- **Global hotkeys** - Keyboard keys, mouse buttons, combinations
- **Always-on-top overlay** - Visible even in fullscreen games
- **Glassmorphism UI** - Modern, transparent design
- **Animated feedback** - Visual confirmation of mic state

### ⚙️ Settings
- **Hotkey customization** - Any key or combination
- **Microphone selection** - Pick which mic to control
- **Overlay customization** - Size, opacity, position
- **Auto-start** - Launch with Windows
- **Volume visualizer** - Real-time audio levels
- **Toggle mode** - Press once to unmute, press again to mute

### 🎨 Visual States
- **Muted** (default): Red mic icon with cross
- **Active** (talking): Green glowing mic with pulse animation
- **Volume bars**: Show microphone input level

## Keyboard Shortcuts

| Action | Shortcut |
|--------|----------|
| Toggle mute | Click overlay or use hotkey |
| Open settings | Right-click tray icon → Settings |
| Exit app | Right-click tray icon → Exit |
| Move overlay | Click and drag |

## Hotkey Examples

You can use any of these as your push-to-talk key:

**Single Keys:**
- Left Alt (default)
- Caps Lock
- F13-F24 (if your keyboard has them)
- Mouse Button 4 or 5

**Combinations:**
- Ctrl + Space
- Alt + Shift + M
- Ctrl + Alt + T

## Troubleshooting

### Microphone not muting?
1. Check Windows microphone permissions
2. Verify correct mic selected in Settings
3. Try running as Administrator

### Overlay not visible?
1. Right-click tray icon → Toggle Overlay
2. Check if it's off-screen (reset position in Settings)
3. Ensure "Always on top" is enabled

### Hotkey not working?
1. Another app might be using the same key
2. Try a different hotkey in Settings
3. Some fullscreen games block global hotkeys

### "Windows protected your PC" warning?
- This is normal for unsigned apps
- Click "More info" → "Run anyway"
- The app is safe and open source

## System Requirements

- **OS**: Windows 10 (1809+) or Windows 11
- **Architecture**: 64-bit (x64)
- **RAM**: 50 MB
- **Disk**: 100 MB
- **Permissions**: Microphone access

## No .NET Installation Required!

The published version includes everything needed. Users don't need to install .NET separately.

## Privacy & Security

- ✅ No internet connection required
- ✅ No telemetry or data collection
- ✅ Settings stored locally only
- ✅ Open source code (auditable)
- ✅ Microphone access only (no other permissions)

## Support

- **Issues**: Open an issue on GitHub
- **Questions**: Check existing issues first
- **Feature requests**: Welcome!

## Distribution

### For End Users
Download the pre-built release from GitHub Releases page.

### For Developers
Run `publish.bat` to create a distributable version.

### Create ZIP for Distribution
```bash
# PowerShell
Compress-Archive -Path ".\publish\MicPushToTalk\*" -DestinationPath ".\MicPushToTalk-v1.0.0-win-x64.zip"
```

## What's Included

When you download the release:
- `MicPushToTalk.exe` - Main application (self-contained)
- `README.md` - Full documentation
- `INSTALLATION.md` - Setup instructions
- `LICENSE.txt` - License information

## Next Steps

1. ✅ Download and run the app
2. ✅ Configure your preferred hotkey
3. ✅ Test in your favorite apps (Discord, Zoom, games, etc.)
4. ✅ Customize the overlay appearance
5. ✅ Enable auto-start if desired

Enjoy your new push-to-talk overlay! 🎤
