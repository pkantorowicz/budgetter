using System;
using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.FinanceOperations;
using Budgetter.Domain.Plans;

namespace Budgetter.Domain.Targets;

public class TargetItem : Entity
{
    private BudgetPlanId _budgetPlanId;
    private DateTime _occurredAt;
    private Title _title;
    private Money _unitPrice;

    private TargetItem()
    {
    }

    public static TargetItem FromExpense(
        FinanceOperationId financeOperationId,
        BudgetPlanId budgetPlanId,
        Title title,
        Money unitPrice,
        DateTime occurredAt)
    {
        var targetItem = new TargetItem
        {
            Id = financeOperationId.Value,
            _budgetPlanId = budgetPlanId,
            _title = title,
            _unitPrice = unitPrice,
            _occurredAt = occurredAt
        };

        return targetItem;
    }

    public TargetItemData GetData()
    {
        return new TargetItemData(
            TargetItemId.New(Id),
            _budgetPlanId,
            _title,
            _unitPrice,
            _occurredAt);
    }
}