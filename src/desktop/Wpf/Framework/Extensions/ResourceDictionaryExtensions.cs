using System;
using System.Threading;
using System.Windows;

namespace Budgetter.Wpf.Framework.Extensions;

public static class ResourceDictionaryExtensions
{
    public static T GetResource<T>(string resourceName) where T : class
    {
        return System.Windows.Application.Current.TryFindResource(resourceName) as T;
    }

    public static ResourceDictionary GetLanguageDictionary()
    {
        var culture = Thread.CurrentThread.CurrentCulture.ToString();

        return culture switch
        {
            "pl-PL" => (ResourceDictionary)System.Windows.Application.LoadComponent(
                new Uri("..\\Resources\\Languages\\StringResources.pl-PL.xaml", UriKind.Relative)),
            "en-GB" => (ResourceDictionary)System.Windows.Application.LoadComponent(
                new Uri("..\\Resources\\Languages\\StringResources.en-GB.xaml", UriKind.Relative)),
            _ => throw new ArgumentOutOfRangeException(nameof(culture), culture, "Unknown culture.")
        };
    }

    public static string GetStringValue(this ResourceDictionary resourceDictionary, object key)
    {
        return resourceDictionary[key].ToString() ?? string.Empty;
    }
}