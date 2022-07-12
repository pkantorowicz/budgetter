using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budgetter.Domain.Commons.Services;

public interface IExchangeRatesProvider
{
    Task<IDictionary<string, double>> GetExchangeRateAsync(string effectiveDate, List<string> code);
}