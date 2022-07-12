using Autofac;
using Budgetter.Infrastructure.Configuration.Feeds.ExchangeRates;
using Budgetter.Infrastructure.Feeds.Exchanges.Nbp;

namespace Budgetter.Infrastructure.Configuration.Feeds;

public class FeedsModule : Module
{
    private readonly NpbApiSettings _npbApiSettings;

    public FeedsModule(NpbApiSettings npbApiSettings)
    {
        _npbApiSettings = npbApiSettings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule(new ExchangeRatesModule(_npbApiSettings));
    }
}