using System.Collections.Generic;
using System.Reflection;
using Budgetter.Application.FinanceOperations.Projections;
using Budgetter.Domain.Exceptions;

namespace Budgetter.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly ApplicationAssembly = typeof(FinanceOperationsProjector).Assembly;
    public static readonly Assembly DomainAssembly = typeof(BusinessRuleDoesNotMatchedException).Assembly;
    public static readonly Assembly InfrastructureAssembly = typeof(BudgetterStartup).Assembly;

    public static List<Assembly> GetAssemblies()
    {
        var assemblies = new List<Assembly>
        {
            ApplicationAssembly,
            DomainAssembly,
            InfrastructureAssembly
        };

        return assemblies;
    }
}