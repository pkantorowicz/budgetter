using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.Targets.ClearTarget;

public record ClearTargetCommand : CommandBase
{
    public ClearTargetCommand(Guid targetId)
    {
        TargetId = targetId;
    }

    public Guid TargetId { get; }
}