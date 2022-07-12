using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Plans;

namespace Budgetter.Application.BudgetPlans.ChangeBudgetPlanAttributes;

internal class ChangeBudgetPlanAttributesCommandHandler : CommandHandlerBase<ChangeBudgetPlanAttributesCommand>
{
    public ChangeBudgetPlanAttributesCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
    }

    public override async Task<ICommandResult> Handle(ChangeBudgetPlanAttributesCommand command,
        CancellationToken cancellationToken)
    {
        var budgetPlanId = BudgetPlanId.New(command.BudgetPlanId);
        var budgetPlan = await AggregateStore.Load(budgetPlanId);

        budgetPlan.ChangeAttributes(
            budgetPlanId,
            Title.Entitle(command.Title),
            command.Currency,
            Duration.Specify(
                command.ValidFrom,
                command.ValidTo),
            command.MaxDaysCount);

        AggregateStore.AppendChanges(budgetPlan);

        return CommandResult.Success();
    }
}