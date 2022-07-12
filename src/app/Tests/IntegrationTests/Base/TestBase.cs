using System.Threading.Tasks;
using Budgetter.BuildingBlocks.Application.Execution;
using Budgetter.BuildingBlocks.Testes.IntegrationTests.Probing;
using Budgetter.Infrastructure.Configuration;
using Budgetter.Infrastructure.Domain.Ef;
using Budgetter.Infrastructure.Feeds.Exchanges.Nbp;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Budgetter.IntegrationTests.Base;

public class TestBase
{
    private Mock<ILogger> _logger;
    protected IExecutor Executor { get; private set; }

    [SetUp]
    public async Task BeforeEachTest()
    {
        var efSettings = new EfSettings("Filename=budgetter.sqlite;");

        var nbpApiSettings = new NpbApiSettings(
            "https://api.nbp.pl",
            5000,
            93);

        _logger = new Mock<ILogger>();

        _logger
            .Setup(c => c.ForContext("App", "Budgetter", false))
            .Returns(_logger.Object);

        BudgetterStartup.Initialize(
            efSettings,
            nbpApiSettings,
            _logger.Object,
            true);

        Executor = BudgetterCompositionRoot.Resolve<IExecutor>();

        await Task.CompletedTask;
    }

    [TearDown]
    public void AfterEachTest()
    {
        BudgetterStartup.Stop();
    }

    protected static async Task<T> GetEventually<T>(IProbe<T> probe, int timeout)
        where T : class
    {
        var poller = new Poller(timeout);

        return await poller.GetAsync(probe);
    }

    protected static async Task AssertEventually(IProbe probe, int timeout)
    {
        await new Poller(timeout).CheckAsync(probe);
    }
}