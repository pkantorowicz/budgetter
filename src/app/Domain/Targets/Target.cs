using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Exceptions;
using Budgetter.Domain.FinanceOperations;
using Budgetter.Domain.Plans;
using Budgetter.Domain.Targets.DomainEvents;
using Budgetter.Domain.Targets.Services;
using Budgetter.Domain.Targets.Services.Interfaces;

namespace Budgetter.Domain.Targets;

public class Target : AggregateRoot
{
    private readonly ExpensesList _expensesList;
    private readonly ITargetDurationAvailabilityInspector _targetDurationAvailabilityInspector;

    private TargetBalance _balance;
    private BudgetPlanId _budgetPlanId;
    private Description _description;
    private Duration _duration;
    private bool _isRemoved;
    private Money _maxAmount;
    private Status _status;
    private Title _title;

    private Target()
    {
        _expensesList = ExpensesList.Create();
        _targetDurationAvailabilityInspector = new TargetDurationAvailabilityInspector();
    }

    public static Target CreateBudgetGoal(
        BudgetPlanId budgetPlanId,
        Title title,
        Description description,
        Duration duration,
        Duration budgetPlanDuration,
        Money maxAmount)
    {
        var target = new Target();

        target.ValidateTargetBeforeModifiedState(
            duration,
            budgetPlanDuration);

        var @event = new TargetCreatedDomainEvent(
            Guid.NewGuid(),
            budgetPlanId.Value,
            title.Value,
            description.Value,
            duration.ValidFrom,
            duration.ValidTo,
            maxAmount.Value,
            maxAmount.Currency);

        target.Apply(@event);
        target.AddDomainEvent(@event);

        return target;
    }

    public TargetSnapshot GetSnapshot()
    {
        return new TargetSnapshot(
            TargetId.New(Id),
            _budgetPlanId,
            _title,
            _description,
            _maxAmount,
            _duration,
            _status,
            _balance,
            _expensesList.GetItems(),
            _isRemoved);
    }

    public void ChangeAttributes(
        Title title,
        Description description,
        Duration duration,
        Duration budgetPlanDuration,
        Money maxAmount)
    {
        ValidateTargetBeforeModifiedState(
            duration,
            budgetPlanDuration);

        var @event = new TargetAttributesChangedDomainEvent(
            Id,
            _budgetPlanId.Value,
            title.Value,
            description.Value,
            duration.ValidFrom,
            duration.ValidTo,
            maxAmount.Value,
            maxAmount.Currency);

        Apply(@event);
        AddDomainEvent(@event);
    }

    public void AllocateExpense(
        FinanceOperationId financeOperationId,
        Title expenseTitle,
        Money expenseUnitPrice,
        DateTime expenseOccurredAt,
        IFinanceOperationExistenceChecker financeOperationExistenceChecker)
    {
        var exists = financeOperationExistenceChecker
            .ExistsAsync(financeOperationId.Value, _budgetPlanId.Value)
            .GetAwaiter()
            .GetResult();

        if (!exists)
            throw new BusinessRuleDoesNotMatchedException(
                $"Finance operation with id: {financeOperationId} for " +
                $"budget plan with id: {_budgetPlanId.Value} doesn't exists in system.");

        if (_targetDurationAvailabilityInspector.IsTargetExpired(_duration))
            throw new BusinessRuleDoesNotMatchedException(
                "Unable to allocate expense into closed target.");

        if (!_duration.IsInRange(expenseOccurredAt))
            throw new BusinessRuleDoesNotMatchedException(
                "Only expenses which was doing in target range" +
                $" {_duration.ValidFrom}-{_duration.ValidTo} could be allocated. " +
                $"This expense occurred at: {expenseOccurredAt}");

        var @event = new ExpenseAllocatedDomainEvent(
            Id,
            _budgetPlanId.Value,
            financeOperationId.Value,
            expenseTitle.Value,
            expenseUnitPrice.Value,
            expenseUnitPrice.Currency,
            expenseOccurredAt);

        Apply(@event);
        AddDomainEvent(@event);
    }

