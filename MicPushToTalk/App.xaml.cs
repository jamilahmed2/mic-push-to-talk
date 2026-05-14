using System.Windows;
using MicPushToTalk.Services;
using MicPushToTalk.Views;

namespace MicPushToTalk;

public partial class App : Application
{
    private TrayService? _trayService;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Initialize tray service
        _trayService = new TrayService();
        _trayService.Initialize();
        
        // Store in application properties for access by ViewModels
        Current.Properties["TrayService"] = _trayService;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayService?.Dispose();
        base.OnExit(e);
    }
}
