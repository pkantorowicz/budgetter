using System;
using System.Reflection;

namespace Budgetter.Wpf;

public static class AssemblyVersion
{
    public static readonly DateTime CreateDate;
    public static readonly Version Version;
    public static readonly string MajorMinorVersion;

    static AssemblyVersion()
    {
        Version = Assembly.GetEntryAssembly()?.GetName().Version;

        if (Version is null)
            return;

        CreateDate = new DateTime(2000, 1, 1).AddDays(Version.Build).AddSeconds(Version.Revision * 2);
        MajorMinorVersion = $"{Version.Major}.{Version.Minor}";
    }
}