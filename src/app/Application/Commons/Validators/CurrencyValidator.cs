using Budgetter.Application.ExchangeRates.GetAvailableCurrencies;
using FluentValidation;

namespace Budgetter.Application.Commons.Validators;

public class CurrencyValidator : AbstractValidator<string>
{
    public CurrencyValidator()
    {
        var availableCurrencies = AvailableCurrenciesExtensions
            .GetAvailableCurrencies();

        RuleFor(currency => currency)
            .NotEmpty()
            .Must(currency => availableCurrencies
                .Contains(currency))
            .WithMessage(currency =>
                $"Currency {currency} must be value from available currencies list. " +
                $"Available currencies: ({string.Join(", ", availableCurrencies)}).");
    }
}