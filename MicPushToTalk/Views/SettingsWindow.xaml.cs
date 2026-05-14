using System;
using System.Windows;
using System.Windows.Input;
using MicPushToTalk.ViewModels;

namespace MicPushToTalk.Views;

public partial class SettingsWindow : Window
{
    private readonly SettingsViewModel _viewModel;
    private bool _isCapturingHotkey;

    public SettingsWindow()
    {
        InitializeComponent();
        
        _viewModel = new SettingsViewModel();
        DataContext = _viewModel;
    }

    private void ChangeHotkey_Click(object sender, RoutedEventArgs e)
    {
        _isCapturingHotkey = true;
        HotkeyTextBox.Text = "Press any key...";
        HotkeyTextBox.Focus();
        HotkeyTextBox.PreviewKeyDown += HotkeyTextBox_PreviewKeyDown;
    }

    private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (!_isCapturingHotkey) return;

        e.Handled = true;
        
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        
        // Ignore modifier keys alone
        if (key == Key.LeftCtrl || key == Key.RightCtrl ||
            key == Key.LeftAlt || key == Key.RightAlt ||
            key == Key.LeftShift || key == Key.RightShift ||
            key == Key.LWin || key == Key.RWin)
        {
            return;
        }

        var virtualKey = KeyInterop.VirtualKeyFromKey(key);
        var modifiers = 0;

        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            modifiers |= 0x0002; // MOD_CONTROL
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            modifiers |= 0x0001; // MOD_ALT
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            modifiers |= 0x0004; // MOD_SHIFT

        var displayName = GetKeyDisplayName(key, Keyboard.Modifiers);
        
        _viewModel.HotkeyVirtualKey = virtualKey;
        _viewModel.HotkeyModifiers = modifiers;
        _viewModel.HotkeyDisplayName = displayName;
        
        HotkeyTextBox.Text = displayName;
        _isCapturingHotkey = false;
        HotkeyTextBox.PreviewKeyDown -= HotkeyTextBox_PreviewKeyDown;
    }

    private string GetKeyDisplayName(Key key, ModifierKeys modifiers)
    {
        var parts = new System.Collections.Generic.List<string>();

        if (modifiers.HasFlag(ModifierKeys.Control))
            parts.Add("Ctrl");
        if (modifiers.HasFlag(ModifierKeys.Alt))
            parts.Add("Alt");
        if (modifiers.HasFlag(ModifierKeys.Shift))
            parts.Add("Shift");

        parts.Add(key.ToString());

        return string.Join(" + ", parts);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.SaveSettings();
        
        // Apply settings immediately without restart
        var mainWindow = Application.Current.MainWindow as OverlayWindow;
        if (mainWindow != null)
        {
            mainWindow.ApplySettings();
        }
        
        MessageBox.Show(
            "Settings saved and applied successfully!",
            "Settings Saved",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
        
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            try
            {
                DragMove();
            }
            catch
            {
                // Ignore errors if window is maximized or in invalid state
            }
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
