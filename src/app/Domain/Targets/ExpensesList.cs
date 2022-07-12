using System;
using System.Collections.Generic;
using System.Linq;
using Budgetter.BuildingBlocks.Domain.Types;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Exceptions;

namespace Budgetter.Domain.Targets;

public class ExpensesList : ValueObject
{
    private readonly HashSet<TargetItem> _expenses;

    private ExpensesList()
    {
        _expenses = new HashSet<TargetItem>();
    }

    public static ExpensesList Create()
    {
        return new ExpensesList();
    }

    public TargetItem GetSingle(TargetItemId targetItemId)
    {
        return _expenses.FirstOrDefault(e => e.Id == targetItemId.Value);
    }

    public void Add(TargetItem item)
    {
        SanitizeInputs(item);

        var existingItem = GetItem(item.Id);

        if (existingItem is not null)
            throw new ArgumentException($"Item with id: {item.Id} already exists.");

        var currentCurrency = GetCurrency();
        var itemData = item.GetData();

        if (currentCurrency is not null && currentCurrency != itemData.UnitPrice.Currency)
            throw new BusinessRuleDoesNotMatchedException(
                $"Adding item in other currency: {itemData.UnitPrice.Currency} " +
                $"than existing: {currentCurrency} is unsupported.");

        _expenses.Add(item);
    }

    public void Remove(TargetItem item)
    {
        SanitizeInputs(item);

        _expenses.Remove(item);
    }

    public void Remove(TargetItemId itemId)
    {
        if (itemId is null)
            throw new ArgumentException("Item id can not be null.");

        var item = GetItem(itemId.Value);

        if (item is null)
            throw new ArgumentException($"Item with id: {itemId} not found.");

        _expenses.Remove(item);
    }

    public Money GetTotalAmount()
    {
        var targetItemsData = ConvertToTargetItemData()
            .ToList();

        return Money.Of(
            targetItemsData.Sum(e => e.UnitPrice.Value), GetCurrency());
    }

    public int Count()
    {
        return _expenses.Count;
    }

    public IEnumerable<TargetItemData> GetItems()
    {
        return ConvertToTargetItemData();
    }

    private string GetCurrency()
    {
        var targetItemsData = ConvertToTargetItemData();

        return targetItemsData.FirstOrDefault()?.UnitPrice.Currency;
    }

    private TargetItem GetItem(Guid itemId)
    {
        return _expenses.FirstOrDefault(e => e.Id == itemId);
    }

    private IEnumerable<TargetItemData> ConvertToTargetItemData()
    {
        return _expenses
            .Select(e => e.GetData())
            .ToList();
    }

    private static void SanitizeInputs(TargetItem item)
    {
        if (item is null)
            throw new ArgumentException("Item can not be null.");
    }

    public void Clear()
    {
        _expenses.Clear();
    }
}