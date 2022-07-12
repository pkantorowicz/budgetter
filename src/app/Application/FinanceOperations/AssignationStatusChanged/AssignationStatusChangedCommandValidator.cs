using FluentValidation;

namespace Budgetter.Application.FinanceOperations.AssignationStatusChanged;

public class AssignationStatusChangedCommandValidator : AbstractValidator<AssignationStatusChangedCommand>
{
    public AssignationStatusChangedCommandValidator()
    {
        RuleFor(command => command.FinanceOperationId)
            .NotEmpty()
            .WithMessage("Finance operation id cannot be empty.");

        RuleFor(command => command.TargetId)
            .NotEmpty()
            .WithMessage("Target id cannot be empty.");
    }
}