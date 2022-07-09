
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustBank.Core.Models;

namespace TrustBank.Infrastructure.Data.EntityConfigurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.LGA).IsRequired();
            builder.Property(x => x.HouseAddress).IsRequired();
            builder.HasOne(x => x.Customer)
            .WithOne(x => x.Address)
            .HasForeignKey<Address>(x => x.CustomerId).IsRequired()
            .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        }
    }
}
