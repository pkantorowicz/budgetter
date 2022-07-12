using System;
using System.Threading;
using System.Threading.Tasks;
using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Budgetter.Application.Targets.Projections.ReadModel;
using Budgetter.BuildingBlocks.Application.Contacts.Notifications;
using Budgetter.BuildingBlocks.Application.Notifications;
using Budgetter.Infrastructure.Configuration;
using Budgetter.Wpf.Framework;
using Budgetter.Wpf.Framework.Events;
using Budgetter.Wpf.Framework.ViewModels;
using Budgetter.Wpf.ViewModels.Controls;
using Budgetter.Wpf.ViewModels.Helpers;
using Budgetter.Wpf.ViewModels.Pages;

namespace Budgetter.Wpf.ViewModels;

internal class ShellViewModel : ViewModelBase, IHandle<object>
{
    private readonly IEventAggregator _eventAggregator;
    private readonly INotificationsHub _notificationsHub;
    private readonly PagesFacade _pagesFacade;
    private readonly IUiThread _uiThread;

    private IPageViewModel _currentPage;

    public ShellViewModel(
        IEventAggregator eventAggregator,
        IUiThread uiThread,
        PagesFacade pagesFacade)
    {
        _eventAggregator = eventAggregator;
        _uiThread = uiThread;
        _pagesFacade = pagesFacade;

        _notificationsHub = BudgetterCompositionRoot.Resolve<INotificationsHub>();
        _notificationsHub.Subscribe(HandleUiNotifications);

        _eventAggregator.SubscribeOnUIThread(this);

        TitleBarViewModel = new TitleBarViewModel();
        UiNotificationViewModel = new UiNotificationViewModel();

        ShowStartPage();
    }

    public TitleBarViewModel TitleBarViewModel { get; }
    public UiNotificationViewModel UiNotificationViewModel { get; }

    public IPageViewModel CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;

            OnPropertyChanged(nameof(CurrentPage));
        }
    }

    public async Task HandleAsync(object message, CancellationToken cancellationToken)
    {
        if (message is NavigationDirection navigationDirection)
        {
            async void NavigateAction()
            {
                if (_currentPage is not null)
                    await CurrentPage.OnDeactivateAsync();

                CurrentPage = await _pagesFacade.CreatePageViewModel(navigationDirection);

                if (_currentPage is not null)
                    await _currentPage.OnActivateAsync();
            }

            await _uiThread.InvokeAsync(NavigateAction);
        }

        await Task.CompletedTask;
    }

    public override void Dispose()
    {
        _notificationsHub.Unsubscribe(HandleUiNotifications);
    }

    private void ShowStartPage()
    {
        Task.Run(async () =>
            await _eventAggregator.PublishOnUIThreadAsync(NavigationDirection.BudgetPlanSelectionPage));
    }

    private void HandleUiNotifications(object sender, object args)
    {
        switch (args)
        {
            case UiNotification<BudgetPlanReadModel> budgetPlanNotification:
                UiNotificationViewModel.ShowNotification(
                    budgetPlanNotification.IsSuccess,
                    budgetPlanNotification.OperationName,
                    budgetPlanNotification.IsSuccess
                        ? budgetPlanNotification.ChangedObject.Title
                        : budgetPlanNotification.GetErrors());
                break;
            case UiNotification<FinanceOperationReadModel> financeOperationNotification:
                UiNotificationViewModel.ShowNotification(
                    financeOperationNotification.IsSuccess,
                    financeOperationNotification.OperationName,
                    financeOperationNotification.IsSuccess
                        ? financeOperationNotification.ChangedObject.Title
                        : financeOperationNotification.GetErrors());
                break;
            case UiNotification<TargetItemReadModel> targetItemNotification:
                UiNotificationViewModel.ShowNotification(
                    targetItemNotification.IsSuccess,
                    targetItemNotification.OperationName,
                    targetItemNotification.IsSuccess
                        ? targetItemNotification.ChangedObject.Title
                        : targetItemNotification.GetErrors());
                break;
            case UiNotification<TargetReadModel> targetNotification:
                UiNotificationViewModel.ShowNotification(
                    targetNotification.IsSuccess,
                    targetNotification.OperationName,
                    targetNotification.IsSuccess
                        ? targetNotification.ChangedObject.Title
                        : targetNotification.GetErrors());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(args), args, "Unknown UiNotification type.");
        }
    }
}