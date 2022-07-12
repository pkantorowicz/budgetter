using Autofac;
using AutoMapper;
using Budgetter.Infrastructure.Configuration.Mappings.Profiles;

namespace Budgetter.Infrastructure.Configuration.Mappings;

public class MappingsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var mapper = ConfigureMapper();

        builder.RegisterInstance(mapper)
            .As<IMapper>()
            .SingleInstance();
    }

    private static IMapper ConfigureMapper()
    {
        var mapperConfiguration = new MapperConfiguration(
            configuration =>
            {
                configuration.AddProfile(new BudgetPlansProfile());
                configuration.AddProfile(new FinanceOperationsProfile());
                configuration.AddProfile(new TargetsProfile());
                configuration.AddProfile(new ExchangeRatesProfile());
            });

        mapperConfiguration.AssertConfigurationIsValid();

        return mapperConfiguration.CreateMapper();
    }
}