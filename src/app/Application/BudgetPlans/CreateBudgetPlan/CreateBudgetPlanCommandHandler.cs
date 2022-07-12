using System;
using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Plans;

namespace Budgetter.Application.BudgetPlans.CreateBudgetPlan;

internal class CreateBudgetPlanCommandHandler : CommandHandlerBase<CreateBudgetPlanCommand, Guid>
{
    public CreateBudgetPlanCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
    }

    public override async Task<ICommandResult<Guid>> Handle(CreateBudgetPlanCommand command,
        CancellationToken cancellationToken)
    {
        var budgetPlan = BudgetPlan.NewBudgetPlan(
            Title.Entitle(command.Title),
            command.Currency,
            Duration.Specify(
                command.ValidFrom,
                command.ValidTo),
            command.MaxDaysCount);

        AggregateStore.AppendChanges(budgetPlan);

        return await Task.FromResult(CommandResult<Guid>.Success(budgetPlan.Id));
    }
}