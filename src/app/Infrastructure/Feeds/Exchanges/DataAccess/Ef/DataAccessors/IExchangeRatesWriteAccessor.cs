using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.Models;

namespace Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.DataAccessors;

internal interface IExchangeRatesWriteAccessor
{
    Task SaveExchangeRatesAsync(List<DailyExchangeRatesEfDao> dailyExchangeRates);
}