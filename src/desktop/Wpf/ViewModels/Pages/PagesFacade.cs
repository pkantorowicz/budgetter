using System;
using System.Threading.Tasks;
using Autofac;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Helpers;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanningPage;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage;
using Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage;

namespace Budgetter.Wpf.ViewModels.Pages;

internal class PagesFacade
{
    private readonly ILifetimeScope _lifetimeScope;

    public PagesFacade(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public async Task<IPageViewModel> CreatePageViewModel(NavigationDirection navigationDirection)
    {
        try
        {
            await using var pagesScope = _lifetimeScope.BeginLifetimeScope();

            return navigationDirection switch
            {
                NavigationDirection.BudgetPlanSelectionPage =>
                    pagesScope.Resolve<BudgetPlanSelectionPageViewModel>(),
                NavigationDirection.FinanceOperationsPage => pagesScope.Resolve<FinanceOperationsPageViewModel>(),
                NavigationDirection.BudgetPlanningPage => pagesScope.Resolve<BudgetPlanningPageViewModel>(),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(navigationDirection),
                    navigationDirection,
                    "Unknown navigation direction.")
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}