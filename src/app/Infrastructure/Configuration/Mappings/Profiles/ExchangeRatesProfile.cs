using System;
using System.Linq;
using AutoMapper;
using Budgetter.BuildingBlocks.Domain.Extensions;
using Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.Models;
using Budgetter.Infrastructure.Feeds.Exchanges.DataContracts;

namespace Budgetter.Infrastructure.Configuration.Mappings.Profiles;

public class ExchangeRatesProfile : Profile
{
    public ExchangeRatesProfile()
    {
        CreateMapBetweenDailyExchangeRatesDataContractAndDailyExchangeRatesEfDao();
        CreateMapBetweenExchangeRateDataContractAndExchangeRateEfDao();
    }

    private void CreateMapBetweenDailyExchangeRatesDataContractAndDailyExchangeRatesEfDao()
    {
        CreateMap<DailyExchangeRatesDataContract, DailyExchangeRatesEfDao>()
            .ForMember(dao => dao.EffectiveDate, opt => opt.MapFrom(dt => dt.EffectiveDate.ToYmd()))
            .ForMember(dao => dao.ExchangeRates,
                opt => opt.MapFrom((dt, dao, _, ctx) =>
                    dt.Rates.Select(r =>
                    {
                        var exchangeRate = ctx.Mapper.Map<ExchangeRateDataContract, ExchangeRateEfDao>(r);

                        exchangeRate.DailyRate = dao.EffectiveDate;
                        exchangeRate.DailyExchangeRateEf = dao;

                        return exchangeRate;
                    })));
    }

    private void CreateMapBetweenExchangeRateDataContractAndExchangeRateEfDao()
    {
        CreateMap<ExchangeRateDataContract, ExchangeRateEfDao>()
            .ForMember(dao => dao.Id, opt => opt.MapFrom(dt => Guid.NewGuid()))
            .ForMember(dao => dao.DailyRate, opt => opt.MapFrom(dt => Guid.NewGuid()))
            .ForMember(dao => dao.DailyRate, opt => opt.Ignore())
            .ForMember(dao => dao.DailyExchangeRateEf, opt => opt.Ignore());
    }
}