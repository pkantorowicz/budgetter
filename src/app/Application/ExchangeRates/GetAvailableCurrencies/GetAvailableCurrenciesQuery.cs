using System.Collections.Generic;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;

namespace Budgetter.Application.ExchangeRates.GetAvailableCurrencies;

public record GetAvailableCurrenciesQuery : QueryBase<IEnumerable<string>>;