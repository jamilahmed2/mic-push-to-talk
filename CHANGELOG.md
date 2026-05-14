# Changelog - Mic Push-To-Talk

## Version 1.1.0 (Latest)

### 🎨 UI/UX Improvements

#### Glassmorphism Design
- ✨ **Enhanced glass effects** with gradient backgrounds
- ✨ **Improved transparency layers** for better depth
- ✨ **Multi-color gradient borders** for modern look
- ✨ **Larger drop shadows** (30px blur) for elevation
- ✨ **Rounded corners** (20px radius) throughout
- ✨ **Better visual hierarchy** with consistent spacing

#### Settings Window
- ✨ **Draggable window** - Click and drag anywhere
- ✨ **Close button** in top-right corner with hover effects
- ✨ **Larger header** (32px) for better readability
- ✨ **Softer text colors** (#CCFFFFFF) for reduced eye strain
- ✨ **Improved button styling** with gradients
- ✨ **Better input field design** with focus states
- ✨ **Enhanced spacing** and padding throughout

#### Overlay Bubble
- ✨ **Larger circle** (90px vs 80px)
- ✨ **Close button** (X) appears on hover
- ✨ **Better icon size** (45px) for visibility
- ✨ **Enhanced glow effects** when active
- ✨ **Improved shadow depth** for floating appearance
- ✨ **Thicker mute line** (3px) with rounded caps
- ✨ **Better volume visualizer** with glow effects

#### Animations
- ✨ **Easing functions** (CubicEase, BackEase) for smooth motion
- ✨ **Fade in/out** for close button
- ✨ **Scale bounce** on state changes
- ✨ **Pulsing glow** when active
- ✨ **Smooth transitions** (200-300ms) throughout

### 🐛 Bug Fixes

#### Hotkey Interference Fix
- 🐛 **Fixed**: Hotkey no longer types in text fields
- 🐛 **Fixed**: Space bar can now be used as push-to-talk
- 🐛 **Fixed**: Key events are properly suppressed
- ✅ **Added**: Support for system keys (Alt, etc.)
- ✅ **Added**: Configurable key suppression

**Technical Details:**
- Modified `LowLevelKeyboardHook` to suppress key events
- Return `(IntPtr)1` to block key propagation
- Added support for `WM_SYSKEYDOWN`/`WM_SYSKEYUP`
- Key is completely consumed by the application

#### Live Settings Application
- 🐛 **Fixed**: No restart required after changing settings
- 🐛 **Fixed**: Settings apply immediately
- ✅ **Added**: Live hotkey update
- ✅ **Added**: Live overlay resize
- ✅ **Added**: Live opacity update
- ✅ **Added**: Live microphone selection

**Technical Details:**
- Added `ReloadSettings()` to ViewModel
- Added `ApplySettings()` to OverlayWindow
- Hotkey hook is recreated with new key
- All visual properties update instantly

### 🚀 Performance Improvements
- ⚡ **Optimized** keyboard hook callback
- ⚡ **Reduced** settings reload time
- ⚡ **Improved** animation performance
- ⚡ **Better** memory management

### 📝 Documentation
- 📄 Added `UI-IMPROVEMENTS.md` - Detailed UI changes
- 📄 Added `FIXES.md` - Bug fix documentation
- 📄 Added `DEPLOYMENT.md` - Build and release guide
- 📄 Added `INSTALLATION.md` - User installation guide
- 📄 Added `QUICK-START.md` - Quick start guide
- 📄 Updated `README.md` - Complete documentation

---

## Version 1.0.0 (Initial Release)

### ✨ Core Features

#### Push-To-Talk System
- 🎤 System-wide microphone mute/unmute
- ⌨️ Global hotkey support (keyboard + mouse)
- 🎯 Low latency (<10ms response time)
- 🔄 Push-to-talk and toggle modes

#### Overlay
- 🪟 Always-on-top floating window
- 🎨 Glassmorphism design
- 🖱️ Draggable with edge snapping
- 📊 Volume visualizer
- ✨ Animated state transitions

#### Settings
- ⚙️ Customizable hotkey
- 🎚️ Adjustable overlay size (60-150px)
- 🌫️ Adjustable opacity (30-100%)
- 🎤 Multiple microphone selection
- 🚀 Start on boot option
- 💾 Persistent settings (JSON)

#### System Tray
- 🔔 Minimize to tray
- 🎨 Icon shows mute state
- 📋 Context menu (Settings, Toggle, Exit)
- 🖱️ Double-click to open settings

#### Audio Control
- 🎤 Windows Core Audio API integration
- 🔇 Instant mute/unmute
- 📊 Real-time volume monitoring
- 🎧 Multiple device support

#### Visual Design
- 🎨 Modern glassmorphism UI
- 🌈 Gradient backgrounds and borders
- ✨ Smooth animations
- 🎭 Clear visual states (muted/active)
- 💫 Glow effects and shadows

### 🏗️ Architecture
- 🏛️ Clean MVVM pattern
- 📦 Modular service architecture
- 🎯 Separation of concerns
- 🧩 Reusable components
- 📝 Well-documented code

### 📦 Distribution
- 📦 Self-contained executable
- 🚫 No .NET installation required
- 💾 Single file deployment (~175 MB)
- 🪟 Windows 10/11 x64 support
- 🔒 No admin privileges required

### 🛠️ Tech Stack
- C# .NET 8
- WPF (Windows Presentation Foundation)
- NAudio for audio control
- CommunityToolkit.Mvvm
- Win32 APIs for global hotkeys

---

## Upgrade Guide

### From 1.0.0 to 1.1.0

**What's Changed:**
- UI has been completely redesigned
- Settings now apply without restart
- Hotkey no longer interferes with typing
- Better animations and visual feedback

**Breaking Changes:**
- None! All settings are backward compatible

**How to Upgrade:**
1. Close the old version
2. Replace `MicPushToTalk.exe` with new version
3. Launch the application
4. Your settings will be preserved

**Note:** Settings file location remains the same:
```
%AppData%\MicPushToTalk\settings.json
```

---

## Known Issues

### Current Limitations
- Some fullscreen games may block the overlay
- Certain anti-cheat systems may block keyboard hooks
- System keys (Windows key, Ctrl+Alt+Del) cannot be used as hotkeys

### Workarounds
- Use borderless windowed mode for games
- Try different hotkey if one doesn't work
- Run as administrator if needed (not recommended)

---

## Roadmap

### Planned Features (v1.2.0)
- [ ] Multiple hotkey profiles
- [ ] Per-application settings
- [ ] Sound effects on toggle
- [ ] Custom themes and colors
- [ ] OBS-safe mode
- [ ] Auto-hide when inactive
- [ ] Keyboard shortcuts for settings

### Future Enhancements (v2.0.0)
- [ ] Plugin system
- [ ] Cloud settings sync
- [ ] Mobile companion app
- [ ] Advanced audio processing
- [ ] Noise suppression
- [ ] Voice activity detection

---

## Support

### Getting Help
- 📖 Read the [README.md](README.md)
- 🚀 Check [QUICK-START.md](QUICK-START.md)
- 🐛 Report issues on GitHub
- 💬 Join community discussions

### Contributing
- 🐛 Report bugs
- 💡 Suggest features
- 🔧 Submit pull requests
- 📝 Improve documentation

---

## License

MIT License - See [LICENSE.txt](LICENSE.txt) for details

---

## Credits

### Dependencies
- [NAudio](https://github.com/naudio/NAudio) - Audio control
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) - MVVM helpers
- [Newtonsoft.Json](https://www.newtonsoft.com/json) - JSON serialization

### Inspiration
- Windows 11 Fluent Design
- Discord overlay
- Stream Deck
- OBS Studio

---

**Thank you for using Mic Push-To-Talk!** 🎤✨
