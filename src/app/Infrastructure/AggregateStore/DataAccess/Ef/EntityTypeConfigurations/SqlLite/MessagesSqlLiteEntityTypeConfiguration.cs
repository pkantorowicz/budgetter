using Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetter.Infrastructure.AggregateStore.DataAccess.Ef.EntityTypeConfigurations.SqlLite;

public class MessagesSqlLiteEntityTypeConfiguration : IEntityTypeConfiguration<MessagesEfDao>
{
    public void Configure(EntityTypeBuilder<MessagesEfDao> builder)
    {
        builder.ToTable("Messages");

        builder.HasKey(x => x.Position);

        builder.HasIndex(e => new { e.StreamIdInternal, e.StreamVersion })
            .IsUnique();

        builder.HasIndex(e => new { e.StreamIdInternal, e.EventId })
            .IsUnique();

        builder.HasCheckConstraint("CK_Messages_stream_version", "[stream_version] >= 0");

        builder.Property(e => e.EventId)
            .HasColumnName("event_id")
            .HasColumnType("CHAR(36)")
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.StreamIdInternal)
            .HasColumnName("stream_id_internal")
            .HasColumnType("integer")
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.StreamVersion)
            .HasColumnName("stream_version")
            .HasColumnType("integer")
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.Position)
            .HasColumnName("position")
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_utc")
            .HasColumnType("DATETIME")
            .IsRequired();

        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasColumnType("CHAR(128)")
            .IsRequired();

        builder.Property(e => e.JsonData)
            .HasColumnName("json_data")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(e => e.JsonMetaData)
            .HasColumnName("json_metadata")
            .HasColumnType("text")
            .IsRequired(false);
    }
}