using Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.EntityTypeConfigurations.SqlLite;

public class
    SubscriptionCheckpointSqlLiteEntityTypeConfiguration : IEntityTypeConfiguration<SubscriptionCheckpointEfDao>
{
    public void Configure(EntityTypeBuilder<SubscriptionCheckpointEfDao> builder)
    {
        builder.ToTable("SubscriptionCheckpoints");

        builder.HasKey(subscriptionCheckpointDao => subscriptionCheckpointDao.Code);

        builder.Property(subscriptionCheckpointDao => subscriptionCheckpointDao.Position)
            .HasColumnName("Position");

        builder.Property(subscriptionCheckpointDao => subscriptionCheckpointDao.Code)
            .HasColumnName("Code")
            .HasConversion<string>();
    }
}