using Budgetter.Application.Commons.Validators;
using FluentValidation;

namespace Budgetter.Application.BudgetPlans.ChangeBudgetPlanAttributes;

internal class ChangeBudgetPlanAttributesCommandValidator : AbstractValidator<ChangeBudgetPlanAttributesCommand>
{
    public ChangeBudgetPlanAttributesCommandValidator()
    {
        RuleFor(command => command.BudgetPlanId)
            .NotEmpty()
            .WithMessage("Budget plan id can not be empty.");

        RuleFor(c => c.Title)
            .SetValidator(new TitleValidator());

        RuleFor(command => command.Currency)
            .SetValidator(new CurrencyValidator());

        RuleFor(command => command.ValidFrom)
            .NotEmpty()
            .Must((command, validFrom) => validFrom < command.ValidTo)
            .WithMessage((command, validFrom) =>
                $"Budget plan valid from: {validFrom} must be lower than target valid to: {command.ValidTo}.");

        RuleFor(command => command.ValidTo)
            .NotEmpty()
            .Must((command, validTo) => validTo > command.ValidFrom)
            .WithMessage((command, validTo) =>
                $"Budget plan valid from: {validTo} must be greater than target valid to: {command.ValidFrom}.");

        RuleFor(command => command.MaxDaysCount)
            .GreaterThan(0)
            .WithMessage("Max days count must be positive value,");
    }
}