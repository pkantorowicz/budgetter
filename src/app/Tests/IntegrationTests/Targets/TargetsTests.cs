using System;
using System.Linq;
using System.Threading.Tasks;
using Budgetter.Application.BudgetPlans.CreateBudgetPlan;
using Budgetter.Application.FinanceOperations.CreateFinanceOperation;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.Targets.AllocateExpense;
using Budgetter.Application.Targets.ChangeTargetAttributes;
using Budgetter.Application.Targets.CreateTarget;
using Budgetter.Application.Targets.DeallocateExpense;
using Budgetter.Application.Targets.Dtos;
using Budgetter.IntegrationTests.Base;
using Budgetter.IntegrationTests.FinanceOperations;
using FluentAssertions;
using NUnit.Framework;

namespace Budgetter.IntegrationTests.Targets;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class TargetsTests : TestBase
{
    [Test]
    public async Task
        CreateBudgetGoal_ThenChangeAttributes_ThenCreateFinanceOperation_ThenAllocateExpense_ThenRemove()
    {
        const string planCurrency = "PLN";

        var createBudgetPlanCommandResult = await Executor.ExecuteCommandAsync(
            new CreateBudgetPlanCommand(
                "Title",
                planCurrency,
                DateTime.UtcNow.AddDays(-5),
                DateTime.UtcNow.AddDays(5),
                93));

        var planId = createBudgetPlanCommandResult.Data;

        await Task.Delay(5000);

        const string title = "title";
        const string description = "description";
        const int maxAmount = 1000;
        const string currency = "PLN";

        var validFrom = DateTime.UtcNow.AddDays(-15);
        var validTo = DateTime.UtcNow.AddDays(15);
        var planValidFrom = DateTime.UtcNow.AddDays(-17);
        var planValidTo = DateTime.UtcNow.AddDays(16);

        var commandResult = await Executor.ExecuteCommandAsync(
            new CreateTargetCommand(
                planId,
                title,
                description,
                validFrom,
                validTo,
                planValidFrom,
                planValidTo,
                maxAmount,
                currency));

        var targetId = commandResult.Data;

        var targets =
            await GetEventually(new GetTargetsProbe(
                Executor,
                x => x.Ok && x.Data.Any(),
                planId), 10000);

        var targetsArray = targets.Data.ToArray();
        var target = targetsArray[0];

        AssertChangedValues(
            targetId,
            planId,
            title,
            description,
            validFrom,
            validTo,
            maxAmount,
            currency,
            0,
            TargetStatusDto.Opened,
            TargetBalanceDto.Safe,
            target);

        const string changedTitle = "some other title";
        const string changedDescription = "some other description";
        const int changedMaxAmount = 1500;
        const string changedCurrency = "EUR";

        var changedValidFrom = DateTime.UtcNow.AddDays(-10);
        var changedValidTo = DateTime.UtcNow.AddDays(15);
        var changedPlanValidFrom = DateTime.UtcNow.AddDays(-11);
        var changedPlanValidTo = DateTime.UtcNow.AddDays(16);

        await Executor.ExecuteCommandAsync(
            new ChangeTargetAttributesCommand(
                targetId,
                changedTitle,
                changedDescription,
                changedValidFrom,
                changedValidTo,
                changedPlanValidFrom,
                changedPlanValidTo,
                changedMaxAmount,
                changedCurrency));

        targets =
            await GetEventually(new GetTargetsProbe(
                Executor,
                x => x.Ok && x.Data.Any(),
                planId), 15000);

        targetsArray = targets.Data.ToArray();
        target = targetsArray[0];

        AssertChangedValues(
            targetId,
            planId,
            changedTitle,
            changedDescription,
            changedValidFrom,
            changedValidTo,
            changedMaxAmount,
            changedCurrency,
            0,
            TargetStatusDto.Opened,
            TargetBalanceDto.Safe,
            target);

        var expenseOccurredAt = DateTime.UtcNow.AddDays(5);

        const decimal expensePrice = 540;
        const string expenseDescription = "expense description";
        const string expenseCurrency = "PLN";

        var createFinanceOperationCommandResult = await Executor.ExecuteCommandAsync(
            new CreateFinanceOperationCommand(
                planId,
                expenseDescription,
                expensePrice,
                expenseCurrency,
                planCurrency,
                planValidFrom,
                planValidTo,
                expenseOccurredAt));

        var expenseId = createFinanceOperationCommandResult.Data;

        var financeOperations =
            await GetEventually(new GetFinanceOperationProbe(
                Executor,
                x => x.Ok && x.Data.Items.Any(),
                planId), 20000);

        financeOperations.Data.Items.Should().HaveCount(1);

        await Executor.ExecuteCommandAsync(
            new AllocateExpenseCommand(
                targetId,
                new FinanceOperationDto
                {
                    Id = expenseId,
                    Title = expenseDescription,
                    Price = expensePrice,
                    Currency = expenseCurrency,
                    OccurredAt = expenseOccurredAt
                }));

        targets =
            await GetEventually(new GetTargetsProbe(
                Executor,
                x => x.Ok && x.Data.Any(),
                planId), 35000);

        targetsArray = targets.Data.ToArray();
        target = targetsArray[0];

        var targetItemsArray = target.TargetItems.ToArray();
        var targetItem = targetItemsArray[0];

        targetItem.Id.Should().Be(expenseId);
        targetItem.TargetId.Should().Be(targetId);
        targetItem.Title.Should().Be(expenseDescription);
        targetItem.UnitPrice.Should().Be(expensePrice);
        targetItem.Currency.Should().Be(expenseCurrency);
        targetItem.OccurredAt.Should().Be(expenseOccurredAt);

        await Executor.ExecuteCommandAsync(
            new DeallocateExpenseCommand(
                targetId,
                expenseId));

        targets =
            await GetEventually(new GetTargetsProbe(
                Executor,
                x => x.Ok && x.Data.Any(),
                planId), 45000);

        targetsArray = targets.Data.ToArray();
        target = targetsArray[0];
        target.TargetItems.Count.Should().Be(0);
    }

    private static void AssertChangedValues(
        Guid targetId,
        Guid planId,
        string title,
        string description,
        DateTime validFrom,
        DateTime validTo,
        decimal maxAmount,
        string currency,
        decimal totalAmount,
        TargetStatusDto status,
        TargetBalanceDto balance,
        TargetDto target)
    {
        target.Id.Should().Be(targetId);
        target.BudgetPlanId.Should().Be(planId);
        target.Title.Should().Be(title);
        target.Description.Should().Be(description);
        target.ValidFrom.Should().Be(validFrom);
        target.ValidTo.Should().Be(validTo);
        target.MaxAmount.Should().Be(maxAmount);
        target.Currency.Should().Be(currency);
        target.TotalAmount.Should().Be(totalAmount);
        target.Status.Should().Be(status);
        target.Balance.Should().Be(balance);
    }
}