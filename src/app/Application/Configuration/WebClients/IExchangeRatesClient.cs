using System;
using System.Threading.Tasks;

namespace Budgetter.Application.Configuration.WebClients;

public interface IExchangeRatesClient
{
    Task DownloadExchangeRatesAsync(DateTime from, DateTime to);
}