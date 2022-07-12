using System.Threading.Tasks;

namespace Budgetter.Application.Targets.Projections.ReadModel;

public interface ITargetItemReadModelAccessor
{
    Task<bool> AddAsync(TargetItemReadModel targetItemReadModel);
}