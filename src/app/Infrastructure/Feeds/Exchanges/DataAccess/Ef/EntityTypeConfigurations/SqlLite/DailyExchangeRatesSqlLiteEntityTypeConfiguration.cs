using Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.EntityTypeConfigurations.SqlLite;

public class DailyExchangeRatesSqlLiteEntityTypeConfiguration : IEntityTypeConfiguration<DailyExchangeRatesEfDao>
{
    public void Configure(EntityTypeBuilder<DailyExchangeRatesEfDao> builder)
    {
        builder.ToTable("DailyExchangeRates");

        builder.HasKey(e => e.EffectiveDate);

        builder.Property(e => e.EffectiveDate)
            .HasColumnName("EffectiveDate");

        builder.HasMany(c => c.ExchangeRates)
            .WithOne(p => p.DailyExchangeRateEf)
            .HasForeignKey(p => p.DailyRate)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}