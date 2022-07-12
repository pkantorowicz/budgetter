using System.Windows;
using System.Windows.Input;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.ViewModels;

namespace Budgetter.Wpf.ViewModels.Controls;

internal class TitleBarViewModel : ViewModelBase
{
    public TitleBarViewModel()
    {
        MinimizeCommand = new RelayCommand(Minimize);
        MaximizeCommand = new RelayCommand(Maximize);
        CloseCommand = new RelayCommand(Close);
    }

    public ICommand MinimizeCommand { get; }
    public ICommand MaximizeCommand { get; }
    public ICommand CloseCommand { get; }

    public void Minimize(object obj)
    {
        var mainWindow = System.Windows.Application.Current.MainWindow;

        if (mainWindow is null)
            return;

        mainWindow.WindowState = WindowState.Minimized;
    }

    public void Maximize(object obj)
    {
        var mainWindow = System.Windows.Application.Current.MainWindow;

        if (mainWindow is null)
            return;

        mainWindow.WindowState =
            mainWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
    }

    public void Close(object obj)
    {
        var mainWindow = System.Windows.Application.Current.MainWindow;

        mainWindow?.Close();
    }
}