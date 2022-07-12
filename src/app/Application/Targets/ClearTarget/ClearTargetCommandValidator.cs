using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using FluentValidation;

namespace Budgetter.Application.Targets.ClearTarget;

internal class ClearTargetCommandValidator : CommandValidatorBase<ClearTargetCommand>
{
    public ClearTargetCommandValidator()
    {
        RuleFor(command => command.TargetId)
            .NotEmpty()
            .WithMessage("Target id can not be empty.");
    }
}