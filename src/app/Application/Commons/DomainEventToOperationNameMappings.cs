using System;
using System.Collections.Generic;
using Budgetter.Domain.FinanceOperations.DomainEvents;
using Budgetter.Domain.Plans.DomainEvents;
using Budgetter.Domain.Targets.DomainEvents;

namespace Budgetter.Application.Commons;

internal static class DomainEventToOperationNameMappings
{
    static DomainEventToOperationNameMappings()
    {
        Dictionary = new Dictionary<Type, string>
        {
            { typeof(FinanceOperationAttributesChangedDomainEvent), "Finance operation updated" },
            { typeof(FinanceOperationCreatedDomainEvent), "Finance operation created" },
            { typeof(ExpenseAllocatedDomainEvent), "Expense allocated" },
            { typeof(ExpenseDeallocatedDomainEvent), "Expense allocated" },
            { typeof(ExpensesClearedDomainEvent), "Expenses cleared" },
            { typeof(TargetAttributesChangedDomainEvent), "Target updated" },
            { typeof(TargetCreatedDomainEvent), "Target created" },
            { typeof(BudgetPlanCreatedDomainEvent), "Budget plan created" },
            { typeof(BudgetPlanAttributesChangedDomainEvent), "Budget plan updated" }
        };
    }

    internal static IDictionary<Type, string> Dictionary { get; }

    internal static string GetOperationName(this Type eventType)
    {
        return Dictionary[eventType];
    }
}