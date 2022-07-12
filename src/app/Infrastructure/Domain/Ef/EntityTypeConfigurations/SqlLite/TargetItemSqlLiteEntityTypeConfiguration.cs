using Budgetter.Application.Targets.Projections.ReadModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetter.Infrastructure.Domain.Ef.EntityTypeConfigurations.SqlLite;

public class TargetItemSqlLiteEntityTypeConfiguration : IEntityTypeConfiguration<TargetItemReadModel>
{
    public void Configure(EntityTypeBuilder<TargetItemReadModel> builder)
    {
        builder.ToTable("TargetItems");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("TargetItemId");

        builder.Property(e => e.TargetId)
            .HasColumnName("TargetId");

        builder.Property(e => e.Title)
            .HasColumnName("Description");

        builder.OwnsOne(ne => ne.UnitPrice,
            b =>
            {
                b.Property(p => p.Value)
                    .HasColumnName("Price");

                b.Property(p => p.Currency)
                    .HasColumnName("Currency");
            });

        builder.Property(e => e.OccurredAt)
            .HasColumnName("OccurredAt");
    }
}