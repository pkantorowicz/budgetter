using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Microsoft.EntityFrameworkCore;

namespace Budgetter.Infrastructure.Domain.Ef.DataAccessors;

public class EfBudgetPlanReadModelAccessor : IBudgetPlanReadModelAccessor
{
    private readonly DbSet<BudgetPlanReadModel> _budgetPlanDbSet;
    private readonly BudgetterDbContext _budgetterDbContext;

    public EfBudgetPlanReadModelAccessor(
        BudgetterDbContext budgetterDbContext)
    {
        _budgetterDbContext = budgetterDbContext;
        _budgetPlanDbSet = _budgetterDbContext.Set<BudgetPlanReadModel>();
    }

    public async Task<BudgetPlanReadModel> GetByIdAsync(Guid budgetPlanId)
    {
        return await _budgetPlanDbSet
            .Include(bp => bp.FinanceOperations)
            .Include(bp => bp.Targets)
            .ThenInclude(t => t.TargetItems)
            .FirstOrDefaultAsync(bp => bp.Id == budgetPlanId);
    }

    public async Task<IEnumerable<BudgetPlanReadModel>> GetAllAsync()
    {
        return await _budgetPlanDbSet.ToListAsync();
    }

    public async Task<bool> AddAsync(BudgetPlanReadModel budgetPlan)
    {
        await _budgetPlanDbSet.AddAsync(budgetPlan);

        return await _budgetterDbContext.SaveAsync();
    }

    public async Task<bool> UpdateAsync(BudgetPlanReadModel budgetPlan)
    {
        _budgetPlanDbSet.Update(budgetPlan);

        return await _budgetterDbContext.SaveAsync();
    }
}