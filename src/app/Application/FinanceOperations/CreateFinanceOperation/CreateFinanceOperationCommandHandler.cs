using System;
using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Commons.Services;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.FinanceOperations;
using Budgetter.Domain.Plans;

namespace Budgetter.Application.FinanceOperations.CreateFinanceOperation;

internal class CreateFinanceOperationCommandHandler : CommandHandlerBase<CreateFinanceOperationCommand, Guid>
{
    private readonly IExchangeRatesProvider _exchangeRatesProvider;

    public CreateFinanceOperationCommandHandler(
        IAggregateStore aggregateStore,
        IExchangeRatesProvider exchangeRatesProvider) : base(aggregateStore)
    {
        _exchangeRatesProvider = exchangeRatesProvider;
    }

    public override async Task<ICommandResult<Guid>> Handle(CreateFinanceOperationCommand command,
        CancellationToken cancellationToken)
    {
        var financeOperation = FinanceOperation.Do(
            BudgetPlanId.New(command.BudgetPlanId),
            Title.Entitle(command.Title),
            Money.Of(command.Price, command.Currency),
            command.PlanCurrency,
            command.OccurredAt,
            command.Price > 0 ? FinanceOperationType.Income : FinanceOperationType.Expense,
            Duration.Specify(command.PlanValidFrom, command.PlanValidTo),
            _exchangeRatesProvider);

        AggregateStore.AppendChanges(financeOperation);

        return await Task.FromResult(CommandResult<Guid>.Success(financeOperation.Id));
    }
}