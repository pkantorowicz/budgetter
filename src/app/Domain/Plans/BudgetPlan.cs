using System;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Exceptions;
using Budgetter.Domain.Plans.DomainEvents;

namespace Budgetter.Domain.Plans;

public class BudgetPlan : AggregateRoot
{
    private string _currency;
    private Duration _duration;
    private bool _isRemoved;
    private Title _title;

    private BudgetPlan()
    {
    }

    public static BudgetPlan NewBudgetPlan(
        Title title,
        string currency,
        Duration duration,
        int maxDaysCount = 0)
    {
        if (maxDaysCount > 0 && duration.DaysRangeCount() > maxDaysCount)
            throw new BusinessRuleDoesNotMatchedException(
                $"Business plan can not be created for longer range than {maxDaysCount} days.");

        var budgetPlan = new BudgetPlan();

        var @event = new BudgetPlanCreatedDomainEvent(
            Guid.NewGuid(),
            title.Value,
            currency,
            duration.ValidFrom,
            duration.ValidTo);

        budgetPlan.Apply(@event);
        budgetPlan.AddDomainEvent(@event);

        return budgetPlan;
    }

    public void ChangeAttributes(
        BudgetPlanId budgetPlanId,
        Title title,
        string currency,
        Duration duration,
        int maxDaysCount)
    {
        if (duration.DaysRangeCount() > maxDaysCount)
            throw new BusinessRuleDoesNotMatchedException(
                $"Business plan can not be modified for longer range than {maxDaysCount} days.");

        var @event = new BudgetPlanAttributesChangedDomainEvent(
            budgetPlanId.Value,
            title.Value,
            currency,
            duration.ValidFrom,
            duration.ValidTo);

        Apply(@event);
        AddDomainEvent(@event);
    }

    public void MarkAsRemoved()
    {
        var @event = new BudgetPlanRemovedDomainEvent(
            Id,
            true);

        Apply(@event);
        AddDomainEvent(@event);
    }

    public BudgetPlanSnapshot BudgetPlanSnapshot()
    {
        return new BudgetPlanSnapshot(
            Id,
            _currency,
            _duration,
            _title,
            _isRemoved);
    }

    protected override void Apply(IDomainEvent @event)
    {
        When((dynamic)@event);
    }

    private void When(BudgetPlanCreatedDomainEvent @event)
    {
        Update(
            @event.BudgetPlanId,
            @event.Title,
            @event.Currency,
            @event.ValidFrom,
            @event.ValidTo);
    }

    private void When(BudgetPlanAttributesChangedDomainEvent @event)
    {
        Update(
            @event.BudgetPlanId,
            @event.Title,
            @event.Currency,
            @event.ValidFrom,
            @event.ValidTo);
    }

    private void When(BudgetPlanRemovedDomainEvent @event)
    {
        _isRemoved = @event.IsRemoved;
    }

    private void Update(
        Guid id,
        string title,
        string currency,
        DateTime validFrom,
        DateTime validTo)
    {
        Id = id;
        _title = Title.Entitle(title);
        _currency = currency;
        _duration = Duration.Specify(validFrom, validTo);
    }
}