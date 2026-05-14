using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace MicPushToTalk.Services;

public class HotkeyService : IDisposable
{
    private const int WM_HOTKEY = 0x0312;
    private const int HOTKEY_ID = 9000;

    private IntPtr _windowHandle;
    private HwndSource? _source;
    private bool _isRegistered;

    public event EventHandler? HotkeyPressed;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    public void Initialize(IntPtr windowHandle)
    {
        _windowHandle = windowHandle;
        _source = HwndSource.FromHwnd(_windowHandle);
        _source?.AddHook(HwndHook);
    }

    public bool RegisterHotkey(int virtualKey, int modifiers)
    {
        UnregisterHotkey();

        try
        {
            _isRegistered = RegisterHotKey(_windowHandle, HOTKEY_ID, (uint)modifiers, (uint)virtualKey);
            return _isRegistered;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error registering hotkey: {ex.Message}");
            return false;
        }
    }

    public void UnregisterHotkey()
    {
        if (_isRegistered)
        {
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            _isRegistered = false;
        }
    }

    public bool IsKeyPressed(int virtualKey)
    {
        return (GetAsyncKeyState(virtualKey) & 0x8000) != 0;
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
        {
            HotkeyPressed?.Invoke(this, EventArgs.Empty);
            handled = true;
        }

        return IntPtr.Zero;
    }

    public void Dispose()
    {
        UnregisterHotkey();
        _source?.RemoveHook(HwndHook);
    }
}

// For low-level keyboard hook (alternative approach for better key detection)
public class LowLevelKeyboardHook : IDisposable
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    private LowLevelKeyboardProc? _proc;
    private IntPtr _hookID = IntPtr.Zero;
    private int _targetKey;

    public event EventHandler? KeyPressed;
    public event EventHandler? KeyReleased;

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    public void SetHook(int virtualKey)
    {
        _targetKey = virtualKey;
        _proc = HookCallback;
        
        using var curProcess = System.Diagnostics.Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;
        
        if (curModule != null)
        {
            _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            
            if (vkCode == _targetKey)
            {
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    KeyPressed?.Invoke(this, EventArgs.Empty);
                }
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    KeyReleased?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        if (_hookID != IntPtr.Zero)
        {
            UnhookWindowsHookEx(_hookID);
            _hookID = IntPtr.Zero;
        }
    }
}
