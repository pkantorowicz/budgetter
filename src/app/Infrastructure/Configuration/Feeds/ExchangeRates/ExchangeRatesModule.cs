using Autofac;
using Budgetter.Application.Configuration.WebClients;
using Budgetter.Infrastructure.Feeds.Exchanges.Nbp;

namespace Budgetter.Infrastructure.Configuration.Feeds.ExchangeRates;

public class ExchangeRatesModule : Module
{
    private readonly NpbApiSettings _npbApiSettings;

    public ExchangeRatesModule(NpbApiSettings npbApiSettings)
    {
        _npbApiSettings = npbApiSettings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_npbApiSettings)
            .SingleInstance();

        builder.RegisterType<NbpExchangeRatesClient>()
            .As<IExchangeRatesClient>()
            .InstancePerLifetimeScope();
    }
}