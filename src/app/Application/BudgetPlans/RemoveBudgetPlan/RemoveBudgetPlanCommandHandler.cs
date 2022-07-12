using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Plans;

namespace Budgetter.Application.BudgetPlans.RemoveBudgetPlan;

internal class RemoveBudgetPlanCommandHandler : CommandHandlerBase<RemoveBudgetPlanCommand>
{
    private readonly IAggregateStore _aggregateStore;

    public RemoveBudgetPlanCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
        _aggregateStore = aggregateStore;
    }

    public override async Task<ICommandResult> Handle(RemoveBudgetPlanCommand command,
        CancellationToken cancellationToken)
    {
        var budgetPlan = await AggregateStore.Load(BudgetPlanId.New(command.BudgetPlanId));

        budgetPlan.MarkAsRemoved();

        _aggregateStore.AppendChanges(budgetPlan);

        return CommandResult.Success();
    }
}