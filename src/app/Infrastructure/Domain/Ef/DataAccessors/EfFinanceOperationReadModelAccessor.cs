using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Budgetter.Domain.Targets.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Budgetter.Infrastructure.Domain.Ef.DataAccessors;

public class EfFinanceOperationReadModelAccessor : IFinanceOperationReadModelAccessor,
    IFinanceOperationExistenceChecker
{
    private readonly BudgetterDbContext _budgetterDbContext;
    private readonly DbSet<FinanceOperationReadModel> _expenseDbSet;

    public EfFinanceOperationReadModelAccessor(
        BudgetterDbContext budgetterDbContext)
    {
        _budgetterDbContext = budgetterDbContext;
        _expenseDbSet = _budgetterDbContext.Set<FinanceOperationReadModel>();
    }

    public async Task<bool> ExistsAsync(Guid financeOperationId, Guid budgetPlanId)
    {
        var exists = await _expenseDbSet
            .AnyAsync(fo => fo.Id == financeOperationId &&
                            fo.BudgetPlanId == budgetPlanId);

        return exists;
    }


    public async Task<FinanceOperationReadModel> GetByIdAsync(Guid expenseId, Guid budgetPlanId)
    {
        return await _expenseDbSet.FirstOrDefaultAsync(fo =>
            fo.Id == expenseId && fo.BudgetPlanId == budgetPlanId);
    }

    public async Task<IEnumerable<FinanceOperationReadModel>> FilterAsync(
        Expression<Func<FinanceOperationReadModel, bool>> criteria)
    {
        var filteredFinanceOperations = await _expenseDbSet
            .Where(criteria)
            .ToListAsync();

        return filteredFinanceOperations;
    }

    public async Task<IEnumerable<FinanceOperationReadModel>> GetAllAsync(Guid budgetPlanId)
    {
        var financeOperations = await _expenseDbSet
            .Where(fo => fo.BudgetPlanId == budgetPlanId)
            .ToListAsync();

        return financeOperations;
    }

    public async Task<bool> AddAsync(FinanceOperationReadModel financeOperationReadModel)
    {
        await _expenseDbSet.AddAsync(financeOperationReadModel);

        return await _budgetterDbContext.SaveAsync();
    }

    public async Task<bool> UpdateAsync(FinanceOperationReadModel financeOperationReadModel)
    {
        _expenseDbSet.Update(financeOperationReadModel);

        return await _budgetterDbContext.SaveAsync();
    }
}