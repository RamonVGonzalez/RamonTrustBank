using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustBank.Application.Dtos
{
    public class DepositAndWithdrawalRequestModel
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Pin { get; set; }
    }
}
