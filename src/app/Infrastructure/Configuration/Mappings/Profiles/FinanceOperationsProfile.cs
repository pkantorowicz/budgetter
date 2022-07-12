using AutoMapper;
using Budgetter.Application.Commons.ReadModel;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Budgetter.Domain.FinanceOperations.DomainEvents;

namespace Budgetter.Infrastructure.Configuration.Mappings.Profiles;

public class FinanceOperationsProfile : Profile
{
    public FinanceOperationsProfile()
    {
        CreateMapBetweenExpenseReadModelAndExpenseDto();
        CreateMapBetweenExpenseCreatedDomainEventAndExpenseReadModel();
    }

    private void CreateMapBetweenExpenseReadModelAndExpenseDto()
    {
        CreateMap<FinanceOperationReadModel, FinanceOperationDto>(MemberList.None)
            .ForMember(dto => dto.Id, opt => opt.MapFrom(rm => rm.Id))
            .ForMember(dto => dto.Price, opt => opt.MapFrom(rm => rm.Price.Value))
            .ForMember(dto => dto.Currency, opt => opt.MapFrom(rm => rm.Price.Currency))
            .ForMember(dto => dto.Type,
                opt => opt.MapFrom((rm, _) =>
                {
                    var financeOperationType = rm.Price?.Value > 0
                        ? FinanceOperationTypeDto.Income
                        : FinanceOperationTypeDto.Expense;

                    return financeOperationType;
                }));
    }

    private void CreateMapBetweenExpenseCreatedDomainEventAndExpenseReadModel()
    {
        CreateMap<FinanceOperationCreatedDomainEvent, FinanceOperationReadModel>(MemberList.None)
            .ForMember(rm => rm.Id, opt => opt.MapFrom(@event => @event.FinanceOperationId))
            .ForMember(rm => rm.Price, opt => opt.MapFrom(@event => new PriceReadModel
            {
                Value = @event.Price,
                Currency = @event.Currency
            }));
    }
}