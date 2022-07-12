using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Budgetter.Wpf.Framework;

internal class UiThread : IUiThread
{
    private readonly Dispatcher _dispatcher;

    public UiThread()
    {
        _dispatcher = System.Windows.Application.Current.Dispatcher;
    }

    public async Task InvokeAsync(Action action)
    {
        await _dispatcher.InvokeAsync(action);
    }
}