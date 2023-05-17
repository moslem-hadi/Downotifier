using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Reflection.Emit;
using System.Text.Json;

namespace Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    { 
        builder.Property(t => t.Receiver)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(t => t.Message)
            .HasMaxLength(100)
            .IsRequired();
    }
}
