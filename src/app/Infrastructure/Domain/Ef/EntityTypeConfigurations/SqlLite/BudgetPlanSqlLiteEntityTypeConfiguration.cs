using Budgetter.Application.BudgetPlans.Projections.ReadModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetter.Infrastructure.Domain.Ef.EntityTypeConfigurations.SqlLite;

public class BudgetPlanSqlLiteEntityTypeConfiguration : IEntityTypeConfiguration<BudgetPlanReadModel>
{
    public void Configure(EntityTypeBuilder<BudgetPlanReadModel> builder)
    {
        builder.ToTable("BudgetPlans");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("BudgetPlanId");

        builder.Property(e => e.Title)
            .HasColumnName("Title");

        builder.Property(e => e.Currency)
            .HasColumnName("Currency");

        builder.Property(e => e.ValidFrom)
            .HasColumnName("ValidFrom");

        builder.Property(e => e.ValidTo)
            .HasColumnName("ValidTo");

        builder.Property(e => e.IsRemoved)
            .HasColumnName("IsRemoved");

        builder.HasMany(c => c.FinanceOperations)
            .WithOne(p => p.BudgetPlan)
            .HasForeignKey(p => p.BudgetPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Targets)
            .WithOne(p => p.BudgetPlan)
            .HasForeignKey(p => p.BudgetPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}