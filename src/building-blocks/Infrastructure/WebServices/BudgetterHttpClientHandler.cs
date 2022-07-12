using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Budgetter.BuildingBlocks.Infrastructure.WebServices;

public class BudgetterHttpClientHandler : HttpClientHandler
{
    private readonly ILogger _logger;

    private BudgetterHttpClientHandler(ILogger logger)
    {
        _logger = logger;

        ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _logger.Information($"Send request to web service endpoint - {request.RequestUri?.AbsoluteUri}");

        var response = await base.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        _logger.Information($"Web service endpoint returns response: {responseContent}");

        return response;
    }

    public static HttpClient CreateHttpClient(string baseUrl, int timeout, ILogger logger)
    {
        return new HttpClient(new BudgetterHttpClientHandler(logger))
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = TimeSpan.FromMilliseconds(timeout)
        };
    }
}