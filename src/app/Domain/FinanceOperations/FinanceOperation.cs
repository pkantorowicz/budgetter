using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.Commons.Extensions;
using Budgetter.Domain.Commons.Services;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Exceptions;
using Budgetter.Domain.FinanceOperations.DomainEvents;
using Budgetter.Domain.Plans;
using Budgetter.Domain.Targets;

namespace Budgetter.Domain.FinanceOperations;

public class FinanceOperation : AggregateRoot
{
    private BudgetPlanId _budgetPlanId;
    private bool _isRemoved;
    private DateTime? _occurredAt;
    private Money _price;
    private Title _title;
    private FinanceOperationType _type;
    private bool _isAllocated;
    private TargetId _targetId;

    private FinanceOperation()
    {
    }

    public static FinanceOperation Do(
        BudgetPlanId budgetPlanId,
        Title title,
        Money price,
        string planCurrency,
        DateTime? occurredAt,
        FinanceOperationType type,
        Duration budgetPlanDuration,
        IExchangeRatesProvider exchangeRatesProvider)
    {
        var expense = new FinanceOperation();

        occurredAt ??= SystemClock.Now;

        if (price.Currency != planCurrency)
            price = price.ConvertCurrency(
                planCurrency,
                occurredAt,
                exchangeRatesProvider);

        if (!ValidatePrice(type, price.Value))
            throw new BusinessRuleDoesNotMatchedException(
                $"Price {price} must be negative when finance operation type is Expense " +
                "or must be positive when finance operation is Income");

        if (!budgetPlanDuration.IsInRange(occurredAt.Value))
            throw new BusinessRuleDoesNotMatchedException(
                "Finance operation independent of type must be in range of budget plan. " +
                $"Values: occurredAt: {occurredAt}, " +
                $"budgetPlanValidFrom: {budgetPlanDuration.ValidFrom}, " +
                $"budgetPlanValidTo: {budgetPlanDuration.ValidTo} ");

        var @event = new FinanceOperationCreatedDomainEvent(
            Guid.NewGuid(),
            budgetPlanId.Value,
            title.Value,
            price.Value,
            price.Currency,
            type.Value,
            occurredAt);

        expense.Apply(@event);
        expense.AddDomainEvent(@event);

        return expense;
    }

    public FinanceOperationSnapshot GetSnapshot()
    {
        return new FinanceOperationSnapshot(
            FinanceOperationId.New(Id),
            _budgetPlanId,
            _title,
            _price,
            _type,
            _occurredAt,
            _targetId,
            _isAllocated,
            _isRemoved);
    }

    public void ChangeAttributes(
        Title title,
        decimal price)
    {
        if (!ValidatePrice(_type, price))
            throw new BusinessRuleDoesNotMatchedException(
                $"Price {price} must be negative when finance operation type is Expense " +
                "or must be positive when finance operation is Income");

        var @event = new FinanceOperationAttributesChangedDomainEvent(
            Id,
            _budgetPlanId.Value,
            title.Value,
            price);

        Apply(@event);
        AddDomainEvent(@event);
    }

    public void MarkAsAssigned(Guid targetId)
    {
        if (_isAllocated)
            throw new BusinessRuleDoesNotMatchedException(
                $"Expense with id: {Id} is already assigned to target with id: {_targetId}.");

        if (_type == FinanceOperationType.Income)
            throw new BusinessRuleDoesNotMatchedException(
                $"Finance operation with id: {Id} is an income, so cannot be allocated.");
        
        var @event = new AssignationStatusChangedDomainEvent(
            Id,
            _budgetPlanId.Value,
            targetId,
            true);
        
        Apply(@event);
        AddDomainEvent(@event);
    }

    public void MarkAsUnAssigned()
    {
        if (!_isAllocated)
            throw new BusinessRuleDoesNotMatchedException(
                $"Expense with id: {Id} is not assigned to any target.");

        var @event = new AssignationStatusChangedDomainEvent(
            Id,
            _budgetPlanId.Value,
            default,
            false);
        
        Apply(@event);
        AddDomainEvent(@event);
    }
    
    public void MarkAsRemoved()
    {
        var @event = new FinanceOperationRemovedDomainEvent(
            Id,
            _budgetPlanId.Value,
            true);

        Apply(@event);
        AddDomainEvent(@event);
    }

    protected override void Apply(IDomainEvent @event)
    {
        When((dynamic)@event);
    }

    private void When(FinanceOperationCreatedDomainEvent @event)
    {
        Id = @event.FinanceOperationId;
        _budgetPlanId = BudgetPlanId.New(@event.BudgetPlanId);
        _title = Title.Entitle(@event.Title);

        _price = Money.Of(
            @event.Price,
            @event.Currency);

        _type = @event.Price > 0 ? FinanceOperationType.Income : FinanceOperationType.Expense;
        _occurredAt = @event.OccurredAt;
    }

    private void When(FinanceOperationAttributesChangedDomainEvent @event)
    {
        _title = Title.Entitle(@event.Title);

        _price = Money.Of(
            @event.Price,
            _price.Currency);
    }

    private void When(AssignationStatusChangedDomainEvent @event)
    {
        _targetId = TargetId.New(@event.TargetId);
        _isAllocated = @event.IsAllocated;
    }

    private void When(FinanceOperationRemovedDomainEvent @event)
    {
        _isRemoved = @event.IsRemoved;
    }

    private static bool ValidatePrice(
        FinanceOperationType financeOperationType,
        decimal price)
    {
        bool validationResult;

        if (financeOperationType == FinanceOperationType.Expense)
            validationResult = price < 0;
        else if (financeOperationType == FinanceOperationType.Income)
            validationResult = price > 0;
        else
            throw new ArgumentOutOfRangeException(nameof(financeOperationType), financeOperationType,
                "Unknown finance operation type.");

        return validationResult;
    }
}