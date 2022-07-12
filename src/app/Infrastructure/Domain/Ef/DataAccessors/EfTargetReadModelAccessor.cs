using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Budgetter.Application.Targets.Projections.ReadModel;
using Microsoft.EntityFrameworkCore;

namespace Budgetter.Infrastructure.Domain.Ef.DataAccessors;

public class EfTargetReadModelAccessor : ITargetReadModelAccessor
{
    private readonly BudgetterDbContext _budgetterDbContext;
    private readonly DbSet<TargetReadModel> _targetDbSet;

    public EfTargetReadModelAccessor(
        BudgetterDbContext budgetterDbContext)
    {
        _budgetterDbContext = budgetterDbContext;
        _targetDbSet = _budgetterDbContext.Set<TargetReadModel>();
    }

    public async Task<TargetReadModel> GetByIdAsync(Guid targetId, Guid budgetPlanId)
    {
        return await _targetDbSet
            .Include(t => t.TargetItems)
            .FirstOrDefaultAsync(t => t.Id == targetId &&
                                      t.BudgetPlanId == budgetPlanId);
    }

    public async Task<IEnumerable<TargetReadModel>> FindAsync(Expression<Func<TargetReadModel, bool>> criteria)
    {
        return await _targetDbSet
            .Where(criteria)
            .Include(t => t.TargetItems)
            .ToListAsync();
    }

    public async Task<bool> AddAsync(TargetReadModel targetReadModel)
    {
        await _targetDbSet.AddAsync(targetReadModel);

        return await _budgetterDbContext.SaveAsync();
    }

    public async Task<bool> UpdateAsync(TargetReadModel targetReadModel)
    {
        _targetDbSet.Update(targetReadModel);

        return await _budgetterDbContext.SaveAsync();
    }
}