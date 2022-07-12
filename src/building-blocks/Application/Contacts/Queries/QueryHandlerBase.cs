using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Budgetter.BuildingBlocks.Application.Contacts.Results;

namespace Budgetter.BuildingBlocks.Application.Contacts.Queries;

public abstract class QueryHandlerBase<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    protected IMapper Mapper;

    protected QueryHandlerBase(IMapper mapper)
    {
        Mapper = mapper;
    }

    public abstract Task<IQueryResult<TResult>> Handle(TQuery request, CancellationToken cancellationToken);
}