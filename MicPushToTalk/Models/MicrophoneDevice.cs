namespace MicPushToTalk.Models;

public class MicrophoneDevice
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsDefault { get; set; }

    public override string ToString() => IsDefault ? $"{Name} (Default)" : Name;
}
