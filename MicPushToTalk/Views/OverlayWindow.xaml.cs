using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Threading;
using MicPushToTalk.Services;
using MicPushToTalk.ViewModels;

namespace MicPushToTalk.Views;

public partial class OverlayWindow : Window
{
    private readonly OverlayViewModel _viewModel;
    private readonly DispatcherTimer _volumeTimer;
    private Point _dragStartPoint;
    private bool _isDragging;

    public OverlayWindow()
    {
        InitializeComponent();
        
        _viewModel = new OverlayViewModel();
        DataContext = _viewModel;

        // Volume visualizer timer
        _volumeTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(50)
        };
        _volumeTimer.Tick += VolumeTimer_Tick;

        Loaded += OverlayWindow_Loaded;
        Closing += OverlayWindow_Closing;
    }

    private void OverlayWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _viewModel.Initialize(new WindowInteropHelper(this).Handle);
        _viewModel.MicStateChanged += OnMicStateChanged;
        
        // Load saved position
        var settings = _viewModel.GetSettings();
        Left = settings.OverlayX;
        Top = settings.OverlayY;
        Width = settings.OverlaySize + 40;
        Height = settings.OverlaySize + 40;
        
        UpdateVisualState(_viewModel.IsMuted);
    }

    private void OverlayWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        _volumeTimer.Stop();
        _viewModel.Dispose();
    }

    private void OnMicStateChanged(object? sender, bool isMuted)
    {
        Dispatcher.Invoke(() => UpdateVisualState(isMuted));
    }

    private void UpdateVisualState(bool isMuted)
    {
        var duration = TimeSpan.FromMilliseconds(200);

        if (isMuted)
        {
            // Muted state - red
            AnimateColor(MicIcon, System.Windows.Shapes.Path.FillProperty, Colors.Red, duration);
            AnimateOpacity(MuteLine, 1, duration);
            AnimateOpacity(GlowRing, 0, duration);
            AnimateScale(MainCircle, 1.0, duration);
            
            _volumeTimer.Stop();
            VolumeVisualizer.Opacity = 0;
        }
        else
        {
            // Active state - green/cyan
            AnimateColor(MicIcon, System.Windows.Shapes.Path.FillProperty, Color.FromRgb(0, 255, 136), duration);
            AnimateOpacity(MuteLine, 0, duration);
            AnimateGlow();
            AnimateScale(MainCircle, 1.1, duration);
            
            if (_viewModel.GetSettings().ShowVolumeVisualizer)
            {
                _volumeTimer.Start();
                VolumeVisualizer.Opacity = 1;
            }
        }
    }

    private void AnimateGlow()
    {
        var glowAnimation = new DoubleAnimation
        {
            From = 0,
            To = 0.6,
            Duration = TimeSpan.FromMilliseconds(300),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever
        };
        GlowRing.BeginAnimation(OpacityProperty, glowAnimation);

        var shadowAnimation = new ColorAnimation
        {
            To = Color.FromRgb(0, 255, 136),
            Duration = TimeSpan.FromMilliseconds(300)
        };
        MainShadow.BeginAnimation(DropShadowEffect.ColorProperty, shadowAnimation);
    }

    private void AnimateColor(DependencyObject target, DependencyProperty property, Color toColor, TimeSpan duration)
    {
        var animation = new ColorAnimation
        {
            To = toColor,
            Duration = duration,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        
        if (target is System.Windows.Shapes.Path path && property == System.Windows.Shapes.Path.FillProperty)
        {
            var brush = new SolidColorBrush();
            path.Fill = brush;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }

    private void AnimateOpacity(UIElement element, double toOpacity, TimeSpan duration)
    {
        var animation = new DoubleAnimation
        {
            To = toOpacity,
            Duration = duration,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        element.BeginAnimation(OpacityProperty, animation);
    }

    private void AnimateScale(UIElement element, double scale, TimeSpan duration)
    {
        var transform = new ScaleTransform(1, 1);
        element.RenderTransform = transform;
        element.RenderTransformOrigin = new Point(0.5, 0.5);

        var animation = new DoubleAnimation
        {
            To = scale,
            Duration = duration,
            EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 }
        };
        
        transform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
        transform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
    }

    private void VolumeTimer_Tick(object? sender, EventArgs e)
    {
        var volume = _viewModel.GetCurrentVolume();
        AnimateVolumeBars(volume);
    }

    private void AnimateVolumeBars(float volume)
    {
        var bars = new[] { Bar1, Bar2, Bar3, Bar4, Bar5 };
        var heights = new[] { 6.0, 10.0, 14.0, 10.0, 6.0 };
        
        for (int i = 0; i < bars.Length; i++)
        {
            var targetHeight = heights[i] * (0.3 + volume * 0.7);
            var animation = new DoubleAnimation
            {
                To = targetHeight,
                Duration = TimeSpan.FromMilliseconds(100),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            bars[i].BeginAnimation(HeightProperty, animation);
        }
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            _isDragging = true;
            _dragStartPoint = e.GetPosition(this);
            DragMove();
        }
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);
        
        if (_isDragging)
        {
            _isDragging = false;
            SnapToEdges();
            SavePosition();
        }
    }

    private void SnapToEdges()
    {
        var settings = _viewModel.GetSettings();
        if (!settings.SnapToEdges) return;

        var screen = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle);
        var workingArea = screen.WorkingArea;
        var threshold = settings.SnapThreshold;

        if (Left < workingArea.Left + threshold)
            Left = workingArea.Left;
        else if (Left + Width > workingArea.Right - threshold)
            Left = workingArea.Right - Width;

        if (Top < workingArea.Top + threshold)
            Top = workingArea.Top;
        else if (Top + Height > workingArea.Bottom - threshold)
            Top = workingArea.Bottom - Height;
    }

    private void SavePosition()
    {
        _viewModel.SavePosition(Left, Top);
    }

    private void MainCircle_Click(object sender, MouseButtonEventArgs e)
    {
        if (!_isDragging)
        {
            _viewModel.ToggleMute();
        }
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);
        // Show close button on hover
        var fadeIn = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));
        CloseButton.BeginAnimation(OpacityProperty, fadeIn);
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);
        // Hide close button when not hovering
        var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
        CloseButton.BeginAnimation(OpacityProperty, fadeOut);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        // Hide the overlay instead of closing the app
        Hide();
    }

    private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
    {
        CloseButton.Background = new SolidColorBrush(Color.FromArgb(160, 255, 68, 68));
    }

    private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
    {
        CloseButton.Background = new SolidColorBrush(Color.FromArgb(96, 0, 0, 0));
    }
}
