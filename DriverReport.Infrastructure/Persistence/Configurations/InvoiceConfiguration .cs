using DriverReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverReports.Infrastructure.Persistence.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            // 🔑 Primary key
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // 🔹 Amount
            builder.Property(x => x.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            // 🔹 Dates
            builder.Property(x => x.InvoiceDate)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            // 🔹 Comment
            builder.Property(x => x.Comment)
                .HasMaxLength(500);
        }
    }
}