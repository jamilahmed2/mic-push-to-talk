using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using MicPushToTalk.Views;
using Application = System.Windows.Application;

namespace MicPushToTalk.Services;

public class TrayService : IDisposable
{
    private NotifyIcon? _notifyIcon;
    private Icon? _mutedIcon;
    private Icon? _unmutedIcon;

    public void Initialize()
    {
        // Create icons (simple colored circles for now)
        _mutedIcon = CreateIcon(Color.Red);
        _unmutedIcon = CreateIcon(Color.Green);

        _notifyIcon = new NotifyIcon
        {
            Icon = _mutedIcon,
            Visible = true,
            Text = "Mic Push-To-Talk (Muted)"
        };

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Settings", null, OnSettings);
        contextMenu.Items.Add("Toggle Overlay", null, OnToggleOverlay);
        contextMenu.Items.Add("-");
        contextMenu.Items.Add("Exit", null, OnExit);

        _notifyIcon.ContextMenuStrip = contextMenu;
        _notifyIcon.DoubleClick += OnDoubleClick;
    }

    public void UpdateIcon(bool isMuted)
    {
        if (_notifyIcon != null)
        {
            _notifyIcon.Icon = isMuted ? _mutedIcon : _unmutedIcon;
            _notifyIcon.Text = isMuted ? "Mic Push-To-Talk (Muted)" : "Mic Push-To-Talk (Active)";
        }
    }

    private Icon CreateIcon(Color color)
    {
        var bitmap = new Bitmap(16, 16);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            using (var brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, 2, 2, 12, 12);
            }
        }
        return Icon.FromHandle(bitmap.GetHicon());
    }

    private void OnSettings(object? sender, EventArgs e)
    {
        var settingsWindow = new SettingsWindow();
        settingsWindow.Show();
    }

    private void OnToggleOverlay(object? sender, EventArgs e)
    {
        var mainWindow = Application.Current.MainWindow;
        if (mainWindow != null)
        {
            if (mainWindow.Visibility == Visibility.Visible)
            {
                // Hiding - disable hotkey
                if (mainWindow is OverlayWindow overlayWindow)
                {
                    overlayWindow.DisableHotkey();
                }
                mainWindow.Visibility = Visibility.Hidden;
            }
            else
            {
                // Showing - enable hotkey
                if (mainWindow is OverlayWindow overlayWindow)
                {
                    overlayWindow.EnableHotkey();
                }
                mainWindow.Visibility = Visibility.Visible;
            }
        }
    }

    private void OnDoubleClick(object? sender, EventArgs e)
    {
        OnSettings(sender, e);
    }

    private void OnExit(object? sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    public void Dispose()
    {
        if (_notifyIcon != null)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }
        _mutedIcon?.Dispose();
        _unmutedIcon?.Dispose();
    }
}
