using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Budgetter.Wpf.Framework.Events;

internal class Handler
{
    private readonly Func<Func<Task>, Task> _marshal;
    private readonly WeakReference _reference;
    private readonly Dictionary<Type, MethodInfo> _supportedHandlers = new();

    public Handler(object handler, Func<Func<Task>, Task> marshal)
    {
        _marshal = marshal;
        _reference = new WeakReference(handler);

        var interfaces = handler.GetType().GetTypeInfo().ImplementedInterfaces
            .Where(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandle<>));

        foreach (var @interface in interfaces)
        {
            var type = @interface.GetTypeInfo().GenericTypeArguments[0];
            var method = @interface.GetRuntimeMethod("HandleAsync", new[] { type, typeof(CancellationToken) });

            if (method != null) _supportedHandlers[type] = method;
        }
    }

    public bool IsDead => _reference.Target == null;

    public bool Matches(object instance)
    {
        return _reference.Target == instance;
    }

    public Task Handle(Type messageType, object message, CancellationToken cancellationToken)
    {
        var target = _reference.Target;

        if (target == null) return Task.FromResult(false);

        return _marshal(() =>
        {
            var tasks = _supportedHandlers
                .Where(handler => handler.Key.GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()))
                .Select(pair => pair.Value.Invoke(target, new[] { message, cancellationToken }))
                .Select(result => (Task)result)
                .ToList();

            return Task.WhenAll(tasks);
        });
    }

    public bool Handles(Type messageType)
    {
        return _supportedHandlers.Any(
            pair => pair.Key.GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()));
    }
}