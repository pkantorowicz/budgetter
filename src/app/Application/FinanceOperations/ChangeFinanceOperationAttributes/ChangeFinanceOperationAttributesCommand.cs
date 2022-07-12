using System;
using Budgetter.BuildingBlocks.Application.Contacts.Commands;

namespace Budgetter.Application.FinanceOperations.ChangeFinanceOperationAttributes;

public record ChangeFinanceOperationAttributesCommand : CommandBase
{
    public ChangeFinanceOperationAttributesCommand(
        Guid financeOperationId,
        string title,
        decimal price)
    {
        FinanceOperationId = financeOperationId;
        Title = title;
        Price = price;
    }

    public Guid FinanceOperationId { get; }
    public string Title { get; }
    public decimal Price { get; }
}