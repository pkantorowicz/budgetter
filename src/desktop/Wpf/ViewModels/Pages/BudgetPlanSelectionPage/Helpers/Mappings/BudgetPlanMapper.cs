using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Budgetter.Application.BudgetPlans.Dtos;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Settings;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Events;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.Mappings;

internal class BudgetPlanMapper : IBudgetPlanMapper
{
    public ObservableCollection<BudgetPlanViewModel> MapBudgetPlanDtosToBudgetPlanViewModels(
        IEnumerable<BudgetPlanDto> budgetPlanDtos,
        IBudgetPlanController budgetPlanController,
        ISettingsProvider settingsProvider,
        IEnumerable<string> availableCurrencies,
        EventHandler<GoBackEventArgs> goBackEventHandler)
    {
        return new ObservableCollection<BudgetPlanViewModel>(
            budgetPlanDtos.Select(bp =>
            {
                var budgetPlan = new BudgetPlanViewModel(
                    budgetPlanController,
                    settingsProvider,
                    availableCurrencies)
                {
                    BudgetPlanId = bp.Id,
                    Title = bp.Title,
                    Currency = bp.Currency,
                    ValidFrom = bp.ValidFrom,
                    ValidTo = bp.ValidTo
                };

                budgetPlan.GoBack += goBackEventHandler;

                return budgetPlan;
            }));
    }
}