using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustBank.Core.Models;

namespace TrustBank.Infrastructure.Data.EntityConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(x => x.AccountName).IsRequired();
            builder.Property(x => x.AccountNumber).IsRequired();
            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Accounts)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Product)
                .WithMany(x => x.Accounts)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.AccountBalance)
                .HasPrecision(25, 2);
        }
    }
}
