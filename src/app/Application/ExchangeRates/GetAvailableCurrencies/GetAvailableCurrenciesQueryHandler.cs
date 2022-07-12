using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.BuildingBlocks.Application.Contacts.Queries;
using Budgetter.BuildingBlocks.Application.Contacts.Results;

namespace Budgetter.Application.ExchangeRates.GetAvailableCurrencies;

internal class
    GetAvailableCurrenciesQueryHandler : QueryHandlerBase<GetAvailableCurrenciesQuery, IEnumerable<string>>
{
    public GetAvailableCurrenciesQueryHandler(IMapper mapper) : base(mapper)
    {
    }

    public override async Task<IQueryResult<IEnumerable<string>>> Handle(GetAvailableCurrenciesQuery query,
        CancellationToken cancellationToken)
    {
        return await Task.FromResult(
            QueryResult<IEnumerable<string>>.Success(
                AvailableCurrenciesExtensions
                    .GetAvailableCurrencies()));
    }
}