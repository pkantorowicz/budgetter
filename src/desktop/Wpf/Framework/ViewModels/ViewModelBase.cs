using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Budgetter.Wpf.Framework.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
{
    public virtual void Dispose()
    {
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public Func<TObject, TKey> BuildKeySelector<TObject, TKey>(string propertyName)
    {
        return obj =>
        {
            var prop = typeof(TObject).GetProperty(propertyName);
            return (TKey)prop?.GetValue(obj);
        };
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void PropagatePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(sender, e);
    }

    protected void UiInvoke(Action action, System.Windows.Application currentApp)
    {
        currentApp?.Dispatcher.Invoke(action);
    }

    protected DispatcherOperation UiInvokeAsync(Action action, System.Windows.Application currentApp)
    {
        return currentApp?.Dispatcher.InvokeAsync(action);
    }
}