using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Budgetter.Infrastructure.Domain.Ef;
using Budgetter.Infrastructure.Feeds.Exchanges.Nbp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Budgetter.Wpf.Settings;

internal class SettingsProvider : ISettingsProvider
{
    private AppSettings _appSettings;

    public SettingsProvider()
    {
        Task.Run(async () => await LoadSettingsAsync()).Wait();
    }

    public string GetCulture()
    {
        if (string.IsNullOrEmpty(_appSettings?.Culture))
            throw new SettingNotFoundException(nameof(AppSettings.Culture));

        return _appSettings.Culture;
    }

    public int GetMaxSupportedDaysCount()
    {
        return _appSettings?.MaxSupportedDaysCount ?? 0;
    }

    public EfSettings GetDbSettings()
    {
        if (_appSettings?.DbSettings?.IsEmpty == true)
            throw new SettingNotFoundException(nameof(AppSettings.DbSettings));

        return _appSettings?.DbSettings;
    }

    public NpbApiSettings GetNbpApiSettings()
    {
        if (_appSettings?.NpbApiSettings?.IsEmpty == true)
            throw new SettingNotFoundException(nameof(AppSettings.NpbApiSettings));

        return _appSettings?.NpbApiSettings;
    }

    private async Task LoadSettingsAsync()
    {
        var file = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        if (string.IsNullOrEmpty(file))
            throw new ArgumentException(
                "Unable to read settings file, because provided path is empty.");

        using var streamReader = File.OpenText($"{file}\\appsettings.json");
        using JsonTextReader textReader = new(streamReader);

        var jToken = await JToken.ReadFromAsync(textReader);

        if (jToken is JObject jObject)
            _appSettings = jObject.ToObject<AppSettings>();
    }
}