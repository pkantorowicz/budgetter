using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budgetter.Wpf.Controllers.Interfaces;

internal interface IEchangeRatesController
{
    Task<IEnumerable<string>> GetAvailableCurrencies();
    Task DownloadExchangeRates(DateTime startDate, DateTime endDate);
}