using System;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Budgetter.Application.Configuration;
using Budgetter.BuildingBlocks.Application.Exceptions;
using Budgetter.BuildingBlocks.Application.Notifications;
using Budgetter.BuildingBlocks.Application.Projections;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.Domain.Plans.DomainEvents;

namespace Budgetter.Application.BudgetPlans.Projections;

public class BudgetPlanProjector : ProjectorBase, IProjector
{
    private readonly IBudgetPlanReadModelAccessor _budgetPlanReadModelAccessor;
    private readonly IMapper _mapper;

    public BudgetPlanProjector(
        IBudgetPlanReadModelAccessor budgetPlanReadModelAccessor,
        INotificationsHub notificationsHub,
        IMapper mapper) : base(notificationsHub)
    {
        _budgetPlanReadModelAccessor = budgetPlanReadModelAccessor;
        _mapper = mapper;
    }

    public async Task Project(IDomainEvent @event)
    {
        await When((dynamic)@event);
    }

    private async Task When(BudgetPlanCreatedDomainEvent @event)
    {
        try
        {
            var budgetPlanReadModel = _mapper.Map<BudgetPlanCreatedDomainEvent, BudgetPlanReadModel>(@event);

            await _budgetPlanReadModelAccessor.AddAsync(budgetPlanReadModel);
            await NotifyAboutSuccess(budgetPlanReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<BudgetPlanReadModel, BudgetPlanCreatedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task When(BudgetPlanAttributesChangedDomainEvent @event)
    {
        try
        {
            var budgetPlanReadModel = await GetBudgetPlan(@event.BudgetPlanId);

            budgetPlanReadModel.Title = @event.Title;
            budgetPlanReadModel.Currency = @event.Currency;
            budgetPlanReadModel.ValidFrom = @event.ValidFrom;
            budgetPlanReadModel.ValidTo = @event.ValidTo;

            await _budgetPlanReadModelAccessor.UpdateAsync(budgetPlanReadModel);
            await NotifyAboutSuccess(budgetPlanReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<BudgetPlanReadModel, BudgetPlanAttributesChangedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task When(BudgetPlanRemovedDomainEvent @event)
    {
        try
        {
            var budgetPlanReadModel = await GetBudgetPlan(@event.BudgetPlanId);

            budgetPlanReadModel.IsRemoved = @event.IsRemoved;

            await _budgetPlanReadModelAccessor.UpdateAsync(budgetPlanReadModel);
            await NotifyAboutSuccess(budgetPlanReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<BudgetPlanReadModel, BudgetPlanRemovedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task<BudgetPlanReadModel> GetBudgetPlan(Guid budgetPlanId)
    {
        var budgetPlanReadModel =
            await _budgetPlanReadModelAccessor.GetByIdAsync(
                budgetPlanId);

        if (budgetPlanReadModel is null)
            throw new RecordNotFoundException(
                "Unable to update read model," +
                $" because budget plan with id: {budgetPlanId} doesn't exists.");

        return budgetPlanReadModel;
    }
}