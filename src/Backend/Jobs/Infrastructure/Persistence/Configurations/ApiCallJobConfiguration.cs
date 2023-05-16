using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Reflection.Emit;
using System.Text.Json;

namespace Infrastructure.Persistence.Configurations;

public class ApiCallJobConfiguration : IEntityTypeConfiguration<ApiCallJob>
{
    public void Configure(EntityTypeBuilder<ApiCallJob> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Url)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(t => t.MonitoringInterval)
            .IsRequired();

        builder.Property(t => t.Method)
            .IsRequired();

        builder
            .Property(b => b.Headers)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
    }
}
