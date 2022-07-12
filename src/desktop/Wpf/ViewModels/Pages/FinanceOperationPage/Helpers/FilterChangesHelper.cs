using System.Collections.Generic;
using System.Linq;

namespace Budgetter.Wpf.ViewModels.Pages.FinanceOperationPage.Helpers;

internal class FilterChangesHelper
{
    private readonly IDictionary<FilterChangeType, object> _changesDictionary;

    public FilterChangesHelper()
    {
        _changesDictionary = new Dictionary<FilterChangeType, object>();
    }

    public IDictionary<FilterChangeType, object> GetChanges()
    {
        return _changesDictionary;
    }

    public void UpdateFiltersState(FilterChangeType filterChangeType, object value)
    {
        if (value is null or "" or false)
        {
            _changesDictionary.Remove(filterChangeType);

            return;
        }

        _changesDictionary.TryGetValue(filterChangeType, out var existingValue);

        if (existingValue is null)
        {
            _changesDictionary.Add(filterChangeType, value);
        }
        else
        {
            _changesDictionary.Remove(filterChangeType);
            _changesDictionary.Add(filterChangeType, value);
        }
    }

    public void RemoveDatePickerChanges()
    {
        var changes = _changesDictionary.Where(cd
            => cd.Key is FilterChangeType.StartDate or FilterChangeType.EndDate);

        foreach (var change in changes)
            _changesDictionary.Remove(change);
    }

    public void RemoveRadioButtonsChanges()
    {
        var changes = _changesDictionary.Where(cd
            => cd.Key is FilterChangeType.CurrentMonth or
                FilterChangeType.LastMonth or
                FilterChangeType.LastMonth or
                FilterChangeType.LastHalfYear);

        foreach (var change in changes)
            _changesDictionary.Remove(change);
    }

    public void Clear()
    {
        _changesDictionary.Clear();
    }
}