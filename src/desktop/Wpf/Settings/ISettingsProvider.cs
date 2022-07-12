using Budgetter.Infrastructure.Domain.Ef;
using Budgetter.Infrastructure.Feeds.Exchanges.Nbp;

namespace Budgetter.Wpf.Settings;

internal interface ISettingsProvider
{
    string GetCulture();
    int GetMaxSupportedDaysCount();
    EfSettings GetDbSettings();
    NpbApiSettings GetNbpApiSettings();
}