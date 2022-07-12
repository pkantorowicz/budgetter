using System;
using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Plans;
using Budgetter.Domain.Targets;

namespace Budgetter.Application.Targets.CreateTarget;

internal class CreateTargetCommandHandler : CommandHandlerBase<CreateTargetCommand, Guid>
{
    public CreateTargetCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
    }

    public override async Task<ICommandResult<Guid>> Handle(CreateTargetCommand command,
        CancellationToken cancellationToken)
    {
        var target = Target.CreateBudgetGoal(
            BudgetPlanId.New(command.BudgetPlanId),
            Title.Entitle(command.Title),
            Description.Describe(command.Description),
            Duration.Specify(command.ValidFrom, command.ValidTo),
            Duration.Specify(command.PlanValidFrom, command.PlanValidTo),
            Money.Of(command.MaxAmount, command.Currency));

        AggregateStore.AppendChanges(target);

        return await Task.FromResult(CommandResult<Guid>.Success(target.Id));
    }
}