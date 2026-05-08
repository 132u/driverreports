using DriverReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DriverReports.Infrastructure.Persistence.Configurations
{
    public class FinancialOperationConfiguration : IEntityTypeConfiguration<FinancialOperation>
    {
        public void Configure(EntityTypeBuilder<FinancialOperation> builder)
        {
            builder.ToTable("FinancialOperations");

            // 🔑 Primary key
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // 🔹 Amount
            builder.Property(x => x.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            // 🔹 Type (enum)
            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<int>();

            // 🔹 Dates
            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            // 🔹 Comment
            builder.Property(x => x.Comment)
                .HasMaxLength(500);

            // 🔹 Relation: User (Driver)
            builder.HasOne(x => x.User)
                .WithMany() // если нет навигации назад
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Indexes (очень важно для отчетов)
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.Date });
            builder.HasIndex(x => x.Type);
        }
    }
}