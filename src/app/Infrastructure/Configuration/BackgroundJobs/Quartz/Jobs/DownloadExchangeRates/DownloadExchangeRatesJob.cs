using System.Threading.Tasks;
using Budgetter.Application.ExchangeRates.DownloadExchangeRates;
using Budgetter.BuildingBlocks.Application.Execution;
using Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz.Extensions;
using Quartz;

namespace Budgetter.Infrastructure.Configuration.BackgroundJobs.Quartz.Jobs.DownloadExchangeRates;

public class DownloadExchangeRatesJob : IJob
{
    private const string From = "from";
    private const string To = "to";
    private const string MaxDaysCount = "maxDaysCount";

    public async Task Execute(IJobExecutionContext context)
    {
        var executor = BudgetterCompositionRoot.Resolve<IExecutor>();

        await executor.ExecuteCommandAsync(
            new DownloadExchangeRatesCommand(
                context.GetDateTimeValueFromJobContext(From),
                context.GetDateTimeValueFromJobContext(To),
                context.GetIntValueFromJobContext(MaxDaysCount)));
    }
}