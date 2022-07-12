using System.Threading.Tasks;

namespace Budgetter.BuildingBlocks.Infrastructure;

public interface IUnitOfWork
{
    Task CommitTransactionAsync();
}