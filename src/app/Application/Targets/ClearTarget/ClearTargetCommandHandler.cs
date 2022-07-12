using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Targets;

namespace Budgetter.Application.Targets.ClearTarget;

internal class ClearTargetCommandHandler : CommandHandlerBase<ClearTargetCommand>
{
    public ClearTargetCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
    }

    public override async Task<ICommandResult> Handle(ClearTargetCommand command,
        CancellationToken cancellationToken)
    {
        var target = await AggregateStore.Load(TargetId.New(command.TargetId));

        target.ClearExpenses();

        AggregateStore.AppendChanges(target);

        return CommandResult.Success();
    }
}