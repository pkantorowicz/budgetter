using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.Targets.RemoveTarget;

public record RemoveTargetCommand : CommandBase
{
    public RemoveTargetCommand(Guid targetId)
    {
        TargetId = targetId;
    }

    public Guid TargetId { get; }
}