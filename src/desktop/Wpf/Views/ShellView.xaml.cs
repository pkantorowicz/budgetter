using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using Budgetter.Wpf.Views.Helpers;

namespace Budgetter.Wpf.Views;

public partial class ShellView : Window
{
    public ShellView()
    {
        InitializeComponent();

        var chrome = new WindowChrome
        {
            CaptionHeight = 0,
            CornerRadius = new CornerRadius(0, 0, 0, 0),
            GlassFrameThickness = new Thickness(0),
            NonClientFrameEdges = NonClientFrameEdges.None,
            ResizeBorderThickness = new Thickness(4),
            UseAeroCaptionButtons = false
        };

        WindowChrome.SetWindowChrome(this, chrome);

        Loaded += (_, _) =>
        {
            var matrix = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice;

            if (!matrix.HasValue)
                return;

            var dpiTransform = new ScaleTransform(
                1 / matrix.Value.M11,
                1 / matrix.Value.M22);

            if (dpiTransform.CanFreeze)
                dpiTransform.Freeze();

            var screen = Screen.FromHandle(new WindowInteropHelper(this).Handle);
            var maxHeight = dpiTransform.ScaleY * screen.Bounds.Height;
            var maxWidth = dpiTransform.ScaleX * screen.Bounds.Width;

            var (screenHeight, screenWidth) =
                ScreenDimensionsHelper.GetScreenDimensions(
                    maxHeight,
                    maxWidth,
                    0.6);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MinHeight = screenHeight;
            MinWidth = screenWidth;
        };
    }

    private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void TitleBar_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        WindowState =
            WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
    }
}