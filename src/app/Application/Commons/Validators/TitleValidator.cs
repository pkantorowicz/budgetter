using FluentValidation;

namespace Budgetter.Application.Commons.Validators;

public class TitleValidator : AbstractValidator<string>
{
    public TitleValidator()
    {
        const int min = 3;
        const int max = 60;

        RuleFor(title => title)
            .NotEmpty()
            .Length(min, max)
            .WithMessage(title =>
                $"Title {title} can not be empty and must has length from {min} to {max}.");
    }
}