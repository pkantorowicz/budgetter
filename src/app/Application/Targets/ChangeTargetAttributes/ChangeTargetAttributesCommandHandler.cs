using System.Threading;
using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using Budgetter.BuildingBlocks.Application.Contacts.Results;
using Budgetter.BuildingBlocks.Domain.EventSourcing;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Targets;

namespace Budgetter.Application.Targets.ChangeTargetAttributes;

internal class ChangeTargetAttributesCommandHandler : CommandHandlerBase<ChangeTargetAttributesCommand>
{
    public ChangeTargetAttributesCommandHandler(IAggregateStore aggregateStore) : base(aggregateStore)
    {
    }

    public override async Task<ICommandResult> Handle(ChangeTargetAttributesCommand command,
        CancellationToken cancellationToken)
    {
        var target = await AggregateStore.Load(TargetId.New(command.TargetId));

        target.ChangeAttributes(
            Title.Entitle(command.Title),
            Description.Describe(command.Description),
            Duration.Specify(command.ValidFrom, command.ValidTo),
            Duration.Specify(command.PlanValidFrom, command.PlanValidTo),
            Money.Of(command.MaxAmount, command.Currency));

        AggregateStore.AppendChanges(target);

        return CommandResult.Success();
    }
}