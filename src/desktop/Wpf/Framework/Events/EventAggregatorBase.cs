using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Budgetter.Wpf.Framework.Extensions;

namespace Budgetter.Wpf.Framework.Events;

internal class EventAggregatorBase
{
    private readonly List<Handler> _handlers = new();

    public virtual bool HandlerExistsFor(Type messageType)
    {
        lock (_handlers)
        {
            return _handlers.Any(handler => handler.Handles(messageType) && !handler.IsDead);
        }
    }

    public virtual void Subscribe(object subscriber, Func<Func<Task>, Task> marshal)
    {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

        if (marshal == null) throw new ArgumentNullException(nameof(marshal));

        lock (_handlers)
        {
            if (_handlers.Any(x => x.Matches(subscriber))) return;

            _handlers.Add(new Handler(subscriber, marshal));
        }
    }

    public virtual void Unsubscribe(object subscriber)
    {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

        lock (_handlers)
        {
            var found = _handlers.FirstOrDefault(x => x.Matches(subscriber));

            if (found != null) _handlers.Remove(found);
        }
    }

    public virtual Task PublishAsync(object message, Func<Func<Task>, Task> marshal,
        CancellationToken cancellationToken = default)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        if (marshal == null) throw new ArgumentNullException(nameof(marshal));

        Handler[] toNotify;

        lock (_handlers)
        {
            toNotify = _handlers.ToArray();
        }

        return marshal(async () =>
        {
            var messageType = message.GetType();
            var tasks = toNotify.Select(h => h.Handle(messageType, message, cancellationToken));

            await Task.WhenAll(tasks);

            var dead = toNotify.Where(h => h.IsDead).ToList();

            if (dead.Any())
                lock (_handlers)
                {
                    dead.Apply(x => _handlers.Remove(x));
                }
        });
    }
}