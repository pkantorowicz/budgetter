using System;
using System.Collections.Generic;
using Budgetter.Domain.FinanceOperations.DomainEvents;
using Budgetter.Domain.Plans.DomainEvents;
using Budgetter.Domain.Targets.DomainEvents;

namespace Budgetter.Infrastructure.AggregateStore;

internal static class DomainEventTypeMappings
{
    static DomainEventTypeMappings()
    {
        Dictionary = new Dictionary<string, Type>
        {
            { "ExpenseAttributesChanged", typeof(FinanceOperationAttributesChangedDomainEvent) },
            { "ExpenseCreated", typeof(FinanceOperationCreatedDomainEvent) },
            { "ExpenseAllocate", typeof(ExpenseAllocatedDomainEvent) },
            { "ExpenseDeallocate", typeof(ExpenseDeallocatedDomainEvent) },
            { "ExpensesCleared", typeof(ExpensesClearedDomainEvent) },
            { "TargetAttributesChanged", typeof(TargetAttributesChangedDomainEvent) },
            { "TargetCreated", typeof(TargetCreatedDomainEvent) },
            { "BudgetPlanCreated", typeof(BudgetPlanCreatedDomainEvent) },
            { "BudgetPlanAttributesChanged", typeof(BudgetPlanAttributesChangedDomainEvent) },
            { "TargetRemoved", typeof(TargetRemovedDomainEvent) },
            { "FinanceOperationRemoved", typeof(FinanceOperationRemovedDomainEvent) },
            { "BudgetPlanRemoved", typeof(BudgetPlanRemovedDomainEvent) },
            { "FinanceOperationMarkedAsAssigned", typeof(AssignationStatusChangedDomainEvent) }
        };
    }

    internal static IDictionary<string, Type> Dictionary { get; }
}