using System;

namespace Budgetter.Wpf.Settings;

internal class SettingNotFoundException : Exception
{
    public SettingNotFoundException(string paramName) : base(
        $"Unable to get particular setting. Param name: {paramName}")
    {
        ParamName = paramName;
    }

    public string ParamName { get; }
}