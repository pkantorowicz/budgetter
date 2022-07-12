using System.Collections.Generic;
using AutoMapper;
using Budgetter.Application.BudgetPlans.Dtos;
using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Budgetter.Application.FinanceOperations.Dtos;
using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Budgetter.Application.Targets.Dtos;
using Budgetter.Application.Targets.Projections.ReadModel;
using Budgetter.Domain.Plans.DomainEvents;

namespace Budgetter.Infrastructure.Configuration.Mappings.Profiles;

public class BudgetPlansProfile : Profile
{
    public BudgetPlansProfile()
    {
        CreateMapBetweenBudgetPlanReadModelAndBudgetPlanDto();
        CreateMapBetweenCreateBudgetPlanDomainEventAndBudgetPlanReadModel();
    }

    private void CreateMapBetweenBudgetPlanReadModelAndBudgetPlanDto()
    {
        CreateMap<BudgetPlanReadModel, BudgetPlanDto>()
            .ForMember(dto => dto.FinanceOperations,
                opt => opt.MapFrom((rm, _, _, ctx) =>
                    ctx.Mapper.Map<ICollection<FinanceOperationReadModel>, ICollection<FinanceOperationDto>>(
                        rm.FinanceOperations)))
            .ForMember(dto => dto.Targets,
                opt => opt.MapFrom((rm, _, _, ctx) =>
                    ctx.Mapper.Map<ICollection<TargetReadModel>, ICollection<TargetDto>>(
                        rm.Targets)));
    }

    private void CreateMapBetweenCreateBudgetPlanDomainEventAndBudgetPlanReadModel()
    {
        CreateMap<BudgetPlanCreatedDomainEvent, BudgetPlanReadModel>()
            .ForMember(rm => rm.Id, opt => opt.MapFrom(@event => @event.BudgetPlanId))
            .ForMember(rm => rm.FinanceOperations, opt => opt.Ignore())
            .ForMember(rm => rm.Targets, opt => opt.Ignore())
            .ForMember(rm => rm.IsRemoved, opt => opt.Ignore());
    }
}