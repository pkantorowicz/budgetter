using System.Threading;
using System.Threading.Tasks;

namespace Budgetter.Wpf.Framework.Events;

public interface IHandle<TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken cancellationToken);
}