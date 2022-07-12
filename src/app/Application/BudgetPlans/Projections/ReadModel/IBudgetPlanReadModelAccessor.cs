using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budgetter.Application.BudgetPlans.Projections.ReadModel;

public interface IBudgetPlanReadModelAccessor
{
    Task<BudgetPlanReadModel> GetByIdAsync(Guid budgetPlanId);
    Task<IEnumerable<BudgetPlanReadModel>> GetAllAsync();
    Task<bool> AddAsync(BudgetPlanReadModel budgetPlan);
    Task<bool> UpdateAsync(BudgetPlanReadModel budgetPlan);
}