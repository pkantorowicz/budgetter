using FluentValidation;

namespace Budgetter.Application.Commons.Validators;

public class PriceValidator : AbstractValidator<decimal>
{
    public PriceValidator()
    {
        const int min = -1000000;
        const int max = 1000000;
        const int zero = 0;

        RuleFor(price => price)
            .NotEmpty()
            .GreaterThan(min)
            .LessThan(max)
            .NotEqual(zero)
            .WithMessage(price =>
                $"Price: {price} must be a value between {min} and {max} and can not be equals to {zero}.");
    }
}