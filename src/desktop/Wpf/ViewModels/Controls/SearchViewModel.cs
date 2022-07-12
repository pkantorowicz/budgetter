using System;
using Budgetter.Wpf.Framework.ViewModels;

namespace Budgetter.Wpf.ViewModels.Controls;

internal class SearchViewModel : ViewModelBase
{
    private string _searchKeyword;

    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length < 3)
            {
                if (_searchKeyword?.Length >= 3)
                    OnSearchTextChanged(value?.ToUpperInvariant());

                _searchKeyword = value;
                return;
            }

            _searchKeyword = value;

            OnSearchTextChanged(value.ToUpperInvariant());
            OnPropertyChanged();
        }
    }

    public event EventHandler<string> SearchTextChanged;

    protected virtual void OnSearchTextChanged(string searchKeyword)
    {
        SearchTextChanged?.Invoke(this, searchKeyword);
    }
}