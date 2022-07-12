using Budgetter.Application.Commons.Validators;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using FluentValidation;

namespace Budgetter.Application.Targets.ChangeTargetAttributes;

internal class ChangeTargetAttributesCommandValidator : CommandValidatorBase<ChangeTargetAttributesCommand>
{
    public ChangeTargetAttributesCommandValidator()
    {
        RuleFor(command => command.TargetId)
            .NotEmpty()
            .WithMessage("Target id can not be empty.");

        RuleFor(command => command.Title)
            .SetValidator(new TitleValidator());

        RuleFor(command => command.Description)
            .SetValidator(new DescriptionValidator());

        RuleFor(command => command.ValidFrom)
            .NotEmpty()
            .Must((command, validFrom) => validFrom < command.ValidTo && validFrom > command.PlanValidFrom)
            .WithMessage((command, validFrom) =>
                $"Target valid from: {validFrom} must be lower than target valid to: {command.ValidFrom} " +
                $"and greater than budget plan valid from: {command.PlanValidFrom}.");

        RuleFor(command => command.ValidTo)
            .NotEmpty()
            .Must((command, validTo) => validTo > command.ValidFrom && validTo < command.PlanValidTo)
            .WithMessage((command, validTo) =>
                $"Target valid to: {validTo} must be greater than target valid from: {command.ValidFrom} " +
                $"and lower than budget plan valid from: {command.PlanValidFrom}.");

        RuleFor(command => command.PlanValidFrom)
            .NotEmpty()
            .Must((command, planValidFrom) =>
                planValidFrom < command.PlanValidTo && planValidFrom < command.ValidFrom)
            .WithMessage((command, planValidFrom) =>
                $"Budget plan valid from: {planValidFrom} must be lower than plan valid to: {command.PlanValidTo} " +
                $"and lower than target valid from: {command.ValidFrom}.");

        RuleFor(command => command.PlanValidTo)
            .NotEmpty()
            .Must((command, planValidTo) => planValidTo > command.PlanValidFrom && planValidTo > command.ValidTo)
            .WithMessage((command, planValidTo) =>
                $"Budget plan valid to: {planValidTo} must be greater than budget plan valid from: {command.PlanValidFrom} " +
                $"and greater than budget valid to: {command.ValidTo}.");

        RuleFor(command => command.MaxAmount)
            .SetValidator(new PriceValidator());

        RuleFor(command => command.Currency)
            .SetValidator(new CurrencyValidator());
    }
}