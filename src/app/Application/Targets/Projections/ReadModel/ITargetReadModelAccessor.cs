using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Budgetter.Application.Targets.Projections.ReadModel;

public interface ITargetReadModelAccessor
{
    Task<TargetReadModel> GetByIdAsync(Guid targetId, Guid budgetPlanId);
    Task<IEnumerable<TargetReadModel>> FindAsync(Expression<Func<TargetReadModel, bool>> criteria);
    Task<bool> AddAsync(TargetReadModel targetReadModel);
    Task<bool> UpdateAsync(TargetReadModel targetReadModel);
}