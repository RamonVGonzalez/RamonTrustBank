using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustBank.Application.Common
{
    public class BankSettings
    {
        public const string Data = "BankSettings";
        public string SettlementAccountNumber { get; set; }
    }
}
