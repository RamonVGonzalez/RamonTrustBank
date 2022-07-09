using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Application.Dtos
{
    public class AccountDisplayDto
    {
        public string AccountName { get; private set; }
        public string AccountNumber { get; set; }
        public decimal AccountBalance { get; private set; }

        public AccountStatus AccountStatus { get; set; }

        public DateTime LastTransactionDate { get; set; }

        public string ProductId { get; set; }
        public ProductDisplayDto Product { get; set; }
    }
}
