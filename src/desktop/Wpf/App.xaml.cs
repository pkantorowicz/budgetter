using System;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using Budgetter.Wpf.Views;

namespace Budgetter.Wpf;

public partial class App
{
    private readonly Bootstrapper _bootstrapper;
    private readonly string _culture;

    public App()
    {
        _bootstrapper = new Bootstrapper();

        try
        {
            _culture = _bootstrapper.InitializeCultures();

            _bootstrapper.PreRun();
            _bootstrapper.Run();
        }
        catch (Exception ex)
        {
            _bootstrapper.Logger.Error(ex, "Program terminated unexpectedly ({ApplicationContext})!",
                AppContext.AppName);

            Shutdown(1);
        }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        var windowTitle =
            $"{AppContext.AppName}- v{AssemblyVersion.MajorMinorVersion}" +
            $" (built {AssemblyVersion.CreateDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)})";

        SetLanguageDictionary(_culture ?? "pl-PL");

        var shellViewModel = _bootstrapper.GetShellViewModel();

        var shellView = new ShellView
        {
            Title = windowTitle,
            DataContext = shellViewModel
        };

        shellView.Show();

        Current.MainWindow = shellView;
    }


    private void SetLanguageDictionary(string culture)
    {
        var languageDictionary = new ResourceDictionary
        {
            Source = culture switch
            {
                "pl-PL" => new Uri("..\\Resources\\Languages\\StringResources.pl-PL.xaml", UriKind.Relative),
                "en-GB" => new Uri("..\\Resources\\Languages\\StringResources.en-GB.xaml", UriKind.Relative),
                _ => throw new ArgumentOutOfRangeException(nameof(culture), culture, "Unknown culture.")
            }
        };

        Resources.MergedDictionaries.Add(languageDictionary);
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
    {
        _bootstrapper.Logger.Error("Unhandled exception caused app crash" + Environment.NewLine + ex);
    }

    private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs ex)
    {
        _bootstrapper.Logger.Error(ex.Exception, "Unhandled exception caused app crash");
    }
}