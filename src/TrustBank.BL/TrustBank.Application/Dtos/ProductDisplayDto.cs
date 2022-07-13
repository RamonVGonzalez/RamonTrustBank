using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Application.Dtos
{
    public class ProductDisplayDto
    {
        public string ProductName { get; set; }

        public ProductType ProductType { get; set; }

        public int MinimumBalanceForProduct { get; set; }

    }
}
