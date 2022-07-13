using System;
using TrustBank.Core.Models.Base;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Core.Models
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public string DebitAccount { get; set; }

        public string CreditAccount { get; set; }

        public string Narration { get; set; }

        public string TransactionReference { get; set; }

        public TransactionStatus TransactionStatus { get; set; }
    }
}
