using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.CoreAudioApi;
using MicPushToTalk.Models;

namespace MicPushToTalk.Services;

public class AudioService : IDisposable
{
    private MMDeviceEnumerator? _deviceEnumerator;
    private MMDevice? _currentDevice;
    private bool _isMuted = true;

    public event EventHandler<bool>? MuteStateChanged;

    public AudioService()
    {
        _deviceEnumerator = new MMDeviceEnumerator();
        InitializeDefaultDevice();
    }

    private void InitializeDefaultDevice()
    {
        try
        {
            _currentDevice = _deviceEnumerator?.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing audio device: {ex.Message}");
        }
    }

    public List<MicrophoneDevice> GetAvailableMicrophones()
    {
        var devices = new List<MicrophoneDevice>();
        
        try
        {
            if (_deviceEnumerator == null) return devices;

            var defaultDevice = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
            var collection = _deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            foreach (var device in collection)
            {
                devices.Add(new MicrophoneDevice
                {
                    Id = device.ID,
                    Name = device.FriendlyName,
                    IsDefault = device.ID == defaultDevice.ID
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error enumerating microphones: {ex.Message}");
        }

        return devices;
    }

    public void SelectMicrophone(string deviceId)
    {
        try
        {
            if (_deviceEnumerator == null) return;

            var collection = _deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            _currentDevice = collection.FirstOrDefault(d => d.ID == deviceId);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error selecting microphone: {ex.Message}");
        }
    }

    public void SetMute(bool mute)
    {
        try
        {
            if (_currentDevice == null)
            {
                InitializeDefaultDevice();
            }

            if (_currentDevice != null)
            {
                _currentDevice.AudioEndpointVolume.Mute = mute;
                _isMuted = mute;
                MuteStateChanged?.Invoke(this, mute);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting mute state: {ex.Message}");
        }
    }

    public bool IsMuted()
    {
        try
        {
            if (_currentDevice == null)
            {
                InitializeDefaultDevice();
            }

            if (_currentDevice != null)
            {
                _isMuted = _currentDevice.AudioEndpointVolume.Mute;
                return _isMuted;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting mute state: {ex.Message}");
        }

        return true;
    }

    public float GetCurrentVolume()
    {
        try
        {
            if (_currentDevice == null)
            {
                InitializeDefaultDevice();
            }

            if (_currentDevice != null)
            {
                return _currentDevice.AudioMeterInformation.MasterPeakValue;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting volume: {ex.Message}");
        }

        return 0f;
    }

    public void Dispose()
    {
        _currentDevice?.Dispose();
        _deviceEnumerator?.Dispose();
    }
}
