using System;
using System.Threading.Tasks;

namespace Budgetter.Domain.Targets.Services.Interfaces;

public interface IFinanceOperationExistenceChecker
{
    Task<bool> ExistsAsync(
        Guid financeOperationId,
        Guid budgetPlanId);
}