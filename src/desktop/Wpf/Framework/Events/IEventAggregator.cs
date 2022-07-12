using System.Threading;
using System.Threading.Tasks;

namespace Budgetter.Wpf.Framework.Events;

public interface IEventAggregator
{
    void SubscribeOnPublishedThread(object subscriber);
    void SubscribeOnBackgroundThread(object subscriber);
    void SubscribeOnUIThread(object subscriber);
    Task PublishOnCurrentThreadAsync(object message, CancellationToken cancellationToken);
    Task PublishOnCurrentThreadAsync(object message);
    Task PublishOnBackgroundThreadAsync(object message, CancellationToken cancellationToken);
    Task PublishOnBackgroundThreadAsync(object message);
    Task PublishOnUIThreadAsync(object message, CancellationToken cancellationToken);
    Task PublishOnUIThreadAsync(object message);
}