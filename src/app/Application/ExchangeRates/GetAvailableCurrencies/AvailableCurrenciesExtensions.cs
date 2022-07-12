using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgetter.Application.ExchangeRates.GetAvailableCurrencies;

internal static class AvailableCurrenciesExtensions
{
    public static List<string> GetAvailableCurrencies()
    {
        return Enum.GetValues(typeof(AvailableCurrencies))
            .Cast<AvailableCurrencies>()
            .Select(c => c.ToString().ToUpper())
            .ToList();
    }
}