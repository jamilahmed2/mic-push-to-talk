# Changelog

All notable changes to Mic Push-To-Talk will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-05-14

### Added
- Initial release
- System-wide push-to-talk functionality
- Global hotkey support (keyboard and mouse buttons)
- Glassmorphism UI with modern animations
- Always-on-top draggable overlay
- Animated microphone icon with state feedback
- Real-time volume visualizer
- System tray integration
- Settings window with customization options
- Microphone selection support
- Edge snapping for overlay positioning
- Auto-start on Windows boot option
- Push-to-talk and toggle modes
- Settings persistence (JSON)
- Self-contained deployment (no .NET required)

### Features
- **Audio Control**: Mute/unmute system microphone using NAudio CoreAudioApi
- **Hotkeys**: Support for any keyboard key or mouse button
- **Overlay**: Circular floating button with glassmorphism design
- **Animations**: Smooth transitions, glow effects, scale animations
- **Tray Icon**: Shows mute state, quick access menu
- **Settings**: Hotkey, size, opacity, colors, startup options
- **MVVM Architecture**: Clean, maintainable code structure

### Technical
- Built with C# .NET 8 and WPF
- NAudio 2.2.1 for audio control
- CommunityToolkit.Mvvm 8.2.2 for MVVM
- Low-level keyboard hooks for reliable hotkey detection
- Hardware-accelerated rendering
- Single-file self-contained executable (~174 MB)

### Known Issues
- Some fullscreen games may block the overlay
- Unsigned executable triggers Windows SmartScreen warning
- First launch may be slower due to self-extraction

### System Requirements
- Windows 10 (1809+) or Windows 11
- 64-bit (x64) architecture
- ~100 MB disk space
- Microphone access permission

## [Unreleased]

### Planned Features
- OBS-safe mode for streaming
- Sound effects on toggle
- Multiple hotkey profiles
- Custom themes and color schemes
- Auto-hide when inactive
- Keyboard shortcuts for settings
- Multi-monitor improvements
- Code signing for trusted executable
- Installer package (Inno Setup)
- Auto-update system

### Under Consideration
- macOS and Linux support
- Multiple microphone simultaneous control
- Voice activity detection
- Integration with Discord, Zoom, etc.
- Portable mode (no registry writes)
- Minimal/compact overlay mode
- Customizable overlay shapes

---

## Version History

- **v1.0.0** (2026-05-14) - Initial release

---

## How to Update

### Automatic (Future)
The app will check for updates on startup and prompt you to download.

### Manual
1. Download the latest release from GitHub
2. Extract and replace the old executable
3. Your settings will be preserved (stored in AppData)

---

## Reporting Issues

Found a bug or have a feature request?
- Open an issue on GitHub
- Include your Windows version
- Describe steps to reproduce
- Attach error messages if any

---

## Contributing

Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.
