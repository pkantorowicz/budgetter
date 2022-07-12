using System;
using System.Collections.Generic;
using Budgetter.BuildingBlocks.Domain.Extensions;
using Budgetter.Domain.Commons.Services;
using Budgetter.Domain.Commons.ValueObjects;

namespace Budgetter.Domain.Commons.Extensions;

public static class MoneyExtensions
{
    public static Money ConvertCurrency(
        this Money price,
        string destinationCurrency,
        DateTime? occuredAt,
        IExchangeRatesProvider exchangeRatesProvider)
    {
        var exchangeCodes = new List<string> { price.Currency, destinationCurrency };

        var exchangeRates = exchangeRatesProvider
            .GetExchangeRateAsync(occuredAt.ToYmd(), exchangeCodes)
            .GetAwaiter()
            .GetResult();

        var exchangeRate = exchangeRates[price.Currency] / exchangeRates[destinationCurrency];

        return Money.Of(price.Value, destinationCurrency, exchangeRate);
    }
}