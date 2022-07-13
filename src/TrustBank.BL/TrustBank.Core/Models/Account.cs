
using System;
using TrustBank.Core.Models.Base;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Core.Models
{
    public class Account : BaseEntity
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public decimal AccountBalance { get; set; }
        public DateTime? AccountClosureDate { get; set; }

        public ClosureStatus ClosureStatus { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public DateTime LastTransactionDate { get; set; }
        public DateTime? ReactivationDate { get; set; }

        public string ProductId { get; set; }

        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }

        
    }
}