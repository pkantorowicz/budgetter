using Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetter.Infrastructure.Feeds.Exchanges.DataAccess.Ef.EntityTypeConfigurations.SqlLite;

public class ExchangeRatesSqlLiteEntityTypeConfiguration : IEntityTypeConfiguration<ExchangeRateEfDao>
{
    public void Configure(EntityTypeBuilder<ExchangeRateEfDao> builder)
    {
        builder.ToTable("ExchangeRates");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .HasColumnName("Code");

        builder.Property(e => e.Currency)
            .HasColumnName("Currency");

        builder.Property(e => e.Rate)
            .HasColumnName("Rate");
    }
}