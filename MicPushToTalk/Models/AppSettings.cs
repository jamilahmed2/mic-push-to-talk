namespace MicPushToTalk.Models;

public class AppSettings
{
    public int HotkeyVirtualKey { get; set; } = 0xA4; // Left Alt
    public int HotkeyModifiers { get; set; } = 0; // No modifiers
    public string HotkeyDisplayName { get; set; } = "Left Alt";
    public double OverlaySize { get; set; } = 80;
    public double OverlayOpacity { get; set; } = 0.95;
    public string ActiveColor { get; set; } = "#00FF88";
    public string InactiveColor { get; set; } = "#FF4444";
    public bool StartOnBoot { get; set; } = false;
    public double OverlayX { get; set; } = 100;
    public double OverlayY { get; set; } = 100;
    public bool SnapToEdges { get; set; } = true;
    public int SnapThreshold { get; set; } = 20;
    public bool ShowVolumeVisualizer { get; set; } = true;
    public string? SelectedMicrophoneId { get; set; }
}
