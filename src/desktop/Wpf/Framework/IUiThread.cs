using System;
using System.Threading.Tasks;

namespace Budgetter.Wpf.Framework;

internal interface IUiThread
{
    Task InvokeAsync(Action action);
}