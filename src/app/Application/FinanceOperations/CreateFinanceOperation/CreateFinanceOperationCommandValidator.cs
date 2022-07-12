using System;
using Budgetter.Application.Commons.Validators;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using FluentValidation;

namespace Budgetter.Application.FinanceOperations.CreateFinanceOperation;

internal class CreateFinanceOperationCommandValidator : CommandValidatorBase<CreateFinanceOperationCommand, Guid>
{
    public CreateFinanceOperationCommandValidator()
    {
        RuleFor(command => command.BudgetPlanId)
            .NotEmpty()
            .WithMessage("Finance operation id can not be empty.");

        RuleFor(c => c.Title)
            .SetValidator(new TitleValidator());

        RuleFor(command => command.Price)
            .SetValidator(new PriceValidator());

        RuleFor(command => command.Currency)
            .SetValidator(new CurrencyValidator());

        RuleFor(command => command.PlanCurrency)
            .SetValidator(new CurrencyValidator());
    }
}