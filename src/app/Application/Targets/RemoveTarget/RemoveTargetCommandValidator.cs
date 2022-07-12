using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using FluentValidation;

namespace Budgetter.Application.Targets.RemoveTarget;

internal class RemoveTargetCommandValidator : CommandValidatorBase<RemoveTargetCommand>
{
    public RemoveTargetCommandValidator()
    {
        RuleFor(c => c.TargetId).NotEmpty();
    }
}