using System.Collections.Generic;

namespace Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.Models;

public class DailyExchangeRatesEfDao
{
    public DailyExchangeRatesEfDao()
    {
        ExchangeRates = new List<ExchangeRateEfDao>();
    }

    public string EffectiveDate { get; set; }
    public ICollection<ExchangeRateEfDao> ExchangeRates { get; set; }
}