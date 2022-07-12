using Budgetter.Application.Commons.Validators;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using FluentValidation;

namespace Budgetter.Application.FinanceOperations.ChangeFinanceOperationAttributes;

internal class
    ChangeFinanceOperationAttributesCommandValidator : CommandValidatorBase<ChangeFinanceOperationAttributesCommand>
{
    public ChangeFinanceOperationAttributesCommandValidator()
    {
        RuleFor(command => command.FinanceOperationId)
            .NotEmpty()
            .WithMessage("Finance operation id can not be empty.");

        RuleFor(c => c.Title)
            .SetValidator(new TitleValidator());

        RuleFor(command => command.Price)
            .SetValidator(new PriceValidator());
    }
}