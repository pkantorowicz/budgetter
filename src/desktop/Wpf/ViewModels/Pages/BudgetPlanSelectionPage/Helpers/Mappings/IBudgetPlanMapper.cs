using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Budgetter.Application.BudgetPlans.Dtos;
using Budgetter.Wpf.Controllers.Interfaces;
using Budgetter.Wpf.Settings;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Controls;
using Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Events;

namespace Budgetter.Wpf.ViewModels.Pages.BudgetPlanSelectionPage.Helpers.Mappings;

internal interface IBudgetPlanMapper
{
    ObservableCollection<BudgetPlanViewModel> MapBudgetPlanDtosToBudgetPlanViewModels(
        IEnumerable<BudgetPlanDto> budgetPlanDtos,
        IBudgetPlanController budgetPlanController,
        ISettingsProvider settingsProvider,
        IEnumerable<string> availableCurrencies,
        EventHandler<GoBackEventArgs> goBackEventHandler);
}