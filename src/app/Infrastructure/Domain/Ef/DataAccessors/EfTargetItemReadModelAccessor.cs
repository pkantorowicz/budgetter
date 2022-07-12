using System.Threading.Tasks;
using Budgetter.Application.Targets.Projections.ReadModel;
using Microsoft.EntityFrameworkCore;

namespace Budgetter.Infrastructure.Domain.Ef.DataAccessors;

public class EfTargetItemReadModelAccessor : ITargetItemReadModelAccessor
{
    private readonly BudgetterDbContext _budgetterDbContext;
    private readonly DbSet<TargetItemReadModel> _targetItemDbSet;

    public EfTargetItemReadModelAccessor(
        BudgetterDbContext budgetterDbContext)
    {
        _budgetterDbContext = budgetterDbContext;
        _targetItemDbSet = _budgetterDbContext.Set<TargetItemReadModel>();
    }

    public async Task<bool> AddAsync(TargetItemReadModel targetItemReadModel)
    {
        await _targetItemDbSet.AddAsync(targetItemReadModel);

        return await _budgetterDbContext.SaveAsync();
    }
}