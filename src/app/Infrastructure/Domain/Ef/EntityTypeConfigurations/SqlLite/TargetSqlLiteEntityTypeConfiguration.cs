using Budgetter.Application.Targets.Projections.ReadModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetter.Infrastructure.Domain.Ef.EntityTypeConfigurations.SqlLite;

public class TargetSqlLiteEntityTypeConfiguration : IEntityTypeConfiguration<TargetReadModel>
{
    public void Configure(EntityTypeBuilder<TargetReadModel> builder)
    {
        builder.ToTable("Targets");

        builder.HasKey(e => new { e.Id, e.BudgetPlanId });

        builder.Property(e => e.Id)
            .HasColumnName("TargetId");

        builder.Property(e => e.BudgetPlanId)
            .HasColumnName("BudgetPlanId");

        builder.Property(e => e.Title)
            .HasColumnName("Title");

        builder.Property(e => e.Description)
            .HasColumnName("Description");

        builder.OwnsOne(ne => ne.MaxAmount,
            b =>
            {
                b.Property(p => p.Value)
                    .HasColumnName("MaxAmountPrice");

                b.Property(p => p.Currency)
                    .HasColumnName("Currency");
            });

        builder.Property(e => e.ValidFrom)
            .HasColumnName("ValidFrom");

        builder.Property(e => e.ValidTo)
            .HasColumnName("ValidTo");

        builder.Property(e => e.IsRemoved)
            .HasColumnName("IsRemoved");

        builder.HasMany(c => c.TargetItems)
            .WithOne(p => p.Target)
            .HasForeignKey(p => new { p.TargetId, p.BudgetPlanId })
            .OnDelete(DeleteBehavior.Cascade);
    }
}