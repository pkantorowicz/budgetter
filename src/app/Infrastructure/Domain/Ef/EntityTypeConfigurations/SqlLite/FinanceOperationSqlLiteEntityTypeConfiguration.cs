using Budgetter.Application.FinanceOperations.Projections.ReadModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetter.Infrastructure.Domain.Ef.EntityTypeConfigurations.SqlLite;

public class FinanceOperationSqlLiteEntityTypeConfiguration : IEntityTypeConfiguration<FinanceOperationReadModel>
{
    public void Configure(EntityTypeBuilder<FinanceOperationReadModel> builder)
    {
        builder.ToTable("Expenses");

        builder.HasKey(e => new { e.Id, e.BudgetPlanId });

        builder.Property(e => e.Id)
            .HasColumnName("FinanceOperationId");

        builder.Property(e => e.BudgetPlanId)
            .HasColumnName("BudgetPlanId");

        builder.Property(e => e.Title)
            .HasColumnName("Title");

        builder.OwnsOne(ne => ne.Price,
            b =>
            {
                b.Property(p => p.Value)
                    .HasColumnName("Price");

                b.Property(p => p.Currency)
                    .HasColumnName("Currency");
            });

        builder.Property(e => e.IsAllocated)
            .HasColumnName("IsAllocated");
        
        builder.Property(e => e.TargetId)
            .HasColumnName("TargetId");
        
        builder.Property(e => e.IsRemoved)
            .HasColumnName("IsRemoved");

        builder.Property(e => e.OccurredAt)
            .HasColumnName("OccurredAt");
    }
}