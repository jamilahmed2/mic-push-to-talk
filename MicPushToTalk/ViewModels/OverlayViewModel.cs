using System;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using MicPushToTalk.Models;
using MicPushToTalk.Services;

namespace MicPushToTalk.ViewModels;

public partial class OverlayViewModel : ObservableObject, IDisposable
{
    private readonly AudioService _audioService;
    private readonly LowLevelKeyboardHook _keyboardHook;
    private readonly SettingsService _settingsService;
    private readonly TrayService _trayService;
    private AppSettings _settings;

    [ObservableProperty]
    private bool _isMuted = true;

    public event EventHandler<bool>? MicStateChanged;

    public OverlayViewModel()
    {
        _audioService = new AudioService();
        _keyboardHook = new LowLevelKeyboardHook();
        _settingsService = new SettingsService();
        _trayService = App.Current.Properties["TrayService"] as TrayService ?? new TrayService();
        
        _settings = _settingsService.LoadSettings();
        
        _keyboardHook.KeyPressed += OnHotkeyPressed;
        _keyboardHook.KeyReleased += OnHotkeyReleased;
    }

    public void Initialize(IntPtr windowHandle)
    {
        // Set initial mute state
        _audioService.SetMute(true);
        IsMuted = true;
        
        // Setup keyboard hook
        _keyboardHook.SetHook(_settings.HotkeyVirtualKey);
        
        // Select microphone if specified
        if (!string.IsNullOrEmpty(_settings.SelectedMicrophoneId))
        {
            _audioService.SelectMicrophone(_settings.SelectedMicrophoneId);
        }
    }

    private void OnHotkeyPressed(object? sender, EventArgs e)
    {
        if (_settings.IsPushToTalkMode)
        {
            SetMicrophoneState(false); // Unmute
        }
        else
        {
            ToggleMute(); // Toggle mode
        }
    }

    private void OnHotkeyReleased(object? sender, EventArgs e)
    {
        if (_settings.IsPushToTalkMode)
        {
            SetMicrophoneState(true); // Mute
        }
    }

    public void ToggleMute()
    {
        SetMicrophoneState(!IsMuted);
    }

    private void SetMicrophoneState(bool mute)
    {
        _audioService.SetMute(mute);
        IsMuted = mute;
        _trayService.UpdateIcon(mute);
        MicStateChanged?.Invoke(this, mute);
    }

    public float GetCurrentVolume()
    {
        return _audioService.GetCurrentVolume();
    }

    public AppSettings GetSettings()
    {
        return _settings;
    }

    public void SavePosition(double x, double y)
    {
        _settings.OverlayX = x;
        _settings.OverlayY = y;
        _settingsService.SaveSettings(_settings);
    }

    public void Dispose()
    {
        _keyboardHook.Dispose();
        _audioService.Dispose();
    }
}
