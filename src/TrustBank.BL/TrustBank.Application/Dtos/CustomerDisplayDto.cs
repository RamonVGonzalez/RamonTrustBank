using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Core.Models;

namespace TrustBank.Application.Dtos
{
    public class CustomerDisplayDto
    {
        public string CustomerName { get; set; }
        public AddressRequestAndDisplayDto Address { get; set; }
        public IEnumerable<AccountDisplayDto> AccountDisplayDtos { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateOfLastUpdate { get; set; }

    }
}
