using System;
using System.Threading;
using System.Threading.Tasks;

namespace Budgetter.Wpf.Framework.Events;

internal class EventAggregator : EventAggregatorBase, IEventAggregator
{
    private readonly IUiThread _uiThread;

    public EventAggregator(IUiThread uiThread)
    {
        _uiThread = uiThread;
    }

    public void SubscribeOnPublishedThread(object subscriber)
    {
        Subscribe(subscriber, f => f());
    }

    public void SubscribeOnBackgroundThread(object subscriber)
    {
        Subscribe(subscriber,
            f => Task.Factory.StartNew(f, default, TaskCreationOptions.None, TaskScheduler.Default));
    }

    public void SubscribeOnUIThread(object subscriber)
    {
        Subscribe(subscriber, f =>
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            async void PublishAction()
            {
                try
                {
                    await f();

                    taskCompletionSource.SetResult(true);
                }
                catch (OperationCanceledException)
                {
                    taskCompletionSource.SetCanceled();
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            }

            _uiThread.InvokeAsync(PublishAction);

            return taskCompletionSource.Task;
        });
    }

    public Task PublishOnCurrentThreadAsync(object message, CancellationToken cancellationToken)
    {
        return PublishAsync(message, f => f(), cancellationToken);
    }

    public Task PublishOnCurrentThreadAsync(object message)
    {
        return PublishOnCurrentThreadAsync(message, default);
    }

    public Task PublishOnBackgroundThreadAsync(object message, CancellationToken cancellationToken)
    {
        return PublishAsync(message,
            f => Task.Factory.StartNew(f, default, TaskCreationOptions.None, TaskScheduler.Default),
            cancellationToken);
    }

    public Task PublishOnBackgroundThreadAsync(object message)
    {
        return PublishOnBackgroundThreadAsync(message, default);
    }

    public Task PublishOnUIThreadAsync(object message, CancellationToken cancellationToken)
    {
        return PublishAsync(message, f =>
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            async void PublishAction()
            {
                try
                {
                    await f();

                    taskCompletionSource.SetResult(true);
                }
                catch (OperationCanceledException)
                {
                    taskCompletionSource.SetCanceled(cancellationToken);
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            }

            _uiThread.InvokeAsync(PublishAction);

            return taskCompletionSource.Task;
        }, cancellationToken);
    }

    public Task PublishOnUIThreadAsync(object message)
    {
        return PublishOnUIThreadAsync(message, default);
    }
}