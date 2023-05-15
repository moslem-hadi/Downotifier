using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ApiCallJobConfiguration : IEntityTypeConfiguration<ApiCallJob>
{
    public void Configure(EntityTypeBuilder<ApiCallJob> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
    }
}
