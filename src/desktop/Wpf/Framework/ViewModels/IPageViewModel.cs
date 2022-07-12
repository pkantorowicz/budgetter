using System;
using System.Threading.Tasks;

namespace Budgetter.Wpf.Framework.ViewModels;

internal interface IPageViewModel : IDisposable
{
    Task OnActivateAsync();
    Task OnDeactivateAsync();
}