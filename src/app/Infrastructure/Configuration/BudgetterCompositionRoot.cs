using Autofac;

namespace Budgetter.Infrastructure.Configuration;

public class BudgetterCompositionRoot
{
    private static IContainer _container;

    public static void SetContainer(IContainer container)
    {
        _container = container;
    }

    public static ILifetimeScope BeginLifetimeScope()
    {
        return _container.BeginLifetimeScope();
    }

    public static T Resolve<T>()
    {
        return _container.Resolve<T>();
    }
}