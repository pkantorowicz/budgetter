using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.Application.Commons.ReadModel;
using Budgetter.Application.Configuration;
using Budgetter.Application.Targets.Projections.ReadModel;
using Budgetter.BuildingBlocks.Application.Exceptions;
using Budgetter.BuildingBlocks.Application.Notifications;
using Budgetter.BuildingBlocks.Application.Projections;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.Domain.Targets.DomainEvents;

namespace Budgetter.Application.Targets.Projections;

public class TargetProjector : ProjectorBase, IProjector
{
    private readonly IMapper _mapper;
    private readonly ITargetItemReadModelAccessor _targetItemReadModelAccessor;
    private readonly ITargetReadModelAccessor _targetReadModelAccessor;

    public TargetProjector(
        IMapper mapper,
        INotificationsHub notificationsHub,
        ITargetReadModelAccessor targetReadModelAccessor,
        ITargetItemReadModelAccessor targetItemReadModelAccessor) : base(notificationsHub)
    {
        _mapper = mapper;
        _targetReadModelAccessor = targetReadModelAccessor;
        _targetItemReadModelAccessor = targetItemReadModelAccessor;
    }

    public async Task Project(IDomainEvent @event)
    {
        await When((dynamic)@event);
    }

    private async Task When(ExpenseAllocatedDomainEvent @event)
    {
        try
        {
            var targetItemReadModel = _mapper.Map<ExpenseAllocatedDomainEvent, TargetItemReadModel>(@event);

            await _targetItemReadModelAccessor.AddAsync(targetItemReadModel);
            await NotifyAboutSuccess(targetItemReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<TargetItemReadModel, ExpenseAllocatedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task When(ExpenseDeallocatedDomainEvent @event)
    {
        try
        {
            var targetReadModel = await GetTargetReadModel(
                @event.TargetId,
                @event.BudgetPlanId);

            var targetItemReadModel = targetReadModel.TargetItems
                .FirstOrDefault(ti => ti.Id == @event.ExpenseId);

            targetReadModel.TargetItems.Remove(targetItemReadModel);

            await _targetReadModelAccessor.UpdateAsync(targetReadModel);
            await NotifyAboutSuccess(targetReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<TargetReadModel, ExpenseDeallocatedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task When(ExpensesClearedDomainEvent @event)
    {
        try
        {
            var targetReadModel = await GetTargetReadModel(
                @event.TargetId,
                @event.BudgetPlanId);

            targetReadModel.TargetItems.Clear();

            await _targetReadModelAccessor.UpdateAsync(targetReadModel);
            await NotifyAboutSuccess(targetReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<TargetReadModel, ExpensesClearedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task When(TargetAttributesChangedDomainEvent @event)
    {
        try
        {
            var targetReadModel = await GetTargetReadModel(
                @event.TargetId,
                @event.BudgetPlanId);

            targetReadModel.Id = @event.TargetId;
            targetReadModel.Description = @event.Description;
            targetReadModel.Title = @event.Title;

            targetReadModel.MaxAmount = new PriceReadModel
            {
                Value = @event.MaxAmount,
                Currency = @event.Currency
            };

            targetReadModel.ValidFrom = @event.ValidFrom;
            targetReadModel.ValidTo = @event.ValidTo;

            await _targetReadModelAccessor.UpdateAsync(targetReadModel);
            await NotifyAboutSuccess(targetReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<TargetReadModel, TargetAttributesChangedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task When(TargetCreatedDomainEvent @event)
    {
        try
        {
            var targetReadModel = _mapper.Map<TargetCreatedDomainEvent, TargetReadModel>(@event);

            await _targetReadModelAccessor.AddAsync(targetReadModel);
            await NotifyAboutSuccess(targetReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<TargetReadModel, TargetCreatedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task When(TargetRemovedDomainEvent @event)
    {
        try
        {
            var targetReadModel = await GetTargetReadModel(
                @event.TargetId,
                @event.BudgetPlanId);

            targetReadModel.IsRemoved = @event.IsRemoved;

            await _targetReadModelAccessor.UpdateAsync(targetReadModel);
            await NotifyAboutSuccess(targetReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<TargetReadModel, TargetRemovedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task<TargetReadModel> GetTargetReadModel(Guid targetId, Guid budgetPlanId)
    {
        var targetReadModel = await _targetReadModelAccessor
            .GetByIdAsync(targetId, budgetPlanId);

        if (targetReadModel is null)
            throw new RecordNotFoundException(
                $"Unable to update read model, because target with id: {targetId} doesn't exists.");

        return targetReadModel;
    }
}