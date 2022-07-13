
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustBank.Core.Models;

namespace TrustBank.Infrastructure.Data.EntityConfigurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.Amount)
                .IsRequired()
                .HasPrecision(25, 2);
            builder.Property(x => x.DebitAccount)
                .IsRequired();
            builder.Property(x => x.CreditAccount)
                .IsRequired();       
            builder.HasIndex(x => x.CreditAccount);
            builder.HasIndex(x => x.DebitAccount);

        }
    }
}
