using System;
using System.Windows.Threading;
using Budgetter.Wpf.Framework.ViewModels;

namespace Budgetter.Wpf.ViewModels.Controls;

internal class UiNotificationViewModel : ViewModelBase
{
    private readonly DispatcherTimer _timer;
    private string _details;

    private bool _isSuccess;
    private bool _isVisible;
    private string _message;

    public UiNotificationViewModel()
    {
        _timer = new DispatcherTimer();

        _timer.Tick += Timer_TimerTick;
        _timer.Interval = new TimeSpan(0, 0, 3);
    }

    public bool IsSuccess
    {
        get => _isSuccess;
        set
        {
            _isSuccess = value;

            OnPropertyChanged();
        }
    }

    public string Message
    {
        get => _message;
        set
        {
            _message = value;

            OnPropertyChanged();
        }
    }

    public string Details
    {
        get => _details;
        set
        {
            _details = value;

            OnPropertyChanged();
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            _isVisible = value;

            OnPropertyChanged();
        }
    }

    public void ShowNotification(
        bool isSuccess,
        string message,
        string details)
    {
        IsSuccess = isSuccess;
        Message = message;
        Details = details;
        IsVisible = true;

        if (_timer.IsEnabled)
        {
            _timer.Stop();
            _timer.Start();

            return;
        }

        _timer.Start();
    }

    private void Timer_TimerTick(object sender, EventArgs e)
    {
        IsVisible = false;

        _timer.Stop();
    }
}