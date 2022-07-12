using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Budgetter.Application.FinanceOperations.Projections.ReadModel;

public interface IFinanceOperationReadModelAccessor
{
    Task<FinanceOperationReadModel> GetByIdAsync(Guid expenseId, Guid budgetPlanId);

    Task<IEnumerable<FinanceOperationReadModel>> FilterAsync(
        Expression<Func<FinanceOperationReadModel, bool>> criteria);

    Task<IEnumerable<FinanceOperationReadModel>> GetAllAsync(Guid budgetPlanId);
    Task<bool> AddAsync(FinanceOperationReadModel financeOperationReadModel);
    Task<bool> UpdateAsync(FinanceOperationReadModel financeOperationReadModel);
}