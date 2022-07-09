using System.Collections.Generic;
using TrustBank.Core.Models.Base;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Core.Models
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }

        public ProductType ProductType { get; set; }

        public int MinimumBalanceForProduct { get; set; }

        public ClosureStatus ClosureStatus { get; set; }

        public List<Account> Accounts { get; set; }

    }
}