using DriverReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Text.Json;

namespace DriverReport.Infrastructure.Persistence.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(b => b.DriverId)
                .IsRequired();

            builder.Property(b => b.Description)
                .IsRequired();

            builder.Property(b => b.CreatedDate)
                .IsRequired();

            builder.Property(b => b.UpdatedDate)
                .IsRequired();

            builder.Property(b => b.ReportDate)
                .IsRequired();

            builder.Property(b => b.Price)
                .IsRequired();

            builder.Property(b => b.PaymentType)
                .IsRequired();

            builder.Property(r => r.ImagePaths)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new()
                )
                .IsRequired(false);
        }
    }
}