    public void DeallocateExpense(FinanceOperationId financeOperationId)
    {
        if (_targetDurationAvailabilityInspector.IsTargetExpired(_duration))
            throw new BusinessRuleDoesNotMatchedException(
                "Unable to deallocate expense from closed target.");

        var @event = new ExpenseDeallocatedDomainEvent(
            Id,
            _budgetPlanId.Value,
            financeOperationId.Value);

        Apply(@event);
        AddDomainEvent(@event);
    }

    public void ClearExpenses()
    {
        if (_targetDurationAvailabilityInspector.IsTargetExpired(_duration))
            throw new BusinessRuleDoesNotMatchedException(
                "Unable to clear expenses from closed target.");

        var @event = new ExpensesClearedDomainEvent(
            Id,
            _budgetPlanId.Value);

        Apply(@event);
        AddDomainEvent(@event);
    }

    public void MarkAsRemoved()
    {
        var @event = new TargetRemovedDomainEvent(
            _budgetPlanId.Value,
            Id,
            true);

        Apply(@event);
        AddDomainEvent(@event);
    }

    private void ValidateTargetBeforeModifiedState(
        Duration targetDuration,
        Duration budgetPlanDuration)
    {
        if (!_targetDurationAvailabilityInspector.InspectIsTargetDurationMatchedPlanDuration(targetDuration,
                budgetPlanDuration))
            throw new BusinessRuleDoesNotMatchedException(
                $"Unable to or create target, because dates range: {targetDuration.ValidFrom}-{targetDuration.ValidTo}" +
                $" doesn't match plan dates range: {budgetPlanDuration.ValidFrom}-{budgetPlanDuration.ValidTo}");

        if (_targetDurationAvailabilityInspector.IsTargetExpired(targetDuration))
            throw new BusinessRuleDoesNotMatchedException(
                "Target status cannot be modified or created, because it's expired." +
                $" Target - ValidTo: {targetDuration.ValidTo} is lower than actual date.");
    }

    private Status PredicateStatus()
    {
        return Status.PredicateStatus(_duration);
    }

    private TargetBalance CalculateBalance()
    {
        return TargetBalance.CalculateBalance(
            _expensesList.GetTotalAmount().Value,
            _maxAmount.Value);
    }

    protected override void Apply(IDomainEvent @event)
    {
        When((dynamic)@event);
    }

    private void When(TargetCreatedDomainEvent @event)
    {
        Id = @event.TargetId;
        _budgetPlanId = BudgetPlanId.New(@event.BudgetPlanId);
        _title = Title.Entitle(@event.Title);
        _description = Description.Describe(@event.Description);
        _duration = Duration.Specify(@event.ValidFrom, @event.ValidTo);
        _maxAmount = Money.Of(@event.MaxAmount, @event.Currency);

        StateChanged();
    }

    private void When(TargetAttributesChangedDomainEvent @event)
    {
        _title = Title.Entitle(@event.Title);
        _description = Description.Describe(@event.Description);
        _duration = Duration.Specify(@event.ValidFrom, @event.ValidTo);
        _maxAmount = Money.Of(@event.MaxAmount, @event.Currency);

        StateChanged();
    }

    private void When(ExpenseAllocatedDomainEvent @event)
    {
        var targetItem = TargetItem.FromExpense(
            FinanceOperationId.New(@event.ExpenseId),
            BudgetPlanId.New(@event.BudgetPlanId),
            Title.Entitle(@event.Title),
            Money.Of(@event.UnitPrice, @event.Currency),
            @event.OccurredAt);

        _expensesList.Add(targetItem);

        StateChanged();
    }

    private void When(ExpenseDeallocatedDomainEvent @event)
    {
        _expensesList.Remove(TargetItemId.FromExpenseId(
            FinanceOperationId.New(@event.ExpenseId)));

        StateChanged();
    }

    private void When(ExpensesClearedDomainEvent @event)
    {
        _expensesList.Clear();

        StateChanged();
    }

    private void When(TargetRemovedDomainEvent @event)
    {
        _isRemoved = @event.IsRemoved;

        foreach (var expense in _expensesList.GetItems())
            DeallocateExpense(
                FinanceOperationId.New(expense.Id.Value));

        StateChanged();
    }

    private void StateChanged()
    {
        _status = PredicateStatus();
        _balance = CalculateBalance();
    }
}