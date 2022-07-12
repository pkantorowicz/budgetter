using System;
using System.Linq;
using System.Threading.Tasks;
using Budgetter.Application.BudgetPlans.CreateBudgetPlan;
using Budgetter.Application.FinanceOperations.ChangeFinanceOperationAttributes;
using Budgetter.Application.FinanceOperations.CreateFinanceOperation;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Infrastructure.Configuration;
using Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz.Jobs.DownloadExchangeRates;
using Budgetter.IntegrationTests.Base;
using FluentAssertions;
using NUnit.Framework;

namespace Budgetter.IntegrationTests.FinanceOperations;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class FinanceOperationTests : TestBase
{
    [Test]
    public async Task DownloadExchangeRates_DoExpense_ThenChangeAttributes_Tests()
    {
        var downloadExchangeJobManager = BudgetterCompositionRoot
            .Resolve<IDownloadExchangeRatesJobManager>();

        await downloadExchangeJobManager.TriggerDownloadExchangeRatesJob(
            DateTime.UtcNow.AddDays(-5),
            DateTime.UtcNow,
            93);

        await Task.Delay(5000);

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

        const string description = "description";
        const int price = 243;
        const string currency = "EUR";

        var commandResult = await Executor.ExecuteCommandAsync(
            new CreateFinanceOperationCommand(
                planId,
                description,
                price,
                currency,
                planCurrency,
                DateTime.UtcNow.AddDays(-20),
                DateTime.UtcNow.AddDays(10),
                null));

        var financeOperationId = commandResult.Data;

        var financeOperations =
            await GetEventually(new GetFinanceOperationProbe(
                Executor,
                x => x.Ok && x.Data.Items.Any(),
                planId), 15000);

        var financeOperation = financeOperations.Data.Items.FirstOrDefault();

        AssertChangedValues(
            planId,
            financeOperationId,
            description,
            planCurrency,
            financeOperation);

        financeOperation?.Price.Should().BeApproximately(
            Convert.ToDecimal(price * 4.5), Convert.ToDecimal(100));

        const string changedDescription = "some other description";
        const int changedPrice = 260;

        await Executor.ExecuteCommandAsync(
            new ChangeFinanceOperationAttributesCommand(
                financeOperationId,
                changedDescription,
                changedPrice));

        financeOperations =
            await GetEventually(new GetFinanceOperationProbe(
                Executor,
                x => x.Ok && x.Data.Items.Any(),
                planId), 20000);

        financeOperation = financeOperations.Data.Items.FirstOrDefault();

        AssertChangedValues(
            planId,
            financeOperationId,
            changedDescription,
            planCurrency,
            financeOperation);

        financeOperation?.Price.Should().Be(changedPrice);
    }

    private static void AssertChangedValues(
        Guid planId,
        Guid financeOperationId,
        string description,
        string currency,
        FinanceOperationDto financeOperation)
    {
        financeOperation?.Id.Should().Be(financeOperationId);
        financeOperation?.BudgetPlanId.Should().Be(planId);
        financeOperation?.Title.Should().Be(description);
        financeOperation?.Currency.Should().Be(currency);
    }
}