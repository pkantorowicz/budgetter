using System;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.Application.Commons.ReadModel;
using Budgetter.Application.Configuration;
using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Budgetter.BuildingBlocks.Application.Exceptions;
using Budgetter.BuildingBlocks.Application.Notifications;
using Budgetter.BuildingBlocks.Application.Projections;
using Budgetter.BuildingBlocks.Domain.DomainEvents;
using Budgetter.Domain.FinanceOperations.DomainEvents;

namespace Budgetter.Application.FinanceOperations.Projections;

public class FinanceOperationsProjector : ProjectorBase, IProjector
{
    private readonly IFinanceOperationReadModelAccessor _financeOperationReadModelAccessor;
    private readonly IMapper _mapper;

    public FinanceOperationsProjector(
        IFinanceOperationReadModelAccessor financeOperationReadModelAccessor,
        INotificationsHub notificationsHub,
        IMapper mapper) : base(notificationsHub)
    {
        _financeOperationReadModelAccessor = financeOperationReadModelAccessor;
        _mapper = mapper;
    }

    public async Task Project(IDomainEvent @event)
    {
        await When((dynamic)@event);
    }

    private async Task When(FinanceOperationCreatedDomainEvent @event)
    {
        try
        {
            var financeOperationReadModel =
                _mapper.Map<FinanceOperationCreatedDomainEvent, FinanceOperationReadModel>(@event);

            await _financeOperationReadModelAccessor.AddAsync(financeOperationReadModel);
            await NotifyAboutSuccess(financeOperationReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<FinanceOperationReadModel, FinanceOperationCreatedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task When(FinanceOperationAttributesChangedDomainEvent @event)
    {
        try
        {
            var financeOperationReadModel = await GetFinanceOperation(
                @event.FinanceOperationId,
                @event.BudgetPlanId);

            financeOperationReadModel.Title = @event.Title;
            financeOperationReadModel.Price = new PriceReadModel
            {
                Value = @event.Price,
                Currency = financeOperationReadModel.Price.Currency
            };

            await _financeOperationReadModelAccessor.UpdateAsync(financeOperationReadModel);
            await NotifyAboutSuccess(financeOperationReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<FinanceOperationReadModel, FinanceOperationAttributesChangedDomainEvent>(
                exception,
                @event);

            throw;
        }
    }

    private async Task When(AssignationStatusChangedDomainEvent @event)
    {
        try
        {
            var financeOperationReadModel = await GetFinanceOperation(
                @event.FinanceOperationId,
                @event.BudgetPlanId);

            financeOperationReadModel.TargetId = @event.TargetId;
            financeOperationReadModel.IsAllocated = @event.IsAllocated;

            await _financeOperationReadModelAccessor.UpdateAsync(financeOperationReadModel);
            await NotifyAboutSuccess(financeOperationReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<FinanceOperationReadModel, AssignationStatusChangedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task When(FinanceOperationRemovedDomainEvent @event)
    {
        try
        {
            var financeOperationReadModel = await GetFinanceOperation(
                @event.FinanceOperationId,
                @event.BudgetPlanId);

            financeOperationReadModel.IsRemoved = @event.IsRemoved;

            await _financeOperationReadModelAccessor.UpdateAsync(financeOperationReadModel);
            await NotifyAboutSuccess(financeOperationReadModel, @event);
        }
        catch (Exception exception)
        {
            await NotifyAboutFailed<FinanceOperationReadModel, FinanceOperationRemovedDomainEvent>(exception,
                @event);

            throw;
        }
    }

    private async Task<FinanceOperationReadModel> GetFinanceOperation(
        Guid financeOperationId,
        Guid budgetPlanId)
    {
        var financeOperationReadModel =
            await _financeOperationReadModelAccessor.GetByIdAsync(
                financeOperationId,
                budgetPlanId);

        if (financeOperationReadModel is null)
            throw new RecordNotFoundException(
                "Unable to mutate state of read model," +
                $" because finance operation with id: {financeOperationId} doesn't exists.");

        return financeOperationReadModel;
    }
}