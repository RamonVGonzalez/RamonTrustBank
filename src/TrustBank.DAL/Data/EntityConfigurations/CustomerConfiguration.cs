
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustBank.Core.Models;

namespace TrustBank.Infrastructure.Data.EntityConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(x => x.LastName)
                 .IsRequired()
                 .HasMaxLength(50);
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(x => x.Email)
                .IsUnique();
            builder.Property(x => x.CustomerName)
                .IsRequired();
            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(15).IsRequired();

        }
    }
}
