using System.Linq;
using AutoMapper;
using Budgetter.Application.Commons.ReadModel;
using Budgetter.Application.Targets.Dtos;
using Budgetter.Application.Targets.Projections.ReadModel;
using Budgetter.Domain.Commons.ValueObjects;
using Budgetter.Domain.Targets;
using Budgetter.Domain.Targets.DomainEvents;

namespace Budgetter.Infrastructure.Configuration.Mappings.Profiles;

public class TargetsProfile : Profile
{
    public TargetsProfile()
    {
        CreateMapBetweenTargetReadModelAndTargetDto();
        CreateMapBetweenExpenseAllocateDomainEventAndTargetDto();
        CreateMapBetweenTargetCreatedDomainEventAndTargetDto();
    }

    private void CreateMapBetweenTargetReadModelAndTargetDto()
    {
        CreateMap<TargetReadModel, TargetDto>()
            .ForMember(dto => dto.MaxAmount, opt => opt.MapFrom(rm => rm.MaxAmount.Value))
            .ForMember(dto => dto.TotalAmount, opt => opt
                .MapFrom(rm => rm.TargetItems.Sum(ti => ti.UnitPrice.Value)))
            .ForMember(dto => dto.Currency, opt => opt.MapFrom(rm => rm.MaxAmount.Currency))
            .ForMember(dto => dto.Balance, opt => opt.MapFrom((rm, _) =>
            {
                var targetBalance =
                    TargetBalance.CalculateBalance(rm.TargetItems.Sum(ti => ti.UnitPrice.Value),
                        rm.MaxAmount.Value);
                var targetBalanceDto = (TargetBalanceDto)targetBalance.Id;

                return targetBalanceDto;
            }))
            .ForMember(dto => dto.Status, opt => opt.MapFrom((rm, _) =>
            {
                var targetStatus = Status.PredicateStatus(Duration.Specify(rm.ValidFrom, rm.ValidTo));
                var targetStatusDto = (TargetStatusDto)targetStatus.Id;

                return targetStatusDto;
            }))
            .ForMember(dto => dto.TargetItems,
                opt => opt.MapFrom((rm, _, _, ctx) =>
                    rm.TargetItems.Select(ti => ctx.Mapper.Map<TargetItemReadModel, TargetItemDto>(ti))));

        CreateMap<TargetItemReadModel, TargetItemDto>()
            .ForMember(dto => dto.UnitPrice, opt => opt.MapFrom(rm => rm.UnitPrice.Value))
            .ForMember(dto => dto.Currency, opt => opt.MapFrom(rm => rm.UnitPrice.Currency));
    }

    private void CreateMapBetweenExpenseAllocateDomainEventAndTargetDto()
    {
        CreateMap<ExpenseAllocatedDomainEvent, TargetItemReadModel>()
            .ForMember(rm => rm.Id, opt => opt.MapFrom(@event => @event.ExpenseId))
            .ForMember(rm => rm.Title, opt => opt.MapFrom(@event => @event.Title))
            .ForMember(rm => rm.UnitPrice, opt => opt.MapFrom(@event => new PriceReadModel
            {
                Value = @event.UnitPrice,
                Currency = @event.Currency
            }))
            .ForMember(rm => rm.OccurredAt, opt => opt.MapFrom(@event => @event.OccurredAt))
            .ForMember(rm => rm.Target, opt => opt.Ignore());
    }

    private void CreateMapBetweenTargetCreatedDomainEventAndTargetDto()
    {
        CreateMap<TargetCreatedDomainEvent, TargetReadModel>()
            .ForMember(rm => rm.Id, opt => opt.MapFrom(@event => @event.TargetId))
            .ForMember(rm => rm.Title, opt => opt.MapFrom(@event => @event.Title))
            .ForMember(rm => rm.Description, opt => opt.MapFrom(@event => @event.Description))
            .ForMember(rm => rm.MaxAmount, opt => opt.MapFrom(@event => new PriceReadModel
            {
                Value = @event.MaxAmount,
                Currency = @event.Currency
            }))
            .ForMember(rm => rm.TargetItems, opt => opt.Ignore())
            .ForMember(rm => rm.BudgetPlan, opt => opt.Ignore())
            .ForMember(rm => rm.IsRemoved, opt => opt.Ignore());
    }
}