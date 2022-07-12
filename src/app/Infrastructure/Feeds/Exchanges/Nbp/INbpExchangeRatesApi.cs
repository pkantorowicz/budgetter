using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Infrastructure.Feeds.Exchanges.DataContracts;
using RestEase;

namespace Budgetter.Infrastructure.Feeds.Exchanges.Nbp;

[Header("Accept", "application/json")]
[Header("Content-Type", "application/json")]
public interface INbpExchangeRatesApi
{
    [Get("api/exchangerates/tables/{table}/{startDate}/{endDate}")]
    Task<IEnumerable<DailyExchangeRatesDataContract>> FetchExchangeRates([Path] string table,
        [Path] string startDate, [Path] string endDate);
}