using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Targets;

namespace Budgetter.Application.Targets.RemoveTarget;

internal class RemoveTargetCommandHandler : CommandHandlerBase<RemoveTargetCommand>
{
    private readonly IAggregateStore _aggregateStore;

    public RemoveTargetCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
        _aggregateStore = aggregateStore;
    }

    public override async Task<ICommandResult> Handle(RemoveTargetCommand command,
        CancellationToken cancellationToken)
    {
        var target = await AggregateStore.Load(TargetId.New(command.TargetId));

        target.MarkAsRemoved();

        _aggregateStore.AppendChanges(target);

        return CommandResult.Success();
    }
}