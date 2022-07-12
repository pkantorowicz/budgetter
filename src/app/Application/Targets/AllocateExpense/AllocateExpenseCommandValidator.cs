using Budgetter.Application.Commons.Validators;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;
using FluentValidation;

namespace Budgetter.Application.Targets.AllocateExpense;

internal class AllocateExpenseCommandValidator : CommandValidatorBase<AllocateExpenseCommand>
{
    public AllocateExpenseCommandValidator()
    {
        RuleFor(command => command.TargetId)
            .NotEmpty()
            .WithMessage("Target id can not be empty.");

        RuleFor(c => c.FinanceOperation)
            .SetValidator(new ExpenseDtoValidator());
    }
}

public class ExpenseDtoValidator : AbstractValidator<FinanceOperationDto>
{
    public ExpenseDtoValidator()
    {
        RuleFor(dto => dto.Id)
            .NotEmpty()
            .WithMessage("Expense dto id can not be empty.");

        RuleFor(c => c.Title)
            .SetValidator(new TitleValidator());

        RuleFor(command => command.Price)
            .SetValidator(new PriceValidator());

        RuleFor(command => command.Currency)
            .SetValidator(new CurrencyValidator());

        RuleFor(dto => dto.OccurredAt)
            .NotEmpty()
            .WithMessage("Occured at date can not be empty.");
    }
}