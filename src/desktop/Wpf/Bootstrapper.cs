using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Autofac;
using Budgetter.Infrastructure.Configuration;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.Events;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.Settings;
using Budgetter.Wpf.ViewModels;
using Budgetter.Wpf.ViewModels.Pages;
using Serilog;
using Serilog.Formatting.Compact;

namespace Budgetter.Wpf;

internal class Bootstrapper
{
    private readonly ISettingsProvider _settingsProvider;

    private IContainer _container;

    public Bootstrapper()
    {
        _settingsProvider = new SettingsProvider();
    }

    public ILogger Logger { get; private set; }

    public void PreRun()
    {
        InitializeLogger();
    }

    public void Run()
    {
        InitializeApp();
        InitializeDesktop();
    }

    public ShellViewModel GetShellViewModel()
    {
        return _container.Resolve<ShellViewModel>();
    }

    public string InitializeCultures()
    {
        var culture = _settingsProvider.GetCulture();

        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
            XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name)));

        return culture;
    }

    private void InitializeDesktop()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<PagesFacade>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterAssemblyTypes(typeof(App).Assembly)
            .Where(type => type.IsAssignableTo(typeof(IPageViewModel)))
            .ExternallyOwned()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(App).Assembly)
            .Where(type => type.Name.EndsWith("Controller"))
            .AsImplementedInterfaces()
            .SingleInstance();

        builder.RegisterAssemblyTypes(typeof(App).Assembly)
            .Where(type => type.Name.EndsWith("Mapper"))
            .AsImplementedInterfaces()
            .SingleInstance();

        builder.RegisterInstance(_settingsProvider)
            .As<ISettingsProvider>()
            .SingleInstance();

        builder.RegisterInstance(BudgetPlanContext.Default)
            .As<IBudgetPlanContext>()
            .SingleInstance();

        builder.RegisterType<ShellViewModel>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<EventAggregator>()
            .As<IEventAggregator>()
            .SingleInstance();

        builder.RegisterType<UiThread>()
            .As<IUiThread>()
            .SingleInstance();

        _container = builder.Build();
    }

    private void InitializeLogger()
    {
        const string logTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] [{App}] [{Version}] {Message:lj}{NewLine}{Exception}";

        Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Debug(outputTemplate: logTemplate)
            .WriteTo.Console(outputTemplate: logTemplate)
            .WriteTo.RollingFile(new CompactJsonFormatter(), "logs/logs")
            .CreateLogger();

        Logger = Logger.ForContext("Version",
            $"{AssemblyVersion.MajorMinorVersion} " +
            $"(built {AssemblyVersion.CreateDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)})");
    }

    private void InitializeApp()
    {
        BudgetterStartup.Initialize(
            _settingsProvider.GetDbSettings(),
            _settingsProvider.GetNbpApiSettings(),
            Logger);
    }
}