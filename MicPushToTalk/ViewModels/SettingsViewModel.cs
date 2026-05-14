using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using MicPushToTalk.Models;
using MicPushToTalk.Services;

namespace MicPushToTalk.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly AudioService _audioService;
    private readonly SettingsService _settingsService;
    private AppSettings _settings;

    [ObservableProperty]
    private string _hotkeyDisplayName = "Left Alt";

    [ObservableProperty]
    private double _overlaySize = 80;

    [ObservableProperty]
    private double _overlayOpacity = 0.95;

    [ObservableProperty]
    private bool _snapToEdges = true;

    [ObservableProperty]
    private bool _showVolumeVisualizer = true;

    [ObservableProperty]
    private bool _startOnBoot = false;

    [ObservableProperty]
    private ObservableCollection<MicrophoneDevice> _availableMicrophones = new();

    [ObservableProperty]
    private MicrophoneDevice? _selectedMicrophone;

    public int HotkeyVirtualKey { get; set; }
    public int HotkeyModifiers { get; set; }

    public SettingsViewModel()
    {
        _audioService = new AudioService();
        _settingsService = new SettingsService();
        _settings = _settingsService.LoadSettings();
        
        LoadSettings();
        LoadMicrophones();
    }

    private void LoadSettings()
    {
        HotkeyDisplayName = _settings.HotkeyDisplayName;
        HotkeyVirtualKey = _settings.HotkeyVirtualKey;
        HotkeyModifiers = _settings.HotkeyModifiers;
        OverlaySize = _settings.OverlaySize;
        OverlayOpacity = _settings.OverlayOpacity;
        SnapToEdges = _settings.SnapToEdges;
        ShowVolumeVisualizer = _settings.ShowVolumeVisualizer;
        StartOnBoot = _settings.StartOnBoot;
    }

    private void LoadMicrophones()
    {
        var mics = _audioService.GetAvailableMicrophones();
        AvailableMicrophones = new ObservableCollection<MicrophoneDevice>(mics);
        
        if (!string.IsNullOrEmpty(_settings.SelectedMicrophoneId))
        {
            SelectedMicrophone = AvailableMicrophones.FirstOrDefault(m => m.Id == _settings.SelectedMicrophoneId);
        }
        
        if (SelectedMicrophone == null)
        {
            SelectedMicrophone = AvailableMicrophones.FirstOrDefault(m => m.IsDefault);
        }
    }

    public void SaveSettings()
    {
        _settings.HotkeyDisplayName = HotkeyDisplayName;
        _settings.HotkeyVirtualKey = HotkeyVirtualKey;
        _settings.HotkeyModifiers = HotkeyModifiers;
        _settings.OverlaySize = OverlaySize;
        _settings.OverlayOpacity = OverlayOpacity;
        _settings.SnapToEdges = SnapToEdges;
        _settings.ShowVolumeVisualizer = ShowVolumeVisualizer;
        _settings.StartOnBoot = StartOnBoot;
        _settings.SelectedMicrophoneId = SelectedMicrophone?.Id;

        _settingsService.SaveSettings(_settings);

        // Update startup registry if needed
        if (StartOnBoot)
        {
            AddToStartup();
        }
        else
        {
            RemoveFromStartup();
        }
    }

    private void AddToStartup()
    {
        try
        {
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            
            if (key != null)
            {
                var exePath = System.AppContext.BaseDirectory;
                var exeFile = System.IO.Path.Combine(exePath, "MicPushToTalk.exe");
                key.SetValue("MicPushToTalk", $"\"{exeFile}\"");
                key.Close();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error adding to startup: {ex.Message}");
        }
    }

    private void RemoveFromStartup()
    {
        try
        {
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            
            if (key != null)
            {
                key.DeleteValue("MicPushToTalk", false);
                key.Close();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error removing from startup: {ex.Message}");
        }
    }
}
