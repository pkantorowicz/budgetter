using Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.EntityTypeConfigurations.SqlLite;

public class StreamsSqlLiteEntityTypeConfiguration : IEntityTypeConfiguration<StreamEfDao>
{
    public void Configure(EntityTypeBuilder<StreamEfDao> builder)
    {
        builder.ToTable("Streams");

        builder.HasKey(x => x.IdInternal);

        builder.HasIndex(e => e.IdOriginal);

        builder.HasCheckConstraint("CK_Streams_version", "[version] >= - 1");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(e => e.IdOriginal)
            .HasColumnName("id_original")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(e => e.IdInternal)
            .HasColumnName("id_internal")
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(e => e.Version)
            .HasColumnName("version")
            .HasColumnType("integer")
            .HasDefaultValue(-1)
            .IsRequired();

        builder.Property(e => e.Position)
            .HasColumnName("position")
            .HasColumnType("integer")
            .HasDefaultValue(-1)
            .IsRequired();

        builder.Property(e => e.MaxAge)
            .HasColumnName("max_age")
            .HasColumnType("integer")
            .IsRequired(false);

        builder.Property(e => e.MaxCount)
            .HasColumnName("max_count")
            .HasColumnType("integer")
            .IsRequired(false);

        builder.HasMany(c => c.Messages)
            .WithOne(p => p.Stream)
            .HasForeignKey(p => p.StreamIdInternal)
            .IsRequired();
    }
}